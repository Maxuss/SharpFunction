using System;

namespace SharpFunction.Universal
{
    /// <summary>
    ///     Class that helps with armor related stuff
    /// </summary>
    public static class ArmorHelper
    {
        /// <summary>
        ///     Parses the hex to int
        /// </summary>
        /// <param name="hex">Hexadecimal string</param>
        /// <returns>Parsed hex int</returns>
        public static int ParseHex(string hex)
        {
            hex = hex.StartsWith("#") ? hex.Remove(0, 1) : hex;
            var r = Convert.ToUInt32(hex.Substring(0, 2), 16);
            var g = Convert.ToUInt32(hex.Substring(2, 2), 16);
            var b = Convert.ToUInt32(hex.Substring(4, 2), 16);
            var ret = (int) (b + 256 * g + 65536 * r);
            return ret;
        }
    }
}