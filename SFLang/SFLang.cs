namespace SFLang
{
    public static class SFLang
    {
        public const string Version = "0.9.4";
        public const string LanguageVersion = "0.9-beta";
        public const string LexerVersion = "0.9.6-pre";
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