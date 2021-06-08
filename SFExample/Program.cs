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

            var tp = new Teleport(SimpleSelector.p);
            tp.Compile(new Vector3("~2 ~56 3"));

            CommandModule m = new();

            var summon = new Summon();
            var n = new EntityNBT();
            n.NoAI = true;
            n.Invulnerable = true;
            n.CustomNameVisible = true;
            var srt = new SuperRawText();
            srt.Append("[", Color.DarkGray); srt.Append("Lv.134", Color.Gray); srt.Append("]", Color.DarkGray);
            srt.Append(" Amogus", Color.Red); srt.Append(" 13000", Color.Green);
            srt.Append("/", Color.White); srt.Append("13000", Color.Green); srt.Append("❤", Color.Red);
            n.CustomName = srt;
            var asn = new ArmorStandNBT();
            asn.Invisible = true;
            Entity e = new("minecraft:armor_stand", NBTWrapper.Wrap(n.Compile(), asn.Compile()));
            summon.Compile(e, SimpleVector.Current);

            m.Append(tp, summon);
            writer.WriteCommand(m, "epic_function");
            Console.WriteLine(summon.Compiled);

            Console.ReadLine();
        }
    }
}
