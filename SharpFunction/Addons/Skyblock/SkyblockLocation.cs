using System.Collections.Generic;
using SharpFunction.Universal;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    ///     Represents a skyblock location
    /// </summary>
    public sealed class SkyblockLocation
    {
        /// <summary>
        ///     Name of location
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Things to do at this location
        /// </summary>
        public List<string> WhatToDo { get; set; } = new();

        /// <summary>
        ///     Compiles the tellraw command to view the message
        /// </summary>
        /// <returns>Compiled tellraw command with locations enter message</returns>
        public string Compile()
        {
            var fb = "--------------------------------------------\\n";
            var filler = new SuperRawText().Append(fb, Color.Green, RawTextFormatting.Bold);
            SuperRawText nz = new();
            nz.Append(" NEW ZONE", Color.Green, RawTextFormatting.Bold);
            nz.Append("", Color.Reset);
            nz.Append(" - ", Color.Gray);
            nz.Append(Name + "\\n", Color.Aqua, RawTextFormatting.Bold);
            SuperRawText e = new();
            e.Append("\\n");
            RawText full = new();
            full.AddField(filler).AddField(nz).AddField(e);
            foreach (var t in WhatToDo)
            {
                SuperRawText tr = new();
                tr.Append($" {SkyblockHelper.ARR}", Color.DarkGray);
                tr.Append($" {t}\\n", Color.Yellow);
                full.AddField(tr);
            }

            full.AddField(e).AddField(filler);
            var f = full.RawJSON.Replace("'", "").Replace("[", "").Replace("]", "");
            var cmd = $"/tellraw @p [\"\",{f}]";
            return cmd;
        }
    }
}