using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents tellraw command. Equal to Minecraft's <code>/tellraw {entity: EntitySelector} {rawtext}</code>
    /// </summary>
    public sealed class Tellraw : ICommand, ISelectorCommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Tellraw Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public Tellraw(EntitySelector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Compiles the command
        /// </summary>
        /// <param name="srt">Raw text to display</param>
        public void Compile(SuperRawText srt)
        {
            var tt = new TellText(srt);
            string raw = tt.Compiled;
            Compiled = $"/tellraw {Selector.String()} {raw}";
        }
    }
}
