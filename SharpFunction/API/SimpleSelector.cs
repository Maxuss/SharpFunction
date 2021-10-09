using SharpFunction.Commands;
using SharpFunction.RCON;

namespace SharpFunction.API
{
    /// <summary>
    ///     Represents simple selectors to use
    /// </summary>
    public struct SimpleSelector
    {
        /// <summary>
        ///     All players
        /// </summary>
        public static EntitySelector All => new(Selector.AllPlayers);

        /// <summary>
        ///     Nearest player
        /// </summary>
        public static EntitySelector Nearest => new(Selector.Nearest);

        /// <summary>
        ///     Current player
        /// </summary>
        public static EntitySelector Current => new(Selector.Current);

        /// <summary>
        ///     All entities
        /// </summary>
        public static EntitySelector AllEntities => new(Selector.AllEntities);

        /// <summary>
        ///     Random player
        /// </summary>
        public static EntitySelector Random => new(Selector.Random);

        /// <summary>
        /// Gets an entity selector for provided player name
        /// </summary>
        /// <param name="name">Name of player</param>
        /// <returns>Compiled entity selector</returns>
        public static EntitySelector Player(string name)
        {
            var sel = new EntitySelector(new Player(null, name));
            return sel;
        }
    }
}