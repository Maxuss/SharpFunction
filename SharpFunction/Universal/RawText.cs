using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;
using static SharpFunction.Universal.NullChecker;
namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents raw json text used for give/tellraw commands
    /// </summary>
    public sealed class RawText
    {
        /// <summary>
        /// Represents generated raw json. Returns empty string if nothing is specified
        /// </summary>
        public string RawJSON { 
            get
            {
                string i = _lines.Count() == 0 ? "" : string.Join("','", _lines);
                return $"['{i}']";
            }
        }

        internal List<string> _lines = new List<string>();
        /// <summary>
        /// Generate starting raw json
        /// </summary>
        public RawText() { }

        /// <summary>
        /// Adds a field to raw text
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        /// <param name="color">Color of the text</param>
        /// <param name="format">Formatting of the text</param>
        /// <param name="extraFormat">Extra formattings of the text</param>
        public void AddField(string text, Color color = Color.White, RawTextFormatting format = RawTextFormatting.None, params RawTextFormatting[] extraFormat)
        {
            string clr = EnumHelper.GetStringValue(color);
            string frm = EnumHelper.GetStringValue(format);
            string extraF = string.Empty;
            if(extraFormat is not null)
            {
                foreach (RawTextFormatting fr in extraFormat)
                {
                    if (extraFormat.Last().Equals(fr)) extraF += $"{EnumHelper.GetStringValue(fr)}";
                    else extraF += $"{EnumHelper.GetStringValue(fr)},";
                }
            }
            string full;
            if (!IsEmpty(extraF) && !IsEmpty(frm)) full = $@"{{""text"": ""{text}"", {clr}, {frm}, {extraF}}}";
            else if (IsEmpty(extraF) && !IsEmpty(frm)) full = $@"{{""text"": ""{text}"", {clr}, {frm}}}";
            else if (!IsEmpty(extraF) && IsEmpty(frm)) full = $@"{{""text"": ""{text}"", {clr}, {extraF}}}";
            else full = $@"{{""text"": ""{text}"", {clr}}}";
            _lines.Add(full);
        }
        /// <summary>
        /// Add field from super raw text
        /// </summary>
        /// <param name="srt">Pre-Made SuperRawText</param>
        public void AddField(SuperRawText srt)
        {
            string compiled = srt.Compile();
            _lines.Add(compiled);
        }

        /// <summary>
        /// Add field from pre-made raw text
        /// </summary>
        /// <param name="rt">RawText to add</param>
        public void AddField(RawText rt)
        {
            int l = rt.RawJSON.Length;
            string normal = rt.RawJSON.Replace("'", "");
            Console.WriteLine(normal);
            _lines.Add(normal);
        }
    }

    /// <summary>
    /// Represents options for text formatting
    /// </summary>
    public enum RawTextFormatting
    {
        [EnumValue(@"""bold"": true")] Bold,
        [EnumValue(@"""italic"": true")] Italic,
        [EnumValue(@"""obfuscated"": true")] Obfuscated,
        [EnumValue(@"""strikethrough"": true")] Strikethrough,
        [EnumValue(@"""underlined"": true")] Underlined,
        /// <summary>
        /// Makes the text non-italic (italic by default)
        /// </summary>
        [EnumValue(@"""italic"": false")] Straight,
        [EnumValue(@"")] None
    }
}
