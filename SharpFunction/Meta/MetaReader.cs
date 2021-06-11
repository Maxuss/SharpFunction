using System;
using System.Collections.Generic;
using System.IO;
namespace SharpFunction.Meta
{
    /// <summary>
    /// Class used for reading .sfmeta files
    /// </summary>
    public sealed class MetaReader
    {
        internal const char COMMENT = '!';

        public string SourceDirectory { get; private set; }
        public string DataDirectory { get; private set; }
        public string ProjectName { get; private set; }
        public bool FullyGenerated { get; private set; }

        /// <summary>
        /// KeyValuePairs of values that werent parsed
        /// </summary>
        public Dictionary<string, string> NonParseable { get; set; }

        /// <summary>
        /// Initialize new instance of MetaReader class. It will try to read and parse .sfmeta contents
        /// </summary>
        /// <param name="dir">Directory that (might) store .sfmeta file</param>
        /// <exception cref="FileNotFoundException">If .sfmeta file does not exist</exception>
        public MetaReader(string dir)
        {
            string f = Path.Combine(dir, ".sfmeta");
            if(!File.Exists(f))
            {
                throw new FileNotFoundException("Could not find .sfmeta file!");
            }
            string[] lines = File.ReadAllLines(f);
            foreach(string line in lines)
            {
                if (!line.StartsWith(COMMENT) && line.Contains("="))
                {
                    string[] kv = line.Split("=");
                    string key = kv[0];
                    string val = kv[1];
                    switch(key)
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
                            FullyGenerated = Commands.Helper.CommandHelper.ByteToBool(val);
                            break;
                        default:
                            NonParseable.Add(key, val);
                            break;
                    }
                }
            }
        }
    }
}
