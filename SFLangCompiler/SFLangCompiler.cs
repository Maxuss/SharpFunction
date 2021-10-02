using System;
using SFLang.Exceptions;
using SFLang.Lexicon;

namespace SFLangCompiler
{
    class SFLangCompiler
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"SFLang v{SFLang.SFLang.Version} with SFLexicon v{SFLang.SFLang.LexerVersion}");
            Console.WriteLine("Welcome to interactive SFLang Interpreter!");
            Console.WriteLine("Just enter code and we will evalutate it!");
            Console.WriteLine("(Type '!stop' to exit program)");
            Lambdas.CreateScope(new ContextBinder<Lambdas.Unit>(new Lambdas.Unit()));
            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();

                if (input?.ToLower() == "!stop")
                {
                    return;
                }
                try
                {
                    Lambdas.Compile(input)();
                }
                catch (PrettyException pretty)
                {
                    var handler = new PrettyException(29, -1, typeof(SFLangCompiler).FullName,
                        "Thread Exception handler", pretty);
                    handler.PrettyPrint();
                }
            }
        }
    }
}