using SharpFunction.Addons.Skyblock;
using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents player skull item. This is mostly just item nbt
    /// </summary>
    public sealed class PlayerSkull
    {
        /// <summary>
        /// Owner of skull to be displayed. Can also contain custom texture nbt data
        /// </summary>
        public string SkullOwner { get; set; }
        /// <summary>
        /// Description of the skull. None by default.
        /// </summary>
        public AdvancedDescription ItemDescripion { get; set; } = null;
        /// <summary>
        /// Name of skull to be shown. None by default.
        /// </summary>
        public RawText ItemName { get; set; } = null;
        /// <summary>
        /// Initialize new player skull class instance
        /// </summary>
        /// <param name="skullOwner">Skull owner. Can also be custom texture nbt data.</param>
        public PlayerSkull(string skullOwner)
        {
            SkullOwner = skullOwner;
        }
        /// <summary>
        /// Generates give command to get the skull and compiles it
        /// </summary>
        /// <returns>Generated and compiled give command</returns>
        public Give GenerateCommand()
        {
            Give give = new(SimpleSelector.@p);
            Item item = new("player_head");
            ItemNBT nbt = new();
            nbt.Display = new();
            if (ItemName is not null) nbt.Display.AddName(ItemName);
            else
            {
                RawText n = new();
                n.AddField("SF Player Head");
            }
            if (ItemDescripion is not null)
            {
                RawText lore = new();
                foreach (SuperRawText line in ItemDescripion.Lines)
                {
                    lore.AddField(line);
                }
                nbt.Display.AddLore(lore);
            }
            else
            {
                RawText filler = new();
                filler.AddField("SharpFunction generated description");
                nbt.Display.AddLore(filler);
            }
            nbt.HideFlags = 31;
            nbt.Unbreakable = true;
            nbt.CustomTags = SkullOwner.StartsWith("{Id:[") ? $"SkullOwner: {SkullOwner}" : $"SkullOwner: \"{SkullOwner}\"";
            item.NBT = nbt;
            give.Compile(item, 1);
            return give;
        }
        /// <summary>
        /// Generates the command and compiles it into string
        /// </summary>
        /// <returns>Compiled give command to obtain this item</returns>
        public string Compile()
        {
            Give g = GenerateCommand();
            return g.Compiled;
        }
    }
}
