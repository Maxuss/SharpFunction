using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands
{
    public struct Coordinate
    {
#nullable enable

        /// <summary>
        /// Tells whether position is relative. E.G. ~3 is relative position
        /// </summary>
        public bool? Relative { get; set; }
        
        /// <summary>
        /// Tells whether position is local and based on rotation. E.G. ^3 is local position
        /// </summary>
        public bool? Local { get; set; }

        /// <summary>
        /// Shows position in space
        /// </summary>
        public float Value { get; set; }


        /// <summary>
        /// Copy coordinate from origin
        /// </summary>
        /// <param name="origin">Original coordinate to copy from</param>
        public Coordinate(Coordinate origin)
        {
            Relative = origin.Relative;
            Value = origin.Value;
            Local = origin.Local;
        }

        /// <summary>
        /// Initialize a coordinate from position. Sets <see cref="Relative"/> to false
        /// </summary>
        /// <param name="pos">Position in space</param>
        public Coordinate(float pos)
        {
            Value = pos;
            Relative = false;
            Local = false;
        }

        /// <summary>
        /// Initialize a coordinate from position and relativity
        /// </summary>
        /// <param name="pos">Position in space</param>
        /// <param name="relative">Whether the position relative to player/command block postion</param>
        public Coordinate(float pos, bool relative)
        {
            Value = pos;
            Relative = relative;
            Local = false;
        }

        /// <summary>
        /// Initialize a coordinate from position and relativity
        /// </summary>
        /// <param name="pos">Position in space</param>
        /// <param name="relative">Whether the position relative to player/command block postion</param>
        /// <param name="caret">Whether the position is relative to rotation. </param>
        public Coordinate(float pos, bool? local)
        {
            Value = pos;
            Relative = false;
            Local = local;
        }

        /// <summary>
        /// Initialize a coordinate from coordinate string
        /// </summary>
        /// <param name="coord">Coordinate string. E.G. "~3" or "3.31" or "^-13".</param>
        public Coordinate(string coord)
        {
            if (coord.StartsWith("~"))
            {
                Value = float.Parse(coord.Replace("~", "").Replace(".", ","));
                Relative = true;
                Local = false;
            }
            else if(coord.StartsWith("^"))
            {
                Value = float.Parse(coord.Replace("^", "").Replace(".", ","));
                Relative = false;
                Local = true;
            }
            else
            {
                Value = float.Parse(coord.Replace(".", ","));
                Relative = false;
                Local = false;
            }
        }
#nullable disable
    }
}
