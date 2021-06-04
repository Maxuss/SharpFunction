using System;
using System.IO;
using SharpFunction.API;
using SharpFunction.Writer;
using SharpFunction.Commands;
using SharpFunction.Commands.Minecraft;
using System.Linq;

namespace SFExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is an example/testing project for SharpFunction");
            Project project = new Project("Example project", Path.Combine(Directory.GetCurrentDirectory(), "Example"));
            project.Generate();
            FunctionWriter writer = project.Writer;
            var s = new EntitySelector(
                Selector.AllEntities, 
                new SelectorParameters("type", "minecraft:armor_stand",
                                       "name", "test"));

            writer.CreateCategory("another_test");
            writer.CreateFunction("test_function_two");
            CommandModule module = new CommandModule();
            var say = new Say();
            var tp = new Teleport(s);
            say.Compile("I am a message");
            tp.Compile(new Vector3("~3 ~2 ~3.5"));
            Console.WriteLine(tp.Compiled);
            writer.WriteCommand(module, "test_function_two");
            Console.ReadLine();
        }
    }
}
