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
    /// Represents a slayer item with requirement
    /// </summary>
    public sealed class SlayerItem
    {
        /// <summary>
        /// Name of item
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of item
        /// </summary>
        public RawText Description { get; set; }
        /// <summary>
        /// Required level of slayer. None by default.
        /// </summary>
        public int SlayerRequirement { get; set; } = 0;
        /// <summary>
        /// Type of slayer. E.G. Zombie or Enderman
        /// </summary>
        public string SlayerName { get; set; }
        /// <summary>
        /// ID of item to give
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Rarity of item
        /// </summary>
        public ItemRarity Rarity { get; set; }
        /// <summary>
        /// Type of item
        /// </summary>
        public ItemType Type { get; set; }
        /// <summary>
        /// Whether the item will have enchantment glint
        /// </summary>
        #nullable enable
        public bool? Glint { get; set; } = null;
#nullable disable
        /// <summary>
        /// If the item has recipes
        /// </summary>
        public bool HasRecipes { get; set; } = false;
        /// <summary>
        /// Create new slayer item
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="description">Description of item</param>
        /// <param name="requirement">Requirement of slayer</param>
        /// <param name="slayername">Name of slayer mob</param>
        /// <param name="id">Item ID of item to give</param>
        /// <param name="glint">Whether the item will have enchantment glint</param>
        /// <param name="rarity">Rarity of item</param>
        /// <param name="type">Type of item</param>
        /// <param name="recipes">Whether the item has recipes</param>
        public SlayerItem(string name, RawText description, string id, ItemRarity rarity,
             ItemType type, int requirement=0,string slayername="", bool? glint=null, bool recipes=false)
        {
            Name = name;
            Description = description;
            SlayerRequirement = requirement;
            SlayerName = slayername;
            ID = id;
            Rarity = rarity;
            Type = type;
            Glint = glint;
            HasRecipes = recipes;
        }

        /// <summary>
        /// Creates slayer item from pre-made skyblock item
        /// </summary>
        /// <param name="item">Item to gather data from</param>
        /// <param name="slayername">Name of slayer mob</param>
        /// <param name="glint">Whether the item will have enchantment glint</param>
        /// <param name="recipes">Whether the item has recipes</param>
        /// <param name="requirement">Requirement of slayer</param>
        public SlayerItem(SkyblockItem item, int requirement = 0, 
            string slayername = "", bool? glint=false)
        {
            Description = item.ItemDescription;
            Name = item.DisplayName;
            SlayerRequirement = requirement;
            SlayerName = slayername;
            ID = item.ID;
            Rarity = item.Rarity;
            Type = item.Type;
            Glint = glint;
            HasRecipes = false;
        }

        /// <summary>
        /// Compiles the item into Item object
        /// </summary>
        /// <returns>Compiled item</returns>
        private Item CompileItem()
        {
            Item item = new(ID);
            ItemNBT nbt = new();
            nbt.EnchantmentData = Glint is null || !(bool)Glint ? null : "{}";
            nbt.HideFlags = 31;
            int last = Description._lines.IndexOf(Description._lines.Last());
            string j2 = Description._lines[last];
            string j1 = Description._lines[last - 1];
            Description._lines.RemoveAt(last);
            Description._lines.RemoveAt(last - 1);
            ItemDisplay disp = new();
            if (HasRecipes)
            {
                Description.AddField("");
                Description.AddField("Right click to view recipes!", Color.Yellow, RawTextFormatting.Straight);
            }
            if (SlayerRequirement > 0)
            {
                Description.AddField("");
                SuperRawText req = new();

                req.Append($"{SkyblockHelper.SLAYER_REQ}", Color.Red, RawTextFormatting.Straight);
                req.Append($" Requires {SlayerName} LVL {SlayerRequirement}", Color.DarkPurple, RawTextFormatting.Straight);
                Description.AddField(req);
            }
            Description._lines.Add(j1);
            Description._lines.Add(j2);
            disp.AddLore(Description);
            RawText name = new();
            name.AddField($"{Name}", SkyblockEnumHelper.GetRarityColor(Rarity), RawTextFormatting.Straight);
            disp.AddName(name);
            nbt.Display = disp;
            nbt.Unbreakable = true;
            item.NBT = nbt;
            return item;
        }

        /// <summary>
        /// Generates the command object
        /// </summary>
        /// <returns>Generated command object</returns>
        public Give Generate()
        {
            Item item = CompileItem();
            Give g = new(SimpleSelector.@p);
            g.Compile(item, 1);
            return g;
        }
        /// <summary>
        /// Generates the command
        /// </summary>
        /// <returns>Generated command string</returns>
        public string GenerateCommand()
        {
            return Generate().Compiled;
        }
    }
}
