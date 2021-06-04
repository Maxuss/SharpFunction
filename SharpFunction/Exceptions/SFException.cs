using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Exceptions
{
    /// <summary>
    /// Represents main class for SharpFunction exceptions inheritance
    /// </summary>
    public abstract partial class SFException : Exception
    {
        public SFException() : base() { }
        public SFException(string message) : base(message) { }
        public SFException(string message, Exception innerException) : base(message, innerException) { }
    }
}
