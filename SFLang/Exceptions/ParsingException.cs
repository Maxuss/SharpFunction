using System.Runtime.CompilerServices;

namespace SFLang.Exceptions
{
    public class ParsingException : CoreException
    {
        public ParsingException(
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string file = "undefined",
            string message = "A Parsing exception occurred!",
            CoreException cause = null) : base(line, -1, file, message, cause)
        {
        }
    }
}