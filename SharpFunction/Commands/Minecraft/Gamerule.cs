using System.Linq;
using SharpFunction.Commands.Helper;
using SharpFunction.Universal;
using GV = SharpFunction.Commands.Minecraft.GameruleValue;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents Gamerule command. Equal to Minecraft's <code>/gamerule {gamerule: GameRule}</code>
    /// </summary>
    public sealed class Gamerule : ICommand
    {
        public string Compiled { get; private set; }

        /// <summary>
        ///     Compiles the /gamerule command
        /// </summary>
        /// <param name="rule">Gamerule to set</param>
        /// <param name="value">Value of gamerule</param>
        public void Compile(GV rule, object value)
        {
            GV[] integers = {GV.MaxCBChainLength, GV.MaxEntityCramming, GV.RandomTickSpeed, GV.SpawnRadius};
            if (integers.Contains(rule))
            {
                var v = (int) value;
                Compiled = $"/gamerule {EnumHelper.GetStringValue(rule)} {v}";
            }
            else
            {
                var v = (bool) value;
                Compiled = $"/gamerule {EnumHelper.GetStringValue(rule)} {CommandHelper.BoolToString(v)}";
            }
        }
    }
}