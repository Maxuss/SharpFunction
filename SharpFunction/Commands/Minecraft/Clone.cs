using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Commands;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents clone command. Equal to Minecraft's <code>/clone {pos1: Vector3} {pos2: Vector3} {destination: Vector3} {params}</code>
    /// </summary>
    public sealed class Clone : ICommand
    {
        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Ban Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="Vector3"/>
        /// </summary>
        public Clone() { }

        /// <summary>
        /// Compiles clone command
        /// </summary>
        /// <param name="corner1">Coordinates Vector3 representing start of structure to clone</param>
        /// <param name="corner2">Coordinates Vector3 representing end of structure to clone</param>
        /// <param name="destination">Coordinates Vector3 representing position to clone structure into</param>
        /// <param name="params">OPTIONAL: parameters for clone command</param>
        /// <param name="extra">OPTIONAL: extra parameters for clone command</param>
        /// <param name="filter">ONLY FOR <see cref="CloneParams.Filtered"/>: represents block to filter for cloning</param>
        public void Compile(Vector3 corner1,
                            Vector3 corner2,
                            Vector3 destination,
                            CloneParams @params=CloneParams.None,
                            ExtraCloneParams extra=ExtraCloneParams.None,
                            string filter=""
            )
        {
            string p1 = corner1.String();
            string p2 = corner2.String();
            string dest = destination.String();
            string extr = string.Empty;
            string kwextra = string.Empty;
            switch(@params)
            {
                case CloneParams.Replace:
                    extr = "replace";
                    break;
                case CloneParams.None:
                    extr = "";
                    break;
                case CloneParams.Masked:
                    extr = "masked";
                    break;
                case CloneParams.Filtered:
                    extr = "filtered";
                    switch(extra)
                    {
                        case ExtraCloneParams.None:
                            kwextra = "";
                            break;
                        case ExtraCloneParams.Force:
                            kwextra = "force";
                            break;
                        case ExtraCloneParams.Move:
                            kwextra = "move";
                            break;
                        case ExtraCloneParams.Normal:
                            kwextra = "normal";
                            break;
                    }
                    break;
            }

            Compiled = $"/clone {p1} {p2} {dest} {extr} {filter} {kwextra}";
        }
    }

    /// <summary>
    /// Parameters for cloning the structure
    /// </summary>
    public enum CloneParams
    {
        Replace,
        Masked,
        Filtered,
        None
    }

    /// <summary>
    /// Extra parameters for cloning the structure
    /// </summary>
    public enum ExtraCloneParams
    {
        Force,
        Move,
        Normal,
        None
    }
}
