using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Commands.Minecraft;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents a /help Command. Equal to Minecraft's <code>/help {page: int}</code>
    /// </summary>
    public sealed class Help : ICommand
    {

        
        /// <summary>
        /// Compiled string of command
        /// </summary>
        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Help Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Help(){ }

        /// <summary>
        /// Compiles Help command
        /// </summary>
        /// <param name="page">Help page</param>
        public void Compile(string page="")
        {
            Compiled = $"/help {page}";
        }
    }
}
