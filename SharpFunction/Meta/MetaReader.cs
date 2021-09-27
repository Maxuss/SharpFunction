using System.Collections.Generic;
using System.IO;
using SharpFunction.Commands.Helper;

namespace SharpFunction.Meta
{
    /// <summary>
    ///     Class used for reading .sfmeta files
    /// </summary>
    public sealed class MetaReader
    {
        internal const char COMMENT = '!';

        /// <summary>
        ///     Initialize new instance of MetaReader class. It will try to read and parse .sfmeta contents
        /// </summary>
        /// <param name="dir">Directory that (might) store .sfmeta file</param>
        /// <exception cref="FileNotFoundException">If .sfmeta file does not exist</exception>
        public MetaReader(string dir)
        {
            var f = Path.Combine(dir, ".sfmeta");
            if (!File.Exists(f)) throw new FileNotFoundException("Could not find .sfmeta file!");
            var lines = File.ReadAllLines(f);
            foreach (var line in lines)
                if (!line.StartsWith(COMMENT) && line.Contains("="))
                {
                    var kv = line.Split("=");
                    var key = kv[0];
                    var val = kv[1];
                    switch (key)
                    {
                        case "SRC":
                            SourceDirectory = val;
                            break;
                        case "DAT":
                            DataDirectory = val;
                            break;
                        case "NAME":
                            ProjectName = val;
                            break;
                        case "GEN":
                            FullyGenerated = CommandHelper.ByteToBool(val);
                            break;
                        default:
                            NonParseable.Add(key, val);
                            break;
                    }
                }
        }

        public string SourceDirectory { get; }
        public string DataDirectory { get; }
        public string ProjectName { get; }
        public bool FullyGenerated { get; }

        /// <summary>
        ///     KeyValuePairs of values that werent parsed
        /// </summary>
        public Dictionary<string, string> NonParseable { get; set; }
    }
}