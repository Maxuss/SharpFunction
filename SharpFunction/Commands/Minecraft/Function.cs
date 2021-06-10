using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents function command. Equal to Minecraft's <code>/function {name: String}</code>
    /// </summary>
    public sealed class Function : ICommand
    {

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Function Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Function() { }

        /// <summary>
        /// Compiles the /function command
        /// </summary>
        /// <param name="name">Name of function to be executed</param>
        public void Compile(string name)
        {
            Compiled = $"/function {name}";
        }
    }
}
