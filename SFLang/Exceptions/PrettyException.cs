using System.Runtime.CompilerServices;

namespace SFLang.Exceptions
{
    public class PrettyException : Exception
    {
        public int Line { get; }
        public int CharPos { get; }
        public string FileName { get; }
        public string Message { get; }
        public PrettyException Cause { get; set; }

        public PrettyException() { }

        public PrettyException(
            string message,
            PrettyException cause = null,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "",
            [CallerFilePath] string path = "")
        {
            throw new PrettyException(line, -1, path + "::" + member, message, cause);
        }
        
        public PrettyException(
            int line,
            int charpos,
            string file,
            string message = "No message provided",
            PrettyException cause = null
            ) : base($"SFLang.Exceptions.PrettyException >>> Dumps {message}", cause)
        {
            Line = line;
            CharPos = charpos;
            FileName = file;
            Message = message;
            Cause = cause;
        }

        public void PrettyPrint()
        {
            Console.WriteLine($"> ${GetType().FullName} in file {FileName} <{Line}::{CharPos}> ${Message}");
            Cause?.PrettyPrint();
        }
    }
}