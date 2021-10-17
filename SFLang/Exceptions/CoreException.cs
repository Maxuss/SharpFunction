using System;
using System.Runtime.CompilerServices;

namespace SFLang.Exceptions
{
    public class CoreException : Exception
    {
        public CoreException()
        {
        }

        public CoreException(
            string message,
            CoreException cause = null,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "",
            [CallerFilePath] string path = "")
        {
            throw new CoreException(line, -1, path + "::" + member, message, cause);
        }

        public CoreException(
            int line,
            int charpos,
            string file,
            string message = "No message provided",
            CoreException cause = null
        ) : base($"SFLang.Exceptions.CoreException >>> Dumps {message}", cause)
        {
            Line = line;
            CharPos = charpos;
            FileName = file;
            Message = message;
            Cause = cause;
        }

        public int Line { get; }
        public int CharPos { get; }
        public string FileName { get; }
        public string Message { get; }
        public CoreException Cause { get; set; }

        public void PrettyPrint()
        {
            Console.WriteLine($"> ${GetType().FullName} in file {FileName} <{Line}::{CharPos}> ${Message}");
            Cause?.PrettyPrint();
        }
    }
}