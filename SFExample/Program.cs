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
            writer.CreateCategory("testing");

            SkyblockItem i = new(ItemType.Sword, ItemRarity.Legendary, "Flower of Truth", "poppy");

            i.Damage = 160;
            i.Strength = 300;

            i.DungeonStats.IsDungeon = true;
            i.DungeonStats.GearScore = 460;
            i.DungeonStats.Quality = 70000;

            i.HasGlint = true;

            ItemAbility abil = new("Heat-Seeking Rose", AbilityType.RightClick, 56, 1);
            abil.Description = new();
            abil.Description.Append("Shoots a rose that ricochets");
            abil.Description.Append("between enemies, damaging up to");
            SuperRawText a = new();
            a.Append("3", Color.Green);
            a.Append(" of your foes! Damage", Color.Gray);
            abil.Description.Append(a);
            abil.Description.Append("multiplies as more enemies are");
            abil.Description.Append("hit");

            i.Abilities.Add(abil);
            AdvancedDescription desc = new();
            desc.Append("The mana cost of this item is");
            SuperRawText b = new();
            b.Append("10.0%", Color.Green);
            b.Append(" of your maximum mana.", Color.Gray);
            desc.Append(b);
            i.AddDescription(desc);
            string cmd = i.Compile();

            SlayerDrop drop = new(i, DropRarity.PrayRNGesus);
            drop.MinimumTier = 4;
            drop.TierAmounts["Tier IV"] = "1";
            drop.TierAmounts["Tier V"] = "1 to 2";
            drop.TierAmounts["Tier VI"] = "3";
            string cmd2 = drop.Compile();

            CommandModule mod = new();
            writer.CreateFunction("pogchamp2");
            writer.WriteCommand(cmd2, "pogchamp2");


            Console.ReadLine();
        }
    }
}
