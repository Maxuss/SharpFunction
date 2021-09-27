using SharpFunction.Commands;

namespace SharpFunction.API
{
    /// <summary>
    ///     Represents simple <see cref="Vector3" />s to use
    /// </summary>
    // TODO: ADD MORE SIMPLE VECTORS
    public readonly struct SimpleVector
    {
        /// <summary>
        ///     Current position. Equal to ~ ~ ~
        /// </summary>
        public static Vector3 Current => new("~0 ~0 ~0");
    }
}