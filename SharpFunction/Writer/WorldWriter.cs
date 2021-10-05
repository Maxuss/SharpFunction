using System;
using System.Linq;
using SharpFunction.API;

namespace SharpFunction.Writer
{
    public class WorldWriter : AbstractWriter
    {
        #region Data

        /// <summary>
        ///     Path for writer to write functions
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     Name of project
        /// </summary>
        public string Name { get; set; }

        private string biome = "None";

        private readonly string[] createdBiomes = Array.Empty<string>();

        #endregion Data

        #region Initialization

        /// <summary>
        ///     Initialize a function writer
        /// </summary>
        /// <param name="path">
        ///     Path where to write functions.<br />
        ///     Functions will be created in <br />
        ///     <code>Path/functions/</code>
        /// </param>
        /// <param name="name">Name of project to write in.</param>
        public WorldWriter(string path, string name)
        {
            Path = path;
            Name = name;
        }

        /// <summary>
        ///     Initialize a function writer
        /// </summary>
        /// <param name="project">Project to get data from</param>
        public WorldWriter(Project project)
        {
            Path = System.IO.Path.Combine(project.ProjectPath, project.Namespace);
            Name = project.ProjectName;
        }

        #endregion Initialization

        #region IO

        /// <summary>
        ///     Checks whether the function is specified.
        /// </summary>
        /// <returns>True if specified and False if not</returns>
        public bool FunctionSpecified()
        {
            return biome != "None";
        }

        /// <summary>
        ///     Checks whether the biome with specified name (without .mcfunction extension) exists
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <returns>True if specified and False if not</returns>
        public bool BiomeExists(string name)
        {
            return createdBiomes.Contains(name);
        }

        #endregion IO

        #region Writing
        
        
        #endregion Writing
    }
}