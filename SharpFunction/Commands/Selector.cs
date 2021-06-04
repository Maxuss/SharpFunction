using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands
{
    /// <summary>
    /// Represents target selector for commands
    /// </summary>
    public enum Selector
    {
        /// <summary>
        /// Equal to @p
        /// </summary>
        Nearest,
        /// <summary>
        /// Equal to @r
        /// </summary>
        Random,
        /// <summary>
        /// Equal to @a
        /// </summary>
        AllPlayers,
        /// <summary>
        /// Equal to @e
        /// </summary>
        AllEntities,
        /// <summary>
        /// Equal to @s
        /// </summary>
        Current
    }
}
