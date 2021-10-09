using static SharpFunction.Universal.EnumHelper;

namespace SharpFunction.Universal
{
    /// <summary>
    ///     Represents Minecraft's game mode
    /// </summary>
    public enum Gamemode
    {
        /// <summary>
        /// Survival
        /// </summary>
        [EnumValue("survival")] Survival,
        /// <summary>
        /// Creative
        /// </summary>
        [EnumValue("creative")] Creative,
        /// <summary>
        /// Adventure
        /// </summary>
        [EnumValue("adventure")] Adventure,
        /// <summary>
        /// Spectator
        /// </summary>
        [EnumValue("spectator")] Spectator
    }
}