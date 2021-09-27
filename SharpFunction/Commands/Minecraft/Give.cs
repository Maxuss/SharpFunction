using SharpFunction.Universal;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents Give command. Equal to Minecraft's <code>/give {user} {item} {nbt} {amount}</code>
    /// </summary>
    public sealed class Give : ICommand, ISelectorCommand
    {
        /// <summary>
        ///     Initialize Give Command class.<br />
        ///     See also: <br />
        ///     <seealso cref="EntitySelector" />
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector" /> to use</param>
        public Give(EntitySelector selector)
        {
            Selector = selector;
        }

        public string Compiled { get; private set; }
        public EntitySelector Selector { get; set; }

        public void Compile(Item item, int amount = 1)
        {
            Compiled = $"/give {Selector.String()} {item.GetCompiled()} {amount}";
        }
    }
}