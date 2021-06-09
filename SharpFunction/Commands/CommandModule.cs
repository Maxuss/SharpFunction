using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Commands;
using SharpFunction.Commands.Minecraft;

namespace SharpFunction.Commands
{
    /// <summary>
    /// Represents module containing multiple commands for writing into .mcfunction file
    /// </summary>
    public sealed class CommandModule
    {
        /// <summary>
        /// Represents all commands in the module as string array
        /// </summary>
        public string[] CommandLines = Array.Empty<string>();
        
        /// <summary>
        /// Appends compiled command string to all lines
        /// </summary>
        /// <param name="command">Command string to append</param>
        public void Append(string command)
        {
            CommandLines.Append(command);
        }

        /// <summary>
        /// Appends compiled command value to all lines.<br/>
        /// Requires compiling command before appending!
        /// </summary>
        /// <param name="command">Command to append</param>
        [Obsolete("Use Append(params ICommand[]) instead.")]
        public void Append(ICommand command)
        {
            CommandLines.Append(command.Compiled);
        }

        /// <summary>
        /// Appends multiple <b>compiled</b> command values to lines<br/>
        /// Commands require <b>compiling</b> before appending!
        /// </summary>
        /// <param name="commands">Multiple compiled commands</param>
        public void Append(params ICommand[] commands)
        {
            CommandLines = new string[commands.Length];
            for(int i = 0; i < commands.Length; i++)
            {
                CommandLines[i] = commands[i].Compiled;
            }
        }

        public void Append(params string[] commands)
        {
            CommandLines = new string[commands.Length];
            for (int i = 0; i < commands.Length; i++)
            {
                CommandLines[i] = commands[i];
            }
        }
    }
}
