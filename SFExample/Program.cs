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

            SkyblockItem i = new(ItemType.Sword, ItemRarity.Rare, "Heroic Aspect of the End", "diamond_sword");
            i.Damage = 100;
            i.Strength = 125;
            i.AttackSpeed = 2;
            i.Intelligence = 65;

            ItemAbility a = new("Instant Transmission", "Haha teleport go brrrrr", AbilityType.RightClick, 50);
            i.Ability = a;
            i.AddDescription("");
            string ac = i.Compile();
            Console.WriteLine(ac);
            CommandModule mod = new();
            mod.Append(ac);
            writer.CreateFunction("pogchamp2");
            writer.WriteCommand(mod, "pogchamp2");


            Console.ReadLine();
        }
    }
}
