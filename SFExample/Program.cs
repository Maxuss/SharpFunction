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

            SlayerAbility abil = new SlayerAbility();
            abil.AbilityColor = Color.Green;
            abil.Description = "I am a test description\nI am new line";
            abil.Name = "Ability name";
            abil.PercentsToHappen = new int[] { 80, 60, 40, 20 };
            SlayerTier tier = new(
                "gold_nugget", "Gamer", 3, "Hard", 5000, 1500, 24000, "blaze", 1000, null, abil
            );
            tier.Compile();
            CommandModule mod = new();
            mod.Append(tier.GiveCommand, tier.SummonCommand);
            writer.CreateFunction("pogchamp");
            writer.WriteCommand(mod, "pogchamp");
            Console.ReadLine();
        }
    }
}
