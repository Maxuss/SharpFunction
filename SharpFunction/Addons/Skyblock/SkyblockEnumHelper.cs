using System;
using SharpFunction.Universal;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    ///     Helper class for skyblock addon related enums
    /// </summary>
    internal static class SkyblockEnumHelper
    {
        internal static string GetSlayerMessage(this Enum @enum)
        {
            var type = @enum.GetType();
            var fieldInfo = type.GetField(@enum.ToString());
            var attribs =
                fieldInfo.GetCustomAttributes(typeof(SlayerRarityAttribute), false) as SlayerRarityAttribute[];
            return attribs.Length > 0 ? attribs[0].Message : null;
        }

        internal static Color GetRarityColor(this Enum @enum)
        {
            var type = @enum.GetType();
            var fieldInfo = type.GetField(@enum.ToString());
            var attribs = fieldInfo.GetCustomAttributes(typeof(RarityColorAttribute), false) as RarityColorAttribute[];
            return attribs.Length > 0 ? attribs[0].Color : Color.White;
        }

        internal class RarityColorAttribute : Attribute
        {
            public RarityColorAttribute(Color color)
            {
                Color = color;
            }

            public Color Color { get; protected set; }
        }

        internal class SlayerRarityAttribute : Attribute
        {
            public SlayerRarityAttribute(string msg)
            {
                Message = msg;
            }

            public string Message { get; protected set; }
        }

        [AttributeUsage(AttributeTargets.Property)]
        internal class StatNameAttribute : Attribute
        {
            public StatNameAttribute(string val)
            {
                Text = val;
            }

            public string Text { get; set; }
        }
    }
}