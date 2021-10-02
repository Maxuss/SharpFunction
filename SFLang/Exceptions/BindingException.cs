namespace SFLang.Exceptions
{
    public class BindingException : PrettyException
    {
        public BindingException(
            Type type, string message = "No message provided", PrettyException cause = null) : base(-1, -1, type.FullName, message, cause) {}
    }
}