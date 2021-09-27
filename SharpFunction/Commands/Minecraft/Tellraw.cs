using SharpFunction.Universal;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents tellraw command. Equal to Minecraft's <code>/tellraw {entity: EntitySelector} {rawtext}</code>
    /// </summary>
    public sealed class Tellraw : ICommand, ISelectorCommand
    {
        /// <summary>
        ///     Initialize Tellraw Command class.<br />
        ///     See also: <br />
        ///     <seealso cref="EntitySelector" />
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector" /> to use</param>
        public Tellraw(EntitySelector selector)
        {
            Selector = selector;
        }

        public string Compiled { get; private set; }
        public EntitySelector Selector { get; set; }

        /// <summary>
        ///     Compiles the command
        /// </summary>
        /// <param name="srt">Raw text to display</param>
        public void Compile(SuperRawText srt)
        {
            var tt = new TellText(srt);
            var raw = tt.Compiled;
            Compiled = $"/tellraw {Selector.String()} {raw}";
        }
    }
}