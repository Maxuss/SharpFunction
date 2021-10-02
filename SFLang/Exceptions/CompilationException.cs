namespace SFLang.Exceptions
{
    public class CompilationException : PrettyException
    {
        public CompilationException(
            int line,
            int charpos,
            string file,
            PrettyException cause = null) : base(line, charpos, file, "A compilation error occurred!", cause) { }
    }
}