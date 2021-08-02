using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.API
{
    /// <summary>
    /// Represents type of pack format in pack.mcmeta
    /// </summary>
    public enum PackFormat
    {
        /// <summary>
        /// 1.13-1.14.4
        /// </summary>
        v1_13 = 4,
        /// <summary>
        /// 1.15-1.16.1
        /// </summary>
        v1_15 = 5,
        /// <summary>
        /// 1.16.2-1.16.5
        /// </summary>
        v1_16 = 6,
        /// <summary>
        /// 1.17+
        /// </summary>
        v1_17 = 7
    }
}
