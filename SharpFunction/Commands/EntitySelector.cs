using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands
{
    /// <summary>
    /// Represents a selector for entities with it's parameters
    /// </summary>
    public struct EntitySelector
    {
        /// <summary>
        /// Type of entity to select
        /// </summary>
        public readonly Selector Selector { get; }

        /// <summary>
        /// Parameters for <see cref="Selector"/>
        /// </summary>
        public readonly SelectorParameters Parameters { get; }

        /// <summary>
        /// Create an Entity Selector with given arguments
        /// </summary>
        /// <param name="sel">Selector for entity</param>
        /// <param name="params">Parameters for selector</param>
        public EntitySelector(Selector sel, SelectorParameters @params)
        {
            Selector = sel;
            Parameters = @params;
        }

        /// <summary>
        /// Create an entity selector without extra parameters
        /// </summary>
        /// <param name="sel"></param>
        public EntitySelector(Selector sel)
        {
            Selector = sel;
            Parameters = new SelectorParameters("placeholder");
        }

        /// <summary>
        /// Initialize empty entity selector
        /// </summary>
        /// <param name="placeholder"></param>
        internal EntitySelector(string placeholder)
        {
            Selector = new Selector();
            Parameters = new SelectorParameters();
        }

        /// <summary>
        /// Convert parameters to string
        /// </summary>
        /// <returns>Parameters as string</returns>
        public string String()
        {
            string sel = string.Empty;
            switch(Selector)
            {
                case Selector.AllEntities:
                    sel = "@e";
                    break;
                case Selector.AllPlayers:
                    sel = "@a";
                    break;
                case Selector.Random:
                    sel = "@r";
                    break;
                case Selector.Current:
                    sel = "@s";
                    break;
                case Selector.Nearest:
                    sel = "@p";
                    break;
                default:
                    goto case Selector.Nearest;
            }
            string pr = Parameters.String();
            return $"{sel}{pr}";
        }
    }
}
