namespace SFLang.Exceptions
{
    public class CompilerException : CoreException
    {
        public CompilerException(
            int line,
            Type obj,
            string message = "Unknown error",
            CoreException cause = null) : base(line, -1, obj.FullName, "An internal error occurred", cause)
        {
        }
    }
}