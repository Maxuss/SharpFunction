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

            CommandModule m = new();

            var e = new SkyblockEntity("blaze", "Furious Warrior", 150000, 120);
            e.CurrentHP = 150000;
            e.NoAI = true;
            e.Compile();
            var summon = e.Command;

            m.Append(tp, summon);
            writer.WriteCommand(m, "epic_function");
            Console.WriteLine(summon.Compiled);

            Console.ReadLine();
        }
    }
}
