namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents kill command. Equal to Minecraft's <code>/kill {entity: EntitySelector}</code>
    /// </summary>
    public sealed class Kill : ICommand, ISelectorCommand
    {
        /// <inheritdoc cref="ISelectorCommand.Selector"/>
        public EntitySelector Selector { get; set; }

        /// <inheritdoc cref="ICommand.Compiled"/>
        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Kill Command class.<br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public Kill(EntitySelector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Compiles the comma
        /// </summary>
        public void Compile()
        {
            Compiled = $"/kill {Selector.String()}";
        }

    }
}