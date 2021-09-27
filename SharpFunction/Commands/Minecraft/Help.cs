namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents a /help Command. Equal to Minecraft's <code>/help {page: int}</code>
    /// </summary>
    public sealed class Help : ICommand
    {
        /// <summary>
        ///     Compiled string of command
        /// </summary>
        public string Compiled { get; private set; }

        /// <summary>
        ///     Compiles Help command
        /// </summary>
        /// <param name="page">Help page</param>
        public void Compile(string page = "")
        {
            Compiled = $"/help {page}";
        }
    }
}