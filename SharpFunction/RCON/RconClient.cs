using System;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;

namespace SharpFunction.RCON
{
    /// <summary>
    /// Client that listens to MC packets and can send them
    /// </summary>
    public sealed class RconClient
    {
        /// <summary>
        /// Main TCP Client of RCON
        /// </summary>
        public TcpClient Client { get; }
        /// <summary>
        /// Packet manager of the client
        /// </summary>
        public PacketManager PacketManager { get; set; }
        /// <summary>
        /// Stream to write/read data
        /// </summary>
        public NetworkStream Stream { get; set; }
        private string _password { get; }
        
        /// <summary>
        /// Initialize a new RCON client
        /// </summary>
        /// <param name="port">Port of the server</param>
        /// <param name="password">Password to login to server</param>
        /// <param name="ip">IP of server (localhost by default)</param>
        public RconClient(ushort port, string password, string ip="127.0.0.1")
        {
            Client = new TcpClient(ip, port);
            _password = password;
            Stream = Client.GetStream();
        }

        /// <summary>
        /// Authenticates client
        /// </summary>
        public void Authenticate()
        {
            RconPacket pkt;
            var packetData = new RconPacket(_password.Length + PacketManager.HeaderLength, Interlocked.Increment(ref PacketManager.LastId), PacketType.Login, _password);
            var auth = PacketManager.SendPacket(packetData, out pkt);
            if (!auth) throw new AuthenticationException($"Could not authenticate to server! Error: {pkt.Contents}");
        }

        /// <summary>
        /// Gets the player from the server
        /// </summary>
        /// <param name="name">Name of player</param>
        /// <returns>Player on the server</returns>
        public Player GetPlayer(string name)
        {
            return new Player(this, name);
        }
    }
}