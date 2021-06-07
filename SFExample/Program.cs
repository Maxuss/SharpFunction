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

            var l = new RawText();
            l.AddField("I am a cool lore text",
                        Color.DarkRed, RawTextFormatting.Straight, RawTextFormatting.Bold);
            l.AddField("BOTTOM TEXT", Color.Gold, RawTextFormatting.Straight);
            l.AddField("I am obfuscated!", Color.White, RawTextFormatting.Obfuscated);
            
            var n = new RawText();
            n.AddField("I am a super cool name", 
                Color.Aqua, RawTextFormatting.Underlined, RawTextFormatting.Straight);

            var display = new ItemDisplay();
            display.AddLore(l); display.AddName(n);
            var nbt = new ItemNBT();
            nbt.Display = display;
            var item = new Item("minecraft:golden_sword", nbt);
            var cmd = new Give(SimpleSelector.@p);
            cmd.Compile(item);

            CommandModule m = new();
            

            Console.WriteLine(cmd.Compiled);

            SkyblockItem sb = new(ItemType.Accessory, ItemRarity.Legendary, "");
            sb.MagicFind = 6;
            sb.PetLuck = 6;
            sb.SeaCreatureChance = 6;

            ItemAbility ability = new(
                "Halo of the burned",
                "You damage enemies around you for \n2x of your Max Health every second.",
                AbilityType.RightClick
            );
            sb.Ability = ability;
            sb.AddDescription("Our lady of The Charred Visage", Color.DarkGray, RawTextFormatting.Italic);
            sb.AddName("Eye of The Forgotten Goddess");
            sb.Compile("minecraft:gold_nugget");
            
            m.Append(cmd, sb.Command);

            writer.WriteCommand(m, "epic_function");

            Console.ReadLine();
        }
    }
}
