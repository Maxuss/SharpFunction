using System.IO;
using System.Linq;
using SharpFunction.Commands.Helper;
using SharpFunction.Universal;

namespace SharpFunction.Meta
{
    /// <summary>
    ///     Class required for writing .sfmeta files
    /// </summary>
    public sealed class MetaWriter
    {
        private readonly string template =
            @"
! This file is used for storing 
! meta information related to
! SharpFunction project
! If you want to learn more
! about .sfmeta check documentation here:
! https://github.com/Maxuss/SharpFunction/wiki/.sfmeta-information

! ####################### !

! path to 'src' directory
SRC=${SOURCE}
! path to 'data' directory
DAT=${DATAPATH}
! formatted name of datapack
NAME=${NAME}
! whether all required folders were generated 0/1 for false/true
GEN=${GENERATED}

! ####################### !
! Append your data here
";

        /// <summary>
        ///     Initialize a new instance of meta file writer
        /// </summary>
        /// <param name="dir">Directory to write .sfmeta file. Should contain src folder!</param>
        /// <param name="project">Non-formatted name of project</param>
        public MetaWriter(string dir, string project)
        {
            Directory = dir;
            ProjectName = project;
            Namespace = ProjectName.ToLower().Replace(" ", "_");
        }

        public string Directory { internal get; set; }
        public string ProjectName { internal get; set; }
        public string Namespace { internal get; set; }

        /// <summary>
        ///     Creates .sfmeta file to be written to src directory
        /// </summary>
        public string CreateMeta()
        {
            var src = Path.Combine(Directory, "src");
            var prd = Path.Combine(src, ProjectName);
            var data = Path.Combine(prd, "data");
            var namespaced = Path.Combine(data, Namespace);
            bool initialized;

            string[] dirs =
            {
                "functions", "loot_tables", "structures", "worldgen",
                "advancements", "recipes", "tags", "predicates", "dimension"
            };
            var predicate = Encapsulator<string, bool>.Encapsulate(p =>
            {
                return System.IO.Directory.Exists(Path.Combine(namespaced, p));
            });
            initialized = dirs.All(predicate);
            var init = CommandHelper.BoolToByte(initialized).Replace("b", "");

            var filled = template.Replace("${SOURCE}", src).Replace("${DATAPATH}", data).Replace("${NAME}", ProjectName)
                .Replace("${GENERATED}", init);
            return filled;
        }
    }
}