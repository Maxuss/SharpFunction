using System;
using System.Collections.Generic;
using System.Linq;
using static SharpFunction.Universal.EnumHelper;
using static SharpFunction.Universal.NullChecker;

namespace SharpFunction.Universal
{
    /// <summary>
    ///     Represents raw json text used for give/tellraw commands
    /// </summary>
    public sealed class RawText
    {
        internal List<string> _deprecated = new();

        internal List<string> _lines = new();

        /// <summary>
        ///     Represents generated raw json. Returns empty string if nothing is specified
        /// </summary>
        public string RawJSON
        {
            get
            {
                var i = _lines.Count == 0 ? "" : string.Join("','", _lines);
                return $"['{i}']";
            }
        }

        /// <summary>
        ///     Adds a field to raw text
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        /// <param name="color">Color of the text</param>
        /// <param name="format">Formatting of the text</param>
        /// <param name="extraFormat">Extra formattings of the text</param>
        public RawText AddField(string text, Color color = Color.White,
            RawTextFormatting format = RawTextFormatting.None, params RawTextFormatting[] extraFormat)
        {
            var clr = color.GetStringValue();
            var frm = format.GetStringValue();
            var deprecatedColor = color.GetDeprecatedValue();
            deprecatedColor = string.IsNullOrEmpty(deprecatedColor) ? "" : deprecatedColor;
            var deprecatedFormatting = format.GetDeprecatedValue();
            deprecatedFormatting = string.IsNullOrEmpty(deprecatedFormatting) ? "" : $"{deprecatedFormatting}";
            var extraF = string.Empty;
            var deprecatedExtraF = string.Empty;
            if (extraFormat is not null)
            {
                var listed = extraFormat.ToList();
                foreach (var fr in listed)
                {
                    var df = fr.GetDeprecatedValue();
                    deprecatedExtraF = $"{deprecatedExtraF}{(string.IsNullOrEmpty(df) ? "" : df)}";
                    extraF =
                        listed.IndexOf(listed.Last()) == listed.IndexOf(fr) 
                            ? $"{extraF}{fr.GetStringValue()}" 
                            : $"{extraF}{fr.GetStringValue()},";
                }
            }

            var deprecatedString =
                $"{deprecatedColor}{deprecatedFormatting}{deprecatedExtraF}{text}".Replace(";", "");
            _deprecated.Add(deprecatedString);
            string full;
            text = text.Replace(";", "");
            if (!IsEmpty(extraF) && !IsEmpty(frm)) full = $@"{{""text"": ""{text}"", {clr}, {frm}, {extraF}}}";
            else if (IsEmpty(extraF) && !IsEmpty(frm)) full = $@"{{""text"": ""{text}"", {clr}, {frm}}}";
            else if (!IsEmpty(extraF) && IsEmpty(frm)) full = $@"{{""text"": ""{text}"", {clr}, {extraF}}}";
            else full = $@"{{""text"": ""{text}"", {clr}}}".Replace(";", "");
            _lines.Add(full);
            return this;
        }

        /// <summary>
        ///     Add field from super raw text
        /// </summary>
        /// <param name="srt">Pre-Made SuperRawText</param>
        public RawText AddField(SuperRawText srt)
        {
            var compiled = srt.Compile();
            _lines.Add(compiled);
            _deprecated.Add(string.Join("", srt.Deprecated));
            return this;
        }
    }

    /// <summary>
    ///     Represents options for text formatting
    /// </summary>
    public enum RawTextFormatting
    {
        [DeprecatedValue("&l")] [EnumValue(@"""bold"": true")]
        Bold,

        [DeprecatedValue("&o")] [EnumValue(@"""italic"": true")]
        Italic,

        [DeprecatedValue("&k")] [EnumValue(@"""obfuscated"": true")]
        Obfuscated,

        [DeprecatedValue("&m")] [EnumValue(@"""strikethrough"": true")]
        Strikethrough,

        [DeprecatedValue("&n")] [EnumValue(@"""underlined"": true")]
        Underlined,

        /// <summary>
        ///     Makes the text non-italic (italic by default)
        /// </summary>
        [DeprecatedValue(null)] [EnumValue(@"""italic"": false")]
        Straight,

        [DeprecatedValue(null)] [EnumValue("")]
        None
    }
}