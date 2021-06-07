using System;
using System.IO;
using SharpFunction.API;
using SharpFunction.Writer;
using SharpFunction.Commands;
using SharpFunction.Commands.Minecraft;
using System.Linq;
using SharpFunction.Universal;

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
            var tp = new Teleport(s);
            tp.Compile(new Vector3("~3 ~2 ~3.5"));
            var arr = new object[] { "zxy", new EntitySelector(Selector.Current)};
            var p = new ExecuteParameters(tp, ExecuteCondition.Align, ExecuteOperator.If, ExecuteSubcondition.Entity, extraParameters:arr);
            var e = new Execute();
            e.Compile(p);
            RawText l = new RawText();
            l.AddField("I am a test", Color.Gold, RawTextFormatting.Bold);
            l.AddField("I am cooler test", Color.Red, RawTextFormatting.Italic);
            RawText n = new RawText();
            n.AddField("I am a super cool name", Color.Aqua);
            ItemDisplay display = new ItemDisplay();
            display.AddLore(l);
            display.AddName(n);
            ItemNBT nb = new ItemNBT();
            nb.Display = display;
            Console.WriteLine(nb.Compile());
            Item item = new Item("minecraft:green_dye", nb);
            var give = new Give(SimpleSelector.@p);
            give.Compile(item, 2);
            Console.WriteLine(give.Compiled);
            writer.WriteCommand(module, "test_function_two");
            Console.ReadLine();
        }
    }
}
