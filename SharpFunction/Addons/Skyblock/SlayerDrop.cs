using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;
using static SharpFunction.Addons.Skyblock.SkyblockEnumHelper;
using SharpFunction.API;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    /// Represents an item that is dropped from slayer
    /// </summary>
    public sealed class SlayerDrop
    {
        /// <summary>
        /// Drop rarity of item
        /// </summary>
        public DropRarity DropRarity { get; set; }
        /// <summary>
        /// Item to drop
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// ID of item to give
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Rarity of item
        /// </summary>
        public ItemRarity Rarity { get; set; }
        /// <summary>
        /// Slayer Level required to obtain the drop
        /// </summary>
        public int RequiredLVL { get; set; } = 0;

        public Dictionary<string, string> TierAmounts = new()
        {
            { "Tier I", ""},
            { "Tier II", "" },
            { "Tier III", "" },
            { "Tier IV", "" },
            { "Tier V", "" },
            { "Tier VI", "" }
        };
        /// <summary>
        /// Minimum tier for this item to drop
        /// </summary>
        public int MinimumTier { get; set; }

        private Dictionary<int, string> ts = new()
        {
            {1, "Tier I"},
            {2, "Tier II"},
            {3, "Tier III"},
            {4, "Tier IV"},
            {5, "Tier V"},
            {6, "Tier VI"}
        };

        private Dictionary<int, Color> tc = new()
        {
            {1, Color.Green},
            {2, Color.Yellow},
            {3, Color.Red},
            {4, Color.DarkRed},
            {5, Color.DarkPurple},
            {6, Color.Blue}
        };

        /// <summary>
        /// Create new slayer drop
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="drop">Drop rarity of item</param>
        /// <param name="rarity">Rarity of item</param>
        /// <param name="id">ID of item to give</param>
        public SlayerDrop(string name, DropRarity drop, ItemRarity rarity, string id)
        {
            DropRarity = drop;
            ItemName = name;
            Rarity = rarity;
            ID = id;
        }

        /// <summary>
        /// Generates slayer drop from pre-existing item
        /// </summary>
        /// <param name="item">Item to steal data from</param>
        /// <param name="rarity">Rarity of item</param>
        public SlayerDrop(SkyblockItem item, DropRarity rarity)
        {
            DropRarity = rarity;
            ItemName = item.DisplayName;
            Rarity = item.Rarity;
            ID = item.ID;
            
        }

        /// <summary>
        /// Generates command to give the item
        /// </summary>
        /// <returns>Generated command</returns>
        public Give Generate()
        {
            RawText t = new();
            foreach(string tier in TierAmounts.Keys)
            {
                string tr = TierAmounts[tier];
                if (tr != "")
                {
                    SuperRawText tx = new();
                    tx.Append($"{tier} amounts: ", Color.Gray);
                    tx.Append($"{TierAmounts[tier]}", Color.Green);
                    t.AddField(tx);
                }
            }
            t.AddField(" ");
            SuperRawText min = new();
            min.Append("Minimum: ", Color.Gray);
            min.Append($"{ts[MinimumTier]}", tc[MinimumTier]);
            t.AddField(min);
            SuperRawText odd = new();
            odd.Append("Odds: ", Color.Gray);
            odd.Append($"{EnumHelper.GetStringValue(DropRarity)}", SkyblockEnumHelper.GetRarityColor(DropRarity));
            odd.Append($" {SkyblockEnumHelper.GetSlayerMessage(DropRarity)}", Color.Gray);
            t.AddField(odd);
            if(RequiredLVL != 0)
            {
                t.AddField("");
                SuperRawText srt = new();
                srt.Append("Required LVL: ", Color.Gray);
                srt.Append($"{RequiredLVL}", Color.Yellow);
                t.AddField(srt);
            }
            RawText n = new();
            n.AddField($"{ItemName}", SkyblockEnumHelper.GetRarityColor(Rarity), RawTextFormatting.Straight);
            ItemDisplay dis = new();
            dis.AddLore(t);
            dis.AddName(n);
            ItemNBT nbt = new();
            nbt.Display = dis;
            nbt.HideFlags = 31;
            Item i = new(ID, nbt);
            Give g = new(SimpleSelector.@p);
            g.Compile(i, 1);
            return g;
        }

        /// <summary>
        /// Generates and compiles the command, returning the string
        /// </summary>
        /// <returns>Compiled string command</returns>
        public string Compile()
        {
            Give g = Generate();
            return g.Compiled;
        }
    }

    /// <summary>
    /// Represents rarity of slayer drop
    /// </summary>
    public enum DropRarity
    {
        /// <summary>
        /// 100%
        /// </summary>
        [RarityColor(Color.Green)] [SlayerRarity("")] [EnumValue("Guaranteed")] Guaranteed,
        /// <summary>
        /// 20%
        /// </summary>
        [RarityColor(Color.Blue)] [SlayerRarity("(20%)")] [EnumValue("Occasional")] Occasional,
        /// <summary>
        /// 5%
        /// </summary>
        [RarityColor(Color.Aqua)] [SlayerRarity("(5%)")] [EnumValue("Rare")] Rare,
        /// <summary>
        /// 1%
        /// </summary>
        [RarityColor(Color.DarkPurple)] [SlayerRarity("(1%)")] [EnumValue("Extraordinary")] Extraordinary,
        /// <summary>
        /// lesser than 1%
        /// </summary>
        [RarityColor(Color.LightPurple)] [SlayerRarity("")] [EnumValue("Pray RNGesus")] PrayRNGesus,
        /// <summary>
        /// lesser than 0.01%
        /// </summary>
        [RarityColor(Color.Red)] [SlayerRarity("")] [EnumValue("RNGesus Incarnate")] RNGesusIncarnate
    }
}
