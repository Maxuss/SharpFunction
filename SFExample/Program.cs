using System;
using System.IO;
using SharpFunction.API;
using SharpFunction.Writer;
using SharpFunction.Commands;
using SharpFunction.Addons.Skyblock;
using static SharpFunction.Addons.Skyblock.SlayerDrop;
using SharpFunction.Commands.Minecraft;
using System.Collections.Generic;

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

            SkyblockItem item = new(ItemType.Axe, ItemRarity.Rare, "new item", "minecraft:gold_axe");
            item.AddDescription("very epic description");
            item.Compile();
            var desc = item.ItemDescription;
            SlayerItem si = new("Item name", desc, "minecraft:gold_axe", ItemRarity.Legendary, ItemType.Accessory, 5, "Amogus", true, true);
            string cmd = si.GenerateCommand();
            SlayerDrop drop = new("Test drop", DropRarity.RNGesusIncarnate, ItemRarity.Legendary, "minecraft:gold_ingot");
            drop.MinimumTier = 5;
            drop.RequiredLVL = 7;
            drop.TierAmounts["Tier VI"] = 1;
            string cmd2 = drop.Generate();
            CommandModule mod = new();
            mod.Append(cmd, _cmd2.Compiled);
            writer.CreateFunction("pogchamp2");
            writer.WriteCommand(mod, "pogchamp2");

            Console.ReadLine();
        }
    }
}
