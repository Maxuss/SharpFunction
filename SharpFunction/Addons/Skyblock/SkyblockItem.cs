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
using System.Reflection;

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
        /// Whether the item have crafts
        /// </summary>
        public bool HasCrafts { get; set; }
        /// <summary>
        /// Rarity of an item
        /// </summary>
        public ItemRarity Rarity { get; set; }

        private Color color;
#nullable enable
        /// <summary>
        /// Damage of item
        /// </summary>
        [StatName("Damage")] public int? Damage { get; set; } = null;
        /// <summary>
        /// Strength of item
        /// </summary>
        [StatName("Strength")] public int? Strength { get; set; } = null;
        /// <summary>
        /// Defense of item
        /// </summary>
        [StatName("Defense")] public int? Defense { get; set; } = null;
        /// <summary>
        /// Health of item
        /// </summary>
        [StatName("Health")] public int? Health { get; set; } = null;
        /// <summary>
        /// Intelligence of item
        /// </summary>
        [StatName("Intelligence")] public int? Intelligence { get; set; } = null;
        /// <summary>
        /// Crit Chance of item
        /// </summary>
        [StatName("Crit Chance")] public int? CritChance { get; set; } = null;
        /// <summary>
        /// Crit Damage of item
        /// </summary>
        [StatName("Crit Damage")] public int? CritDamage { get; set; } = null;
        /// <summary>
        /// Attack Speed of item
        /// </summary>
        [StatName("Bonus Attack Speed")] public int? AttackSpeed { get; set; } = null;
        /// <summary>
        /// Speed of item
        /// </summary>
        [StatName("Speed")] public int? Speed { get; set; } = null;
        /// <summary>
        /// True Defense of item
        /// </summary>
        [StatName("True Defense")] public int? TrueDefense { get; set; } = null;
        /// <summary>
        /// SCC of item
        /// </summary>
        [StatName("Sea Creature Chance")] public int? SeaCreatureChance { get; set; } = null;
        /// <summary>
        /// Magic Find of item
        /// </summary>
        [StatName("Magic Find")] public int? MagicFind { get; set; } = null;
        /// <summary>
        /// Pet Luck of item
        /// </summary>
        [StatName("Pet Luck")] public int? PetLuck { get; set; } = null;
        /// <summary>
        /// Pet Luck of item
        /// </summary>
        [StatName("Ferocity")] public int? Ferocity { get; set; } = null;

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
            ID = itemname;
            DisplayName = name;
            RawText _name = new();
            _name.AddField(name, color, RawTextFormatting.Straight, RawTextFormatting.None);
            ItemName = _name;
        }

        private RawText ParseStats()
        {
            RawText rt = new();
            Dictionary<string, int?> red = new()
            {
                { "Damage", Damage },
                { "Strength", Strength },
                { "Bonus Attack Speed", AttackSpeed },
                { "Crit Chance", CritChance },
                { "Crit Damage", CritDamage },
                { "Sea Creature Chance", SeaCreatureChance }
            };
            Dictionary<string, int?> green = new()
            {
                { "Health", Health},
                { "Defense", Defense},
                { "Speed", Speed},
                { "Intelligence", Intelligence},
                { "Magic Find", MagicFind},
                { "Pet Luck", PetLuck },
                { "True Defense", TrueDefense},
                { "Ferocity", Ferocity}
            };
            List<string> percented = new[] { "Crit Chance", "Crit Damage", "Sea Creature Chance", "Bonus Attack Speed" }.ToList();

            Action<RawText> space = VoidEncapsulator<RawText>.Encapsulate(m =>
            {
                m.AddField("");
            });

            var a = red.Where(predicate => 
            {
                return !IsNull(predicate.Value); 
            });
            var b = green.Where(predicate =>
            {
                return !IsNull(predicate.Value);
            });

            Action<KeyValuePair<string, int?>> addRed = VoidEncapsulator<KeyValuePair<string, int?>>.Encapsulate(p =>
            {
                string name = p.Key;
                string stat = percented.Contains(name) ? $"+{p.Value}%" : $"+{p.Value}";
                string n = $"{name}: ";
                SuperRawText srt = new();
                srt.Append(n, Color.Gray, RawTextFormatting.Straight);
                srt.Append(stat, Color.Red, RawTextFormatting.Straight);
                rt.AddField(srt);
                if(a.Last().Equals(p) && b is not null)
                {
                    space(rt);
                }
            });

            Action<KeyValuePair<string, int?>> addGreen = VoidEncapsulator<KeyValuePair<string, int?>>.Encapsulate(p =>
            {
                string name = p.Key;
                string stat = percented.Contains(name) ? $"+{p.Value}%" : $"+{p.Value}";
                string n = $"{name}: ";
                SuperRawText srt = new();
                srt.Append(n, Color.Gray, RawTextFormatting.Straight);
                srt.Append(stat, Color.Green, RawTextFormatting.Straight);
                rt.AddField(srt);
            });

            foreach(KeyValuePair<string, int?> r in a)
            {
                addRed(r);
            }
            foreach(KeyValuePair<string, int?> g in b)
            {
                addGreen(g);
            }

            if(a is not null && b is not null)
            {
                space(rt);
            }   
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
                string rawJson = $@"[{{""text"":""Item Ability: {Ability.Name} "",""italic"":false,""color"":""gold""}},{{""text"":""{EnumHelper.GetStringValue(Ability.Type)}"",""color"":""yellow"",""bold"":true}}]";
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
                    cd.Append("Cooldown: ", Color.Green);
                    cd.Append($"{Ability.Cooldown}s", Color.Green);
                    txt.AddField(cd);
                }
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
            ItemType[] nonreforgeable = new[]
            {
                ItemType.ReforgeStone, ItemType.Shears, ItemType.BrewingIngredient, ItemType.None
            };
            if (!nonreforgeable.Contains(Type))
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
