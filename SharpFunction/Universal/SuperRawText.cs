using System.Collections.Generic;
using System.Linq;

namespace SharpFunction.Universal
{
    /// <summary>
    ///     Represents a single line of raw json text containing more than one formatting code.
    /// </summary>
    public sealed class SuperRawText
    {
        internal List<string> Deprecated = new();

        private readonly List<string> lines = new();

        /// <summary>
        ///     Compiled line of SRText
        /// </summary>
        public string Line { get; private set; }

        /// <summary>
        ///     Appends pre-baked json text formatting to line
        /// </summary>
        /// <param name="text">Text with field defined</param>
        public SuperRawText Append(RawText text)
        {
            lines.Add(text._lines[0]);
            Deprecated.Add(text._deprecated[0]);
            return this;
        }

        /// <summary>
        ///     Appends raw formatting to line
        /// </summary>
        /// <param name="text">Text to apped</param>
        /// <param name="color">Color of text</param>
        /// <param name="format">Formatting of text</param>
        /// <param name="formattings">Extra formattings of text</param>
        public SuperRawText Append(string text, Color color = Color.White,
            RawTextFormatting format = RawTextFormatting.Straight, params RawTextFormatting[] formattings)
        {
            RawText txt = new();
            txt.AddField(text, color, format, formattings);
            lines.Add(txt._lines[0]);
            Deprecated.Add(txt._deprecated[0]);
            return this;
        }

        /// <summary>
        ///     Bakes the formatting string
        /// </summary>
        /// <returns>Compiled string of SRT</returns>
        public string Compile()
        {
            var a = string.Empty;
            foreach (var line in lines) a += !lines.Last().Equals(line) ? $"{line}," : $"{line}";
            Line = $"[{a}]";
            return Line;
        }
    }
}