namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents main interface inherited by all command classes
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        ///     Represents compiled version of command
        /// </summary>
        public string Compiled { get; }
    }
}