using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Exceptions
{
    /// <summary>
    /// Occurs when function is not specified
    /// </summary>
    public sealed class FunctionNotSpecifiedException : SFException
    {
        /// <inheritdoc cref="Exception()"/>
        public FunctionNotSpecifiedException() : base($"Specified function does not exist.") { }
        /// <inheritdoc cref="Exception(string)"/>
        public FunctionNotSpecifiedException(string message) : base($"Specified function does not exist, extra message: {message}") { }
        /// <inheritdoc cref="Exception(string, Exception)"/>
        public FunctionNotSpecifiedException(string message, Exception innerException) : base($"Specified function does not exist, extra message: {message}", innerException) { }
    }
}
