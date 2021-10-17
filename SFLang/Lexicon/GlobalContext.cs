using System.Collections.Generic;

namespace SFLang.Lexicon
{
    public static class GlobalContext<TContext>
    {
        public static Dictionary<string, ContextBinder<TContext>> Contexts { get; set; } = new();
    }
}