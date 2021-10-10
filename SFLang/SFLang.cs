namespace SFLang
{
    public static class SFLang
    {
        public const string Version = "0.8.3";
        public const string LanguageVersion = "0.8-PUBLIC-BETA";
        public const string LexerVersion = "0.9-PRIVATE-BETA";
        public const string Framework = "sfCORE";

        public static List<string> GetInformation()
        {
            return new List<string>
            {
                $"SFLang-Version: {LanguageVersion}",
                $"SFLang-Lexer-Version: {LexerVersion}",
                "Author: Maxuss"
            };
        }
    }
}