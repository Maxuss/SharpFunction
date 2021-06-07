using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;
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

        public void AddField(string text, Color color = Color.White, RawTextFormatting format = RawTextFormatting.None)
        {
            string clr = EnumHelper.GetStringValue(color);
            string frm = EnumHelper.GetStringValue(format);
            string full = $@"{{""text"": ""{text}"", {clr}, {frm}}}";
            _lines.Add(full);
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
        [EnumValue(@"")] None
    }
}
