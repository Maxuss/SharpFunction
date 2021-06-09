using System;
using System.IO;
using SharpFunction.API;
using SharpFunction.Writer;
using SharpFunction.Commands;
using SharpFunction.Commands.Minecraft;
using System.Linq;
using SharpFunction.Universal;
using SharpFunction.Addons.Skyblock;

namespace SFExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is an example/testing project for SharpFunction");
            Project project = new Project("test", Directory.GetCurrentDirectory());
            project.Generate();
            FunctionWriter writer = project.Writer;
            writer.CreateCategory("cool category");
            writer.CreateFunction("epic_function");

            var tp = new Teleport(SimpleSelector.p);
            tp.Compile(new Vector3("~2 ~56 3"));

            var srt = new SuperRawText();
            srt.Append("kek ", Color.White, RawTextFormatting.Bold, RawTextFormatting.Obfuscated);
            srt.Append("I am cool gold text. ", Color.Gold, RawTextFormatting.Bold);
            srt.Append("And I am very amazing red text.", Color.Red, RawTextFormatting.Italic);
            srt.Append(" kek", Color.White, RawTextFormatting.Bold, RawTextFormatting.Obfuscated);
            var tr = new Tellraw(SimpleSelector.a);
            tr.Compile(srt);
            Console.WriteLine(tr.Compiled);
            CommandModule m = new();
            Console.ReadLine();
        }
    }
}
