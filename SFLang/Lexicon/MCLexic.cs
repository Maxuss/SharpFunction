using System.Globalization;
using System.Linq;
using SharpFunction.API;
using SharpFunction.Commands;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;
using Gamemode = SharpFunction.Universal.Gamemode;

namespace SFLang.Lexicon
{
    public static class MCLexic<TContext>
    {
        public static Function<TContext> Command = (ctx, binder, args) =>
        {
            if (args.Count <= 0) return null;
            var str = args.Get<string>(0);
            var sselector = args.Get(0);
            if (sselector is string && new[] {"@r", "@a", "@e", "@p"}.Contains(sselector))
            {
                // With selector
                var selector = args.Get<string>(1) switch
                {
                    "@r" => SimpleSelector.Random,
                    "@a" => SimpleSelector.All,
                    "@e" => SimpleSelector.AllEntities,
                    "@p" => SimpleSelector.Current,
                    _ => SimpleSelector.Current
                };
                switch (str)
                {
                    case "tp":
                    case "teleport":
                    {
                        var cmd = new Teleport(selector);
                        cmd.Compile(args.Get<Vector3>(2));
                        return cmd;
                    }
                    case "tellraw":
                    {
                        var cmd = new Tellraw(selector);
                        cmd.Compile(args.Get<SuperRawText>(2));
                        return cmd;
                    }
                    case "title":
                    {
                        var cmd = new Title(selector);
                        var stri = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(((string) args.Get(2)).Replace("_", " "));
                        Enum.TryParse(stri.Replace(" ", ""), out TitlePosition tpos);
                        cmd.Compile(tpos, args.Get<SuperRawText>(3));
                        return cmd;
                    }
                    case "kill":
                    {
                        var cmd = new Kill(selector);
                        cmd.Compile();
                        return cmd;
                    }
                    case "attribute":
                    {
                        var cmd = new SharpFunction.Commands.Minecraft.Attribute(selector);
                        var stri = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(((string) args.Get(2)).Replace("_", " "));
                        Enum.TryParse(stri.Replace(" ", ""), out AttributeAction aa);
                        var atr = args.Get(3, "null");
                        switch (aa)
                        {
                            case AttributeAction.GetBase:
                            case AttributeAction.Get:
                            {
                                cmd.Compile(atr, aa, args.Get(4, 0d));
                                return cmd;
                            }
                            case AttributeAction.SetBase:
                            {
                                cmd.Compile(atr, aa, args.Get(4, 0));
                                return cmd;
                            }
                            case AttributeAction.AddModifier:
                            {
                                var act 
                                    = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(((string) args.Get(4)).Replace("_", " "));
                                Enum.TryParse(stri.Replace(" ", ""), out ModifierAction ma);
                                cmd.Compile(atr, aa, args.Get(5, ""), args.Get(6, ""), args.Get(7, 0), act);
                                return cmd;
                            }
                            case AttributeAction.RemoveModifier:
                            {
                                cmd.Compile(atr, aa, args.Get<string>(4));
                                return cmd;
                            }
                            case AttributeAction.GetModifierValue:
                            {
                                cmd.Compile(atr, aa, args.Get<string>(4), args.Get(5, 0d));
                                return cmd;
                            }
                            default:
                                return cmd;
                        }
                    }
                    case "gamemode":
                    {
                        var cmd = new SharpFunction.Commands.Minecraft.Gamemode();
                        var stri = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(((string) args.Get(2)).Replace("_", " "));
                        Enum.TryParse(stri.Replace(" ", ""), out Gamemode gm);
                        cmd.Compile(gm, selector);
                        return cmd;
                    }
                    case "enchant":
                    {
                        var cmd = new Enchant(selector);
                        var stri = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(((string) args.Get(2)).Replace("_", " "));
                        Enum.TryParse(stri.Replace(" ", ""), out Enchantment ench);
                        cmd.Compile(ench, args.Get(3, 1U));
                        return cmd;
                    }
                    case "effect":
                    {
                        var cmd = new Effect(selector);
                        var stri = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(((string) args.Get(2)).Replace("_", " "));
                        Enum.TryParse(stri.Replace(" ", ""), out StatusEffect effect);
                        cmd.Compile(
                            effect,
                            args.Get(3, 10U),
                            args.Get(4, 1U),
                            args.Get(5, false));
                        return cmd;
                    }
                    case "clear":
                    {
                        var cmd = new Clear(selector);
                        cmd.Compile(args.Get(2, ""), args.Get(3, -1));
                        return cmd;
                    }
                    case "banip":
                    {
                        var cmd = new BanIP(selector);
                        cmd.Compile(args.Count > 2 ? args.Get<string>(2) : "");
                        return cmd;
                    }
                    case "ban":
                    {
                        var cmd = new Ban(selector);
                        cmd.Compile(args.Count > 2 ? args.Get<string>(2) : "");
                        return cmd;
                    }
                    case "give":
                    {
                        var cmd = new Give(selector);
                        cmd.Compile(args.Get<Item>(2), args.Count > 3 ? args.Get<int>(3) : 1);
                        return cmd;
                    }
                    default:
                        return null;
                }
            }

            switch (str)
            {
                case "bossbar":
                {
                    var cmd = new Bossbar();
                    var id = args.Get<string>(2);
                    var sec = args.Get<string>(3);
                    var cult = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sec);
                    if (Enum.TryParse(cult, out BossbarGet get))
                    {
                        cmd.Compile(id, get);
                        return cmd;
                    }

                    if (Enum.TryParse(cult, out BossbarSet set))
                    {
                        cmd.Compile(id, set, args.Get(4));
                        return cmd;
                    }
                    cmd.Compile(id, sec);
                    return cmd;
                }
                case "fill":
                {
                    var cmd = new Fill();
                    cmd.Compile(args.Get<Vector3>(1), args.Get<Vector3>(2), args.Get<string>(3));
                    return cmd;
                }
                case "clone":
                {
                    var cmd = new Clone();
                    cmd.Compile(args.Get<Vector3>(1), args.Get<Vector3>(2), args.Get<Vector3>(3));
                    return cmd;
                }
            }
        };
    }
}