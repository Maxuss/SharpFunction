using SharpFunction.Universal;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents defaultgamemode command. Equal to Minecraft's <code>/defaultgamemode {mode: Gamemode}</code>
    /// </summary>
    public sealed class DefaultGamemode : ICommand
    {
        public string Compiled { get; private set; }

        /// <summary>
        ///     Compile defaultgamemode command
        /// </summary>
        /// <param name="mode">Gamemode to set default</param>
        public void Compile(Universal.Gamemode mode)
        {
            var gm = mode.GetStringValue();
            Compiled = $"/defaultgamemode {gm}";
        }
    }
}