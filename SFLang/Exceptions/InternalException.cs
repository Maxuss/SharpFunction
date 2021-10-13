namespace SFLang.Exceptions
{
    public class InternalException : CoreException
    {
        public InternalException(
            int line,
            int charpos,
            string file,
            CoreException cause = null) : base(line, charpos, file, "An internal exception occurred!", cause)
        {
        }
    }
}