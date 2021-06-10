using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents /enchant command. Equal to Minecraft's <code>/enchat {target: EntitySelector} {enchantment: ID} {level: int}</code>
    /// </summary>
    public sealed class Enchant : ICommand, ISelectorCommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Enchant Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Enchant(EntitySelector sel) => Selector = sel;

        /// <summary>
        /// Compile /enchant command
        /// </summary>
        /// <param name="enchant">Enchantment to enchant</param>
        /// <param name="lvl">Level of enchantment</param>
        public void Compile(Enchantment enchant, uint lvl)
        {
            string ench = Universal.EnumHelper.GetStringValue(enchant);
            Compiled = $"/enchant {Selector.String()} {ench} {lvl}";
        }
    }
    /// <summary>
    /// Represents minecraft enchantment id
    /// </summary>
    public enum Enchantment
    {
        [EnumValue("aqua_affinity")] AquaAffinity,
        [EnumValue("bane_of_arthropods")] BaneOfArthropods,
        [EnumValue("blast_protection")] BlastProtection,
        [EnumValue("channeling")] Channeling,
        [EnumValue("curse_of_binding")] CurseOfBinding,
        [EnumValue("curse_of_vanishing")] CurseOfVanishing,
        [EnumValue("depth_strider")] DepthStrider,
        [EnumValue("efficiency")] Efficiency,
        [EnumValue("feather_falling")] FeatherFalling,
        [EnumValue("fire_aspect")] FireAspect,
        [EnumValue("fire_protection")] FireProtection,
        [EnumValue("flame")] Flame,
        [EnumValue("fortune")] Fortune,
        [EnumValue("frost_walker")] FrostWalker,
        [EnumValue("impaling")] Impaling,
        [EnumValue("infinity")] Infinity,
        [EnumValue("knockback")] Knockback,
        [EnumValue("looting")] Looting,
        [EnumValue("loyalty")] Loyalty,
        [EnumValue("luck_of_the_sea")] LuckOfTheSea,
        [EnumValue("lure")] Lure,
        [EnumValue("mending")] Mending,
        [EnumValue("multishot")] Multishot,
        [EnumValue("piercing")] Piercing,
        [EnumValue("power")] Power,
        [EnumValue("projectile_protection")] ProjectileProtection,
        [EnumValue("protection")] Protection,
        [EnumValue("punch")] Punch,
        [EnumValue("quick_charge")] QuickCharge,
        [EnumValue("respiration")] Respiration,
        [EnumValue("riptide")] Riptide,
        [EnumValue("sharpness")] Sharpness,
        [EnumValue("silk_touch")] SilkTouch,
        [EnumValue("smite")] Smite,
        [EnumValue("soul_speed")] SoulSpeed,
        [EnumValue("sweeping_edge")] SweepingEdge,
        [EnumValue("thorns")] Thorns,
        [EnumValue("unbreaking")] Unbreaking
    }
}
