using SharpFunction.RCON;

namespace SharpFunction.Commands
{
    /// <summary>
    ///     Represents a selector for entities with it's parameters
    /// </summary>
    public struct EntitySelector
    {
        /// <summary>
        ///     Type of entity to select
        /// </summary>
        public readonly Selector Selector { get; }

        /// <summary>
        ///     Parameters for <see cref="Selector" />
        /// </summary>
        public readonly SelectorParameters Parameters { get; }

        /// <summary>
        /// Name of player bound to selector. Null by default
        /// </summary>
        public string PlayerName { get; }

        /// <summary>
        ///     Create an Entity Selector with given arguments
        /// </summary>
        /// <param name="sel">Selector for entity</param>
        /// <param name="params">Parameters for selector</param>
        public EntitySelector(Selector sel, SelectorParameters @params)
        {
            Selector = sel;
            Parameters = @params;
            PlayerName = null;
        }

        /// <summary>
        ///     Create an entity selector without extra parameters
        /// </summary>
        /// <param name="sel"></param>
        public EntitySelector(Selector sel)
        {
            Selector = sel;
            Parameters = new SelectorParameters("placeholder");
            PlayerName = null;
        }

        public EntitySelector(Player player)
        {
            Selector = new Selector();
            Parameters = new SelectorParameters();
            PlayerName = player.Name;
        }

        /// <summary>
        ///     Initialize empty entity selector
        /// </summary>
        /// <param name="placeholder"></param>
        internal EntitySelector(string placeholder)
        {
            Selector = new Selector();
            Parameters = new SelectorParameters();
            PlayerName = null;
        }

        /// <summary>
        ///     Convert parameters to string
        /// </summary>
        /// <returns>Parameters as string</returns>
        public string String()
        {
            var sel = Selector switch
            {
                Selector.AllEntities => "@e",
                Selector.AllPlayers => "@a",
                Selector.Random => "@r",
                Selector.Current => "@s",
                Selector.Nearest => "@p",
                _ => PlayerName
            };

            var pr = Parameters.String();
            return $"{sel}{pr}";
        }
    }
}