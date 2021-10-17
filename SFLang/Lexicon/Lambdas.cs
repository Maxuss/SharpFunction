
using System;
using System.IO;
using SFLang.Language;

namespace SFLang.Lexicon
{
    public static class Lambdas
    {
        public static void CreateScope<TContext>(ContextBinder<TContext> binder)
        {
            ContextBinder<TContext>.InstanceBinder = binder;
            PopulateLexic(binder);
        }

        public static Func<object> Compile<TContext>(string code, TContext ctx)
        {
            var trueCode = Parser.UpgradeCode(code);
            var function = Lexer.Compile<TContext>(Parser.Default, trueCode);
            ContextBinder<TContext> binder;
            if (ContextBinder<TContext>.InstanceBinder != null)
            {
                binder = ContextBinder<TContext>.InstanceBinder;
            }
            else
            {
                binder = new ContextBinder<TContext>();
                PopulateLexic(binder);
            }

            return () => function(ctx, binder);
        }

        public static Func<object> Compile(string code)
        {
            code = Parser.UpgradeCode(code);
            var function = Lexer.Compile<Unit>(Parser.Default, code);
            ContextBinder<Unit> binder;
            if (ContextBinder<Unit>.InstanceBinder != null)
            {
                binder = ContextBinder<Unit>.InstanceBinder;
            }
            else
            {
                binder = new ContextBinder<Unit>();
                PopulateLexic(binder);
            }

            var nothing = new Unit();
            return () => function(nothing, binder);
        }

        public static Func<object> Compile(Parser parser)
        {
            var function = Lexer.Compile<Unit>(parser, Parser.UpgradeCode(parser.RawContents ?? "null"));
            ContextBinder<Unit> binder;
            if (ContextBinder<Unit>.InstanceBinder != null)
            {
                binder = ContextBinder<Unit>.InstanceBinder;
            }
            else
            {
                binder = new ContextBinder<Unit>();
                PopulateLexic(binder);
            }

            var nothing = new Unit();
            return () => function(nothing, binder);
        }

        public static Func<object> Compile<TContext>(TContext context, string code, bool bindDeep = false)
        {
            code = Parser.UpgradeCode(code);
            var function = Lexer.Compile<TContext>(Parser.Default, code);
            ContextBinder<TContext> binder;
            if (ContextBinder<TContext>.InstanceBinder != null)
            {
                binder = ContextBinder<TContext>.InstanceBinder;
            }
            else
            {
                binder = new ContextBinder<TContext>(bindDeep ? context : default);
                PopulateLexic(binder);
            }

            return () => { return function(context, binder); };
        }

        public static Func<object> Compile<TContext>(TContext context, ContextBinder<TContext> binder, string code)
        {
            code = Parser.UpgradeCode(code);
            var function = Lexer.Compile<TContext>(Parser.Default, code);
            return () => { return function(context, binder); };
        }

        public static void PopulateLexic<TContext>(ContextBinder<TContext> binder)
        {
            binder["PATH"] = new Constant { Value = Directory.GetCurrentDirectory() };

            binder["&"] = Lexic<TContext>.Dereference;
            binder["deref"] = Lexic<TContext>.Dereference;
            binder["*"] = Lexic<TContext>.Reference;
            binder["ref"] = Lexic<TContext>.Reference;

            binder["nullptr"] = null;

            binder["assert"] = Lexic<TContext>.Assertion;
            
            binder["invoke"] = Lexic<TContext>.Invoke;
            binder["invoke-static"] = Lexic<TContext>.InvokeStatic;
            binder["field"] = Lexic<TContext>.Field;
            binder["prop"] = Lexic<TContext>.Property;

            binder["startlocal"] = Lexic<TContext>.StartLocalScope;
            binder["stoplocal"] = Lexic<TContext>.StartLocalScope;

            binder["loop"] = Lexic<TContext>.Loop;
            binder["dir"] = Lexic<TContext>.Dir;
            binder["extern"] = Lexic<TContext>.External;
            binder["const"] = Lexic<TContext>.Constant;
            binder["let"] = Lexic<TContext>.Let;
            binder["local"] = Lexic<TContext>.Local;
            binder["set"] = Lexic<TContext>.Set;

            binder["if"] = Lexic<TContext>.If;
            binder["eq"] = Lexic<TContext>.Eq;
            binder["mt"] = Lexic<TContext>.Mt;
            binder["lt"] = Lexic<TContext>.Lt;
            binder["mte"] = Lexic<TContext>.Mte;
            binder["lte"] = Lexic<TContext>.Lte;
            binder["not"] = Lexic<TContext>.Not;

            binder["any"] = Lexic<TContext>.Any;
            binder["all"] = Lexic<TContext>.All;

            binder["method"] = Lexic<TContext>.Method;
            binder["apply"] = Lexic<TContext>.Apply;

            binder["list"] = Lexic<TContext>.List;
            binder["slice"] = Lexic<TContext>.Slice;

            binder["map"] = Lexic<TContext>.Map;

            binder["get"] = Lexic<TContext>.Get;
            binder["count"] = Lexic<TContext>.Count;
            binder["add"] = Lexic<TContext>.AddValue;
            binder["update"] = Lexic<TContext>.UpdateValue;
            binder["each"] = Lexic<TContext>.Each;

            binder["string"] = Lexic<TContext>.String;
            binder["number"] = Lexic<TContext>.Number;
            binder["json"] = Lexic<TContext>.Json;

            binder["sum"] = Lexic<TContext>.Add;
            binder["sub"] = Lexic<TContext>.Subtract;
            binder["mul"] = Lexic<TContext>.Multiply;
            binder["div"] = Lexic<TContext>.Divide;
            binder["mod"] = Lexic<TContext>.Modulo;
            binder["pow"] = Lexic<TContext>.Power;

            binder["substr"] = Lexic<TContext>.Substr;
            binder["length"] = Lexic<TContext>.Length;
            binder["replace"] = Lexic<TContext>.Replace;

            binder["eval"] = Lexic<TContext>.Eval;

            binder["null"] = null;
            binder["true"] = true;
            binder["false"] = false;
            binder["NaN"] = double.NaN;

            binder["error"] = Lexic<TContext>.Exit;

            binder["out"] = Lexic<TContext>.Out;
            // binder["input"] = Lexic<TContext>.Out;

            binder["include"] = Lexic<TContext>.Include;
            binder["declare"] = Lexic<TContext>.Module;
            
            // SharpFunction related lexic

            binder["command"] = MCLexic<TContext>.Command;
            binder["vector"] = MCLexic<TContext>.Vector;
            binder["extract"] = MCLexic<TContext>.Extract;
        }

        public class Unit
        {
            
        }
    }
}