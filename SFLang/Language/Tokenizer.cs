using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SFLang.Language
{
    public class Tokenizer
    {
        public Tokenizer(Parser p)
        {
            Parser = p;
        }

        public Parser Parser { get; }

        public IEnumerable<string> Tokenize(Stream stream, Encoding encoding = null)
        {
            // Notice! We do NOT take ownership over stream!
            var reader = new StreamReader(stream, encoding ?? Encoding.UTF8, true, 1024);
            while (true)
            {
                var token = Parser.Next(reader);
                if (token == null)
                    break;
                yield return token;
            }
        }

        public IEnumerable<string> Tokenize(IEnumerable<Stream> streams, Encoding encoding = null)
        {
            foreach (var ixStream in streams)
            foreach (var ixToken in Tokenize(ixStream, encoding))
                yield return ixToken;
        }

        public IEnumerable<string> Tokenize(string code)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(code)))
            {
                foreach (var ix in Tokenize(stream)) yield return ix;
            }
        }

        public IEnumerable<string> Tokenize(IEnumerable<string> snippets)
        {
            foreach (var ixCode in snippets)
            foreach (var ixToken in Tokenize(ixCode))
                yield return ixToken;
        }

        public void EatSpace(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var ch = (char) reader.Peek();
                if (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n')
                {
                    reader.Read();
                    continue;
                }

                break;
            }
        }

        /// <summary>
        ///     Eats and discards the rest of the line from your reader.
        /// </summary>
        /// <param name="reader">Reader to eat the rest of the lines from.</param>
        public void EatLine(StreamReader reader)
        {
            reader.ReadLine();
        }

        /// <summary>
        ///     Eats and discards characters from the reader until the specified sequence is found.
        /// </summary>
        /// <param name="reader">Reader to eat from.</param>
        /// <param name="sequence">Sequence to look for that will end further eating.</param>
        /// <param name="throwIfNotFound">If true, will throw an exception if end sequence is not found before end of stream.</param>
        public void EatUntil(StreamReader reader, string sequence, bool throwIfNotFound = false)
        {
            // Sanity checking invocation.
            if (string.IsNullOrEmpty(sequence))
                throw new CompilerException(174, typeof(Tokenizer), "No stop sequence specified to EatUntil.");

            /*
             * Not sure if this is the optimal method to do this, but I think it
             * shouldn't be too far away from optimal either ...
             */
            var buffer = new List<char>(sequence.Length + 1);
            while (!reader.EndOfStream)
            {
                buffer.Add((char) reader.Read());
                if (buffer.Count > sequence.Length) buffer.RemoveAt(0);
                if (buffer[0] == sequence[0])
                    if (sequence == new string(buffer.ToArray()))
                        return; // Done!
            }

            // Sanity checking that stream is not corrupted, if we're told to do so.
            if (throwIfNotFound)
                throw new CompilerException(174, typeof(Tokenizer),
                    $"The '{sequence}' sequence was not found before EOF.");
        }

        /// <summary>
        ///     Reads a single line string literal from the reader, escaping characters if necessary,
        ///     and also supporting UNICODE hex syntax to reference UNICODE characters.
        /// </summary>
        /// <returns>The string literal.</returns>
        /// <param name="reader">Reader to read from.</param>
        /// <param name="stop">Stop what character that ends the string.</param>
        /// <param name="maxStringSize">The maximum sise of strings the tokenizer accepts before throwing a Lizzie exception.</param>
        public string ReadString(StreamReader reader, char stop = '"', int maxStringSize = -1)
        {
            var builder = new StringBuilder();
            for (var c = reader.Read(); c != -1; c = reader.Read())
                switch (c)
                {
                    case '\\':
                        builder.Append(GetEscapedCharacter(reader, stop));
                        break;
                    case '\n':
                    case '\r':
                        throw new CompilerException(174, typeof(Tokenizer),
                            $"String literal contains CR or LF characters close to '{builder}'.");
                    default:
                        if (c == stop)
                            return builder.ToString();
                        if (maxStringSize != -1 && builder.Length >= maxStringSize)
                            throw new CompilerException(174, typeof(Tokenizer),
                                $"String size exceeded maximum allowed size of '{maxStringSize}' characters.");
                        builder.Append((char) c);
                        break;
                }

            throw new CompilerException(174, typeof(Tokenizer),
                $"Syntax error, string literal not closed before EOF near '{builder}'");
        }

        /*
         * Returns escape character.
         */
        private static string GetEscapedCharacter(StreamReader reader, char stop)
        {
            var ch = reader.Read();
            if (ch == -1)
                throw new CompilerException(174, typeof(Tokenizer), "EOF found before string literal was closed");
            switch ((char) ch)
            {
                case '\\':
                    return "\\";
                case 'a':
                    return "\a";
                case 'b':
                    return "\b";
                case 'f':
                    return "\f";
                case 't':
                    return "\t";
                case 'v':
                    return "\v";
                case 'n':
                    return "\n";
                case 'r':
                    return "\r";
                case 'x':
                    return HexCharacter(reader);
                default:
                    if (ch == stop)
                        return stop.ToString();
                    throw new CompilerException(194, typeof(Tokenizer),
                        $"Invalid escape sequence character '{Convert.ToInt32(ch)}' found in string literal");
            }
        }

        /*
         * Returns hex encoded character.
         */
        private static string HexCharacter(StreamReader reader)
        {
            var hexNumberString = "";
            for (var idxNo = 0; idxNo < 4; idxNo++)
            {
                var tmp = reader.Read();
                if (tmp == -1)
                    return ""; // Incomplete hex char ...!!
                hexNumberString += (char) tmp;
            }

            var integerNo = Convert.ToInt32(hexNumberString, 16);
            return Encoding.UTF8.GetString(BitConverter.GetBytes(integerNo).Reverse().ToArray());
        }

        public string EatSpace(string str)
        {
            var spaced = Regex.Replace(str, "\\s+", " ");
            return spaced.Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }
    }
}