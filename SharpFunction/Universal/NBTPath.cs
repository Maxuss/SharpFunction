using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents NBT Path as JSON string
    /// </summary>
    public sealed class NBTPath
    {
        /// <summary>
        /// JSON string of NBT minecraft data
        /// </summary>
        public string JSON { get; set; }

        /// <summary>
        /// Initialize an NBT Path
        /// </summary>
        /// <param name="json">JSONed NBT data</param>
        public NBTPath(string json) => JSON = json;
    }
}
