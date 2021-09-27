namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents ban command. Equal to Minecraft's <code>/ban {target: game_profile} {reason: String}</code>
    /// </summary>
    public sealed class Ban : ICommand, ISelectorCommand
    {
        /// <summary>
        ///     Initialize Ban Command class.<br />
        ///     See also: <br />
        ///     <seealso cref="EntitySelector" />
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector" /> to use</param>
        public Ban(EntitySelector selector)
        {
            Selector = selector;
        }

        public string Compiled { get; private set; }
        public EntitySelector Selector { get; set; }

        /// <summary>
        ///     Compiles the /ban command
        /// </summary>
        /// <param name="reason">Reason for ban. None by default</param>
        public void Compile(string reason = "")
        {
            Compiled = $"/ban {Selector.String()} {reason}";
        }
    }

    /// <summary>
    ///     Represents ban-ip command. Equal to Minecraft's <code>/ban-ip {target: game_profile} {reason: String}</code>
    /// </summary>
    public sealed class BanIP : ICommand
    {
        /// <summary>
        ///     Initialize BanIP Command class.<br />
        ///     See also: <br />
        ///     <seealso cref="EntitySelector" />
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector" /> to use</param>
        public BanIP(EntitySelector selector)
        {
            Selector = selector;
        }

        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        ///     Compiles the /ban-ip command
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
        ///     Compiles the /ban-ip command
        /// </summary>
        /// <param name="reason">Reason for ip ban. None by default</param>
        public void Compile(BanlistType type)
        {
            var d = string.Empty;
            switch (type)
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