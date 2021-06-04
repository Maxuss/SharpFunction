using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents a color for Minecraft
    /// </summary>
    public struct Color
    {
        public static string Black { get => "black"; }
        public static string DarkBlue { get => "dark_blue"; }
        public static string DarkGreen { get => "dark_green"; }
        public static string DarkAqua { get => "dark_aqua"; }
        public static string DarkRed { get => "dark_red"; }
        public static string DarkPurple { get => "dark_purple"; }
        public static string Gold { get => "gold"; }
        public static string Gray { get => "gray"; }
        public static string Blue { get => "blue"; }
        public static string Green { get => "green"; }
        public static string Aqua { get => "aqua"; }
        public static string Red { get => "red"; }
        public static string LightPurple { get => "light_purple"; }
        public static string Yellow { get => "yellow"; }
        public static string White { get => "white"; }

        /// <summary>
        /// Resets color to default (<see cref="White"/>)
        /// </summary>
        public readonly string ResetColor { get => "reset"; }
        /// <summary>
        /// Provides prefix for Hex color. E.G. <code>#FF0000</code>
        /// </summary>
        public readonly string Hexadecimal { get => "#"; }
    }
}
