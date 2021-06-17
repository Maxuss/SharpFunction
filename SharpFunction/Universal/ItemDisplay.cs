using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents display data for item
    /// </summary>
    public sealed class ItemDisplay
    {
        /// <summary>
        /// Returns NOT full item display json string
        /// </summary>
        public string DisplayJSON
        {
            get
            {
                return string.IsNullOrEmpty(_colr) ? $"display: {{Name: '{_name}', Lore: {_lore}}}" : $"display: {{Name: '{_name}', Lore: {_lore}, color: {_colr} }}";
            }
        }


        private string _lore;
        private string _name;
        private string _colr = "";
        /// <summary>
        /// Initialize an item display json data
        /// </summary>
        public ItemDisplay() { }

        /// <summary>
        /// Adds a name for item's display
        /// </summary>
        /// <param name="name">Raw text with first line field being name</param>
        public void AddName(RawText name)
        {
            _name = name._lines[0];
        }

        /// <summary>
        /// Adds a lore for item's display
        /// </summary>
        /// <param name="lore">Raw text with field lines representing lore</param>
        public void AddLore(RawText lore)
        {
            if (lore is not null)
            {
                _lore = lore.RawJSON;
            }
            else
            {
                _lore = "[]";
            }
        }
        /// <summary>
        /// Adds color to leather item
        /// </summary>
        /// <param name="color">Parsed hex value of color</param>
        public void AddColor(int color)
        {
            _colr = $"{color}";
        }
    }
}
