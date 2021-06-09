using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Text used by Tellraw
    /// </summary>
    internal sealed class TellText
    {
        internal string Compiled;

        internal TellText(SuperRawText srt)
        {
            string tmp = srt.Compile();
            tmp = Regex.Replace(tmp, @"(\[)(?=(?:[^""]|""[^""]*"")*$)", "");
            Console.WriteLine(tmp);
            Compiled = $@"["""",{tmp}";
        }
    }
}
