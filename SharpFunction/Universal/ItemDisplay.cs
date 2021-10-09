using System.Linq;
using System.Text.RegularExpressions;

namespace SharpFunction.Universal
{
    /// <summary>
    ///     Represents display data for item
    /// </summary>
    public sealed class ItemDisplay
    {
        private string colr = "";
        
        private string lore;
        private string name;

        /// <summary>
        ///     Returns NOT full item display json string
        /// </summary>
        public string DisplayJson =>
            string.IsNullOrEmpty(colr)
                ? $"display: {{Name: '{name}', Lore: {lore}}}"
                : $"display: {{Name: '{name}', Lore: {lore}, color: {colr} }}";

        /// <summary>
        ///     Adds a name for item's display
        /// </summary>
        /// <param name="itemName">Raw text with first line field being name</param>
        /// <param name="legacy">Whether the name should be in legacy format</param>
        public void AddName(RawText itemName, bool legacy=false)
        {
            if (legacy)
            {
                name = $"{{\"text\":\"{itemName._deprecated[0].Replace("&", "\\\\u00A7")}\"}}";
                return;
            }
            name = itemName._lines[0];
        }

        /// <summary>
        /// Adds a lore for item's display
        /// </summary>
        /// <param name="itemLore">Raw text with field lines representing lore</param>
        /// <param name="legacy">Whether the lore should be in legacy format</param>
        public void AddLore(RawText itemLore, bool legacy=false)
        {
            if (legacy)
            {
                var depr = itemLore._deprecated;
                var enumerable = depr.Select(s =>
                {
                    var ts = $"{{\"text\":\"{s ?? "&cnull"}\"}}";
                    return ts.Replace("&", "\\\\u00A7");
                });
                var values = enumerable as string[] ?? enumerable.ToArray();
                var i = !values.Any() ? "" : string.Join("','", values);
                lore = $"['{i}']";
                return;
            }
            lore = itemLore is not null ? itemLore.RawJSON : "[]";
        }

        /// <summary>
        /// Adds color to leather item
        /// </summary>
        /// <param name="color">Parsed hex value of color</param>
        public void AddColor(int color)
        {
            colr = $"{color}";
        }
    }
}