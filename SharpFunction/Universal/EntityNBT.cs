using System;
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
        ///<summary>If the entity will have AI</summary>
        public bool? NoAI { get; set; } = null;
        ///<summary>If the will produce sounds</summary>
        public bool? Silent { get; set; } = null;
        ///<summary>If the entity will not take damage</summary>
        public bool? Invulnerable { get; set; } = null;
        ///<summary>If the entity will be glowing</summary>
        public bool? Glowing { get; set; } = null;
        ///<summary>If the entity will have its custom name visible all the time</summary>
        public bool? CustomNameVisible { get; set; } = null;
        ///<summary>Custom name of entity</summary>
        public SuperRawText? CustomName { get; set; } = null;
        /// <summary>Passengers, sitting on entity</summary>
        public EntityPassenger[]? Passengers { get; set; } = null;
#nullable disable

        /// <summary>
        /// Compiles tags to non jsonified NBT data
        /// </summary>
        /// <returns>Not jsonified NBT data, requiring wrapping</returns>
        public string Compile()
        {
            string full = string.Empty;
            // again, not the best practice but i dont really
            // know how to optimise this thing

            bool aiN = IsNull(NoAI);
            bool sN = IsNull(Silent);
            bool iN = IsNull(Invulnerable);
            bool gN = IsNull(Glowing);
            bool cnN = IsNull(CustomName);
            bool cnvN = IsNull(CustomNameVisible);
            bool pN = IsNull(Passengers);

            full += aiN ? "" : $"NoAI: {BoolToByte((bool)NoAI)},";
            full += sN ? "" : $"Silent: {BoolToByte((bool)Silent)},";
            full += iN ? "" : $"Invulnerable: {BoolToByte((bool)Invulnerable)},";
            full += gN ? "" : $"Glowing: {BoolToByte((bool)Glowing)},";
            full += cnN ? "" : $"CustomName: '{CustomName.Compile()}',";
            full += cnvN ? "" : $"CustomNameVisible: {BoolToByte((bool)CustomNameVisible)},";
            if (!pN)
            {
                full += "Passengers:[";
                foreach(EntityPassenger p in Passengers)
                {
                    full += IsNull(p.NBT) ? $"{{id:{p.ID}}}" : $"{{id:{p.ID}, {p.NBT}}}";
                    full += Passengers.Last().Equals(p) ? "" : ",";
                }
                full += "]";
            }
            return full;
        }
    }

    /// <summary>
    /// Represents specific NBT data for Armor Stands
    /// </summary>
    public sealed class ArmorStandNBT
    {
#nullable enable
        /// <summary>
        /// Will the armor stand have arms
        /// </summary>
        /// <value></value>
        public bool? ShowArms { get; set; } = null;
        /// <summary>
        /// Will the armor stand be invisible and have small hitbox
        /// </summary>
        /// <value></value>
        public bool? Marker { get; set; } = null;
        /// <summary>
        /// Will the armor stand be invisible
        /// </summary>
        /// <value></value>
        public bool? Invisible { get; set; } = null;
        /// <summary>
        /// Will the armor stand have plate 
        /// </summary>
        /// <value></value>
        public bool? NoBasePlate { get; set; } = null;
        /// <summary>
        /// Will the armor stand be small
        /// </summary>
        /// <value></value>
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
        /// [0] are boots<br/>[1] are legs<br/>[2] is body<br/>[3] is head<br/>
        /// Should be wrapped in quotes(e.g. \"value\")
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
                string a = hand;
                hs += Hands.Last().Equals(hand) ? $@"{{id: {a}, Count: 1b}}" : $@"{{id: {a}, Count: 1b}},";
            };
            foreach(string armor in Armor)
            {
                string b = armor;
                arm += Armor.Last().Equals(armor) ? $@"{{id: {b}, Count: 1b}}" : $@"{{id: {b}, Count: 1b}},";
            };
            string hands = $"[{hs}]";
            string armors = $"[{arm}]";
            Console.WriteLine(armors);
            return $"HandItems: {hands}, ArmorItems: {armors}";
        }
    }
}
