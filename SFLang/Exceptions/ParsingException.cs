namespace SFLang.Exceptions
{
    public class ParsingException : PrettyException
    {
        public ParsingException(
            int line,
            int charpos,
            string file,
            string message = "A Parsing exception occurred!",
            PrettyException cause = null) : base(line, charpos, file, message, cause) { }
    }
}