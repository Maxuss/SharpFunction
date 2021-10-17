using SFLang.Exceptions;

namespace SFLang.Lexicon
{
    public class AssertionError : CoreException
    {
        public AssertionError(string message) : base(message) { }
    } 
}