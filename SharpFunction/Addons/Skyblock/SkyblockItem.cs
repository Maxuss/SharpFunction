using System;
using System.Collections.Generic;
using System.Linq;
using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;
using static SharpFunction.Universal.EnumHelper;
using static SharpFunction.Addons.Skyblock.SkyblockEnumHelper;
using static SharpFunction.Universal.NullChecker;
using static SharpFunction.Addons.Skyblock.SkyblockHelper;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    ///     Represents 'fake' item from Hypixel Skyblock
    /// </summary>
    public class SkyblockItem
    {
        private Color color;

        private RawText rawDescription;

        /// <summary>
        ///     Create new Skyblock Item
        /// </summary>
        /// <param name="type">Type of item</param>
        /// <param name="rarity">Rarity of item</param>
        /// <param name="name">Display name of item</param>
        /// <param name="itemname">ID name of item to give</param>
        public SkyblockItem(ItemType type, ItemRarity rarity, string name, string itemname)
        {
            Type = type;
            Rarity = rarity;
            color = rarity.GetRarityColor();
            ID = itemname;
            DisplayName = name;
        }

        /// <summary>
        ///     Type of item
        /// </summary>
        public ItemType Type { get; set; }

        /// <summary>
        ///     Display name of item
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Give ID of item
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///     Compiled command
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        ///     Whether the item has crafts
        /// </summary>
        public bool HasCrafts { get; set; }

        /// <summary>
        ///     Rarity of an item
        /// </summary>
        public ItemRarity Rarity { get; set; }

        /// <summary>
        ///     Whether the item has enchantment glint
        /// </summary>
        public bool HasGlint { get; set; } = false;

        /// <summary>
        ///     Should item have empty lines between stats etc.
        /// </summary>
        /// <value></value>
        public bool MakeEmptyLines { get; set; } = true;

        /// <summary>
        ///     Extra color for name different from rarity color. Useful for creating runes.
        /// </summary>
        /// <value></value>
        public Color NameColor { get; set; } = Color.Default;

        /// <summary>
        ///     Name of the item
        /// </summary>
        public RawText ItemName { get; set; }

        /// <summary>
        ///     Description of the item
        /// </summary>
        public RawText ItemDescription { get; set; }

        /// <summary>
        ///     Description that appears under item abilities etc.
        /// </summary>
        public AdvancedDescription AdvancedDescription { get; set; } = new();

        /// <summary>
        ///     Makes the item hide it's stats only show rarity+recipes
        /// </summary>
        public bool HideStats { get; set; } = false;

        /// <summary>
        ///     Marks that the item is leather armor
        /// </summary>
        public bool IsLeather { get; set; } = false;

        /// <summary>
        ///     If marked that <see cref="IsLeather" /> this is hex color code for leather armor
        /// </summary>
        public string LeatherColorHex { get; set; }

        private RawText ParseStats()
        {
            RawText rt = new();
            Dictionary<string, int?> red = new()
            {
                {"Damage", Damage},
                {"Strength", Strength},
                {"Bonus Attack Speed", AttackSpeed},
                {"Crit Chance", CritChance},
                {"Crit Damage", CritDamage},
                {"Sea Creature Chance", SeaCreatureChance}
            };
            Dictionary<string, int?> green = new()
            {
                {"Health", Health},
                {"Defense", Defense},
                {"Speed", Speed},
                {"Intelligence", Intelligence},
                {"Magic Find", MagicFind},
                {"Pet Luck", PetLuck},
                {"True Defense", TrueDefense},
                {"Ferocity", Ferocity}
            };
            var percented = new[] {"Crit Chance", "Crit Damage", "Sea Creature Chance", "Bonus Attack Speed"}.ToList();

            var space = VoidEncapsulator<RawText>.Encapsulate(m =>
            {
                if (MakeEmptyLines) m.AddField("");
            });

            var a = red.Where(predicate => !IsNull(predicate.Value));
            var b = green.Where(predicate => !IsNull(predicate.Value));

            var addRed = VoidEncapsulator<KeyValuePair<string, int?>>.Encapsulate(p =>
            {
                var name = p.Key;
                var stat = percented.Contains(name) ? $"{ParseStat(p.Value)}%" : $"{ParseStat(p.Value)}";
                var n = $"{name}: ";
                SuperRawText srt = new();
                srt.Append(n, Color.Gray);
                srt.Append(stat, Color.Red);
                if (DungeonStats.IsDungeon) srt.Append($" ({ParseStat(p.Value)})", Color.DarkGray);
                rt.AddField(srt);
                if (a.Last().Equals(p)) space(rt);
            });

            var addGreen = VoidEncapsulator<KeyValuePair<string, int?>>.Encapsulate(p =>
            {
                var name = p.Key;
                var stat = percented.Contains(name) ? $"{ParseStat(p.Value)}%" : $"{ParseStat(p.Value)}";
                var n = $"{name}: ";
                SuperRawText srt = new();
                srt.Append(n, Color.Gray);
                srt.Append(stat, Color.Green);
                if (DungeonStats.IsDungeon) srt.Append($" ({ParseStat(p.Value)})", Color.DarkGray);
                rt.AddField(srt);
            });

            if (DungeonStats.IsDungeon) rt.AddField(DungeonStats.GetGearScore());

            var valuePairs = a as KeyValuePair<string, int?>[] ?? a.ToArray();
            foreach (var r in valuePairs) addRed(r);

            var keyValuePairs = b.ToList();
            foreach (var g in keyValuePairs) addGreen(g);

            if (valuePairs.Any() && keyValuePairs.Any()) space(rt);
            return rt;
        }

        /// <summary>
        ///     Adds description to item
        /// </summary>
        /// <param name="description">Description string to add</param>
        public void AddDescription(AdvancedDescription description)
        {
            // parse name so custom color works
            if (color != NameColor && NameColor != Color.Default) color = NameColor;
            RawText _name = new();
            _name.AddField(DisplayName, color, RawTextFormatting.Straight, RawTextFormatting.None);
            ItemName = _name;

            // parse description and stuff
            var txt = ParseStats();
            if (!HideStats)
            {
                txt = AddAbility(txt);
                rawDescription = ParseDescription(description, txt);
                ItemDescription = AddRarity(rawDescription);
            }
            else
            {
                txt.AddField("");
                if (HasCrafts) txt.AddField("Right click to view recipes!", Color.Yellow, RawTextFormatting.Straight);
                txt.AddField($"{Rarity.GetStringValue()} {Type.GetStringValue()}", color, RawTextFormatting.Straight,
                    RawTextFormatting.Bold);
            }
        }

        private string ParseStat(int? stat)
        {
            if (stat <= 0) return $"{stat}";
            return $"+{stat};";
        }

        private RawText AddSlayerRequirement(RawText rt)
        {
            if (Requirement is not null && Requirement.HasRequirement)
            {
                if (MakeEmptyLines) rt.AddField("");
                rt.AddField(Requirement.Generate());
            }

            return rt;
        }

        private RawText AddAbility(RawText txt)
        {
            if (Abilities is not null)
                foreach (var Ability in Abilities)
                    if (Ability is not null)
                    {
                        if (!Abilities.First().Equals(Ability))
                            if (MakeEmptyLines)
                                txt.AddField("");
                        var rawJson =
                            $@"[{{""text"":""Item Ability: {Ability.Name} "",""italic"":false,""color"":""gold""}},{{""text"":""{Ability.Type.GetStringValue()}"",""color"":""yellow"",""bold"":true}}]";
                        txt._lines.Add(rawJson);

                        foreach (var line in Ability.Description.Lines)
                            if (line != null)
                                txt.AddField(line);

                        if (Ability.ManaCost != 0)
                        {
                            SuperRawText mana = new();
                            mana.Append("Mana cost: ", Color.DarkGray);
                            mana.Append($"{Ability.ManaCost}", Color.DarkAqua);
                            txt.AddField(mana);
                        }

                        if (Ability.Cooldown != 0)
                        {
                            SuperRawText cd = new();
                            cd.Append("Cooldown: ", Color.DarkGray);
                            cd.Append($"{Ability.Cooldown}s", Color.Green);
                            txt.AddField(cd);
                        }
                    }

            return txt;
        }

        private RawText ParseDescription(AdvancedDescription description, RawText text)
        {
            int?[] stats =
            {
                Damage,
                Strength,
                AttackSpeed,
                CritChance,
                CritDamage,
                SeaCreatureChance,
                Health,
                Defense,
                Speed,
                Intelligence,
                MagicFind,
                PetLuck,
                TrueDefense,
                Ferocity
            };
            if (stats.Any(p => !IsNull(p)))
                if (MakeEmptyLines)
                    text.AddField("");
            foreach (var line in description.Lines) text.AddField(line);
            return text;
        }

        private RawText AddRarity(RawText rawDesc)
        {
            if (HasCrafts) rawDesc.AddField("Right click to view recipes!", Color.Yellow, RawTextFormatting.Straight);
            AddSlayerRequirement(rawDesc);
            ItemType[] nonreforgeable =
            {
                ItemType.ReforgeStone, ItemType.Shears, ItemType.BrewingIngredient, ItemType.None
            };
            if (!nonreforgeable.Contains(Type))
                rawDesc.AddField("This item can be reforged!", Color.DarkGray, RawTextFormatting.Straight);
            if (DungeonStats.IsDungeon)
            {
                rawDesc.AddField(DungeonStats.GetQuality());
                rawDesc.AddField($"{Rarity.GetStringValue()} DUNGEON {Type.GetStringValue()}", color,
                    RawTextFormatting.Straight, RawTextFormatting.Bold);
            }
            else
            {
                rawDesc.AddField($"{Rarity.GetStringValue()} {Type.GetStringValue()}", color,
                    RawTextFormatting.Straight, RawTextFormatting.Bold);
            }

            return rawDesc;
        }

        /// <summary>
        ///     Compile the skyblock item give command
        /// </summary>
        /// <returns>Compiled give command</returns>
        public virtual string Compile()
        {
            var itemname = ID;
            var nbt = new ItemNBT();
            nbt.HideFlags = 31;
            var display = new ItemDisplay();
            display.AddLore(ItemDescription);
            display.AddName(ItemName);
            nbt.Display = display;
            nbt.Unbreakable = true;
            if (HasGlint) nbt.EnchantmentData = "{}";
            if (IsLeather && !IsNull(LeatherColorHex))
            {
                var colorCode = ArmorHelper.ParseHex(LeatherColorHex);
                nbt.Display.AddColor(colorCode);
            }

            var item = new Item(itemname, nbt);
            var cmd = new Give(SimpleSelector.Nearest);
            cmd.Compile(item);
            Command = cmd;
            return cmd.Compiled;
        }
#nullable enable
        /// <summary>
        ///     Damage of item
        /// </summary>
        public int? Damage { get; set; } = null;

        /// <summary>
        ///     Strength of item
        /// </summary>
        public int? Strength { get; set; } = null;

        /// <summary>
        ///     Defense of item
        /// </summary>
        public int? Defense { get; set; } = null;

        /// <summary>
        ///     Health of item
        /// </summary>
        public int? Health { get; set; } = null;

        /// <summary>
        ///     Intelligence of item
        /// </summary>
        public int? Intelligence { get; set; } = null;

        /// <summary>
        ///     Crit Chance of item
        /// </summary>
        public int? CritChance { get; set; } = null;

        /// <summary>
        ///     Crit Damage of item
        /// </summary>
        public int? CritDamage { get; set; } = null;

        /// <summary>
        ///     Attack Speed of item
        /// </summary>
        public int? AttackSpeed { get; set; } = null;

        /// <summary>
        ///     Speed of item
        /// </summary>
        public int? Speed { get; set; } = null;

        /// <summary>
        ///     True Defense of item
        /// </summary>
        public int? TrueDefense { get; set; } = null;

        /// <summary>
        ///     SCC of item
        /// </summary>
        public int? SeaCreatureChance { get; set; } = null;

        /// <summary>
        ///     Magic Find of item
        /// </summary>
        public int? MagicFind { get; set; } = null;

        /// <summary>
        ///     Pet Luck of item
        /// </summary>
        public int? PetLuck { get; set; } = null;

        /// <summary>
        ///     Pet Luck of item
        /// </summary>
        public int? Ferocity { get; set; } = null;

        /// <summary>
        ///     Abilities of an item
        /// </summary>
        public List<ItemAbility> Abilities { get; set; } = new();

        /// <summary>
        ///     Slayer requirement for the item
        /// </summary>
        public SlayerRequirement Requirement { get; set; } = new();

        /// <summary>
        ///     Dungeon stats of an item
        /// </summary>
        public DungeonStats DungeonStats { get; set; } = new();
#nullable disable
    }

    /// <summary>
    ///     Represents incapsulated item ability
    /// </summary>
    public sealed class ItemAbility
    {
        /// <summary>
        ///     Cooldown of the ability
        /// </summary>
        public int Cooldown;

        /// <summary>
        ///     Description of ability
        /// </summary>
        public AdvancedDescription Description;

        /// <summary>
        ///     Mana cost of the ability
        /// </summary>
        public int ManaCost;

        /// <summary>
        ///     Name of the ability
        /// </summary>
        public string Name;

        /// <summary>
        ///     Type of ability. Passive will not show hint tooltip
        /// </summary>
        public AbilityType Type;

        /// <summary>
        ///     Create a new item ability
        /// </summary>
        /// <param name="name">Name of the ability</param>
        /// <param name="type">Type of the ability</param>
        /// <param name="manacost">Mana cost of the ability. 0 by default</param>
        /// <param name="cooldown">Cooldown of the ability. 0s by default.</param>
        public ItemAbility(string name, AbilityType type = AbilityType.Passive, int manacost = 0, int cooldown = 0)
        {
            Name = name;
            Type = type;
            ManaCost = manacost;
            Cooldown = cooldown;
        }
    }

    /// <summary>
    ///     Represents description of ability containing multiple super raw text lines
    /// </summary>
    public sealed class AdvancedDescription
    {
        /// <summary>
        ///     Lines of super raw text
        /// </summary>
        public List<SuperRawText> Lines { get; set; } = Array.Empty<SuperRawText>().ToList();

        /// <summary>
        ///     Appends a single super raw text line. Each line with start from new line in lore!
        /// </summary>
        /// <param name="line">Line to append</param>
        public AdvancedDescription Append(SuperRawText line)
        {
            Lines.Add(line);
            return this;
        }

        /// <summary>
        ///     Converts description to deprecated format with colors as section signs
        /// </summary>
        public string OldValue()
        {
            return string.Join("\\n", Lines.ConvertAll(c => string.Join(" ", c.Deprecated)));
        }

        /// <summary>
        ///     Appends a simple single color line
        /// </summary>
        /// <param name="line">Text in line</param>
        /// <param name="color">Color of line</param>
        /// <param name="formatting">Formatting of line</param>
        /// <param name="formattings">Extra formattings of line</param>
        public AdvancedDescription Append(string line, Color color = Color.Gray,
            RawTextFormatting formatting = RawTextFormatting.Straight, params RawTextFormatting[] formattings)
        {
            SuperRawText tmp = new();
            tmp.Append(line, color, formatting, formattings);
            Lines.Add(tmp);
            return this;
        }
    }

    /// <summary>
    ///     Dungeon stats of an item
    /// </summary>
    public sealed class DungeonStats
    {
        /// <summary>
        ///     Whether the item is dungeon item
        /// </summary>
        public bool IsDungeon { get; set; } = false;

        /// <summary>
        ///     The quality of an item
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        ///     Gear score of an item
        /// </summary>
        public int GearScore { get; set; }

        /// <summary>
        ///     Compiles the gear score into stat text
        /// </summary>
        /// <returns>Compiled super raw text with gear score</returns>
        public SuperRawText GetGearScore()
        {
            SuperRawText t = new();
            t.Append("Gear Score: ", Color.Gray);
            t.Append($"{GearScore}", Color.LightPurple);
            t.Append($" ({GearScore})", Color.DarkGray);
            return t;
        }

        /// <summary>
        ///     Compiles the quality into stat text
        /// </summary>
        /// <returns>Compiled super raw text with quality</returns>
        public SuperRawText GetQuality()
        {
            SuperRawText t = new();
            t.Append($"Perfect {Quality} / {Quality}", Color.Green);
            return t;
        }
    }

    /// <summary>
    ///     Represents specific slayer requirement on an item
    /// </summary>
    public sealed class SlayerRequirement
    {
        /// <summary>
        ///     Whether the item will have slayer requirement. False by default.
        /// </summary>
        public bool HasRequirement { get; set; } = false;

        /// <summary>
        ///     Name of the slayer to display. E.G. Zombie, or Enderman
        /// </summary>
        public string SlayerName { get; set; }

        /// <summary>
        ///     Level of slayer required
        /// </summary>
        public int SlayerLevel { get; set; }

        /// <summary>
        ///     Generates the super raw text requirement field
        /// </summary>
        /// <returns></returns>
        public SuperRawText Generate()
        {
            SuperRawText srt = new();
            srt.Append($"{SLAYER_REQ} ", Color.Red);
            srt.Append($"Requires {SlayerName} LVL {SlayerLevel}", Color.DarkPurple);
            return srt;
        }
    }

    /// <summary>
    ///     Represents type of skyblock item ability
    /// </summary>
    public enum AbilityType
    {
        [EnumValue("")] Passive,
        [EnumValue("RIGHT CLICK")] RightClick,
        [EnumValue("SNEAK")] Sneak,
        [EnumValue("DOUBLE RIGHT-CLICK")] DoubleRightClick
    }

    /// <summary>
    ///     Represents type of skyblock item
    /// </summary>
    public enum ItemType
    {
        [EnumValue("SWORD")] Sword,
        [EnumValue("BOW")] Bow,
        [EnumValue("HELMET")] Helmet,
        [EnumValue("CHESTPLATE")] Chestplate,
        [EnumValue("LEGGINGS")] Leggings,
        [EnumValue("BOOTS")] Boots,
        [EnumValue("ACCESSORY")] Accessory,
        [EnumValue("REFORGE STONE")] ReforgeStone,
        [EnumValue("FISHING ROD")] FishingRod,
        [EnumValue("PICKAXE")] Pickaxe,
        [EnumValue("AXE")] Axe,
        [EnumValue("SHOVEL")] Shovel,
        [EnumValue("HOE")] Hoe,
        [EnumValue("SHEARS")] Shears,
        [EnumValue("BREWING INGREDIENT")] BrewingIngredient,
        [EnumValue("COSMETIC")] Cosmetic,
        [EnumValue("")] None
    }

    /// <summary>
    ///     Represents rarity of a custom skyblock item
    /// </summary>
    public enum ItemRarity
    {
        /// <summary>
        ///     <b>COMMON</b>
        /// </summary>
        [RarityColor(Color.White)] [EnumValue("COMMON")]
        Common,

        /// <summary>
        ///     <b>UNCOMMON</b>
        /// </summary>
        [RarityColor(Color.Green)] [EnumValue("UNCOMMON")]
        Uncommon,

        /// <summary>
        ///     <b>RARE</b>
        /// </summary>
        [RarityColor(Color.Blue)] [EnumValue("RARE")]
        Rare,

        /// <summary>
        ///     <b>EPIC</b>
        /// </summary>
        [RarityColor(Color.DarkPurple)] [EnumValue("EPIC")]
        Epic,

        /// <summary>
        ///     <b>LEGENDARY</b>
        /// </summary>
        [RarityColor(Color.Gold)] [EnumValue("LEGENDARY")]
        Legendary,

        /// <summary>
        ///     <b>MYTHIC</b>
        /// </summary>
        [RarityColor(Color.LightPurple)] [EnumValue("MYTHIC")]
        Mythic,

        /// <summary>
        ///     <b>SPECIAL</b>
        /// </summary>
        [RarityColor(Color.Red)] [EnumValue("SPECIAL")]
        Special,

        /// <summary>
        ///     <b>VERY SPECIAL</b>
        /// </summary>
        [RarityColor(Color.Red)] [EnumValue("VERY SPECIAL")]
        VerySpecial,

        /// <summary><b>TRASH</b> (custom)</summary>
        [RarityColor(Color.Gray)] [EnumValue("VERY SPECIAL")]
        Trash,

        /// <summary>
        ///     <b>SUPREME</b>
        /// </summary>
        [RarityColor(Color.DarkRed)] [EnumValue("VERY SPECIAL")]
        Supreme
    }
}