using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;
namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents a color for Minecraft
    /// </summary>
    public enum Color
    {
        [EnumValue(@"""color"": ""black""")] Black,
        [EnumValue(@"""color"": ""dark_blue""")] DarkBlue,
        [EnumValue(@"""color"": ""dark_green""")] DarkGreen,
        [EnumValue(@"""color"": ""dark_aqua""")] DarkAqua,
        [EnumValue(@"""color"": ""dark_red""")] DarkRed,
        [EnumValue(@"""color"": ""dark_purple""")] DarkPurple,
        [EnumValue(@"""color"": ""gold""")] Gold,
        [EnumValue(@"""color"": ""gray""")] Gray,
        [EnumValue(@"""color"": ""blue""")] Blue,
        [EnumValue(@"""color"": ""green""")] Green,
        [EnumValue(@"""color"": ""aqua""")] Aqua,
        [EnumValue(@"""color"": ""red""")] Red,
        [EnumValue(@"""color"": ""light_purple""")] LightPurple,
        [EnumValue(@"""color"": ""yellow""")] Yellow,
        [EnumValue(@"""color"": ""white""")] White,
        [EnumValue(@"""color"": ""reset""")] Reset
    }
}
