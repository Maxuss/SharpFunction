using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents main interface inherited by all command classes
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Selector for entities
        /// </summary>
        public EntitySelector Selector { get; set; }

        /// <summary>
        /// Represents compiled version of command
        /// </summary>
        public string Compiled { get; }


    }
}
