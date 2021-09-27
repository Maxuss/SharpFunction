using System.IO;
using SharpFunction.Exceptions;
using SharpFunction.Meta;
using SharpFunction.Writer;

namespace SharpFunction.API
{
    /// <summary>
    ///     Represents a Minecraft datapack project
    /// </summary>
    public sealed class Project
    {
        private const string mcmeta_example =
            @"
{
    ""pack"": {
    ""pack_format"": {FORMAT},
    ""description"": ""{DESCRIPTION}""
  }
}
";

        private readonly string[] requiredDirectories =
        {
            "functions", "loot_tables", "structures", "worldgen",
            "advancements", "recipes", "tags", "predicates", "dimension"
        };

        /// <summary>
        ///     Initialize a new empty Minecraft datapack project
        /// </summary>
        /// <param name="name">Name of root datapack folder, a.k.a. datapack name.</param>
        /// <param name="path">Path where root folder should be created.</param>
        public Project(string name, string path)
        {
            ProjectPath = path;
            ProjectName = name;
            Namespace = name.ToLower().Replace(" ", "_");
        }

        /// <summary>
        ///     Path to root folder of project
        /// </summary>
        public string ProjectPath { get; }

        /// <summary>
        ///     Name of project
        /// </summary>
        public string ProjectName { get; }

        /// <summary>
        ///     Namespace of project
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        ///     Description in pack.mcmeta
        /// </summary>
        public string Description { get; set; } = "Made with SharpFunction for Datapacks.";

        /// <summary>
        ///     Pack format shown in pack.mcmeta
        /// </summary>
        public PackFormat Format { get; set; } = PackFormat.v1_16;

        /// <summary>
        ///     Allows the writing of minecraft functions.<br />
        ///     Can only be accessed after initializing the project using <see cref="Generate()" />
        /// </summary>
        public FunctionWriter Writer { get; private set; }

        /// <summary>
        ///     Loads the project from directory if .sfmeta file exists
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
                if (reader.FullyGenerated) project.GenerateLoad(Path.Combine(reader.DataDirectory, project.Namespace));
                else project.Generate();
                return project;
            }
            catch (FileNotFoundException)
            {
                throw new MetaFileNotFound();
            }
        }

        /// <summary>
        ///     Initializes the datapack without generating files
        /// </summary>
        internal void GenerateLoad(string namespaced)
        {
            Writer = new FunctionWriter(this);
            Writer.Initialize(Path.Combine(namespaced, "functions"));
        }

        /// <summary>Initializes a datapack, allowing the use of <see cref="FunctionWriter" /></summary>
        public void Generate()
        {
            Writer = new FunctionWriter(this);
            var mainDir = Path.Combine(ProjectPath, "src", ProjectName);
            Directory.CreateDirectory(mainDir);

            var tmp = mcmeta_example
                .Replace("{FORMAT}", $"{(int) Format}")
                .Replace("{DESCRIPTION}", Description);
            File.WriteAllText(Path.Combine(mainDir, "pack.mcmeta"), tmp);
            var namespaceDir = Path.Combine(mainDir, "data", Namespace);
            var workingDir = Path.Combine(mainDir, "data", Namespace);
            Directory.CreateDirectory(workingDir);
            Directory.CreateDirectory(namespaceDir);
            foreach (var dir in requiredDirectories) Directory.CreateDirectory(Path.Combine(namespaceDir, dir));
            Writer.Initialize(Path.Combine(namespaceDir, "functions"));
            MetaWriter writer = new(Path.Combine(ProjectPath), ProjectName);
            var d = writer.CreateMeta();
            File.WriteAllText(Path.Combine(ProjectPath, ".sfmeta"), d);
        }
    }
}