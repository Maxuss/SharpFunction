using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents ban command. Equal to Minecraft's <code>/ban {target: game_profile} {reason: String}</code>
    /// </summary>
    public sealed class Ban : ICommand, ISelectorCommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Ban Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public Ban(EntitySelector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Compiles the /ban command
        /// </summary>
        /// <param name="reason">Reason for ban. None by default</param>
        public void Compile(string reason="")
        {
            Compiled = $"/ban {Selector.String()} {reason}";
        }
    }

    /// <summary>
    /// Represents ban-ip command. Equal to Minecraft's <code>/ban-ip {target: game_profile} {reason: String}</code>
    /// </summary>
    public sealed class BanIP : ICommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize BanIP Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public BanIP(EntitySelector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Compiles the /ban-ip command
        /// </summary>
        /// <param name="reason">Reason for ip ban. None by default</param>
        public void Compile(string reason = "")
        {
            Compiled = $"/ban-ip {Selector.String()} {reason}";
        }
    }

    public sealed class Banlist
    {

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize BanList Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Banlist(){}

        /// <summary>
        /// Compiles the /ban-ip command
        /// </summary>
        /// <param name="reason">Reason for ip ban. None by default</param>
        public void Compile(BanlistType type)
        {
            string d = string.Empty;
            switch(type)
            {
                case BanlistType.IPs:
                    d = "ips";
                    break;
                case BanlistType.Players:
                    d = "players";
                    break;
                default: goto case BanlistType.Players;
            }
            Compiled = $"/banlist {d}";
        }
    }

    public enum BanlistType
    {
        IPs,
        Players
    }
}
