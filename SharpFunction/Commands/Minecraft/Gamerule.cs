using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = SharpFunction.Commands.Minecraft.GameruleValue;
namespace SharpFunction.Commands.Minecraft
{

    /// <summary>
    /// Represents Gamerule command. Equal to Minecraft's <code>/gamerule {gamerule: GameRule}</code>
    /// </summary>
    public sealed class Gamerule : ICommand
    {

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Gamerule Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="GV"/>
        /// </summary>
        public Gamerule() { }

        /// <summary>
        /// Compiles the /gamerule command
        /// </summary>
        /// <param name="rule">Gamerule to set</param>
        /// <param name="value">Value of gamerule</param>
        public void Compile(GV rule, object value)
        {
            GV[] integers = { GV.MaxCBChainLength, GV.MaxEntityCramming, GV.RandomTickSpeed, GV.SpawnRadius };
            if (integers.Contains(rule))
            {
                int v = (int)value;
                Compiled = $"/gamerule {Universal.EnumHelper.GetStringValue(rule)} {v}";
            }
            else
            {
                bool v = (bool)value;
                Compiled = $"/gamerule {Universal.EnumHelper.GetStringValue(rule)} {Helper.CommandHelper.BoolToString(v)}";
            }
        }
    }
}
