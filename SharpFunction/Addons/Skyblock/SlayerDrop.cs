using System.Collections.Generic;
using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;
using static SharpFunction.Universal.EnumHelper;
using static SharpFunction.Addons.Skyblock.SkyblockEnumHelper;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    ///     Represents an item that is dropped from slayer
    /// </summary>
    public sealed class SlayerDrop
    {
        private readonly Dictionary<int, Color> tc = new()
        {
            {1, Color.Green},
            {2, Color.Yellow},
            {3, Color.Red},
            {4, Color.DarkRed},
            {5, Color.DarkPurple},
            {6, Color.Blue}
        };

        /// <summary>
        ///     Amount of drops per certain tier
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> TierAmounts = new()
        {
            {"Tier I", ""},
            {"Tier II", ""},
            {"Tier III", ""},
            {"Tier IV", ""},
            {"Tier V", ""},
            {"Tier VI", ""}
        };

        private readonly Dictionary<int, string> ts = new()
        {
            {1, "Tier I"},
            {2, "Tier II"},
            {3, "Tier III"},
            {4, "Tier IV"},
            {5, "Tier V"},
            {6, "Tier VI"}
        };


        /// <summary>
        ///     Create new slayer drop
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
        ///     Generates slayer drop from pre-existing item
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
        ///     Drop rarity of item
        /// </summary>
        public DropRarity DropRarity { get; set; }

        /// <summary>
        ///     Item to drop
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        ///     ID of item to give
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///     Rarity of item
        /// </summary>
        public ItemRarity Rarity { get; set; }

        /// <summary>
        ///     Slayer Level required to obtain the drop
        /// </summary>
        public int RequiredLVL { get; set; } = 0;

        /// <summary>
        ///     Minimum tier for this item to drop
        /// </summary>
        public int MinimumTier { get; set; }

        /// <summary>
        ///     Generates command to give the item
        /// </summary>
        /// <returns>Generated command</returns>
        public Give Generate()
        {
            RawText t = new();
            foreach (var tier in TierAmounts.Keys)
            {
                var tr = TierAmounts[tier];
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
            odd.Append($"{DropRarity.GetStringValue()}", DropRarity.GetRarityColor());
            odd.Append($" {DropRarity.GetSlayerMessage()}", Color.Gray);
            t.AddField(odd);
            if (RequiredLVL != 0)
            {
                t.AddField("");
                SuperRawText srt = new();
                srt.Append("Required LVL: ", Color.Gray);
                srt.Append($"{RequiredLVL}", Color.Yellow);
                t.AddField(srt);
            }

            RawText n = new();
            n.AddField($"{ItemName}", Rarity.GetRarityColor(), RawTextFormatting.Straight);
            ItemDisplay dis = new();
            dis.AddLore(t);
            dis.AddName(n);
            ItemNBT nbt = new();
            nbt.Display = dis;
            nbt.HideFlags = 31;
            Item i = new(ID, nbt);
            Give g = new(SimpleSelector.p);
            g.Compile(i);
            return g;
        }

        /// <summary>
        ///     Gets drop message to display
        /// </summary>
        /// <returns>Compiled /tellraw message</returns>
        public Tellraw GetDropMessage()
        {
            var tl = new Tellraw(SimpleSelector.p);
            DropMessage msg = new(DropRarity, ItemName, Rarity.GetRarityColor());
            if (msg is null)
            {
                SuperRawText empty = new();
                empty.Append("");
                tl.Compile(empty);
            }
            else
            {
                var final = msg.GetMessage();
                tl.Compile(final);
            }

            return tl;
        }

        /// <summary>
        ///     Generates and compiles the command, returning the string
        /// </summary>
        /// <returns>Compiled string command</returns>
        public string Compile()
        {
            var g = Generate();
            return g.Compiled;
        }
    }

    /// <summary>
    ///     Represents message to be displayed when item is dropped
    /// </summary>
    public sealed class DropMessage
    {
        public DropMessage(DropRarity rarity, string name, Color color)
        {
            ItemName = name;
            ItemColor = color;
            switch (rarity)
            {
                case DropRarity.Rare:
                    RarityMessage = "RARE DROP!";
                    RarityMessageColor = Color.Blue;
                    break;
                case DropRarity.Extraordinary:
                    RarityMessage = "VERY RARE DROP!";
                    RarityMessageColor = Color.DarkPurple;
                    break;
                case DropRarity.PrayRNGesus:
                    RarityMessage = "CRAZY RARE DROP!";
                    RarityMessageColor = Color.LightPurple;
                    break;
                case DropRarity.RNGesusIncarnate:
                    RarityMessage = "INSANE DROP!";
                    RarityMessageColor = Color.Red;
                    break;
                default:
                    HideMessage = true;
                    break;
            }
        }

        /// <summary>
        ///     Rarity message to be displayed. E.G. **VERY RARE DROP!** or **CRAZY RARE DROP!**
        /// </summary>
        public string RarityMessage { get; set; }

        /// <summary>
        ///     Color of rarity message.
        /// </summary>
        public Color RarityMessageColor { get; set; }

        /// <summary>
        ///     Name of item to show drop
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        ///     Color of item drop to show
        /// </summary>
        public Color ItemColor { get; set; }

        /// <summary>
        ///     Whether the message should not be displayed. Should be displayed by default
        /// </summary>
        public bool HideMessage { get; set; }

        /// <summary>
        ///     Compiles the message to super raw text
        /// </summary>
        /// <returns>Compiled super raw text ready to use</returns>
        public SuperRawText GetMessage()
        {
            if (!HideMessage)
            {
                SuperRawText srt = new();
                srt.Append($"{RarityMessage}", RarityMessageColor, RawTextFormatting.Straight, RawTextFormatting.Bold);
                srt.Append(" (", Color.DarkGray);
                srt.Append($"{ItemName}", ItemColor);
                srt.Append(" )", Color.DarkGray);
                srt.Append(" (+121% Magic Find)", Color.Aqua);
                return srt;
            }

            return null;
        }
    }

    /// <summary>
    ///     Represents rarity of slayer drop
    /// </summary>
    public enum DropRarity
    {
        /// <summary>
        ///     100%
        /// </summary>
        [RarityColor(Color.Green)] [SlayerRarity("")] [EnumValue("Guaranteed")]
        Guaranteed,

        /// <summary>
        ///     20%
        /// </summary>
        [RarityColor(Color.Blue)] [SlayerRarity("(20%)")] [EnumValue("Occasional")]
        Occasional,

        /// <summary>
        ///     5%
        /// </summary>
        [RarityColor(Color.Aqua)] [SlayerRarity("(5%)")] [EnumValue("Rare")]
        Rare,

        /// <summary>
        ///     1%
        /// </summary>
        [RarityColor(Color.DarkPurple)] [SlayerRarity("(1%)")] [EnumValue("Extraordinary")]
        Extraordinary,

        /// <summary>
        ///     lesser than 1%
        /// </summary>
        [RarityColor(Color.LightPurple)] [SlayerRarity("")] [EnumValue("Pray RNGesus")]
        PrayRNGesus,

        /// <summary>
        ///     lesser than 0.01%
        /// </summary>
        [RarityColor(Color.Red)] [SlayerRarity("")] [EnumValue("RNGesus Incarnate")]
        RNGesusIncarnate
    }
}