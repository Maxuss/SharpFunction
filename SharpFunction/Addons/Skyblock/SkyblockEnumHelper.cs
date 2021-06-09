using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    /// Helper class for skyblock addon related enums
    /// </summary>
    internal static class SkyblockEnumHelper
    {
        internal class RarityColorAttribute : Attribute
        {
            public Color Color { get; protected set; }

            public RarityColorAttribute(Color color)
            {
                Color = color;
            }
        }

        internal class SlayerRarityAttribute : Attribute
        {
            public string Message { get; protected set; }

            public SlayerRarityAttribute(string msg)
            {
                Message = msg;
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        internal class StatNameAttribute : System.Attribute
        {
            public string Text { get; set; }

            public StatNameAttribute(string val)
            {
                Text = val;
            }
        }


        internal static string GetSlayerMessage(this Enum @enum)
        {
            Type type = @enum.GetType();
            FieldInfo fieldInfo = type.GetField(@enum.ToString());
            SlayerRarityAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(SlayerRarityAttribute), false) as SlayerRarityAttribute[];
            return attribs.Length > 0 ? attribs[0].Message : null;
        }

        internal static Color GetRarityColor(this Enum @enum)
        {
            Type type = @enum.GetType();
            FieldInfo fieldInfo = type.GetField(@enum.ToString());
            RarityColorAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(RarityColorAttribute), false) as RarityColorAttribute[];
            return attribs.Length > 0 ? attribs[0].Color : Color.White;
        }
    }


}
