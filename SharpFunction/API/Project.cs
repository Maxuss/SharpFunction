using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Writer;
namespace SharpFunction.API
{
    /// <summary>
    /// Represents a Minecraft datapack project 
    /// </summary>
    public sealed class Project
    {
        
        /// <summary>
        /// Path to root folder of project
        /// </summary>
        public string ProjectPath { get; private set; }

        /// <summary>
        /// Name of project
        /// </summary>
        public string ProjectName { get; private set; }

        /// <summary>
        /// Namespace of project
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// Description in pack.mcmeta
        /// </summary>
        public string Description { get; set; } = "Made with SharpFunction for Datapacks.";

        /// <summary>
        /// Pack format shown in pack.mcmeta
        /// </summary>
        public PackFormat Format { get; set; } = PackFormat.DotSixteen;

        /// <summary>
        /// Allows the writing of minecraft functions.<br/>
        /// Can only be accessed after initializing the project using <see cref="Generate()"/>
        /// </summary>
        public FunctionWriter Writer { get; private set; } = null;
        /// <summary>
        /// Initialize a new empty Minecraft datapack project
        /// </summary>
        /// <param name="name">Name of root datapack folder, a.k.a. datapack name.</param>
        /// <param name="path">Path where root folder should be created.</param>
        public Project(string name, string path)
        {
            ProjectPath = path;
            ProjectName = name;
            Namespace = name.ToLower().Replace(" ", "_");
        }

        private const string mcmeta_example =
@"
{
    ""pack"": {
    ""pack_format"": {FORMAT},
    ""description"": ""{DESCRIPTION}""
  }
}
";

        private string[] requiredDirectories =
        {
            "functions", "loot_tables", "structures", "worldgen",
            "advancements", "recipes", "tags", "predicates", "dimension"
        };



        /// <summary>Initializes a datapack, allowing the use of <see cref="Writer.FunctionWriter"/></summary>
        public void Generate()
        {
            Writer = new FunctionWriter(this);
            string mainDir = Path.Combine(ProjectPath, "src", ProjectName);
            Directory.CreateDirectory(mainDir);
            string tmp = mcmeta_example
                .Replace("{FORMAT}", $"{Array.IndexOf(Enum.GetValues(typeof(PackFormat)), Format)}")
                .Replace("{DESCRIPTION}", Description);
            File.WriteAllText(Path.Combine(mainDir, "pack.mcmeta"), tmp);
            string namespaceDir = Path.Combine(mainDir, "data", Namespace);
            string workingDir = Path.Combine(mainDir,"data",Namespace);
            Directory.CreateDirectory(workingDir);
            Directory.CreateDirectory(namespaceDir);
            foreach(string dir in requiredDirectories)
            {
                Directory.CreateDirectory(Path.Combine(namespaceDir, dir));
            }
            Writer.Initialize(Path.Combine(namespaceDir, "functions"));
        }
    }
}
