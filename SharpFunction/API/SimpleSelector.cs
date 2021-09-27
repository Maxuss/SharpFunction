using SharpFunction.Commands;

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
        public static EntitySelector a => new(Selector.AllPlayers);

        /// <summary>
        ///     Nearest player
        /// </summary>
        public static EntitySelector p => new(Selector.Nearest);

        /// <summary>
        ///     Current player
        /// </summary>
        public static EntitySelector s => new(Selector.Current);

        /// <summary>
        ///     All entities
        /// </summary>
        public static EntitySelector e => new(Selector.AllEntities);

        /// <summary>
        ///     Random player
        /// </summary>
        public static EntitySelector r => new(Selector.Random);
    }
}