namespace SharpFunction.Commands
{
    /// <summary>
    ///     Represents coordinates in ingame world
    /// </summary>
    public struct Vector3
    {
#nullable enable
        /// <summary>
        ///     X coordinate in space
        /// </summary>
        public readonly Coordinate X { get; }

        /// <summary>
        ///     Y coordinate in space
        /// </summary>
        public readonly Coordinate Y { get; }

        /// <summary>
        ///     Z coordinate in space
        /// </summary>
        public readonly Coordinate Z { get; }
#nullable disable
        /// <summary>
        ///     Copy Vector from origin
        /// </summary>
        /// <param name="origin">Vector with specified coordinates</param>
        public Vector3(Vector3 origin)
        {
            X = origin.X;
            Y = origin.Y;
            Z = origin.Z;
        }

        /// <summary>
        ///     Initialize Vector from specified coordinates
        /// </summary>
        /// <param name="X">X Coordinate</param>
        /// <param name="Y">Y Coordinate</param>
        /// <param name="Z">Z Coordinate</param>
        public Vector3(Coordinate X, Coordinate Y, Coordinate Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /// <summary>
        ///     Initialize Vector from specified float location
        /// </summary>
        /// <param name="X">X Coordinate</param>
        /// <param name="Y">Y Coordinate</param>
        /// <param name="Z">Z Coordinate</param>
        public Vector3(float X, float Y, float Z)
        {
            this.X = new Coordinate(X);
            this.Y = new Coordinate(Y);
            this.Z = new Coordinate(Z);
        }

        /// <summary>
        ///     Initialize 3D Vector from coordinates string
        /// </summary>
        /// <param name="coordinates">Coordinates in a string. E.G. "~4 ~3 14.1" or "^0 ^1 ^0"</param>
        public Vector3(string coordinates)
        {
            var p = coordinates.Split(" ");
            X = new Coordinate(p[0]);
            Y = new Coordinate(p[1]);
            Z = new Coordinate(p[2]);
        }

        /// <summary>
        ///     Initialize Vector from specified float location
        /// </summary>
        /// <param name="X">X Coordinate</param>
        /// <param name="Y">Y Coordinate</param>
        /// <param name="Z">Z Coordinate</param>
        /// <param name="relative">Whether coordinates are relative</param>
        public Vector3(float X, float Y, float Z, bool relative)
        {
            this.X = new Coordinate(X, relative);
            this.Y = new Coordinate(Y, relative);
            this.Z = new Coordinate(Z, relative);
        }

#nullable enable
        /// <summary>
        ///     Coverts Position to game coordinates
        /// </summary>
        /// <returns>Vector as ingame coordinate string</returns>
        public string String()
        {
            Coordinate[] coords = {X, Y, Z};
            string crd = string.Empty;
            foreach (var coord in coords)
            {
                string tmp = string.Empty;
                tmp += coord.Relative != null && (bool) coord.Relative ? "~" :
                    coord.Local != null && (bool) coord.Local ? "^" : "";
                tmp += $"{coord.Value}";
                crd += $"{tmp} ";
            }

            return crd.Replace(",", ".");
        }

        public override string ToString()
        {
            return String();
        }
#nullable disable
    }
}