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
            // Project project = new Project("ShaprFunction Sample 2", $@"{Directory.GetCurrentDirectory()}\Sample2");
            // project.Format = PackFormat.DotSixteen;
            // project.Generate();

            Project project = Project.Load($@"{Directory.GetCurrentDirectory()}\Sample2");
            FunctionWriter w = project.Writer;
            // SlayerExample ex = new(w);
            Console.WriteLine(project.ProjectPath);
            string d = "{Id:[I;-1658131564,-640792186,-1301039191,1645992239],Properties:{textures:[{Value:\"eyJ0ZXh0dXJlcyI6eyJTS0lOIjp7InVybCI6Imh0dHA6Ly90ZXh0dXJlcy5taW5lY3JhZnQubmV0L3RleHR1cmUvMTFiMzE4OGZkNDQ5MDJmNzI2MDJiZDdjMjE0MWY1YTcwNjczYTQxMWFkYjNkODE4NjJjNjllNTM2MTY2YiJ9fX0=\"}]}}";

            SkyblockSkull world = new(ItemType.Accessory, ItemRarity.VerySpecial, "The World", d);
            world.MagicFind = 3;
            world.PetLuck = 3;

            ItemAbility abil = new("Everything within");
            abil.Description = new();
            abil.Description.Append("Each time you join SkyBlock island, this");
            abil.Description.Append("accessory will gain random effect of any");
            abil.Description.Append("other accessory.");
            SuperRawText stack = new();
            stack.Append("Effects", Color.Gray);
            stack.Append(" can stack", Color.Green);
            stack.Append("!", Color.Gray);
            abil.Description.Append(stack);
            world.Abilities.Add(abil);
            AdvancedDescription desc = new();
            SuperRawText eff = new();
            eff.Append("Current effect: ", Color.Gold);
            eff.Append("NONE!", Color.Red, RawTextFormatting.Straight, RawTextFormatting.Bold);
            desc.Append(eff);
            desc.Append("");
            desc.Append("The whole SkyBlock lies inside you...", Color.DarkGray);
            desc.Append("");
            world.AddDescription(desc);

            Console.WriteLine(world.Compile());

            Console.WriteLine("Finished generating datapack!");
            Console.ReadLine();
        }
    }
}
