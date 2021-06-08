﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents a single line of raw json text containing more than one formatting code.
    /// </summary>
    public sealed class SuperRawText
    {
        /// <summary>
        /// Compiled line of SRText
        /// </summary>
        public string Line { get; private set; }

        private List<string> lines = new List<string>();

        /// <summary>
        /// Appends pre-baked json text formatting to line
        /// </summary>
        /// <param name="text">Text with field defined</param>
        public void Append(RawText text)
        {
            lines.Add(text._lines[0]);
        }

        /// <summary>
        /// Appends raw formatting to line
        /// </summary>
        /// <param name="text">Text to apped</param>
        /// <param name="color">Color of text</param>
        /// <param name="format">Formatting of text</param>
        /// <param name="formattings">Extra formattings of text</param>
        public void Append(string text, Color color=Color.White, RawTextFormatting format=RawTextFormatting.Straight, params RawTextFormatting[] formattings)
        {
            RawText txt = new();
            txt.AddField(text, color, format, formattings);
            lines.Add(txt._lines[0]);
        }

        /// <summary>
        /// Bakes the formatting string
        /// </summary>
        /// <returns>Compiled string</returns>
        public string Compile()
        {
            string a = string.Empty;
            foreach(string line in lines)
            {
                a += !lines.Last().Equals(line) ? $"{line}," : $"{line}";
            }
            Line = $"[{a}]";
            return Line;
        }
    }
}