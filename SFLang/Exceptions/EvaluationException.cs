using System;

namespace SFLang.Exceptions
{
    public class EvaluationException : CoreException
    {
        public EvaluationException(
            Type type, string message = "No message provided", CoreException cause = null) : base(-1, -1,
            type.FullName, message, cause)
        {
        }
    }
}