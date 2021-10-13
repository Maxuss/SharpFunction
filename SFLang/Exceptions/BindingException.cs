namespace SFLang.Exceptions
{
    public class BindingException : CoreException
    {
        public BindingException(
            Type type, string message = "No message provided", CoreException cause = null) : base(-1, -1,
            type.FullName, message, cause)
        {
        }
    }
}