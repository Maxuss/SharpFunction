namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents actions to do with "/bossbar set"
    /// </summary>
    public enum BossbarSet
    {
        /// <summary>
        ///     Sets the color of boss bar
        /// </summary>
        Color,

        /// <summary>
        ///     Sets maximum possible value of boss bar
        /// </summary>
        MaxValue,

        /// <summary>
        ///     Sets visible name of boss bar
        /// </summary>
        Name,

        /// <summary>
        ///     Sets players who can see the boss bar
        /// </summary>
        Players,

        /// <summary>
        ///     Sets the style of boss bar
        /// </summary>
        Style,

        /// <summary>
        ///     Sets the current value of boss bar
        /// </summary>
        Value,

        /// <summary>
        ///     Sets the visibility of boss bar
        /// </summary>
        Visible
    }

    /// <summary>
    ///     Represents actions to do with '/bossbar get' command
    /// </summary>
    public enum BossbarGet
    {
        /// <summary>
        ///     Returns max value of boss bar
        /// </summary>
        MaxValue,

        /// <summary>
        ///     Returns players who can see the boss bar
        /// </summary>
        Players,

        /// <summary>
        ///     Returns current value of boss bar
        /// </summary>
        Value,

        /// <summary>
        ///     Returns if boss bar is visible
        /// </summary>
        Visible
    }
}