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
            Project project = new Project("ShaprFunction Sample 2", $@"{Directory.GetCurrentDirectory()}\Sample2");
            project.Format = PackFormat.DotSixteen;
            project.Generate();

            //Project project = Project.Load($@"{Directory.GetCurrentDirectory()}\Sample2");
            //FunctionWriter w = project.Writer;
            // SlayerExample ex = new(w);

            SkyblockItem test = new(ItemType.Bow, ItemRarity.Trash, "TEST", "leather_helmet");
            test.NameColor = Color.Red;
            test.MagicFind = -999;
            test.IsLeather = true;
            test.LeatherColorHex = "#403b8c";
            test.AddDescription(new AdvancedDescription().Append("Testy"));
            Console.WriteLine(test.Compile());

            Console.WriteLine("Finished generating datapack!");
            Console.ReadLine();
        }
    }
}
