using SharpFunction.Universal;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents gamemode command. Equal to Minecraft's <code>/gamemode {mode: Gamemode} {target: EntitySelector}</code>
    /// </summary>
    public sealed class Gamemode : ICommand
    {
        public string Compiled { get; private set; }

        /// <summary>
        ///     Compile gamemode command
        /// </summary>
        /// <param name="mode">Gamemode to set</param>
        /// <param name="target">Target to set gamemode for</param>
        public void Compile(Universal.Gamemode mode, EntitySelector target)
        {
            var t = target.String();
            var gm = EnumHelper.GetStringValue(mode);
            Compiled = $"/gamemode {gm} {t}";
        }
    }
}