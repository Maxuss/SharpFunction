using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Exceptions
{
    /// <summary>
    /// Thrown when initialization did not yet began but post-init method was invoked
    /// </summary>
    public class InitializationException : SFException
    {
        /// <inheritdoc cref="SFException()"/>
        public InitializationException() : base($"You can not access this field before initialization!") { }
        /// <inheritdoc cref="SFException(string)"/>
        public InitializationException(string fieldName) : base($"You can not access field {fieldName} before initialization!") { }
        /// <inheritdoc cref="SFException(string, Exception)"/>
        public InitializationException(string fieldName, Exception innerException) : base($"You can not access field {fieldName} before initialization!", innerException) { }

    }
}
