using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents clear command. Equal to Minecraft's <code>/clear {target: entity} {item: String} {maxCount : int}</code>
    /// </summary>
    public sealed class Clear : ICommand, ISelectorCommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Clear Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public Clear(EntitySelector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Compiles clear command
        /// </summary>
        /// <param name="itemName">Name of items to filter. None by default</param>
        /// <param name="maxCount">Amount of items to clear. All by default</param>
        public void Compile(string itemName="", int maxCount=-1)
        {
            string count;
            if (maxCount == -1) count = "";
            else count = $"{maxCount}";
            Compiled = $"/clear {Selector.String()} {itemName} {count}";
        }
    }
}
