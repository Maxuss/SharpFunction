using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    ///     Represents skyblock-like skull item
    /// </summary>
    public class SkyblockSkull : SkyblockItem
    {
        /// <inheritdoc cref="SkyblockItem(ItemType, ItemRarity, string, string)" />
        /// <param name="skullOwner">Owner of the skull. Can also be nbt texture data</param>
        /// <param name="itemname">ID of item to give</param>
        /// <param name="name">Display name of item</param>
        /// <param name="rarity">Rarity of item</param>
        /// <param name="type">Type of item</param>
        public SkyblockSkull(ItemType type, ItemRarity rarity, string name, string skullOwner) : base(type, rarity,
            name, "player_head")
        {
            SkullOwner = skullOwner;
        }

        /// <summary>
        ///     The owner of the skull
        /// </summary>
        private string SkullOwner { get; }

        /// <inheritdoc cref="SkyblockItem.Compile()" />
        public override string Compile()
        {
            var itemname = ID;
            var nbt = new ItemNBT();
            nbt.HideFlags = 31;
            var display = new ItemDisplay();
            display.AddLore(ItemDescription);
            display.AddName(ItemName);
            nbt.Display = display;
            nbt.Unbreakable = true;
            nbt.CustomTags = SkullOwner.StartsWith("{Id:[")
                ? $"SkullOwner: {SkullOwner}"
                : $"SkullOwner: \"{SkullOwner}\"";
            nbt.EnchantmentData = HasGlint ? "{}" : null;
            var item = new Item(itemname, nbt);
            var cmd = new Give(SimpleSelector.p);
            cmd.Compile(item);
            return cmd.Compiled;
        }
    }
}