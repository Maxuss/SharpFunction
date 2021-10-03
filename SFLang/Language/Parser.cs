#nullable enable
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SFLang.Lexicon;

namespace SFLang.Language
{
    public class Parser
    {
        public string? FileName { get; }
        public string? RawContents { get; set; }
        public List<string>? Lines { get; set; }
        public StreamReader? Reader { get; set; }
        public Tokenizer Tokenizer { get; }
        public int MaxStringSize { get; set; } = -1;
        private Stack<string> Cached { get; } = new();

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
            RawContents = UpgradeCode(File.ReadAllText(FileName));
            Reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(RawContents)));
            Lines = RawContents.Split("\n").ToList();
            Tokenizer = new Tokenizer(this);
            Default = this;
        }

        public Lambda<TContext> Compile<TContext>(TContext ctx)
        {
            return Lexer.Compile<TContext>(this, Reader?.BaseStream);
        }
        
        public IEnumerable<string> Tokenize(Stream stream, Encoding? encoding = null)
        {
            // Notice! We do NOT take ownership over stream!
            StreamReader reader = new(stream, encoding ?? Encoding.UTF8, true, 1024);
            while (true) {
                var token = Next(reader);
                if (token == null)
                    break;
                yield return token;
            }
            yield break;
        }
        
        public IEnumerable<string> Tokenize(IEnumerable<Stream> streams, Encoding? encoding = null)
        {
            return streams.SelectMany(ixStream => Tokenize(ixStream, encoding));
        }
        
        public IEnumerable<string> Tokenize(string code)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(code));
            foreach (var ix in Tokenize(stream)) {
                yield return ix;
            }
        }
        
        public IEnumerable<string> Tokenize(IEnumerable<string> snippets)
        {
            foreach (var ixCode in snippets) {
                foreach (var ixToken in Tokenize(ixCode)) {
                    yield return ixToken;
                }
            }
        }
        public static string UpgradeCode(string code)
        {
            var lines = Regex.Replace(code, "\\s", " ");
            // Replace v0.5+ let statements to old let(@name, value) statements
            var firstReplaced = Regex.Replace(
                        lines, "(let\\s+@(?<varname>.+)\\s*=\\s*(?<varval>.+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)", 
                        match =>
                        {
                            var name = match.Groups["varname"];
                            var val = match.Groups["varval"];
                            if (!name.Success || !val.Success) return match.Value;
                            
                            return $"let(@{name.Captures[0].Value}, {val.Captures[0].Value})";
                        }, RegexOptions.Multiline);
            
            // Replace v0.5+ set statements to old set(@name, value) statements
            var secondReplaced = Regex
                .Replace(
                        firstReplaced,
                        "(@(?<varname>.+)\\s*=(?<varval>.+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)", 
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
                    "(if[\\r\\n\\t\\s]*\\((?<condition>.+)\\)[\\r\\n\\t\\s]*{(?<evaluation>[\\r\\n\\t\\s]*.*)}[\\r\\n\\t\\s]*(else[\\r\\n\\t\\s]*{(?<elsecase>[\\r\\n\\t\\s]*.*)})*)(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                    match =>
                    {
                        var condition = match.Groups["condition"];
                        var evaluation = match.Groups["evaluation"];
                        var elsecase = match.Groups["elsecase"];

                        if (!condition.Success || !evaluation.Success) return match.Value;
                        var ret =  elsecase.Success
                            ? $"if({condition.Captures[0].Value}, {{{evaluation.Captures[0].Value}}}, {{{elsecase.Captures[0].Value}}})"
                            : $"if({condition.Captures[0].Value}, {{{evaluation.Captures[0].Value}}})";
                        ret = ret.Replace("else", ",");
                        return ret;
                    }, RegexOptions.Multiline);

            // replace equality and unequality
            var eqReplaced = Regex.Replace(
                thirdReplaced,
                "((?<first>[^\"']+)\\s*==\\s*(?<second>[^\"']+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                match =>
                {
                    var first = match.Groups["first"];
                    var second = match.Groups["second"];
                    return !first.Success || !second.Success 
                        ? match.Value 
                        : $"eq({first.Captures[0].Value}, {second.Captures[0].Value})";
                }, RegexOptions.Multiline);

            var nEqReplaced = Regex.Replace(
                eqReplaced,
                "((?<first>[^\"']+)\\s*!=\\s*(?<second>[^\"']+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                match =>
                {
                    var first = match.Groups["first"];
                    var second = match.Groups["second"];
                    return !first.Success || !second.Success 
                        ? match.Value 
                        : $"not(eq({first.Captures[0].Value}, {second.Captures[0].Value}))";
                }, RegexOptions.Multiline);

            var ltReplaced = Regex.Replace(
                nEqReplaced,
                "((?<first>[^\"']+)\\s*<\\s*(?<second>[^\"']+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                match =>
                {
                    var first = match.Groups["first"];
                    var second = match.Groups["second"];
                    return !first.Success || !second.Success 
                        ? match.Value 
                        : $"lt({first.Captures[0].Value}, {second.Captures[0].Value})";
                }, RegexOptions.Multiline);

            var mtReplaced = Regex.Replace(
                ltReplaced,
                "((?<first>[^\"']+)\\s*>\\s*(?<second>[^\"']+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                match =>
                {
                    var first = match.Groups["first"];
                    var second = match.Groups["second"];
                    return !first.Success || !second.Success 
                        ? match.Value 
                        : $"mt({first.Captures[0].Value}, {second.Captures[0].Value})";
                }, RegexOptions.Multiline);

            var lteReplaced = Regex.Replace(
                mtReplaced,
                "((?<first>[^\"']+)\\s*<=\\s*(?<second>[^\"']+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                match =>
                {
                    var first = match.Groups["first"];
                    var second = match.Groups["second"];
                    return !first.Success || !second.Success 
                        ? match.Value 
                        : $"lte({first.Captures[0].Value}, {second.Captures[0].Value})";
                }, RegexOptions.Multiline);

            var mteReplaced = Regex.Replace(
                lteReplaced,
                "((?<first>[^\"']+)\\s*>=\\s*(?<second>[^\"']+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                match =>
                {
                    var first = match.Groups["first"];
                    var second = match.Groups["second"];
                    return !first.Success || !second.Success 
                        ? match.Value 
                        : $"mte({first.Captures[0].Value}, {second.Captures[0].Value})";
                }, RegexOptions.Multiline);

            // replace ! to not
            var fourthReplaced = Regex.Replace(
                mteReplaced,
                "(!(?<vname>[^\"']+))(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                match =>
                {
                    var value = match.Groups["vname"];
                    return !value.Success ? match.Value : $"not({value.Captures[0].Value})";
                }, RegexOptions.Multiline
            );
            
            // Upgrade methods
            var methodReplaced = Regex.Replace(
                fourthReplaced,
                "(method\\((?<params>[^'\"]*)\\)[\\r\\t\\n\\s]*{[\\r\\t\\n\\s]*(?<body>[^\"']*)})(?=([^\"']*\"'[^\"']*\"')*[^'\"]*$)",
                match =>
                {
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
                    return def;
                }, RegexOptions.Multiline);
            lines = methodReplaced;
            
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
            while (!reader.EndOfStream) {

                // Peeking next character in stream, and checking its classification.
                var ch = (char)reader.Peek();
                switch (ch) {
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
                        return retVal ?? ((char)reader.Read()).ToString();
                    case '"':
                    case '\'':

                        reader.Read();
                        var strLiteral = Tokenizer.ReadString(reader, ch, MaxStringSize);
                        
                        Cached.Push(ch == '\'' ? "'" : "\"");
                        Cached.Push(strLiteral);
                        return ch == '\'' ? "'" : "\"";
                    
                    case '/':

                        reader.Read(); 
                        ch = (char)reader.Peek();
                        if (ch == '/') {

                            Tokenizer.EatLine(reader);

                            // There might be some spaces at the front of our stream now ...
                            Tokenizer.EatSpace(reader);

                            // Checking if we currently have a token.
                            if (retVal != null)
                                return retVal;

                        } else if (ch == '*') {
                            // Multiline comment, making sure we discard opening "*" character from stream.
                            reader.Read();
                            Tokenizer.EatUntil(reader, "*/", true);

                            // There might be some spaces at the front of our stream now ...
                            Tokenizer.EatSpace(reader);

                            // Checking if we currently have a token.
                            if (!string.IsNullOrEmpty(retVal))
                                return retVal;

                        } else {
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