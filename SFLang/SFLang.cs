namespace SFLang
{
    public static class SFLang
    {
        public static readonly string Version = "0.3-BETA";
        public static readonly string LexerVersion = "0.2-ALPHA";
        
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