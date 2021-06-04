using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents a Teleport command. Equal to Minecraft's <code>/tp {player: Entity} {position: float[3]/player: Entity}</code>
    /// </summary>
    public sealed class Teleport : ICommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Teleport Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public Teleport(EntitySelector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Compile teleport command from coordinates
        /// </summary>
        /// <param name="destination">Destination to teleport</param>
        /// <returns>Compiled command as string</returns>
        public void Compile(Vector3 destination)
        {
            string coords = destination.String();
            Compiled =  $"/tp {Selector.String()} {coords}";
        }

        /// <summary>
        /// Compile teleport command from entity position
        /// </summary>
        /// <param name="destination">Entity to teleport <i>to</i></param>
        /// <returns>Compiled command as string</returns>
        public void Compile(EntitySelector destination)
        {
            string coords = destination.String();
            Compiled = $"/tp {Selector.String()} {coords}";
        }
    }
}
