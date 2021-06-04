using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    public sealed class Datapack : ICommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Datapack Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Datapack(){ }

        /// <summary>
        /// Compiles /datapack command
        /// </summary>
        /// <param name="action">Action to do with datapack</param>
        /// <param name="name">Name of datapack</param>
        public void Compile(DatapackAction action, string name)
        {
            string s = string.Empty;
            switch (action)
            {
                case DatapackAction.Enable:
                    s = "enable";
                    break;
                case DatapackAction.Disable:
                    s = "disable";
                    break;
            }
            Compiled = $"/datapack {s} {name}";
        }

        /// <summary>
        /// Compile /datapack command
        /// </summary>
        /// <param name="name">Name of datapack</param>
        /// <param name="priority">Priority for datapack to be enabled</param>
        /// <param name="existing">Name of datapack to load current before or after. Disabled by default.</param>
        public void Compile(string name, DatapackPriority priority, string existing = "")
        {
            string pr = string.Empty;
            switch(existing)
            {
                case "":
                    switch(priority)
                    {
                        case DatapackPriority.High:
                            pr = "first";
                            break;
                        case DatapackPriority.Low:
                            pr = "last";
                            break;
                    }
                    break;
                default:
                    switch(priority)
                    {
                        case DatapackPriority.High:
                            pr = "after";
                            break;
                        case DatapackPriority.Low:
                            pr = "before";
                            break;
                    }
                    break;
            }
            Compiled = $"/datapack enable {name} {pr} {existing}";
        }

        /// <summary>
        /// Compiled /datapack command as list of packs
        /// </summary>
        /// <param name="type">Type of datapacks to list</param>
        public void Compile(DatapackList type)
        {
            string s = string.Empty;
            switch(type)
            {
                case DatapackList.Enabled:
                    s = "enabled";
                    break;
                case DatapackList.All:
                    s = "all";
                    break;
            }
            Compiled = $"/datapack list {s}";
        }
    }

    /// <summary>
    /// Represents actions to do with "/datapack" command
    /// </summary>
    public enum DatapackAction
    {
        Enable,
        Disable
    }

    /// <summary>
    /// Represents priority for enabled datapacks
    /// </summary>
    public enum DatapackPriority
    { 
        High,
        Low
    }

    /// <summary>
    /// Represents types of datapacks to show
    /// </summary>
    public enum DatapackList
    {
        Enabled,
        All
    }
}
