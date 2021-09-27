using System.Text.RegularExpressions;

namespace SharpFunction.Universal
{
    /// <summary>
    ///     A helper providing multiple methods for operations with colors
    /// </summary>
    public static class ColorHelper
    {
        private const string COLOR_CODE_PATTERN = "(?i)(\u00A7|(\\){0,1}\\u00A7)[\\dA-FK-OR]";
        private const char COLOR_CODE_CHAR = '\u00A7';
        private const string COLOR_CODE = "\\u00A7";
        private static readonly Regex regex = new(COLOR_CODE_PATTERN);

        /// <summary>
        ///     Translates color codes
        /// </summary>
        /// <param name="code">Code from which to be translated, e.g. '$'</param>
        /// <param name="input">Input to be parsed</param>
        /// <returns>Translated string with colors</returns>
        public static string TranslateColorCodes(char code, string input)
        {
            var b = input.ToCharArray();
            for (var i = 0; i < b.Length - 1; i++)
                if (b[i] == code && "0123456789AaBbCcDdEeFfKkLlMmNnOoRr".IndexOf(b[i + 1]) > -1)
                {
                    b[i] = char.MinValue;
                    b[i + 1] = char.ToLower(b[i + 1]);
                }

            return new string(b);
        }

        /// <summary>
        ///     Strips the color from input string
        /// </summary>
        /// <param name="input">String from which the color will be stripped</param>
        /// <returns>String without color codes</returns>
        public static string StripColor(string input)
        {
            if (input == null) return null;
            return regex.Replace(input, "");
        }
    }
}