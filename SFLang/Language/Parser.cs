#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            RawContents = File.ReadAllText(FileName);
            Reader = File.OpenText(FileName);
            Lines = RawContents.Split("\n").ToList();
            Tokenizer = new Tokenizer(this);
            Default = this;
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

    public class LambdaParser : Parser
    {
        public new string FileName => null;
        public static readonly LambdaParser Default;

        static LambdaParser()
        {
            Default = new LambdaParser();
        }
    }
}