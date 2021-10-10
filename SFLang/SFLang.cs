namespace SFLang
{
    public static class SFLang
    {
        public static readonly string Version = "0.6-PRIVATE-BETA";
        public static readonly string LexerVersion = "0.6.2-PRIVATE-BETA";
        public static readonly string Framework = "sfl";
            
        public static List<string> GetInformation()
        {
            return new List<string>
            {
                $"SFLang-Version: {Version}",
                $"SFLang-Lexer-Version: {LexerVersion}",
                "Author: Maxuss"
            };
        }
    }
}