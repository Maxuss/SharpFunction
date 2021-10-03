using System;
using SFLang.Exceptions;
using SFLang.Language;
using SFLang.Lexicon;

namespace SFLangCompiler
{
    internal class ExampleSFClass : SFClass
    {
        public override object __init__(Parameters args)
        {
            Console.WriteLine("Initialized class!");
            if(args.Count > 0) Console.WriteLine($"Args: {args.Get(0)}");
            return "Something";
        }

        public override object __dstr__(Parameters args)
        {
            Console.WriteLine("Destroyed class!");
            return null;
        }

        public object __typeof__()
        {
            Console.WriteLine("Typeof invoked!");
            return typeof(ExampleSFClass);
        }
    }
    
    class SFLangCompiler
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"SFLang v{SFLang.SFLang.Version} with SFLexicon v{SFLang.SFLang.LexerVersion}");
            Console.WriteLine("Welcome to interactive SFLang Interpreter!");
            Console.WriteLine("Just enter code and we will evalutate it!");
            Console.WriteLine("(Type '!stop' to exit program)");
            var binder = new ContextBinder<Lambdas.Unit>(new Lambdas.Unit());
            binder.RegisterClass(new ExampleSFClass());
            Lambdas.CreateScope(binder);
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