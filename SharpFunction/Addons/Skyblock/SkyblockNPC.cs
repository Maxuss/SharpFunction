using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Universal;
using SharpFunction.API;
using SharpFunction.Commands.Minecraft;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    /// Represents a skyblock NPC. Currently can only make message commands.
    /// </summary>
    public sealed class SkyblockNPC
    {
        /// <summary>
        /// Name of NPC
        /// </summary>
        public string Name { get; set; } = "!undefined!";
        /// <summary>
        /// Color of NPC's name
        /// </summary>
        public Color NameColor { get; set; } = Color.White;
        /// <summary>
        /// Color of NPC's replics
        /// </summary>
        public Color ReplicColor { get; set; } = Color.Yellow;
        /// <summary>
        /// Generates replics of NPC
        /// </summary>
        /// <param name="replics">Replics for NPC to say</param>
        /// <returns>Generated replics of NPC</returns>
        public List<string> GenerateReplics(params string[] replics)
        {
            List<string> ds = new();
            foreach(string r in replics)
            {
                var tl = new Tellraw(SimpleSelector.@p);
                SuperRawText m = new SuperRawText().Append("[NPC] ", Color.Yellow).Append($" {Name}: ", NameColor).Append(r, ReplicColor);
                tl.Compile(m);
                ds.Add(tl.Compiled);
            }
            return ds;
        }
    }
}
