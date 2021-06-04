using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Universal
{
    public static class EnumHelper
    {
        /// <summary>
        /// Represents attribute applicable to enums to store string value in them
        /// </summary>

        public class EnumValueAttribute : Attribute
        {

            /// <summary>
            /// Holds the stringvalue for a value in an enum.
            /// </summary>
            public string Value { get; protected set; }

            /// <summary>
            /// Initialize an attribute
            /// </summary>
            /// <param name="value"></param>
            public EnumValueAttribute(string value)
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// Gets string value from [<see cref="EnumValueAttribute"/>] attribute
        /// </summary>
        /// <param name="enum">Enum with value to get</param>
        /// <returns>String value from attribute</returns>
        public static string GetStringValue(this Enum @enum)
        {
            Type type = @enum.GetType();

            FieldInfo fieldInfo = type.GetField(@enum.ToString());

            EnumValueAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(EnumValueAttribute), false) as EnumValueAttribute[];
            
            return attribs.Length > 0 ? attribs[0].Value : null;
        }
    }
}
