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
            SlayerExample ex = new(w);
            Console.WriteLine("Finished generating datapack!");
            Console.ReadLine();
        }
    }
}
