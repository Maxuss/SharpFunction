using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents /execute command. Equal to Minecraft's <code>/execute {tons of params}</code>
    /// </summary>
    public sealed class Execute : ICommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize DefaultGamemode Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Execute() { };
    }
}
