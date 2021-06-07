using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.NullChecker;
namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents NBT data of item
    /// </summary>
    public sealed class ItemNBT
    {
#nullable enable
        /// <summary>
        /// Display (display: {...}) data of item
        /// </summary>
        public ItemDisplay? Display { get; set; } = null;
        /// <summary>
        /// Whether the item is unbreakable
        /// </summary>
        public bool? Unbreakable { get; set; } = null;
        /// <summary>
        /// Damage of item (if applicable)
        /// </summary>
        public int? Damage { get; set; } = null;
        /// <summary>
        /// Represents custom json model data of item
        /// </summary>
        public string? CustomModelData { get; set; } = null;
        /// <summary>
        /// Custom key:value pairs of NBT tags of item
        /// </summary>
        public string? CustomTags { get; set; } = null;
        /// <summary>
        /// Block ids on which this item can be placed (if applicable)
        /// </summary>
        public string? CanPlaceOn { get; set; } = null;
        /// <summary>
        /// Block ids which this item can destroy
        /// </summary>
        public string? CanDestroy { get; set; } = null;
        /// <summary>
        /// Which flags to hide from this item
        /// </summary>
        public int? HideFlags { get; set; } = null;
        /// <summary>
        /// Represents extra modifier data
        /// </summary>
        public string? ModifierData { get; set; } = null;
        /// <summary>
        /// Represents extra enchantments data
        /// </summary>
        public string? EnchantmentData { get; set; } = null;
        /// <summary>
        /// Represents an empty NBT data of item
        /// </summary>
        public static ItemNBT Empty { get => new ItemNBT(); }

        public string Compile()
        {
            string disp = !IsNull(Display) ? $"{Display.DisplayJSON}" : "";
            string unbr = !IsNull(Unbreakable) ? $"Unbreakable: {Commands.Helper.CommandHelper.BoolToByte((bool)Unbreakable)}" : "";
            string dmg = !IsNull(Damage) ? $"Damage: {Damage}" : "";
            string cmd = !IsNull(CustomModelData) ? $"CustomModelData: {CustomModelData}" : "";
            string ct = !IsNull(CustomTags) ? $"CustomTags: {CustomTags}" : "";
            string cpo = !IsNull(CanPlaceOn) ? $"CanPlaceOn: [{CanPlaceOn}]" : "";
            string cd = !IsNull(CanDestroy) ? $"CanDestroy: [{CanDestroy}]" : "";
            string hfl = !IsNull(HideFlags) ? $"HideFlags: {HideFlags}" : "";
            string md = !IsNull(ModifierData) ? $"AttributeModifiers: [{ModifierData}]" : "";
            string ed = !IsNull(EnchantmentData) ? $"Enchantments: [{EnchantmentData}]" : "";
            string[] all = { disp, unbr, dmg, cmd, ct, cpo, cd, hfl, md, ed };
            string full = "";
            foreach(string a in all)
            {
                full += IsEmpty(a) ? "" : $"{a}, ";
            }
            return $"{{{full}}}";
        }
#nullable disable
    }
}
