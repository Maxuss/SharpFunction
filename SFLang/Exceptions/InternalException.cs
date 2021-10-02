namespace SFLang.Exceptions
{
    public class InternalException : PrettyException
    {
        public InternalException(
            int line,
            int charpos,
            string file,
            PrettyException cause = null) : base(line, charpos, file, "An internal exception occurred!", cause) { }
    }
}