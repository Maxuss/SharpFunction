using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SharpFunction.Commands;

namespace SharpFunction.RCON
{
    /// <summary>
    /// Manager that is responsible for sending, receiving and encoding packets
    /// </summary>
    public class PacketManager
    {
        /// <summary>
        /// Length of packet header
        /// </summary>
        public const int HeaderLength = 10;
        /// <summary>
        /// Maximum possible packet length
        /// </summary>
        public const int MaximumPacketLength = 4110;
        private RconClient _parent;

        /// <summary>
        /// Parent stream
        /// </summary>
        public NetworkStream Stream => _parent.Stream;
        /// <summary>
        /// Parent client
        /// </summary>
        public TcpClient Client => _parent.Client;
        internal int LastId = 0;

        /// <summary>
        /// Creates new packet manager instance
        /// </summary>
        /// <param name="client">Parent RCON Client</param>
        public PacketManager(RconClient client)
        {
            _parent = client;
        }
        
        /// <summary>
        /// Encodes the packet
        /// </summary>
        /// <param name="packet">Packet to be encoded</param>
        /// <returns>Byte data of packet</returns>
        public byte[] EncodePacket(RconPacket packet)
        {
            var bytes = new List<byte>();
            
            // length
            bytes.AddRange(BitConverter.GetBytes(packet.Length));
            // uuid
            bytes.AddRange(BitConverter.GetBytes(packet.ID));
            // type
            bytes.AddRange(BitConverter.GetBytes(packet.Type.GetHashCode()));
            // payload
            bytes.AddRange(Encoding.GetEncoding("ISO-8859-1").GetBytes(packet.Contents));
            // pad
            bytes.AddRange(new byte[] { 0, 0 });

            return bytes.ToArray();
        }

        /// <summary>
        /// Decodes packet from input
        /// </summary>
        /// <param name="bytes">Byte packet data</param>
        /// <returns>Decoded packet</returns>
        public RconPacket DecodePacket(byte[] bytes)
        {
            var length = BitConverter.ToInt32(bytes, 0);
            var requestId = BitConverter.ToInt32(bytes, 4);
            var type = BitConverter.ToInt32(bytes, 8);
            var payloadLength = bytes.Length - (HeaderLength + 4);

            if (payloadLength <= 0) return new RconPacket(length, requestId, (PacketType) type, "");
            var payloadBytes = new byte[payloadLength];
            Array.Copy(bytes, 12, payloadBytes, 0, payloadLength);
            Array.Resize(ref payloadBytes, payloadLength);
            return new RconPacket(length, requestId, (PacketType)type, Encoding.GetEncoding("ISO-8859-1").GetString(payloadBytes));
        }

        /// <summary>
        /// Executes a single command
        /// </summary>
        /// <param name="command">A command to be executed</param>
        /// <returns>Return value of command</returns>
        public string ExecuteCommand(string command)
        {
            RconPacket response;
            var packetdata = new RconPacket(command.Length + HeaderLength, Interlocked.Increment(ref LastId), PacketType.Command, command);
            _parent.Authenticate();
            SendPacket(packetdata, out response);
            return response.Contents;
        }

        /// <summary>
        /// Executes all commands from your command module
        /// </summary>
        /// <param name="cmds">All commands to be executed</param>
        /// <returns>Return values of command execution</returns>
        public IEnumerable<string> ExecuteCommands(CommandModule cmds)
        {
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var cmd in cmds.CommandLines)
            {
                yield return ExecuteCommand(cmd);
            }
        }

        /// <summary>
        /// Sends the packet to server
        /// </summary>
        /// <param name="packet">Packet to be sent</param>
        /// <param name="responsePacket">Response packet to be written response data into.</param>
        /// <returns>Success of this operation</returns>
        public bool SendPacket(RconPacket packet, out RconPacket responsePacket)
        {
            var encoded = EncodePacket(packet);
            
            Stream.Write(encoded, 0, encoded.Length);

            var response = new byte[MaximumPacketLength];
            var read = Stream.Read(response, 0, response.Length);
            Array.Resize(ref response, read);

            responsePacket = DecodePacket(response);
            return packet.ID == responsePacket.ID;
        }
    }
}