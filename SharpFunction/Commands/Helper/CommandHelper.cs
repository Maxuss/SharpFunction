using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Helper
{
    /// <summary>
    /// Helper for command
    /// </summary>
    public static class CommandHelper
    {
        public static string BoolToString(bool @bool)
        {
            switch(@bool)
            {
                case true: return "true";
                case false: return "false";
            }
        }
    }
}
