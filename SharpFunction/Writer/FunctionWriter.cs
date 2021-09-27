using System;
using System.IO;
using System.Linq;
using SharpFunction.API;
using SharpFunction.Commands;
using SharpFunction.Exceptions;

namespace SharpFunction.Writer
{
    /// <summary>
    ///     This class is used for writing functions to file
    /// </summary>
    public sealed class FunctionWriter : AbstractWriter
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

        private string function = "None";

        private readonly string[] createdFunctions = { };

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
        public FunctionWriter(string path, string name)
        {
            Path = path;
            Name = name;
        }

        /// <summary>
        ///     Initialize a function writer
        /// </summary>
        /// <param name="project">Project to get data from</param>
        public FunctionWriter(Project project)
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
            if (function != "None") return true;
            return false;
        }

        /// <summary>
        ///     Checks whether the function with specified name (without .mcfunction extension) exists
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <returns>True if specified and False if not</returns>
        public bool FunctionExists(string name)
        {
            if (createdFunctions.Contains(name)) return true;
            return false;
        }

        #endregion IO

        #region Functions

        /// <summary>
        ///     Creates an empty function with specified name in current working category.<br />
        ///     Overwrites it if it exists, so be careful.
        /// </summary>
        /// <param name="name">Name of function</param>
        public void CreateFunction(string name)
        {
            string tmp;
            function = name;
            createdFunctions.Append(name);
            if (CategorySpecified()) tmp = category;
            else tmp = Main;
            File.WriteAllText(System.IO.Path.Combine(tmp, $"{name}.mcfunction".ToLower().Replace(" ", "_")),
                "# Pre-Generated file using SharpFunction ");
        }

        /// <summary>
        ///     Writes a simple command to function
        /// </summary>
        /// <param name="command">Command string</param>
        /// <param name="name">Name of function. Not case sensitive</param>
        public void WriteCommand(string command, string name)
        {
            string tmp;
            if (name != "Current") tmp = name.ToLower().Replace(" ", "_");
            else throw new FunctionNotSpecifiedException($"Function name: {name}");

            var category = GetCurrentCategory();
            var path = System.IO.Path.Combine(category, $"{name}.mcfunction");

            var parsed = command.StartsWith("/") ? command.Remove(0, 1) : command;
            File.WriteAllText(path, parsed);
        }

        /// <summary>
        ///     Writes a command module to function
        /// </summary>
        /// <param name="command">Command as module</param>
        /// <param name="name">Name of function. Not case sensitive</param>
        public void WriteCommand(CommandModule command, string name)
        {
            string tmp;
            if (FunctionSpecified() && name == "Current") tmp = createdFunctions.Last().ToLower().Replace(" ", "_");
            else if (name != "Current") tmp = name.ToLower().Replace(" ", "_");
            else throw new FunctionNotSpecifiedException($"Function name: {name}");

            var category = GetCurrentCategory();
            var lines = Array.Empty<string>().ToList();
            foreach (var line in command.CommandLines)
            {
                var parsed = line.Contains("/") ? line.Remove(0, 1) : line;
                lines.Add(parsed);
            }

            var path = System.IO.Path.Combine(category, $"{name}.mcfunction");
            File.WriteAllLines(path, lines);
        }

        #endregion Functions
    }
}