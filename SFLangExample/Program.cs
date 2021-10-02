using System;
using System.IO;
using SFLang.Exceptions;
using SFLang.Language;
using SFLang.Lexicon;

namespace SFLangExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser p = new($"{Directory.GetCurrentDirectory()}\\sflang.txt");
                Lambdas.Compile(p)();
            }
            catch (PrettyException pe)
            {
                var ex = new PrettyException(-1, -1, typeof(Program).FullName, "Thread Death prevention", pe);
                ex.PrettyPrint();
            }
        }
    }
}