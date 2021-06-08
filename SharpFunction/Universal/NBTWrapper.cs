using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Universal
{
    /// <summary>
    /// Class used for wrapping multiple NBT tags into a single NBT-JSON line
    /// </summary>
    public static class NBTWrapper
    {
        /// <summary>
        /// Wraps the nbt data into a single json string
        /// </summary>
        /// <param name="tags">NBT tags, separated by commas</param>
        /// <returns>Wrapped NBT JSONified string</returns>
        public static string Wrap(params string[] tags)
        {
            string full = string.Empty;
            Parallel.ForEach(tags, tag =>
            {
                full += tags.Last().Equals(tag) ? tag : $"{tag},";
            });
            return $"{{{full}}}";
        }
    }
}
