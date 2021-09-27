namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents function command. Equal to Minecraft's <code>/function {name: String}</code>
    /// </summary>
    public sealed class Function : ICommand
    {
        public string Compiled { get; private set; }

        /// <summary>
        ///     Compiles the /function command
        /// </summary>
        /// <param name="name">Name of function to be executed</param>
        public void Compile(string name)
        {
            Compiled = $"/function {name}";
        }
    }
}