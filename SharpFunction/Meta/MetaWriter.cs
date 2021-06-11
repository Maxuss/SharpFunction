using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Universal;
using SharpFunction.Commands.Helper;
namespace SharpFunction.Meta
{
    /// <summary>
    /// Class required for writing .sfmeta files
    /// </summary>
    public sealed class MetaWriter
    {
        public string Directory { internal get; set; }
        public string ProjectName { internal get; set; }
        public string Namespace { internal get; set; }
        /// <summary>
        /// Initialize a new instance of meta file writer
        /// </summary>
        /// <param name="dir">Directory to write .sfmeta file. Should contain src folder!</param>
        /// <param name="project">Non-formatted name of project</param>
        public MetaWriter(string dir, string project)
        {
            Directory = dir;
            ProjectName = project;
            Namespace = ProjectName.ToLower().Replace(" ", "_");
        }

        private string template =
@"
! This file is used for storing 
! meta information related to
! SharpFunction project
! If you want to learn more
! about .sfmeta check documentation

! path to 'src' directory
SRC=${SOURCE}
! path to 'data' directory
DAT=${DATAPATH}
! formatted name of datapack
NAME=${NAME}
! whether all required folders were generated 0/1 for false/true
GEN=${GENERATED}
";

        /// <summary>
        /// Writes .sfmeta file to src directory
        /// </summary>
        public void CreateMeta()
        {
            string src = Path.Combine(Directory, "src");
            string prd = Path.Combine(src, ProjectName);
            string data = Path.Combine(prd, "data");
            string namespaced = Path.Combine(data, Namespace);
            bool initialized;

            string[] dirs = new[]
            {
                "functions", "loot_tables", "structures", "worldgen",
                "advancements", "recipes", "tags", "predicates", "dimension"
            };
            var predicate = Encapsulator<string, bool>.Encapsulate(p =>
            {
                return System.IO.Directory.Exists(Path.Combine(namespaced, p));
            });
            initialized = dirs.All(predicate);
            string init = CommandHelper.BoolToByte(initialized).Replace("b", "");

            string filled = template.Replace("${SOURCE}", src).Replace("${DATAPATH}", data).Replace("${NAME}", ProjectName).Replace("${GENERATED}", init);
            File.WriteAllText(Directory, filled);
        }
    }
}
