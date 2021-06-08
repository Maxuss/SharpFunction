﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.NullChecker;
using static SharpFunction.Commands.Helper.CommandHelper;
namespace SharpFunction.Universal
{
    /// <summary>
    /// Contains some simple NBT data of an entity
    /// </summary>
    public sealed class EntityNBT
    {
#nullable enable
        public bool? NoAI { get; set; } = null;
        public bool? Silent { get; set; } = null;
        public bool? Invulnerable { get; set; } = null;
        public bool? Glowing { get; set; } = null;
        public bool? CustomNameVisible { get; set; } = null;
        public SuperRawText? CustomName { get; set; } = null;
#nullable disable

        /// <summary>
        /// Compiles tags to non jsonified NBT data
        /// </summary>
        /// <returns>Not jsonified NBT data, requiring wrapping</returns>
        public string Compile()
        {
            string full = string.Empty;
            string rawName = CustomName.Compile();
            full += IsNull(NoAI) ? "" : $"NoAI: {BoolToByte((bool)NoAI)},";
            full += IsNull(Silent) ? "" : $"Silent: {BoolToByte((bool)Silent)},";
            full += IsNull(Invulnerable) ? "" : $"Invulnerable: {BoolToByte((bool)Invulnerable)},";
            full += IsNull(Glowing) ? "" : $"Glowing: {BoolToByte((bool)Glowing)},";
            full += IsNull(CustomName) ? "" : $"CustomName: '{rawName}',";
            full += IsNull(CustomNameVisible) ? "" : $"CustomNameVisible: {BoolToByte((bool)CustomNameVisible)}";
            return full;
        }
    }

    /// <summary>
    /// Represents specific NBT data for Armor Stands
    /// </summary>
    public sealed class ArmorStandNBT
    {
#nullable enable
        public bool? ShowArms { get; set; } = null;
        public bool? Marker { get; set; } = null;
        public bool? Invisible { get; set; } = null;
        public bool? NoBasePlate { get; set; } = null;
        public bool? Small { get; set; } = null;
#nullable disable

        /// <summary>
        /// Compiles the nbt tag of armor stand
        /// </summary>
        /// <returns>Compiled NBT non-JSON string</returns>
        public string Compile()
        {
            string full = string.Empty;
            full += IsNull(ShowArms) ? "" : $"ShowArms: {BoolToByte((bool)ShowArms)},";
            full += IsNull(Marker) ? "" : $"Marker: {BoolToByte((bool)Marker)},";
            full += IsNull(Invisible) ? "" : $"Invisible: {BoolToByte((bool)Invisible)},";
            full += IsNull(NoBasePlate) ? "" : $"NoBasePlate: {BoolToByte((bool)NoBasePlate)},";
            full += IsNull(Small) ? "" : $"Small: {BoolToByte((bool)Small)}";
            return full;
        }
    }

    /// <summary>
    /// Represents armor inventory of entity
    /// </summary>
    public sealed class ArmorItems
    {
        /// <summary>
        /// [0] is right hand, [1] is left hand
        /// </summary>
        public string[] Hands { get; set; } = new string[2] {"", ""};
        /// <summary>
        /// [0] are boots<br/>[1] are legs<br/>[2] is body<br/>[3] is head
        /// </summary>
        public string[] Armor { get; set; } = new string[4] { "", "", "", "" };

        /// <summary>
        /// Initializes armor items class
        /// </summary>
        /// <param name="hands">Items to store in hands</param>
        /// <param name="armor">Armor for entity to wear</param>
        public ArmorItems(string[] hands, string[] armor)
        {
            Hands = hands;
            Armor = armor;
        }

        /// <summary>
        /// Compiles the string to nbt data
        /// </summary>
        /// <returns>NBT data in not fully jsonified string</returns>
        public string Compile()
        {
            string hs = "";
            string arm = "";
            foreach(string hand in Hands)
            {
                string a = hand.StartsWith("minecraft:") ? hand : $"minecraft:{hand}";
                hs += Hands.Last().Equals(hand) ? $@"{{id: ""{a}"", Count: 1b}}" : $@"{{id: ""{a}"", Count: 1b}},";
            };
            foreach(string armor in Armor)
            {
                string b = armor.StartsWith("minecraft:") ? armor : $"minecraft:{armor}";
                arm += Hands.Last().Equals(armor) ? $@"{{id: ""{b}"", Count: 1b}}" : $@"{{id: ""{b}"", Count: 1b}},";
            };
            string hands = $"[{hs}]";
            string armors = $"[{arm}]";
            Console.WriteLine(armors);
            return $"HandItems: {hands}, ArmorItems: {armors}";
        }
    }
}