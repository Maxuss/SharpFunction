namespace SharpFunction.Universal
{
    /// <summary>
    ///     Helps for different actions with vectors and coordinates
    /// </summary>
    public static class VectorHelper
    {
        /**
         * <summary>Converts decimal to minecraft decimal</summary>
         * <param name="value">Decimal to be coverted</param>
         * <returns>Value as minecraft float string</returns>
         */
        public static string DecimalToMCString(decimal value)
        {
            return $"{value}".Replace(",", ".");
        }

        /**
         * <summary>Converts double to minecraft decimal</summary>
         * <param name="value">Double to be coverted</param>
         * <returns>Value as minecraft float string</returns>
         */
        public static string DecimalToMCString(double value)
        {
            return $"{value}".Replace(",", ".");
        }

        /**
         * <summary>Converts float to minecraft decimal</summary>
         * <param name="value">Float to be coverted</param>
         * <returns>Value as minecraft float string</returns>
         */
        public static string DecimalToMCString(float value)
        {
            return $"{value}".Replace(",", ".");
        }
    }
}