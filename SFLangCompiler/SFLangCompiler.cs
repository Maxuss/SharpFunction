using System;
using System.IO;
using SFLang.Exceptions;
using SFLang.Language;
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
            var ctx = new Lambdas.Unit();
            var binder = new ContextBinder<Lambdas.Unit>(ctx);
            Lambdas.CreateScope(binder);
            
            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine() ?? "null";
            
                if (input?.ToLower() == "!stop")
                {
                    return;
                }
                try
                {
                    Lambdas.Compile(input, ctx)();
                }
                catch (PrettyException pretty)
                {
                    var handler = new PrettyException(29, -1, typeof(SFLangCompiler).FullName,
                        "Thread Exception handler", pretty);
                    handler.PrettyPrint();
                }
                catch (Exception normal)
                {
                    try
                    {
                        var handler = new PrettyException("An error occurred in main thread!",
                            new PrettyException(normal.Message + $" Cause: {normal.InnerException?.Message}"));
                    }
                    catch (PrettyException expected)
                    {
                        expected.PrettyPrint();
                    }
                }
            }
        }
    }
}