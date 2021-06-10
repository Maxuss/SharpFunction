using System;
using System.Collections.Generic;
using SharpFunction.Universal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    /// Represents ability of slayer
    /// </summary>
    public sealed class SlayerAbility
    {
        /// <summary>
        /// Name of the ability
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of ability
        /// </summary>
        public AdvancedDescription Description { get; set; } = new();
        /// <summary>
        /// Color of ability
        /// </summary>
        public Color AbilityColor { get; set; }
        /// <summary>
        /// Represents percents of slayer boss hp for this ability to trigger
        /// </summary>
        #nullable enable
        public int[]? PercentsToHappen { get; set; } = null;
#nullable disable

        /// <summary>
        /// Compiles the slayer ability 
        /// </summary>
        /// <returns>Compiled Slayer ability</returns>
        public RawText Compile(RawText r)
        {
            r.AddField($"{Name}", AbilityColor, RawTextFormatting.Straight);
            if(PercentsToHappen is not null)
            {
                string per = "At ";
                foreach(int percent in PercentsToHappen)
                {
                    per += PercentsToHappen.Last().Equals(percent) ? $"{percent}% HP" : $"{percent}%/";
                }
                r.AddField(per, Color.DarkGray, RawTextFormatting.Straight);
            }
            foreach (SuperRawText line in Description.Lines)
            {
                r.AddField(line);
            }
            return r;
        }
    }
}
