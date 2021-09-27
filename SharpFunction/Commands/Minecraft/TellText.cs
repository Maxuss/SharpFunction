using System;
using System.Text.RegularExpressions;
using SharpFunction.Universal;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Text used by Tellraw
    /// </summary>
    internal sealed class TellText
    {
        internal string Compiled;

        internal TellText(SuperRawText srt)
        {
            var tmp = srt.Compile();
            tmp = Regex.Replace(tmp, @"(\[)(?=(?:[^""]|""[^""]*"")*$)", "");
            Console.WriteLine(tmp);
            Compiled = $@"["""",{tmp}";
        }
    }
}