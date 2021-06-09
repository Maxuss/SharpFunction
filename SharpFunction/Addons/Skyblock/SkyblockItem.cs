using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;
using static SharpFunction.Addons.Skyblock.SkyblockEnumHelper;
using Color = SharpFunction.Universal.Color;
using static SharpFunction.Universal.NullChecker;
using SharpFunction.Universal;
using static SharpFunction.Addons.Skyblock.SkyblockHelper;
using SharpFunction.API;
using SharpFunction.Commands.Minecraft;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    /// Represents 'fake' item from Hypixel Skyblock
    /// </summary>
    public sealed class SkyblockItem
    {
        /// <summary>
        /// Type of item
        /// </summary>
        public ItemType Type { get; set; }
        /// <summary>
        /// Display name of item
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Give ID of item
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Compiled command
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Rarity of an item
        /// </summary>
        public ItemRarity Rarity { get; set; }

        private Color color;
#nullable enable
        /// <summary>
        /// Damage of item
        /// </summary>
        public int? Damage { get; set; } = null;
        /// <summary>
        /// Strength of item
        /// </summary>
        public int? Strength { get; set; } = null;
        /// <summary>
        /// Defense of item
        /// </summary>
        public int? Defense { get; set; } = null;
        /// <summary>
        /// Health of item
        /// </summary>
        public int? Health { get; set; } = null;
        /// <summary>
        /// Intelligence of item
        /// </summary>
        public int? Intelligence { get; set; } = null;
        /// <summary>
        /// Crit Chance of item
        /// </summary>
        public int? CritChance { get; set; } = null;
        /// <summary>
        /// Crit Damage of item
        /// </summary>
        public int? CritDamage { get; set; } = null;
        /// <summary>
        /// Attack Speed of item
        /// </summary>
        public int? AttackSpeed { get; set; } = null;
        /// <summary>
        /// Speed of item
        /// </summary>
        public int? Speed { get; set; } = null;
        /// <summary>
        /// True Defense of item
        /// </summary>
        public int? TrueDefense { get; set; } = null;
        /// <summary>
        /// SCC of item
        /// </summary>
        public int? SeaCreatureChance { get; set; } = null;
        /// <summary>
        /// Magic Find of item
        /// </summary>
        public int? MagicFind { get; set; } = null;
        /// <summary>
        /// Pet Luck of item
        /// </summary>
        public int? PetLuck { get; set; } = null;
        /// <summary>
        /// Pet Luck of item
        /// </summary>
        public int? Ferocity { get; set; } = null;

        /// <summary>
        /// Ability of an item
        /// </summary>
        public ItemAbility? Ability { get; set; } = null;
#nullable disable
        public RawText ItemName { get; set; }
        public RawText ItemDescription { get; set; }
        private RawText rawDescription;
        /// <summary>
        /// Create new Skyblock Item
        /// </summary>
        /// <param name="type">Type of item</param>
        /// <param name="rarity">Rarity of item</param>
        /// <param name="name">Display name of item</param>
        public SkyblockItem(ItemType type, ItemRarity rarity, string name, string itemname)
        {
            Type = type;
            Rarity = rarity;
            color = SkyblockEnumHelper.GetRarityColor(rarity);
            DisplayName = name;
            RawText _name = new();
            _name.AddField(name, color, RawTextFormatting.Straight, RawTextFormatting.None);
            ItemName = _name;
        }

        private RawText ParseStats()
        {
            string dmg = IsNull(Damage) ? "" : $"+{Damage}{DAMAGE} Damage";
            string str = IsNull(Strength) ? "" : $"+{Strength}{STRENGTH} Strength";
            string def = IsNull(Defense) ? "" : $"+{Defense}{DEFENCE} Defense";
            string hp = IsNull(Health) ? "" : $"+{Health}{HEALTH} Health";
            string @int = IsNull(Intelligence) ? "" : $"+{Intelligence}{INTELLIGENCE} Intelligence";
            string cc = IsNull(CritChance) ? "" : $"+{CritChance}{CRIT_CHANCE} Crit Chance";
            string cd = IsNull(CritDamage) ? "" : $"+{CritDamage}{CRIT_DAMAGE} Crit Damage";
            string ats = IsNull(AttackSpeed) ? "" : $"+{AttackSpeed}{AttackSpeed} Attack Speed";
            string spd = IsNull(Speed) ? "" : $"+{Speed}{SPEED} Speed";
            string td = IsNull(TrueDefense) ? "" : $"+{TrueDefense}{TRUE_DEFENCE} True Defense";
            string scc = IsNull(SeaCreatureChance) ? "" : $"+{SeaCreatureChance}{SCC} Sea Creature Chance";
            string mf = IsNull(MagicFind) ? "" : $"+{MagicFind}{MAGIC_FIND} Magic Find";
            string pl = IsNull(PetLuck) ? "" : $"+{PetLuck}{PET_LUCK} Pet Luck";
            string fc = IsNull(Ferocity) ? "" : $"+{Ferocity}{FEROCITY} Ferocity";
            RawText rt = new();
            string[] red = new[] { dmg, str, hp, fc };
            string[] light_blue = new[] { @int, mf };
            string[] blue = new[] { cc, cd };
            //green: def
            //aqua: scc
            //white: speed
            //light_purple: pl
            //yellow: ats
            Parallel.ForEach(red, r =>
            {
                if (!IsEmpty(r)) rt.AddField(r, Color.Red, RawTextFormatting.Straight, RawTextFormatting.None);
            });
            Parallel.ForEach(light_blue, l =>
            {
                if (!IsEmpty(l)) rt.AddField(l, Color.Aqua, RawTextFormatting.Straight, RawTextFormatting.None);
            });
            Parallel.ForEach(blue, b =>
            {
                if (!IsEmpty(b)) rt.AddField(b, Color.Blue, RawTextFormatting.Straight, RawTextFormatting.None);
            });
            if (!IsEmpty(def)) rt.AddField(def, Color.Green, RawTextFormatting.Straight, RawTextFormatting.None);
            if (!IsEmpty(scc)) rt.AddField(scc, Color.DarkAqua, RawTextFormatting.Straight, RawTextFormatting.None);
            if (!IsEmpty(spd)) rt.AddField(spd, Color.White, RawTextFormatting.Straight, RawTextFormatting.None);
            if (!IsEmpty(pl)) rt.AddField(pl, Color.LightPurple, RawTextFormatting.Straight, RawTextFormatting.None);
            if (!IsEmpty(ats)) rt.AddField(ats, Color.Yellow, RawTextFormatting.Straight, RawTextFormatting.None);
            rt.AddField(" ");
            return rt;
        }

        public void AddDescription(string description, Color color = Color.Gray, RawTextFormatting format=RawTextFormatting.Straight)
        {
            RawText txt = ParseStats();
            txt = AddAbility(txt);
            rawDescription = ParseDescription(description, color, txt, format);
            ItemDescription = AddRarity(rawDescription);
        }

        private RawText AddAbility(RawText txt)
        {
            if (Ability is not null)
            {
                string mana = Ability.ManaCost != 0 ? $"Mana Cost: {Ability.ManaCost}" : "";
                string cd = Ability.Cooldown != 0 ? $"Cooldown: {Ability.Cooldown}s" : "";
                string rawJson = $@"[{{""text"":""Item Ability: {Ability.Name} "",""italic"":false,""color"":""gold""}},{{""text"":""{EnumHelper.GetStringValue(Ability.Type)}"",""color"":""yellow"",""bold"":true}}]";
                txt.AddField(" ");
                txt._lines.Add(rawJson);
                if (Ability.Description.Contains("\\n") || Ability.Description.Contains("\n"))
                {
                    string[] lines = Ability.Description.Split("\n");
                    foreach (string line in lines)
                    {
                        txt.AddField(line, Color.Gray, RawTextFormatting.Straight);
                    }
                }
                else txt.AddField(Ability.Description, Color.Gray, RawTextFormatting.Straight);
                if (!IsEmpty(mana)) txt.AddField(mana, Color.Gray, RawTextFormatting.Straight);
                if (!IsEmpty(cd)) txt.AddField(cd, Color.Gray, RawTextFormatting.Straight);
            }
            return txt;
        }

        private RawText ParseDescription(string description, Color color, RawText text, params RawTextFormatting[] formattings)
        {
            if(description.Contains("\\n") || description.Contains("\n"))
            {
                string[] lines = description.Split("\n");
                foreach(string line in lines)
                {
                    text.AddField(line, color, RawTextFormatting.None, formattings);
                }
            }
            else
            {
                text.AddField(description, color, RawTextFormatting.None, formattings);
            }
            return text;
        }

        private RawText AddRarity(RawText rawDesc)
        {
            rawDesc.AddField(" ");
            if (Type != ItemType.ReforgeStone
               && Type != ItemType.Shears
               && Type != ItemType.BrewingIngredient
               && Type != ItemType.None)
            {
                rawDesc.AddField("This item can be reforged!", Color.DarkGray, RawTextFormatting.Straight);
            }
            rawDesc.AddField($"{EnumHelper.GetStringValue(Rarity)} {EnumHelper.GetStringValue(Type)}", color, RawTextFormatting.Straight, RawTextFormatting.Bold);
            return rawDesc;
        }

        public string Compile()
        {
            string itemname = ID;
            ItemNBT nbt = new ItemNBT();
            nbt.HideFlags = 31;
            ItemDisplay display = new ItemDisplay();
            display.AddLore(ItemDescription);
            display.AddName(ItemName);
            nbt.Display = display;
            nbt.Unbreakable = true;
            Item item = new Item(itemname, nbt);
            var cmd = new Give(SimpleSelector.@p);
            cmd.Compile(item);
            Command = cmd;
            return cmd.Compiled;
        }
    }

    /// <summary>
    /// Represents incapsulated item ability
    /// </summary>
    public sealed class ItemAbility
    {
        public string Name;
        public AbilityType Type;
        public string Description;
        public int ManaCost;
        public int Cooldown;

        /// <summary>
        /// Create a new item ability
        /// </summary>
        /// <param name="name">Name of the ability</param>
        /// <param name="description">Description of the ability</param>
        /// <param name="type">Type of the ability</param>
        /// <param name="manacost">Mana cost of the ability. 0 by default</param>
        /// <param name="cooldown">Cooldown of the ability. 0s by default.</param>
        public ItemAbility(string name, string description, AbilityType type=AbilityType.Passive, int manacost=0, int cooldown=0)
        {
            Name = name;
            Description = description;
            Type = type; ManaCost = manacost;
            Cooldown = cooldown;
        }
    }

    /// <summary>
    /// Represents type of skyblock item ability
    /// </summary>
    public enum AbilityType
    {
        [EnumValue("")] Passive,
        [EnumValue("RIGHT CLICK")] RightClick,
        [EnumValue("SNEAK")] Sneak,
        [EnumValue("DOUBLE RIGHT-CLICK")] DoubleRightClick
    }

    /// <summary>
    /// Represents type of skyblock item
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
        [EnumValue("")] None
    }

    /// <summary>
    /// Represents rarity of a custom skyblock item
    /// </summary>
    public enum ItemRarity
    {
        [RarityColor(Color.White)] [EnumValue("COMMON")] Common,
        [RarityColor(Color.Green)] [EnumValue("UNCOMMON")] Uncommon,
        [RarityColor(Color.Blue)] [EnumValue("RARE")] Rare,
        [RarityColor(Color.DarkPurple)] [EnumValue("EPIC")] Epic,
        [RarityColor(Color.Gold)] [EnumValue("LEGENDARY")] Legendary,
        [RarityColor(Color.LightPurple)] [EnumValue("MYTHIC")] Mythic,
        [RarityColor(Color.Red)] [EnumValue("SPECIAL")] Special,
        [RarityColor(Color.Red)] [EnumValue("VERY SPECIAL")] VerySpecial
    }
}
