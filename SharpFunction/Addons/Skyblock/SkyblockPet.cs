using System;
using System.Collections.Generic;
using System.Linq;
using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;
using static SharpFunction.Universal.EnumHelper;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    ///     Represents a *fake* skyblock pet
    /// </summary>
    public sealed class SkyblockPet
    {
        private readonly RawText fullDesc;
        private readonly RawText name;

        private readonly Dictionary<ItemRarity, int> rar = new()
        {
            {ItemRarity.Common, 100},
            {ItemRarity.Uncommon, 175},
            {ItemRarity.Rare, 275},
            {ItemRarity.Epic, 440},
            {ItemRarity.Legendary, 660},
            {ItemRarity.Special, 720},
            {ItemRarity.VerySpecial, 990}
        };

        /// <summary>
        ///     Initialize a new skyblock pet
        /// </summary>
        /// <param name="name">Name of the pet</param>
        /// <param name="skullOwner">Player name owning the head of pet. Can also be NBT texture data</param>
        /// <param name="rarity">Rarity of the pet</param>
        /// <param name="type">Type of pet</param>
        public SkyblockPet(string name, string skullOwner, ItemRarity rarity, PetType type)
        {
            Name = name;
            SkullOwner = skullOwner;
            Rarity = rarity;
            Type = type;
            fullDesc = new RawText();
            this.name = new RawText();
            SuperRawText pn = new();
            pn.Append("[Lvl 100] ", Color.Gray);
            pn.Append($"{name}", rarity.GetRarityColor());
            this.name.AddField(pn);
            fullDesc.AddField($"{type.GetStringValue()}", Color.DarkGray, RawTextFormatting.Straight);
        }

        /// <summary>
        ///     What skill the pet is related to?
        /// </summary>
        public PetType Type { get; set; }

        /// <summary>
        ///     Name of the pet
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     All the abilities of the pet
        /// </summary>
        public List<PetAbility> Abilities { get; set; } = Array.Empty<PetAbility>().ToList();

        /// <summary>
        ///     Rarity of the pet
        /// </summary>
        public ItemRarity Rarity { get; set; }

        /// <summary>
        ///     Skull owner of the pet head
        /// </summary>
        public string SkullOwner { get; set; }

        /// <summary>
        ///     Damage of pet
        /// </summary>
        public int? Damage { get; set; } = null;

        /// <summary>
        ///     Strength of pet
        /// </summary>
        public int? Strength { get; set; } = null;

        /// <summary>
        ///     Defense of pet
        /// </summary>
        public int? Defense { get; set; } = null;

        /// <summary>
        ///     Health of pet
        /// </summary>
        public int? Health { get; set; } = null;

        /// <summary>
        ///     Intelligence of pet
        /// </summary>
        public int? Intelligence { get; set; } = null;

        /// <summary>
        ///     Crit Chance of pet
        /// </summary>
        public int? CritChance { get; set; } = null;

        /// <summary>
        ///     Crit Damage of pet
        /// </summary>
        public int? CritDamage { get; set; } = null;

        /// <summary>
        ///     Attack Speed of pet
        /// </summary>
        public int? AttackSpeed { get; set; } = null;

        /// <summary>
        ///     Speed of pet
        /// </summary>
        public int? Speed { get; set; } = null;

        /// <summary>
        ///     True Defense of pet
        /// </summary>
        public int? TrueDefense { get; set; } = null;

        /// <summary>
        ///     SCC of pet
        /// </summary>
        public int? SeaCreatureChance { get; set; } = null;

        /// <summary>
        ///     Magic Find of pet
        /// </summary>
        public int? MagicFind { get; set; } = null;

        /// <summary>
        ///     Pet Luck of pet
        /// </summary>
        public int? PetLuck { get; set; } = null;

        /// <summary>
        ///     Pet Luck of pet
        /// </summary>
        public int? Ferocity { get; set; } = null;

        /// <summary>
        ///     Preview pets will not have experience bar in description.
        /// </summary>
        public bool IsPreview { get; set; } = true;

        /// <summary>
        ///     If max level it will have MAX LEVEL display instead of progress bar
        /// </summary>
        public bool MaxLevel { get; set; } = true;

        /// <summary>
        ///     Generates the give command
        /// </summary>
        /// <returns>Compilde give command to obtain the pet item</returns>
        public Give Generate()
        {
            Dictionary<string, int?> stats = new()
            {
                {"Health", Health},
                {"Defense", Defense},
                {"Speed", Speed},
                {"Intelligence", Intelligence},
                {"Magic Find", MagicFind},
                {"Pet Luck", PetLuck},
                {"True Defense", TrueDefense},
                {"Ferocity", Ferocity},
                {"Damage", Damage},
                {"Strength", Strength},
                {"Bonus Attack Speed", AttackSpeed},
                {"Crit Chance", CritChance},
                {"Crit Damage", CritDamage},
                {"Sea Creature Chance", SeaCreatureChance}
            };
            var percented = new[] {"Crit Chance", "Crit Damage", "Sea Creature Chance", "Bonus Attack Speed"}.ToList();
            // encapsulate the predicate
            var predicate = Encapsulator<KeyValuePair<string, int?>, bool>.Encapsulate(predicate =>
            {
                return !NullChecker.IsNull(predicate.Value);
            });
            // test stats with predicate
            // if no stats specified dont make extra empty field
            if (stats.Any(predicate))
            {
                fullDesc.AddField("");
                // parse the stats since we know that at least one pair does not contain null
                var parsed = stats.Where(predicate);
                foreach (var pair in parsed)
                {
                    SuperRawText p = new();
                    p.Append(pair.Key + ':', Color.Gray);
                    var stat = percented.Contains(pair.Key) ? $" +{pair.Value}%" : $" +{pair.Value}";
                    p.Append(stat, Color.Green);
                    fullDesc.AddField(p);
                }

                if (!(Abilities.Count > 0)) fullDesc.AddField("");
            }

            if (Abilities.Count > 0)
                foreach (var abil in Abilities)
                {
                    var d = abil.GetAbility();
                    foreach (var line in d.Lines) fullDesc.AddField(line);
                }

            fullDesc.AddField("");
            // complete description
            if (IsPreview)
            {
                fullDesc.AddField("This is a preview of Lvl 100.", Color.DarkGray, RawTextFormatting.Straight);
                fullDesc.AddField("New pets are lowest level!", Color.DarkGray, RawTextFormatting.Straight);
                fullDesc.AddField("");
                fullDesc.AddField("Right-click to add this pet to", Color.Yellow, RawTextFormatting.Straight);
                fullDesc.AddField("your pet menu!", Color.Yellow, RawTextFormatting.Straight);
            }
            else if (MaxLevel)
            {
                fullDesc.AddField("MAX LEVEL", Color.Aqua, RawTextFormatting.Straight, RawTextFormatting.Bold);
            }
            else
            {
                SuperRawText p = new();
                p.Append("Progress to Level 2: ", Color.Gray);
                p.Append("0%", Color.Yellow);
                fullDesc.AddField(p);
                SuperRawText b = new();
                b.Append("-", Color.DarkGreen);
                b.Append("------------------- ", Color.Gray);
                b.Append("0", Color.Yellow);
                b.Append("/", Color.Gold);
                b.Append($"{rar[Rarity]}", Color.Yellow);
                fullDesc.AddField(b);
            }

            fullDesc.AddField("");
            fullDesc.AddField($"{Rarity.GetStringValue()}", Rarity.GetRarityColor(), RawTextFormatting.Straight,
                RawTextFormatting.Bold);

            // compile to nbt
            ItemNBT nbt = new();
            nbt.CustomTags = SkullOwner.StartsWith("{Id:[")
                ? $"SkullOwner:{SkullOwner}"
                : $"{SkullOwner}: \"{SkullOwner}\"";
            nbt.HideFlags = 31;
            nbt.Unbreakable = true;
            nbt.Display = new ItemDisplay();
            nbt.Display.AddLore(fullDesc);
            nbt.Display.AddName(name);
            Item item = new("player_head", nbt);
            Give g = new(SimpleSelector.p);
            g.Compile(item);
            return g;
        }

        /// <summary>
        ///     Generates the /give command and compiles it to string
        /// </summary>
        /// <returns>Compiled /give command</returns>
        public string Compile()
        {
            return Generate().Compiled;
        }
    }

    /// <summary>
    ///     Represents a single pet ability
    /// </summary>
    public sealed class PetAbility
    {
        /// <summary>
        ///     Initialize a new pet ability
        /// </summary>
        /// <param name="name">Name of the ability</param>
        public PetAbility(string name)
        {
            AbilityName = name;
        }

        /// <summary>
        ///     Represents name of the ability
        /// </summary>
        public string AbilityName { get; set; }

        /// <summary>
        ///     Description of the ability
        /// </summary>
        public AdvancedDescription AbilityDescription { get; set; }

        /// <summary>
        ///     Compiles the description into single description
        /// </summary>
        /// <returns>Raw ability as AdvancedDescription</returns>
        public AdvancedDescription GetAbility()
        {
            AdvancedDescription d = new();
            d.Append("");
            d.Append(AbilityName, Color.Gold);
            if (AbilityDescription is not null)
                foreach (var line in AbilityDescription.Lines)
                    d.Append(line);
            return d;
        }
    }

    /// <summary>
    ///     Represents type of pet
    /// </summary>
    public enum PetType
    {
        /// <summary>
        ///     Farming pet
        /// </summary>
        [EnumValue("Farming Pet")] Farming,

        /// <summary>
        ///     Mining pet
        /// </summary>
        [EnumValue("Mining Pet")] Mining,

        /// <summary>
        ///     Combat pet
        /// </summary>
        [EnumValue("Combat Pet")] Combat,

        /// <summary>
        ///     Foraging pet
        /// </summary>
        [EnumValue("Foraging Pet")] Foraging,

        /// <summary>
        ///     Fishing pet
        /// </summary>
        [EnumValue("Fishing Pet")] Fishing,

        /// <summary>
        ///     Enchanting pet
        /// </summary>
        [EnumValue("Enchanting Pet")] Enchanting,

        /// <summary>
        ///     Alchemy pet
        /// </summary>
        [EnumValue("Alchemy Pet")] Alchemy,

        /// <summary>
        ///     Slayer pet (custom)
        /// </summary>
        [EnumValue("Slayer Pet")] Slayer,

        /// <summary>
        ///     Runecrafting pet (custom)
        /// </summary>
        [EnumValue("Runecrafting Pet")] Runecrafting,

        /// <summary>
        ///     Carpentry pet (custom)
        /// </summary>
        [EnumValue("Carpentry Pet")] Carpentry,

        /// <summary>
        ///     Dungeons pet (custom)
        /// </summary>
        [EnumValue("Dungeon Pet")] Dungeon
    }
}