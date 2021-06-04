using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents gamemode command. Equal to Minecraft's <code>/gamemode {mode: Gamemode} {target: EntitySelector}</code>
    /// </summary>
    public sealed class Gamemode : ICommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Gamemode Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="Universal.Gamemode"/>
        /// </summary>
        public Gamemode() { }

        /// <summary>
        /// Compile gamemode command
        /// </summary>
        /// <param name="mode">Gamemode to set</param>
        /// <param name="target">Target to set gamemode for</param>
        public void Compile(Universal.Gamemode mode, EntitySelector target)
        {
            string t = target.String();
            string gm = Universal.EnumHelper.GetStringValue(mode);
            Compiled = $"/gamemode {gm} {t}";
        }
    }
}
