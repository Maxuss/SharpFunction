namespace SFLang.Exceptions
{
    public class CompilerException : PrettyException
    {
        public CompilerException(
            int line,
            Type obj,
            string message = "Unknown error",
            PrettyException cause = null) : base(line, -1, obj.FullName, "An internal error occurred", cause)
        {
        }
    }
}