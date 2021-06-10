using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents Give command. Equal to Minecraft's <code>/give {user} {item} {nbt} {amount}</code>
    /// </summary>
    public sealed class Give : ICommand, ISelectorCommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Give Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public Give(EntitySelector selector)
        {
            Selector = selector;
        }

        public void Compile(Item item, int amount=1)
        {
            
            Compiled = $"/give {Selector.String()} {item.GetCompiled()} {amount}";
        }
    }
}
