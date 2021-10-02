using System;

namespace SFLang.Exceptions
{
    public class PrettyException : Exception
    {
        public int Line { get; }
        public int CharPos { get; }
        public string FileName { get; }
        public string Message { get; }
        public PrettyException Cause { get; set; }

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
            Console.WriteLine($"> ${GetType().FullName} F {FileName} <{Line}::{CharPos}> ${Message}");
            Cause?.PrettyPrint();
        }
    }
}