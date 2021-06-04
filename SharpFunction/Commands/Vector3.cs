using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands
{
    /// <summary>
    /// Represents coordinates in ingame world
    /// </summary>
    public struct Vector3
    {
#nullable enable
        /// <summary>
        /// X coordinate in space
        /// </summary>
        public readonly Coordinate x { get; }
        /// <summary>
        /// Y coordinate in space
        /// </summary>
        public readonly Coordinate y { get; }
        /// <summary>
        /// Z coordinate in space
        /// </summary>
        public readonly Coordinate z { get; }
#nullable disable
        /// <summary>
        /// Copy Vector from origin
        /// </summary>
        /// <param name="origin">Vector with specified coordinates</param>
        public Vector3(Vector3 origin)
        {
            x = origin.x;
            y = origin.y;
            z = origin.z;
        }

        /// <summary>
        /// Initialize Vector from specified coordinates
        /// </summary>
        /// <param name="X">X Coordinate</param>
        /// <param name="Y">Y Coordinate</param>
        /// <param name="Z">Z Coordinate</param>
        public Vector3(Coordinate X, Coordinate Y, Coordinate Z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        /// <summary>
        /// Initialize Vector from specified float location
        /// </summary>
        /// <param name="X">X Coordinate</param>
        /// <param name="Y">Y Coordinate</param>
        /// <param name="Z">Z Coordinate</param>
        public Vector3(float X, float Y, float Z)
        {
            x = new Coordinate(X);
            y = new Coordinate(Y);
            z = new Coordinate(Z);
        }

        /// <summary>
        /// Initialize 3D Vector from coordinates string
        /// </summary>
        /// <param name="coordinates">Coordinates in a string. E.G. "~4 ~3 14.1" or "^0 ^1 ^0"</param>
        public Vector3(string coordinates)
        {
            string[] @p = coordinates.Split(" ");
            x = new Coordinate(@p[0]);
            y = new Coordinate(@p[1]);
            z = new Coordinate(@p[2]);
        }

        /// <summary>
        /// Initialize Vector from specified float location
        /// </summary>
        /// <param name="X">X Coordinate</param>
        /// <param name="Y">Y Coordinate</param>
        /// <param name="Z">Z Coordinate</param>
        /// <param name="relative">Whether coordinates are relative</param>
        public Vector3(float X, float Y, float Z, bool relative)
        {
            x = new Coordinate(X, relative);
            y = new Coordinate(Y, relative);
            z = new Coordinate(Z, relative);
        }

#nullable enable
        /// <summary>
        /// Coverts Position to game coordinates
        /// </summary>
        /// <returns>Vector as ingame coordinate string</returns>
        public string String()
        {
            Coordinate[] coords = { x, y, z };
            string crd = string.Empty;
            foreach(Coordinate coord in coords)
            {
                string tmp = string.Empty;
                tmp += (coord.Relative != null && (bool)coord.Relative) ? "~" : (coord.Local != null && (bool)coord.Local) ? "^" : "";
                tmp += $"{coord.Value}";
                crd += $"{tmp} ";
            }
            return crd.Replace(",", ".");
        }
#nullable disable
    }
}
