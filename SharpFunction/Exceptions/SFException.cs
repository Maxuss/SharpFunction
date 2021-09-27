using System;

namespace SharpFunction.Exceptions
{
    /// <summary>
    ///     Represents main class for SharpFunction exceptions inheritance
    /// </summary>
    public class SFException : Exception
    {
        public SFException()
        {
        }

        public SFException(string message) : base(message)
        {
        }

        public SFException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}