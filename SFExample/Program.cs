using System;
using System.IO;
using SharpFunction.API;
using SharpFunction.Writer;
using SharpFunction.Commands;
using SharpFunction.Addons.Skyblock;
using static SharpFunction.Addons.Skyblock.SlayerDrop;
using SharpFunction.Commands.Minecraft;
using System.Collections.Generic;
using SFExample.Skyblock.Items;
using SharpFunction.Universal;
using SFExample.Skyblock.Slayer;

namespace SFExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is an example/testing project for SharpFunction");
            // creating new project
            // Project project = new Project("SharpFunction Sample 2", $@"{Directory.GetCurrentDirectory()}\Sample2");
            // project.Format = PackFormat.v1_16;
            // project.Generate();
            //
            // // OR
            //
            // // Reading the project
            // project = Project.Load($@"{Directory.GetCurrentDirectory()}\Sample2");
            // FunctionWriter w = project.Writer;
            //
            // // Make name of zombie
            // SuperRawText srt = new();
            // srt.Append("Zombie Walker", Color.Blue);
            //
            // RawText n = new();
            // n.AddField(srt);
            //
            // RawText[] reqs = {
            //     new RawText().AddField("Fishing Skill L", Color.Gray, RawTextFormatting.Straight),
            //     new RawText().AddField("Among us game", Color.Red, RawTextFormatting.Straight),
            // };
            //
            // RawText[] drops =
            // {
            //     new RawText().AddField("Super rare item", Color.Gold, RawTextFormatting.Straight),
            //     new RawText().AddField("Rare item", Color.Blue, RawTextFormatting.Straight),
            //     new RawText().AddField("Gamer chair", Color.Green, RawTextFormatting.Straight, RawTextFormatting.Bold),
            // };
            //
            // // make new sea creature with provided drops
            // SkyblockSeaCreature cr = new("zombie", "rotten_flesh", 150, 15000, n, "Zombie Walker", ItemRarity.Legendary);
            // // set equipment for zombie
            // cr.Equipment = new(new string[] { "\"golden_axe\", tag:{Enchantments:[{}]}" }, new string[] { "\"chainmail_helmet\"", "\"chainmail_chestplate\"", "\"leather_leggings\"", "\"iron_boots\"" });
            // cr.Requirements = reqs;
            // cr.Drops = drops;

            // output generated stuff
            var item = new Items();
            Console.WriteLine("Finished generating datapack!");
            Console.ReadLine();
        }
    }
}
