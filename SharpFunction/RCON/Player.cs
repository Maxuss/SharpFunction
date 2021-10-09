using SharpFunction.Commands;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;

namespace SharpFunction.RCON
{
    /// <summary>
    /// A player on the server
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Name of the player
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// Client owner of player
        /// </summary>
        public RconClient ParentClient { get; }

        internal Player(RconClient cl, string name)
        {
            ParentClient = cl;
            Name = name;
        }

        /// <summary>
        /// Gives item to the player
        /// </summary>
        /// <param name="item">Item to be given</param>
        /// <param name="amount">Amount of items to be given. 1 by default</param>
        /// <returns>Command response.</returns>
        public string GiveItem(Item item, int amount=1)
        {
            var cmd = new Give(new EntitySelector(this));
            cmd.Compile(item, amount);
            return ParentClient.PacketManager.ExecuteCommand(cmd.Compiled);
        }
    }
}