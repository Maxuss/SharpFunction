namespace SFLang.Exceptions
{
    public class CompilationException : CoreException
    {
        public CompilationException(
            int line,
            int charpos,
            string file,
            CoreException cause = null) : base(line, charpos, file, "A compilation error occurred!", cause)
        {
        }
    }
}