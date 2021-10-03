#nullable enable
using SFLang.Lexicon;

namespace SFLang.Language
{
    public abstract class SFClass
    {
        public virtual object? __init__(ContextBinder<Lambdas.Unit> binder, Parameters args)
        {
            return null;
        }

        public virtual object? __dstr__(ContextBinder<Lambdas.Unit> binder, Parameters args)
        {
            return null;
        }
    }
}