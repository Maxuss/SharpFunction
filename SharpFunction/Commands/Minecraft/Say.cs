using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Say command. Equal to Minecraft's<code>/say {message: String}</code>
    /// </summary>
    public sealed class Say : ICommand
    {


        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Say Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        /// 
        public Say() { }

        /// <summary>
        /// Compiles say command
        /// </summary>
        /// <param name="message">message for /say command</param>
        public void Compile(string message)
        {
            Compiled = $"/say {message}";
        }
    }
}
