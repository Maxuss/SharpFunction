using System;
using System.IO;
using SharpFunction.API;
using SharpFunction.Writer;
using SharpFunction.Commands;
using SharpFunction.Addons.Skyblock;
using static SharpFunction.Addons.Skyblock.SlayerDrop;
using SharpFunction.Commands.Minecraft;
using System.Collections.Generic;
using SharpFunction.Universal;
using SFExample.Skyblock.Slayer;

namespace SFExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is an example/testing project for SharpFunction");
            Project project = new Project("ShaprFunction Sample", $@"{Directory.GetCurrentDirectory()}\Samples");
            project.Format = PackFormat.DotSixteen;
            project.Generate();
            FunctionWriter w = project.Writer;
            // SlayerExample ex = new(w);
            var ttl = new Title(SimpleSelector.@p);
            SuperRawText c = new();
            c.Append("I am bold", Color.Gold, RawTextFormatting.Straight, RawTextFormatting.Bold);
            c.Append(" i am italic", Color.Red, RawTextFormatting.Italic, RawTextFormatting.Underlined);
            ttl.Compile(TitlePosition.Title, c);
            Console.WriteLine(ttl.Compiled);
            Console.WriteLine("Finished generating datapack!");
            Console.ReadLine();
        }
    }
}
