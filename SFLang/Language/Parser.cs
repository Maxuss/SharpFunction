#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using SFLang.Exceptions;
using SFLang.Lexicon;

namespace SFLang.Language
{
    public class Parser
    {
        public static Parser Default = new();

        public Parser()
        {
            FileName = null;
            RawContents = null;
            Lines = null;
            Reader = null;
            Tokenizer = new Tokenizer(this);
            Default = this;
        }

        public Parser(string file)
        {
            FileName = file;
            RawContents = UpgradeCode(System.IO.File.ReadAllText(FileName));
            Reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(RawContents)));
            Lines = RawContents.Split("\n").ToList();
            Tokenizer = new Tokenizer(this);
            Default = this;
        }

        public string? FileName { get; }
        public string? RawContents { get; set; }
        public List<string>? Lines { get; set; }
        public StreamReader? Reader { get; set; }
        public Tokenizer Tokenizer { get; }
        public int MaxStringSize { get; set; } = -1;
        private Stack<string> Cached { get; set; } = new();

        public Lambda<TContext> Compile<TContext>(TContext ctx)
        {
            return Lexer.Compile<TContext>(this, Reader?.BaseStream);
        }

        public IEnumerable<string> Tokenize(Stream stream, Encoding? encoding = null)
        {
            // Notice! We do NOT take ownership over stream!
            StreamReader reader = new(stream, encoding ?? Encoding.UTF8, true, 1024);
            while (true)
            {
                var token = Next(reader);
                if (token == null)
                    break;
                yield return token;
            }
        }

        public IEnumerable<string> Tokenize(IEnumerable<Stream> streams, Encoding? encoding = null)
        {
            return streams.SelectMany(ixStream => Tokenize(ixStream, encoding));
        }

        public IEnumerable<string> Tokenize(string code)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(code));
            foreach (var ix in Tokenize(stream)) yield return ix;
        }

        public IEnumerable<string> Tokenize(IEnumerable<string> snippets)
        {
            return snippets.SelectMany(ixCode => Tokenize(ixCode));
        }

        public static string UpgradeCode(string code)
        {
            var lines = code;
            var methodCount = 0;
            // Upgrade methods
            var methodReplaced = Regex.Replace(
                lines,
                "(method\\((?<params>[^\'\"]*)\\)[\\r\\t\\n\\s]*\\{[\\r\\t\\n\\s]*(?<body>(.|[\\n\\r\\t\\s])*)})",
                match =>
                {
                    methodCount++;
                    if (methodCount > 1) throw new CoreException("Expected only 1 v0.6+ method in a single file");
                    var para = match.Groups["params"];
                    var body = match.Groups["body"];
                    var def = "method({%BODY%}%PARAMS%)";
                    def = para.Success
                        ? def.Replace("%PARAMS%", ", " + para.Captures[0].Value)
                        : def.Replace("%PARAMS%", "");
                    def =
                        def.Replace("%BODY%", body.Success
                            ? body.Captures[0].Value
                            : "");
                    return $"{def}.mend";
                }, RegexOptions.Multiline);

            // Replace v0.5+ let statements to old let(@name, value) statements
            var firstReplaced = Regex.Replace(
                methodReplaced, "let\\s+@(?<varname>.+)\\s*=\\s*(?<varval>(.|[\\n\\r\\t\\s])+)",
                match =>
                {
                    var name = match.Groups["varname"];
                    var val = match.Groups["varval"];
                    if (!name.Success || !val.Success) return match.Value;
                    if (!val.Captures[0].Value.Contains(".mend"))
                        return $"let(@{name.Captures[0].Value}, {val.Captures[0].Value})";
                    var split = val.Captures[0].Value.Split(".mend");
                    return $"let(@{name.Captures[0].Value}, {split[0]}){split[1]}";
                }, RegexOptions.Multiline);

            // replace ! to not
            var fourthReplaced = Regex.Replace(
                firstReplaced,
                "(!(?<vname>[^\"']+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                match =>
                {
                    var value = match.Groups["vname"];
                    return !value.Success ? match.Value : $"not({value.Captures[0].Value})";
                }, RegexOptions.Multiline
            );

            var moduleReplaced = Regex.Replace(
                fourthReplaced,
                "(include\\s+(?<mod>\\S+))",
                match =>
                {
                    var value = match.Groups["mod"];
                    return !value.Success ? match.Value : $"include('{value.Captures[0].Value}')";
                }, RegexOptions.Multiline
            );

            var declarationReplaced = Regex.Replace(
                moduleReplaced,
                "(declare\\s+(?<mod>\\S+))",
                match =>
                {
                    var value = match.Groups["mod"];
                    return !value.Success ? match.Value : $"declare('{value.Captures[0].Value}')";
                }, RegexOptions.Multiline);

            var constReplaced = Regex.Replace(
                declarationReplaced, "const\\s+@(?<varname>.+)\\s*=\\s*(?<varval>(.|[\\n\\r\\t\\s])+)",
                match =>
                {
                    var name = match.Groups["varname"];
                    var val = match.Groups["varval"];
                    if (!name.Success || !val.Success) return match.Value;
                    if (!val.Captures[0].Value.Contains(".mend"))
                        return $"const(@{name.Captures[0].Value}, {val.Captures[0].Value})";
                    var split = val.Captures[0].Value.Split(".mend");
                    return $"const(@{name.Captures[0].Value}, {split[0]}){split[1]}";
                }, RegexOptions.Multiline);

            // Replace v0.5- set statements to old set(@name, value) statements
            var secondReplaced = Regex
                .Replace(
                    constReplaced,
                    "(@(?<varname>.+)\\s*=(?<varval>.+))",
                    match =>
                    {
                        var name = match.Groups["varname"];
                        var val = match.Groups["varval"];
                        if (!name.Success || !val.Success) return match.Value;
                        return $"set(@{name.Captures[0].Value}, {val.Captures[0].Value})";
                    }, RegexOptions.Multiline
                );


            lines = secondReplaced.Replace(".mend", "");
            return lines;
        }

        private readonly string[] allowed = {"class", "function", "assembly"};

        private readonly Dictionary<string, string> replaceable = new()
        {
            {"(", ")"},
            {")", "("},
            {"{", "}"},
            {"}", "{"}
        };
        public string? Next(StreamReader reader, bool checkCached = true)
        {
            reader = Reader ?? reader;
            // Checking if we have cached tokens.
            if (checkCached && Cached.Count > 0)
                return Cached.Pop(); // Returning cached token and popping it off our stack.

            // Eating white space from stream.
            Tokenizer.EatSpace(reader);
            if (reader.EndOfStream)
                return null; // No more tokens.

            // Finding next token from reader.
            string? retVal = null;
            while (!reader.EndOfStream)
            {
                // Peeking next character in stream, and checking its classification.
                var ch = (char) reader.Peek();
                switch (ch)
                {
                    // If statement
                    case '?':
                        reader.Read();
                        Tokenizer.EatSpace(reader);
                        if (reader.Peek() != '(')
                            throw new ParsingException(message: "Expected parenthesis after if statement declaration!");
                        reader.Read();
                        Console.WriteLine("Successfully read Parenthesis condition.");
                        Console.WriteLine($"Cache before pushing: {string.Concat(Cached)}");
                        Cached.Push(")");
                        var read = Tokenizer.ReadScope(reader, '(', ')');
                        
                        reader.Read();
                        
                        Cached.Push(read);
                        Cached.Push("(");
                        
                        Console.WriteLine($"Cache after pushing: {string.Concat(Cached)}");
                        
                        Tokenizer.EatSpace(reader);
                        
                        if(reader.Read() != '=' && reader.Peek() != '>')
                            throw new ParsingException(message: "Expected curly brackets after if statement declaration!");
                        
                        reader.Read();
                        Tokenizer.EatSpace(reader);
                        Console.WriteLine("Successfully read Brackets body.");
                        Console.WriteLine($"Cache before pushing: {string.Concat(Cached)}");
                        
                        // Cached.Push("then");
                        
                        var methodRef = Tokenizer.ReadUntil(reader, ';');
                        if (methodRef.StartsWith("{"))
                        {
                            var lambda = Tokenizer.Tokenize(methodRef).ToList();
                            var index = lambda.IndexOf("{");
                            lambda.Insert(index, "then");
                            lambda.Insert(index+1, ".lambda");
                            Cached = new Stack<string>(lambda);
                            Cached = ReverseStack(Cached);
                        }
                        else
                        {
                            Cached.Push("then");
                            Cached.Push(".invoke");
                            Cached.Push(methodRef);
                            Cached = ReverseStack(Cached, el => replaceable.ContainsKey(el) ? replaceable[el] : el);
                        }

                        Console.WriteLine($"Cache after pushing: {string.Concat(Cached)}");
                        reader.Read();
                        
                        if (reader.Peek() != ':') return "if";
                        
                        reader.Read();
                        Cached.Push("else");
                        Tokenizer.EatSpace(reader);
                        if (reader.Peek() != '{')
                            throw new ParsingException(
                                message: "Expected curly brackets after else statement declaration!");
                        reader.Read();
                        var elseScope = Tokenizer.Tokenize(Tokenizer.ReadScope(reader, '{', '}')).ToList();
                        foreach (var cnd in elseScope)
                            Cached.Push(cnd);
                        reader.Read();
                        Cached.Push("}");
                        return "if";
                    case '>':
                        reader.Read();

                        Tokenizer.EatSpace(reader);
                        var type = Tokenizer.ReadString(reader, ' ', 15).Replace(" ", "");
                        if (!allowed.Contains(type))
                            throw new ParsingException(
                                message:
                                $"Pointer contains invalid type '{type}'! Expected one of [{string.Join(',', allowed)}]");
                        var path = Tokenizer.ReadString(reader, ';');
                        path = path.Replace(";", "");
                        Cached.Push(path);
                        
                        return $"> {type}";
                    case ' ':
                    case '\r':
                    case '\n':
                    case '\t':
                        return retVal;
                    case '@':
                    case ',':
                    case '(':
                    case ')':
                    case '{':
                    case '}':
                        return retVal ?? ((char) reader.Read()).ToString();
                    case '"':
                    case '\'':

                        reader.Read();
                        var strLiteral = Tokenizer.ReadString(reader, ch, MaxStringSize);

                        Cached.Push(ch == '\'' ? "'" : "\"");
                        Cached.Push(strLiteral);
                        return ch == '\'' ? "'" : "\"";

                    case '/':

                        reader.Read();
                        ch = (char) reader.Peek();
                        switch (ch)
                        {
                            case '/':
                            {
                                Tokenizer.EatLine(reader);

                                // There might be some spaces at the front of our stream now ...
                                Tokenizer.EatSpace(reader);

                                // Checking if we currently have a token.
                                if (retVal != null)
                                    return retVal;
                                break;
                            }
                            case '*':
                            {
                                // Multiline comment, making sure we discard opening "*" character from stream.
                                reader.Read();
                                Tokenizer.EatUntil(reader, "*/", true);

                                // There might be some spaces at the front of our stream now ...
                                Tokenizer.EatSpace(reader);

                                // Checking if we currently have a token.
                                if (!string.IsNullOrEmpty(retVal))
                                    return retVal;
                                break;
                            }
                            default:
                                return "/";
                        }

                        break;

                    default:
                        // Eating next character, and appending to retVal.
                        retVal += (char) reader.Read();
                        break;
                }
            }

            return retVal;
        }

        public static Stack<T> ReverseStack<T>(Stack<T> stack, Func<T, T>? forEach = null)
        {
            var rev = new Stack<T>();
            while (stack.Count != 0)
            {
                var el = stack.Pop();
                var conv = forEach != null ? forEach(el) : el;
                rev.Push(conv);
            }
            return rev;
        }
    }
}