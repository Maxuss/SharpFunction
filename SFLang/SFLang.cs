namespace SFLang
{
    public static class SFLang
    {
        public static readonly string Version = "0.5-PRIVATE-BETA";
        public static readonly string LexerVersion = "0.45-PRIVATE-BETA";
        
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