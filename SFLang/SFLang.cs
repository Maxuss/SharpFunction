namespace SFLang
{
    public static class SFLang
    {
        public const string Version = "0.6.3";
        public const string LanguageVersion = "0.6-PRIVATE-BETA";
        public const string LexerVersion = "0.6.2-PRIVATE-BETA";
        public const string Framework = "sfl";

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