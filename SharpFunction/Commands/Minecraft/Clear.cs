namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents clear command. Equal to Minecraft's <code>/clear {target: entity} {item: String} {maxCount : int}</code>
    /// </summary>
    public sealed class Clear : ICommand, ISelectorCommand
    {
        /// <summary>
        ///     Initialize Clear Command class.<br />
        ///     See also: <br />
        ///     <seealso cref="EntitySelector" />
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector" /> to use</param>
        public Clear(EntitySelector selector)
        {
            Selector = selector;
        }

        public string Compiled { get; private set; }
        public EntitySelector Selector { get; set; }

        /// <summary>
        ///     Compiles clear command
        /// </summary>
        /// <param name="itemName">Name of items to filter. None by default</param>
        /// <param name="maxCount">Amount of items to clear. All by default</param>
        public void Compile(string itemName = "", int maxCount = -1)
        {
            string count;
            count = maxCount == -1 ? "" : $"{maxCount}";
            Compiled = $"/clear {Selector.String()} {itemName} {count}";
        }
    }
}