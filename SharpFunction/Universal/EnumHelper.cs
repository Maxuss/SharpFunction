using System;

namespace SharpFunction.Universal
{
    public static class EnumHelper
    {
        /// <summary>
        ///     Gets string value from [<see cref="EnumValueAttribute" />] attribute
        /// </summary>
        /// <param name="enum">Enum with value to get</param>
        /// <returns>String value from attribute</returns>
        public static string GetStringValue(this Enum @enum)
        {
            var type = @enum.GetType();
            var fieldInfo = type.GetField(@enum.ToString());
            var attribs = fieldInfo?.GetCustomAttributes(typeof(EnumValueAttribute), false) as EnumValueAttribute[];
            return attribs?.Length > 0 ? attribs[0].Value : null;
        }

        /// <summary>
        ///     Gets deprecated string value from [<see cref="DeprecatedValueAttribute" />] attribute
        /// </summary>
        /// <param name="enum">Enum with value to get</param>
        /// <returns>String value from attribute</returns>
        public static string GetDeprecatedValue(this Enum @enum)
        {
            var type = @enum.GetType();
            var fieldInfo = type.GetField(@enum.ToString());
            var attribs =
                fieldInfo?.GetCustomAttributes(typeof(DeprecatedValueAttribute), false) as DeprecatedValueAttribute[];
            return attribs?.Length > 0 ? attribs[0].Value : null;
        }

        /// <summary>
        ///     Represents attribute applicable to enums to store string value in them
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class EnumValueAttribute : Attribute
        {
            /// <summary>
            ///     Initialize an attribute
            /// </summary>
            /// <param name="value"></param>
            public EnumValueAttribute(string value)
            {
                Value = value;
            }

            /// <summary>
            ///     Holds the stringvalue for a value in an enum.
            /// </summary>
            public string Value { get; protected set; }
        }

        /// <summary>
        ///     Represents deprecated value of enum as opposed to <see cref="EnumValueAttribute" />
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class DeprecatedValueAttribute : Attribute
        {
            /// <summary>
            ///     Initialize an attribute
            /// </summary>
            /// <param name="value"></param>
            public DeprecatedValueAttribute(string value)
            {
                Value = value;
            }

            /// <summary>
            ///     Holds the stringvalue for a value in an enum.
            /// </summary>
            public string Value { get; protected set; }
        }
    }
}