using System;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents minecraft datapack command. Equal to Minecraft's <code>/datapack {action} {ActionParams}</code>
    /// </summary>
    public sealed class Datapack : ICommand
    {
        public string Compiled { get; private set; }

        /// <summary>
        ///     Compiles /datapack command
        /// </summary>
        /// <param name="action">Action to do with datapack</param>
        /// <param name="name">Name of datapack</param>
        public void Compile(DatapackAction action, string name)
        {
            var s = string.Empty;
            switch (action)
            {
                case DatapackAction.Enable:
                    s = "enable";
                    break;
                case DatapackAction.Disable:
                    s = "disable";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

            Compiled = $"/datapack {s} {name}";
        }

        /// <summary>
        ///     Compile /datapack command
        /// </summary>
        /// <param name="name">Name of datapack</param>
        /// <param name="priority">Priority for datapack to be enabled</param>
        /// <param name="existing">Name of datapack to load current before or after. Disabled by default.</param>
        public void Compile(string name, DatapackPriority priority, string existing = "")
        {
            var pr = string.Empty;
            pr = existing switch
            {
                "" => priority switch
                {
                    DatapackPriority.High => "first",
                    DatapackPriority.Low => "last",
                    _ => pr
                },
                _ => priority switch
                {
                    DatapackPriority.High => "after",
                    DatapackPriority.Low => "before",
                    _ => pr
                }
            };

            Compiled = $"/datapack enable {name} {pr} {existing}";
        }

        /// <summary>
        ///     Compiled /datapack command as list of packs
        /// </summary>
        /// <param name="type">Type of datapacks to list</param>
        public void Compile(DatapackList type)
        {
            var s = string.Empty;
            switch (type)
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
    ///     Represents actions to do with "/datapack" command
    /// </summary>
    public enum DatapackAction
    {
        Enable,
        Disable
    }

    /// <summary>
    ///     Represents priority for enabled datapacks
    /// </summary>
    public enum DatapackPriority
    {
        High,
        Low
    }

    /// <summary>
    ///     Represents types of datapacks to show
    /// </summary>
    public enum DatapackList
    {
        Enabled,
        All
    }
}