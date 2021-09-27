namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Say command. Equal to Minecraft's<code>/say {message: String}</code>
    /// </summary>
    public sealed class Say : ICommand
    {
        public string Compiled { get; private set; }

        /// <summary>
        ///     Compiles say command
        /// </summary>
        /// <param name="message">message for /say command</param>
        public void Compile(string message)
        {
            Compiled = $"/say {message}";
        }
    }
}