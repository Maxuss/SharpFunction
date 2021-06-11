using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Helper
{
    /// <summary>
    /// Helper for commands
    /// </summary>
    internal static class CommandHelper
    {
        public static string BoolToString(bool @bool)
        {
            switch(@bool)
            {
                case true: return "true";
                case false: return "false";
            }
        }

        public static string BoolToByte(bool @bool)
        {
            switch (@bool)
            {
                case true: return "1b";
                case false: return "0b";
            }
        }

        public static bool ByteToBool(string @byte)
        {
            switch(@byte)
            {
                case "0": return false;
                case "1": return true;
                default: throw new FormatException(".sfmeta File is corrupted or written wrongly!");
            }
        }
    }
}
