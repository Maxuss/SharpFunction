using System.Collections.Generic;
using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    ///     Represents a skyblock NPC. Currently can only make message commands.
    /// </summary>
    public sealed class SkyblockNPC
    {
        /// <summary>
        ///     Name of NPC
        /// </summary>
        public string Name { get; set; } = "!undefined!";

        /// <summary>
        ///     Color of NPC's name
        /// </summary>
        public Color NameColor { get; set; } = Color.White;

        /// <summary>
        ///     Color of NPC's replics
        /// </summary>
        public Color ReplicColor { get; set; } = Color.Yellow;

        /// <summary>
        ///     Type of entity to be summoned
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        ///     NBT of entity to be summoned. null by default
        /// </summary>
        public EntityNBT? EntityNBT { get; set; }

        /// <summary>
        ///     Generates replics of NPC
        /// </summary>
        /// <param name="replics">Replics for NPC to say</param>
        /// <returns>Generated replics of NPC</returns>
        public List<string> GenerateReplics(params string[] replics)
        {
            List<string> ds = new();
            foreach (var r in replics)
            {
                var tl = new Tellraw(SimpleSelector.Nearest);
                var m = new SuperRawText().Append("[NPC] ", Color.Yellow).Append($" {Name}: ", NameColor)
                    .Append(r, ReplicColor);
                tl.Compile(m);
                ds.Add(tl.Compiled);
            }

            return ds;
        }

        /// <summary>
        ///     Generates command to summon the NPC
        /// </summary>
        /// <returns>Generated command for summoning the NPC</returns>
        public string GenerateEntity()
        {
            EntityNBT nbt;
            if (EntityNBT is null)
            {
                nbt = new EntityNBT();
                nbt.CustomNameVisible = true;
                nbt.CustomName = new SuperRawText();
                nbt.CustomName
                    .Append(Name, Color.Yellow)
                    .Append(" CLICK", Color.Gold, RawTextFormatting.Straight, RawTextFormatting.Bold);
                nbt.NoAI = true;
                nbt.Invulnerable = true;
            }
            else
            {
                nbt = EntityNBT;
            }

            Entity e = new(EntityType, nbt.Compile());
            Summon s = new();
            s.Compile(e, SimpleVector.Current);
            return s.Compiled;
        }
    }
}