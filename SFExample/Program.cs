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

            var result = new ChestSlot("blaze_rod", 32);
            var recipe = new SkyblockRecipe();
            recipe.Result = result;
            recipe.AddIngredient(result, 2, 1);
            string[] cmds = recipe.Compile();
            Console.WriteLine(cmds[0]);
            Console.WriteLine(cmds[1]);

            Console.ReadLine();
        }
    }
}
