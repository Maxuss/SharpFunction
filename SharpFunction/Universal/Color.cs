using static SharpFunction.Universal.EnumHelper;

namespace SharpFunction.Universal
{
    /// <summary>
    ///     Represents a color for Minecraft
    /// </summary>
    public enum Color
    {
        [DeprecatedValue("&0")] [EnumValue(@"""color"": ""black""")]
        Black,

        [DeprecatedValue("&1")] [EnumValue(@"""color"": ""dark_blue""")]
        DarkBlue,

        [DeprecatedValue("&2")] [EnumValue(@"""color"": ""dark_green""")]
        DarkGreen,

        [DeprecatedValue("&3")] [EnumValue(@"""color"": ""dark_aqua""")]
        DarkAqua,

        [DeprecatedValue("&4")] [EnumValue(@"""color"": ""dark_red""")]
        DarkRed,

        [DeprecatedValue("&5")] [EnumValue(@"""color"": ""dark_purple""")]
        DarkPurple,

        [DeprecatedValue("&6")] [EnumValue(@"""color"": ""gold""")]
        Gold,

        [DeprecatedValue("&7")] [EnumValue(@"""color"": ""gray""")]
        Gray,

        [DeprecatedValue("&9")] [EnumValue(@"""color"": ""blue""")]
        Blue,

        [DeprecatedValue("&a")] [EnumValue(@"""color"": ""green""")]
        Green,

        [DeprecatedValue("&b")] [EnumValue(@"""color"": ""aqua""")]
        Aqua,

        [DeprecatedValue("&c")] [EnumValue(@"""color"": ""red""")]
        Red,

        [DeprecatedValue("&d")] [EnumValue(@"""color"": ""light_purple""")]
        LightPurple,

        [DeprecatedValue("&e")] [EnumValue(@"""color"": ""yellow""")]
        Yellow,

        [DeprecatedValue("&f")] [EnumValue(@"""color"": ""white""")]
        White,

        [DeprecatedValue("&8")] [EnumValue(@"""color"": ""dark_gray""")]
        DarkGray,

        [DeprecatedValue("&r")] [EnumValue(@"""color"": ""reset""")]
        Reset,

        [DeprecatedValue("&f")] [EnumValue(@"""color"": ""white""")]
        Default,
        
        [DeprecatedValue("")] [EnumValue(@"""color"": ""white""")]
        None
    }
}