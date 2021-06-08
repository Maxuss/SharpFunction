using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Commands;
namespace SharpFunction.API
{
    /// <summary>
    /// Represents simple <see cref="Vector3"/>s to use
    /// </summary>
    // TODO: ADD MORE SIMPLE VECTORS
    public readonly struct SimpleVector
    {
        /// <summary>
        /// Current position. Equal to ~ ~ ~
        /// </summary>
        public static Vector3 Current { get => new Vector3("~0 ~0 ~0"); }
    }
}
