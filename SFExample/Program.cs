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
            Project project = new Project("SharpFunction Sample 2", $@"{Directory.GetCurrentDirectory()}\Sample2");
            project.Format = PackFormat.DotSixteen;
            project.Generate();

            // Project project = Project.Load($@"{Directory.GetCurrentDirectory()}\Sample2");
            // FunctionWriter w = project.Writer;
            // SlayerExample ex = new(w);

            SuperRawText srt = new();
            srt.Append("Zombie Walker", Color.Blue);

            RawText n = new();
            n.AddField(srt);

            RawText[] reqs = {
                new RawText().AddField("Fishing Skill L", Color.Gray, RawTextFormatting.Straight),
                new RawText().AddField("Among us game", Color.Red, RawTextFormatting.Straight),
            };

            RawText[] drops =
            {
                new RawText().AddField("Super rare item", Color.Gold, RawTextFormatting.Straight),
                new RawText().AddField("Rare item", Color.Blue, RawTextFormatting.Straight),
                new RawText().AddField("Deez nuts lmao", Color.Green, RawTextFormatting.Straight, RawTextFormatting.Bold),
            };

            SkyblockSeaCreature cr = new("zombie", "rotten_flesh", 150, 15000, n, "Zombie Walker", ItemRarity.Legendary);
            cr.Equipment = new(new string[] { "\"golden_axe\", tag:{Enchantments:[{}]}" }, new string[] { "\"chainmail_helmet\"", "\"chainmail_chestplate\"", "\"leather_leggings\"", "\"iron_boots\"" });
            cr.Requirements = reqs;
            cr.Drops = drops;

            Console.WriteLine(cr.GenerateItem());
            Console.WriteLine(cr.GenerateMob());
            Console.WriteLine("Finished generating datapack!");
            Console.ReadLine();
        }
    }
}
