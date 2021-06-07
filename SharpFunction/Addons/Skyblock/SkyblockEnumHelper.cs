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
    public static class SkyblockEnumHelper
    {
        public class RarityColorAttribute : Attribute
        {
            public Color Color { get; protected set; }

            public RarityColorAttribute(Color color)
            {
                Color = color;
            }
        }

        public static Color GetRarityColor(this Enum @enum)
        {
            Type type = @enum.GetType();
            FieldInfo fieldInfo = type.GetField(@enum.ToString());
            RarityColorAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(RarityColorAttribute), false) as RarityColorAttribute[];
            return attribs.Length > 0 ? attribs[0].Color : Color.White;
        }
    }


}
