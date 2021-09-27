using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpFunction.Universal
{
    /// <summary>
    ///     Class used for wrapping multiple NBT tags into a single NBT-JSON line
    /// </summary>
    public static class NBTWrapper
    {
        /// <summary>
        ///     Wraps the nbt data into a single json string
        /// </summary>
        /// <param name="tags">NBT tags, separated by commas</param>
        /// <returns>Wrapped NBT JSONified string</returns>
        public static string Wrap(params string[] tags)
        {
            var full = string.Empty;
            Parallel.ForEach(tags, tag => { full += tags.Last().Equals(tag) ? tag : $"{tag},"; });
            // sometimes wrapper bugs and creates two commas
            // replaces two commas by one
            // not the best practice tho, but its the only safe
            // exit because i dont really know what causes the bug
            var tf = Regex.Replace(Regex.Replace(full, ",,(?=([^\"]* \"[^\"]*\")*[^\"]*$)", ","),
                ",,(?=([^']* '[^']*')*[^']*$)", ",");
            return $"{{{tf}}}";
        }
    }
}