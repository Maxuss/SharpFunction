using System;

namespace SharpFunction.RCON
{
    public enum PacketType
    {
        /// <summary>
        /// Response to command from server
        /// </summary>
        CommandResponse = 0,
        /// <summary>
        /// Command that will be sent to server
        /// </summary>
        Command = 2,
        /// <summary>
        /// RCON Login
        /// </summary>
        Login = 3
    }

    
    /// <summary>
    /// The packet that can be sent through RCON to Minecraft
    /// </summary>
    public readonly struct RconPacket
    {
        /// <summary>
        /// Length of packet
        /// </summary>
        public int Length { get; }
        /// <summary>
        /// ID of packet
        /// </summary>
        public int ID { get; }
        /// <summary>
        /// Type of packet
        /// </summary>
        public PacketType Type { get; }
        /// <summary>
        /// Payload of packet
        /// </summary>
        public string Contents { get; }

        /// <summary>
        /// Initialize new RCON packet
        /// </summary>
        /// <param name="length">Length of packet</param>
        /// <param name="id">UUID of packet</param>
        /// <param name="type">Type of packet</param>
        /// <param name="contents">Payload of packet</param>
        public RconPacket(int length, int id, PacketType type, string contents)
        {
            Length = length;
            ID = id;
            Type = type;
            Contents = contents;
        }
    }
}