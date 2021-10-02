using SFLang.Language;

namespace SFLang.Lexicon
{
    public static class Lambdas
    {
        /// <summary>
        /// Compiling the specified code to a lambda function, without requiring
        /// the caller to bind the evaluation towards a particular type.
        /// 
        /// Will bind to all the default 'keywords' in SFLang found in the
        /// Lexic class.
        /// </summary>
        /// <returns>The compiled lambda function.</returns>
        /// <param name="code">SFLang code to compile.</param>
        public static Func<object> Compile(string code)
        {
            var function = Lexer.Compile<Unit>(Parser.Default, code);
            var binder = new ContextBinder<Unit>();
            BindLexic(binder);
            var nothing = new Unit();
            return () => function(nothing, binder);
        }

        public static Func<object> Compile(Parser parser)
        {
            var function = Lexer.Compile<Unit>(parser, parser.RawContents);
            var binder = new ContextBinder<Unit>();
            BindLexic(binder);
            var nothing = new Unit();
            return () => function(nothing, binder);
        }
        
        /// <summary>
        /// Compiles the specified code, binding to the specified context, and
        /// returns a function allowing you to evaluate the specified code.
        /// 
        /// Will bind to all the default 'keywords' in SFLang found in the
        /// Lexic class.
        /// </summary>
        /// <returns>The compiled lambda function.</returns>
        /// <param name="context">Context to bind the evaluation towards.</param>
        /// <param name="code">SFLang code to compile.</param>
        /// <param name="bindDeep">If true will perform binding on type of instance, and not on type TContext.</param>
        /// <typeparam name="TContext">The type of context you want to bind towards.</typeparam>
        public static Func<object> Compile<TContext>(TContext context, string code, bool bindDeep = false)
        {
            var function = Lexer.Compile<TContext>(Parser.Default, code);
            var binder = new ContextBinder<TContext>(bindDeep ? context : default);
            BindLexic(binder);
            return () => {
                return function(context, binder);
            };
        }

        /// <summary>
        /// Compiles the specified code, binding to the specified context, and
        /// returns a function allowing you to evaluate the specified code.
        /// 
        /// Will not bind the binder to any Lexic. If you wish to bind the
        /// binder to the default Lexic, you can use 'LambdaCompiler.BindLexic'.
        /// 
        /// If you use this overload, and you cache your Binder, you will
        /// experience significant performance improvements, since the process of creating
        /// a Binder has some overhead, due to the compilation of lambda expressions,
        /// and dependencies upon reflection. If you do, you must never use your
        /// "master" Binder instance, but Clone it every time you want to use it.
        /// </summary>
        /// <returns>The compiled lambda function.</returns>
        /// <param name="context">Context to bind the lambda towards.</param>
        /// <param name="binder">Binder to use for your lambda.</param>
        /// <param name="code">SFLang code to compile.</param>
        /// <typeparam name="TContext">The type of context you want to bind towards.</typeparam>
        public static Func<object> Compile<TContext>(TContext context, ContextBinder<TContext> binder, string code)
        {
            var function = Lexer.Compile<TContext>(Parser.Default, code);
            return new Func<object>(() => {
                return function(context, binder);
            });
        }

        /// <summary>
        /// Binds the specified binder to all default Lexic in SFLang from the Lexic class.
        /// </summary>
        /// <param name="binder">Binder to bind.</param>
        /// <typeparam name="TContext">The type of context you want to use.</typeparam>
        public static void BindLexic<TContext>(ContextBinder<TContext> binder)
        {
            binder["let"] = Lexic<TContext>.Let;
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

            binder["+"] = Lexic<TContext>.Add;
            binder["-"] = Lexic<TContext>.Subtract;
            binder["*"] = Lexic<TContext>.Multiply;
            binder["/"] = Lexic<TContext>.Divide;
            binder["%"] = Lexic<TContext>.Modulo;

            binder["substr"] = Lexic<TContext>.Substr;
            binder["length"] = Lexic<TContext>.Length;
            binder["replace"] = Lexic<TContext>.Replace;

            binder["eval"] = Lexic<TContext>.Eval;

            binder["null"] = null;
            binder["error"] = Lexic<TContext>.Exit;

            binder["out"] = Lexic<TContext>.Out;
        }

        public class Unit { };
    }
}