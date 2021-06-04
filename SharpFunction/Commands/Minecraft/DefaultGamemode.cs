using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Universal;
namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents defaultgamemode command. Equal to Minecraft's <code>/defaultgamemode {mode: Gamemode}</code>
    /// </summary>
    public sealed class DefaultGamemode : ICommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize DefaultGamemode Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="Gamemode"/>
        /// </summary>
        public DefaultGamemode(){ }

        /// <summary>
        /// Compile defaultgamemode command
        /// </summary>
        /// <param name="mode">Gamemode to set default</param>
        public void Compile(Gamemode mode)
        {
            string gm = EnumHelper.GetStringValue(mode);
            Compiled = $"/defaultgamemode {gm}";
        }
    }
}
