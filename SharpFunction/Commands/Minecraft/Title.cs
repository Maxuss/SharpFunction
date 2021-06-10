using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents Title command. Equal to Minecraft's <code>/title {params: TitleParameters}</code>
    /// </summary>
    public sealed class Title : ICommand, ISelectorCommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Title Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public Title(EntitySelector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Compiles the /title (clear|reset) command
        /// </summary>
        /// <param name="action">Action to do with title for selected entities</param>
        public void Compile(TitleAction action)
        {
            Compiled = $"/title {Selector.String()} {EnumHelper.GetStringValue(action)}";
        }

        /// <summary>
        /// Compiles the /title (title|subtitle|actionbar) (title) command
        /// </summary>
        /// <param name="position">Position to display title</param>
        /// <param name="title">Title SuperRawText</param>
        public void Compile(TitlePosition position, SuperRawText title)
        {
            Compiled = $"/title {Selector.String()} {EnumHelper.GetStringValue(position)} {title.Compile()}";
        }

        /// <summary>
        /// Compiles the /title times (in) (stay) (out)
        /// </summary>
        /// <param name="fadeIn">Fade in time for all titles</param>
        /// <param name="stay">Stay time for all titles</param>
        /// <param name="fadeOut">Fade out time for all titles</param>
        public void Compile(int fadeIn, int stay, int fadeOut)
        {
            Compiled = $"/title {Selector.String()} {fadeIn} {stay} {fadeOut}";
        }
    }

    /// <summary>
    /// Represents action to do with title
    /// </summary>
    public enum TitleAction
    {
        /// <summary>
        /// Clears title from entities
        /// </summary>
        [EnumValue("clear")] Clear,
        /// <summary>
        /// Resets title display for some entities
        /// </summary>
        [EnumValue("reset")] Reset,
    }

    /// <summary>
    /// Represents position of title on the screen
    /// </summary>
    public enum TitlePosition
    {
        /// <summary>
        /// Center of screen
        /// </summary>
        [EnumValue("title")] Title,
        /// <summary>
        /// Under title
        /// </summary>
        [EnumValue("subtitle")] Subtitle,
        /// <summary>
        /// Over hotbar
        /// </summary>
        [EnumValue("actionbar")] Actionbar
    }
}
