namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Secondary interface for commands that have EntitySelector field
    /// </summary>
    public interface ISelectorCommand
    {
        /// <summary>
        ///     Entity selector to use
        /// </summary>
        public EntitySelector Selector { get; set; }
    }
}