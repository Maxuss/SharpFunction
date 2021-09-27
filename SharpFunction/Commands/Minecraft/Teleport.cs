namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents a Teleport command. Equal to Minecraft's
    ///     <code>/tp {player: Entity} {position: float[3]/player: Entity}</code>
    /// </summary>
    public sealed class Teleport : ICommand, ISelectorCommand
    {
        /// <summary>
        ///     Initialize Teleport Command class.<br />
        ///     See also: <br />
        ///     <seealso cref="EntitySelector" />
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector" /> to use</param>
        public Teleport(EntitySelector selector)
        {
            Selector = selector;
        }

        public string Compiled { get; private set; }
        public EntitySelector Selector { get; set; }

        /// <summary>
        ///     Compile teleport command from coordinates
        /// </summary>
        /// <param name="destination">Destination to teleport</param>
        /// <returns>Compiled command as string</returns>
        public void Compile(Vector3 destination)
        {
            var coords = destination.String();
            Compiled = $"/tp {Selector.String()} {coords}";
        }

        /// <summary>
        ///     Compile teleport command from entity position
        /// </summary>
        /// <param name="destination">Entity to teleport <i>to</i></param>
        /// <returns>Compiled command as string</returns>
        public void Compile(EntitySelector destination)
        {
            var coords = destination.String();
            Compiled = $"/tp {Selector.String()} {coords}";
        }
    }
}