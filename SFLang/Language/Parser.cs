#nullable enable
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        private Stack<string> Cached { get; } = new();

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
                    if (methodCount > 1) throw new PrettyException("Expected only 1 v0.6+ method in a single file");
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
            // Replace v0.5- set statements to old set(@name, value) statements
            var secondReplaced = Regex
                .Replace(
                    firstReplaced,
                    "(@(?<varname>.+)\\s*=(?<varval>.+))",
                    match =>
                    {
                        var name = match.Groups["varname"];
                        var val = match.Groups["varval"];
                        if (!name.Success || !val.Success) return match.Value;
                        return $"set(@{name.Captures[0].Value}, {val.Captures[0].Value})";
                    }, RegexOptions.Multiline
                );
            // Upgrade if's
            var thirdReplaced = Regex
                .Replace(
                    secondReplaced,
                    "(if[\\r\\n\\t\\s]*\\((?<condition>.+)\\)[\\r\\n\\t\\s]*{(?<evaluation>[\\r\\n\\t\\s]*.*)}[\\r\\n\\t\\s]*(else[\\r\\n\\t\\s]*{(?<elsecase>[\\r\\n\\t\\s]*.*)})*)",
                    match =>
                    {
                        var condition = match.Groups["condition"];
                        var evaluation = match.Groups["evaluation"];
                        var elsecase = match.Groups["elsecase"];

                        if (!condition.Success || !evaluation.Success) return match.Value;
                        var ret = elsecase.Success
                            ? $"if({condition.Captures[0].Value}, {{{evaluation.Captures[0].Value}}}, {{{elsecase.Captures[0].Value}}})"
                            : $"if({condition.Captures[0].Value}, {{{evaluation.Captures[0].Value}}})";
                        ret = ret.Replace("else", ",");
                        return ret;
                    }, RegexOptions.Multiline);

            // replace ! to not
            var fourthReplaced = Regex.Replace(
                thirdReplaced,
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
            
            lines = declarationReplaced.Replace(".mend", "");
            return lines;
        }

        public string? Next(StreamReader reader)
        {
            reader = Reader ?? reader;
            // Checking if we have cached tokens.
            if (Cached.Count > 0)
                return Cached.Pop(); // Returning cached token and popping it off our stack.

            // Eating white space from stream.
            Tokenizer.EatSpace(reader);
            if (reader.EndOfStream)
                return null; // No more tokens.

            // Finding next token from reader.
            string retVal = null;
            while (!reader.EndOfStream)
            {
                // Peeking next character in stream, and checking its classification.
                var ch = (char) reader.Peek();
                switch (ch)
                {
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
    }
}