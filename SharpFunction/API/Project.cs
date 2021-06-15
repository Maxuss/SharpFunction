using System;
using System.IO;
using SharpFunction.Writer;
using SharpFunction.Meta;
using SharpFunction.Exceptions;

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

        /// <summary>
        /// Loads the project from directory if .sfmeta file exists
        /// </summary>
        /// <param name="path">Path to directory with src folder and .sfmeta file</param>
        /// <returns>Loaded project</returns>
        /// <exception cref="MetaFileNotFound">If .sfmeta file does not exist</exception>
        public static Project Load(string path)
        {
            try
            {
                MetaReader reader = new(path);
                Project project = new(reader.ProjectName, reader.SourceDirectory);
                if(reader.FullyGenerated) project.GenerateLoad(Path.Combine(reader.DataDirectory, project.Namespace));
                else project.Generate();
                return project;
            }
            catch(FileNotFoundException)
            {
                throw new MetaFileNotFound();
            }
        }

        private string[] requiredDirectories =
        {
            "functions", "loot_tables", "structures", "worldgen",
            "advancements", "recipes", "tags", "predicates", "dimension"
        };

        /// <summary>
        /// Initializes the datapack without generating files
        /// </summary>
        internal void GenerateLoad(string namespaced)
        {
            Writer = new(this);
            Writer.Initialize(Path.Combine(namespaced, "functions"));
        }

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
            MetaWriter writer = new(Path.Combine(ProjectPath), ProjectName);
            string d = writer.CreateMeta();
            File.WriteAllText(Path.Combine(ProjectPath, ".sfmeta"), d);
        }
    }
}
