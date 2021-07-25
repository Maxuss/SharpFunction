using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    /// Represents a sea creature entry
    /// </summary>
    public sealed class SkyblockSeaCreature
    {
        /// <summary>
        /// Name of the mob to be spawned
        /// </summary>
        public string Mob { get; set; }
        /// <summary>
        /// Name of the sea creature
        /// </summary>
        public RawText Name { get; set; }
        /// <summary>
        /// Level of sea creature
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Health of sea creature
        /// </summary>
        public int Health { get; set; }
        /// <summary>
        /// What the creature drops
        /// </summary>
        public RawText[] Drops { get; set; }
        /// <summary>
        /// Requirements to fish the sea creature
        /// </summary>
        public RawText[] Requirements { get; set; }
        /// <summary>
        /// Item representing the mob
        /// </summary>
        public string MobItem { get; set; }
        /// <summary>
        /// Rarity of the mob
        /// </summary>
        public ItemRarity MobRarity { get; set; }
        /// <summary>
        /// Equipment of mob
        /// </summary>
        public ArmorItems Equipment { get; set; }
        /// <summary>
        /// Unformatted name of entity
        /// </summary>
        public string RawName { get; set; }
        /// <summary>
        /// Initialize a new sea creature
        /// </summary>
        /// <param name="mob">Mob to be spawned</param>
        /// <param name="item">Item representing the mob</param>
        /// <param name="lvl">Level of the mob</param>
        /// <param name="hp">HP of the mob</param>
        /// <param name="name">Name of mob</param>
        /// <param name="rawName">Raw name of entity</param>
        /// <param name="rarity">Rarity of the mob</param>
        public SkyblockSeaCreature(string mob, string item, int lvl, int hp, RawText name, string rawName, ItemRarity rarity)
        {
            Mob = mob;
            MobItem = item;
            Level = lvl;
            Health = hp;
            Name = name;
            MobRarity = rarity;
        }
        /// <summary>
        /// Generates the item representing sea creature
        /// </summary>
        /// <returns>Generated command to give the item</returns>
        public string GenerateItem()
        {
            Item item = new(MobItem);
            ItemNBT nbt = new();
            ItemDisplay d = new();

            RawText name = new();
            SuperRawText n = new();
            n.Append($"[Lvl {Level}] ", Color.Gray);
            n.Append(Name);
            name.AddField(n);
            d.AddName(name);

            RawText de = new();
            de.AddField(MobRarity.GetStringValue(), MobRarity.GetRarityColor(), RawTextFormatting.Straight, RawTextFormatting.Bold);
            if (!NullChecker.IsNull(Drops))
            {
                de.AddField("");
                de.AddField("Drops: ", Color.Red, RawTextFormatting.Straight);
                foreach (RawText drop in Drops)
                {
                    SuperRawText dr = new();
                    dr.Append("- ", Color.Gray);
                    dr.Append(drop);
                    de.AddField(dr);
                }
            }
            if (!NullChecker.IsNull(Requirements))
            {
                de.AddField("");
                de.AddField("Requirements: ", Color.Red, RawTextFormatting.Straight);
                foreach (RawText req in Requirements)
                {
                    SuperRawText dr = new();
                    dr.Append("- ", Color.Gray);
                    dr.Append(req);
                    de.AddField(dr);
                }
            }

            d.AddLore(de);

            nbt.HideFlags = 31;
            nbt.Unbreakable = true;
            nbt.Display = d;

            item.NBT = nbt;

            Give g = new(SimpleSelector.@p);
            g.Compile(item, 1);
            return g.Compiled;
        }
        /// <summary>
        /// Generates the mob representing sea creature
        /// </summary>
        /// <returns>Generated command to summon the mob</returns>
        public string GenerateMob()
        {
            SkyblockEntity e = new(Mob, RawName, Health, Level);
            e.CurrentHP = Health;
            e.Equipment = Equipment;
            return e.Compile();
        }
    }
}
