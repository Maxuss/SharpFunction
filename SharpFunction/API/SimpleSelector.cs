using SharpFunction.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.API
{
    /// <summary>
    /// Represents simple selectors to use
    /// </summary>
    public static class SimpleSelector
    {
        /// <summary>
        /// All players
        /// </summary>
        public static EntitySelector @a = new EntitySelector(Selector.AllPlayers);
        /// <summary>
        /// Nearest player
        /// </summary>
        public static EntitySelector @p = new EntitySelector(Selector.Nearest);
        /// <summary>
        /// Current player
        /// </summary>
        public static EntitySelector @s = new EntitySelector(Selector.Current);
        /// <summary>
        /// All entities
        /// </summary>
        public static EntitySelector @e = new EntitySelector(Selector.AllEntities);
        /// <summary>
        /// Random player
        /// </summary>
        public static EntitySelector @r = new EntitySelector(Selector.Random);
    }
}
