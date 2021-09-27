using System;

namespace SharpFunction.Exceptions
{
    public sealed class InvalidSelectorParameters : SFException
    {
        /// <inheritdoc cref="Exception()" />
        public InvalidSelectorParameters() : base(
            "Selector parameters are typed wrongly! You should input them as <key1>, <value1>, <key2>, <value2> etc.")
        {
        }

        /// <inheritdoc cref="Exception(string)" />
        public InvalidSelectorParameters(string message) : base(
            $"Selector parameters are typed wrongly! You should input them as <key1>, <value1>, <key2>, <value2> etc., extra message: {message}")
        {
        }

        /// <inheritdoc cref="Exception(string, Exception)" />
        public InvalidSelectorParameters(string message, Exception innerException) : base(
            $"Selector parameters are typed wrongly! You should input them as <key1>, <value1>, <key2>, <value2> etc., extra message: {message}",
            innerException)
        {
        }
    }
}