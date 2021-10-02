namespace SFLang.Exceptions
{
    public class EvaluationException : PrettyException
    {
        public EvaluationException(
            Type type, string message = "No message provided", PrettyException cause = null) : base(-1, -1, type.FullName, message, cause) {}
    }
}