using System;
using SFLang.Lexicon;

namespace SFLang.Mixins
{
    [Mixin]
    public static class TestMixin
    {
        [Method(Name = "testing-method", ExpectedParameters = 1)]
        public static bool TestingStuff(ContextBinder<Lambdas.Unit> binder, Parameters args)
        {
            Console.WriteLine($"TEST SUCCESSFUL! {args[0]}");
            return true;
        }
    }
}