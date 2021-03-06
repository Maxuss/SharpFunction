using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SFLang.Exceptions;
using SFLang.Language;
using File = System.IO.File;

namespace SFLang.Lexicon
{
    public static class Lexer
    {

        public static bool UseStrict { get; set; } = false;
        
        /// <summary>
        ///     Compile the SFLang code found in the specified stream.
        /// </summary>
        /// <returns>The compiled lambda object.</returns>
        /// <param name="tokenizer">The tokenizer to use.</param>
        /// <param name="stream">
        ///     Stream containing SFLang code. Notice, this method does not claim ownership over
        ///     your stream, and you are responsible for correctly disposing it yourself
        /// </param>
        /// <typeparam name="TContext">The type of your context object.</typeparam>
        public static Lambda<TContext> Compile<TContext>(Parser parser, Stream stream)
        {
            var tokenized = parser.Tokenize(stream).ToList();
            Console.WriteLine($"TOKENIZED: {string.Join(',', tokenized)}");
            return Compile<TContext>(tokenized);
        }

        /// <summary>
        ///     Compile the SFLang code found in the specified streams.
        /// </summary>
        /// <returns>The compiled lambda object.</returns>
        /// <param name="tokenizer">The tokenizer to use.</param>
        /// <param name="streams">
        ///     Streams containing SFLang code. Notice, this method does not claim ownership over
        ///     your streams, and you are responsible for correctly disposing your streams yourself
        /// </param>
        /// <typeparam name="TContext">The type of your context object.</typeparam>
        public static Lambda<TContext> Compile<TContext>(Parser parser, IEnumerable<Stream> streams)
        {
            return Compile<TContext>(parser.Tokenize(streams));
        }

        /// <summary>
        ///     Compile the specified SFLang code.
        /// </summary>
        /// <returns>The compiled lambda object.</returns>
        /// <param name="tokenizer">The tokenizer to use.</param>
        /// <param name="snippet">Your SFLang code.</param>
        /// <typeparam name="TContext">The type of your context object.</typeparam>
        public static Lambda<TContext> Compile<TContext>(Parser parser, string snippet)
        {
            var tokenized = parser.Tokenize(snippet).ToList();
            return Compile<TContext>(tokenized);
        }

        public static Lambda<TContext> Compile<TContext>(Parser tokenizer, IEnumerable<string> snippets)
        {
            return Compile<TContext>(tokenizer.Tokenize(snippets));
        }

        internal static void SanityCheckSymbolName(string symbolName)
        {
            if (symbolName.IndexOfAny(new[] {' ', '\r', '\n', '\t'}) != -1)
                throw new ParsingException(message: $"'{symbolName}' is not a valid symbol name.");
        }

        private static Lambda<TContext> Compile<TContext>(IEnumerable<string> tokens)
        {
            /*
             * Compiling main content of code.
             *
             * NOTICE!
             * We use CompileStatements, without expecting '{' or '}' surrounding
             * our statements here, since this is the "root level" content of our code.
             */
            var tuples = CompileStatements<TContext>(tokens.GetEnumerator(), false);
            var functions = tuples.Item1;

            /*
             * Creating a function wrapping evaluation of all of our root level functions in body,
             * making sure we always return the result of the last function invocation to caller,
             * also making sure we have our root level stack object available during evaluation of
             * our lambda.
             */
            return (ctx, binder) =>
            {
                /*
                 * Looping through each symbolic delegate, returning the 
                 * return value from the last to caller.
                 */
                object result = null;
                foreach (var ix in functions)
                {
                    Console.WriteLine("INVOKING");
                    result = ix(ctx, binder, null);
                }
                return result;
            };
        }


        private static Tuple<List<Function<TContext>>, bool> CompileStatements<TContext>(
            IEnumerator<string> en,
            bool forceClose = true)
        {
            // Creating a list of functions and returning these to caller.
            var content = new List<Function<TContext>>();
            var eof = !en.MoveNext();
            while (!eof && en.Current != "}")
            {
                // Compiling currently tokenized symbol.
                var (item1, item2) = CompileStatement<TContext>(en);

                // Adding function invocation to list of functions.
                content.Add(item1);

                // Checking if we're done compiling body.
                eof = item2;
                if (eof || en.Current == "}")
                    break; // Even if we're not at EOF, we might be at '}', ending the current body.
            }

            return forceClose switch
            {
                // Sanity checking tokenizer's content, before returning functions to caller.
                true when en.Current != "}" => throw new CoreException(
                    "Premature EOF while parsing code, missing an '}' character."),
                false when !eof && en.Current == "}" => throw new CoreException(
                    "Unexpected closing brace '}' in code, did you add one too many '}' characters?"),
                _ => new Tuple<List<Function<TContext>>, bool>(content, eof)
            };
        }

        /*
         * Compiles a statement, which might be a constant, a lambda object,
         * or a function invocation.
         */
        private static Tuple<Function<TContext>, bool> CompileStatement<TContext>(
            IEnumerator<string> en)
        {
            // Checking type of token, and acting accordingly.
            switch (en.Current)
            {
                case "if":
                    return CompileIfStatement<TContext>(en);
                case "{":
                    return CompileLambda<TContext>(en);
                case "@":
                    return CompileSymbolReference<TContext>(en);
                case "\"":
                case "'":
                    return CompileString<TContext>(en);
                case "> assembly":
                    return CompileAssemblyPointer<TContext>(en);
                case "> class":
                    return CompileClassPointer<TContext>(en);
                case "> method":
                    return CompileMethodPointer<TContext>(en);
                default:
                    return IsNumeric(en.Current) ? CompileNumber<TContext>(en) : CompileSymbol<TContext>(en);
            }
        }

        private static void SanitizeEnumerator(bool move, string message = "Unexpected EOF when iteration Enumerator!")
        {
            if (!move)
                throw new ParsingException(message: message);
        }
        
        private static Tuple<Function<TContext>, bool> CompileIfStatement<TContext>(IEnumerator<string> en)
        {
            if (!en.MoveNext())
                throw new ParsingException(message: "Unexpected EOF after if statement declaration!");

            while (en.Current == "(")
                SanitizeEnumerator(en.MoveNext());
            
            var condition = en.Current;
            Console.WriteLine(condition);
            SanitizeEnumerator(en.MoveNext());
            
            while (en.Current == ")")
                SanitizeEnumerator(en.MoveNext());
            
            // Checking if we are entering lambda scope
            if (en.Current != "then")
                throw new ParsingException(message: "Expected a method to come after if statement!");

            SanitizeEnumerator(en.MoveNext());
            var oper = en.Current;
            Function<TContext> lazy = null;
            switch (oper)
            {
                case ".lambda":
                {
                    SanitizeEnumerator(en.MoveNext());
                    var lambda = CompileLambda<TContext>(en);
                    var testEnum = Parser.Default.Tokenizer.Tokenize("{ out('test') }").ToList();
                    Console.WriteLine(string.Concat(testEnum));
                    var enumerator = testEnum.GetEnumerator();
                    var testLambda = CompileLambda<TContext>(enumerator);
                    lazy = (ctx, binder, args) =>
                    {
                        var condt = binder[condition];

                        testLambda.Item1(ctx, binder, new Parameters());
                        var invk = lambda.Item1;
                        if (UseStrict)
                            return condt is true ? lambda.Item1(ctx, binder, new Parameters()) : false;
                        return condt != null ? invk(ctx, binder, new Parameters()) : null;
                    };
                    break;
                }
                case ".invoke":
                {
                    SanitizeEnumerator(en.MoveNext());
                    var mtd = en.Current;
                    lazy = (ctx, binder, args) =>
                    {
                        var condt = binder[condition];
                        var methodRef = binder[mtd];
                        if (methodRef is not Function<TContext> cnd)
                            throw new ParsingException(
                                message: $"Expected a lambda or method reference but instead got {methodRef.GetType()}");

                        var isTrue = condt != null;
                        Console.WriteLine($"TRUE? {condt is true} {isTrue}");
                        if (UseStrict)
                            return condt is true ? cnd(ctx, binder, new Parameters()) : false;
                        return condt != null ? cnd(ctx, binder, new Parameters()) : null;
                    };
                    break;
                }
            }

            return new Tuple<Function<TContext>, bool>(lazy, !en.MoveNext());
        }
        
        /*
         * Compiles a lambda down to a function and returns the function to caller.
         */
        private static Tuple<Function<TContext>, bool> CompileLambda<TContext>(
            IEnumerator<string> en)
        {
            // Compiling body, and retrieving functions.
            var (functions, item2) = CompileStatements<TContext>(en);

            /*
             * Creating a function that evaluates every function sequentially, and
             * returns the result of the last function evaluation to the caller.
             */
            var function = new Function<TContext>((ctx, binder, arguments) =>
            {
                object result = null;
                foreach (var ix in functions)
                {
                    result = ix(ctx, binder, new Parameters());
                }

                return result;
            });

            /*
             * Making sure the body becomes evaluated lazy.
             * 
             * Notice!
             * A body is the only thing that is "by default" evaluated as "lazy" in
             * SFLang, and does not require the '@' character to accomplish "lazy
             * evaluation".
             */
            var lazyFunction =
                new Function<TContext>((ctx2, binder2, arguments2) => { return function; });
            return new Tuple<Function<TContext>, bool>(lazyFunction,
                item2 || !en.MoveNext());
        }

        /*
         * Compiles a literal reference down to a function and returns the function to caller.
         */
        private static Tuple<Function<TContext>, bool> CompileSymbolReference<TContext>(
            IEnumerator<string> en)
        {
            // Sanity checking tokenizer's content, since an '@' must reference an actual symbol.
            if (!en.MoveNext())
                throw new ParsingException(message: "Unexpected EOF after '@'.");

            // Storing symbol's name and sanity checking its name.
            var symbolName = en.Current;

            // Sanity checking symbol name.
            SanityCheckSymbolName(symbolName);

            // Discarding "(" token and checking if we're at EOF.
            var eof = !en.MoveNext();

            // Checking if this is a function invocation.
            if (!eof && en.Current == "(")
            {
                /*
                 * Notice, since this is a literally referenced function invocation, we
                 * don't want to apply its arguments if the function is being passed around,
                 * but rather return the function as a function, which once evaluated, applies
                 * its arguments. Hence, this becomes a "lazy function evaluation", allowing us
                 * to pass in a function evaluation, that is not evaluated before the caller
                 * explicitly evaluates the function wrapping our "inner function".
                 */
                var tuple = ApplyArguments<TContext>(symbolName, en);
                var functor = tuple.Item1;
                return new Tuple<Function<TContext>, bool>(
                    (ctx, binder, arguments) => { return functor; },
                    tuple.Item2);
            }

            /*
                 * Creating a function that evaluates to the constant value of the symbol's name.
                 * When you use the '@' character with a symbol, this implies simply returning the
                 * symbol's name.
                 */
            return new Tuple<Function<TContext>, bool>(
                (ctx, binder, arguments) => { return symbolName; }, eof);
        }

        /*
         * Compiles a constant string down to a symbol and returns to caller.
         */
        private static Tuple<Function<TContext>, bool> CompileString<TContext>(
            IEnumerator<string> en)
        {
            // Storing type of string literal quote.
            var quote = en.Current;

            // Sanity checking tokenizer's content.
            if (!en.MoveNext())
                throw new ParsingException(message: $"Unexpected EOF after {quote}.");

            // Retrieving actual string constant, and discarding closing quote character.
            var stringConstant = en.Current;
            en.MoveNext();

            // Returning a function that evaluates to the actual string's constant value.
            var function = new Function<TContext>((ctx, binder, arguments) => { return stringConstant; });
            return new Tuple<Function<TContext>, bool>(function, !en.MoveNext());
        }
        
        public static Tuple<Function<TContext>, bool> CompileAssemblyPointer<TContext>(
            IEnumerator<string> en
        )
        {
            // Pointer:
            // > class SFLang.Lexicon.Lexer
            // > function SFLang.Lexicon.Lexer.CompilerPointer
            // If method is specified it tries to access it and invoke
            // If class is specified it instantiates the class
            if (!en.MoveNext())
                throw new ParsingException(message: "Unexpected EOF after assembly pointer declaration.");

            var pointer = en.Current ?? "null";
            en.MoveNext();

            if (pointer.StartsWith(".."))
                pointer = Path.Join(Directory.GetCurrentDirectory(), pointer.Replace("..", ""));

            var function = new Function<TContext>((ctx, binder, arguments) => Assembly.LoadFile(pointer ?? "null"));
            return new Tuple<Function<TContext>, bool>(function, !en.MoveNext());
        }


        public static Tuple<Function<TContext>, bool> CompileMethodPointer<TContext>(
            IEnumerator<string> en
        )
        {
            if (!en.MoveNext())
                throw new ParsingException(message: "Unexpected EOF after pointer declaration.");

            var pointer = en.Current;
            var split = Regex.Split(pointer ?? "null.null", "\\.");
            var method = split[^1];
            var m = split.ToList().Remove(method);
            en.MoveNext();
            
            var function = new Function<TContext>((ctx, binder, arguments) =>
            {
                var type =  typeof(Lexer).Assembly.GetType(string.Join('.', m));
                if (type == null) throw new ParsingException(message: "Could not find specified type pointer!");
                var mtd = type.GetMethod(method) ?? throw new ParsingException(message: "Could not find specified method pointer!");
                return mtd;
            });
            return new Tuple<Function<TContext>, bool>(function, !en.MoveNext());
        }

        
        public static Tuple<Function<TContext>, bool> CompileClassPointer<TContext>(
            IEnumerator<string> en
        )
        {
            // Pointer:
            // > class SFLang.Lexicon.Lexer
            // > function SFLang.Lexicon.Lexer.CompilerPointer
            // If method is specified it tries to access it and invoke
            // If class is specified it instantiates the class
            if (!en.MoveNext())
                throw new ParsingException(message: "Unexpected EOF after pointer declaration.");

            var pointer = en.Current;
            en.MoveNext();
            
            var function = new Function<TContext>((ctx, binder, arguments) =>
            {
                var type =  typeof(Lexer).Assembly.GetType(string.Join('.', pointer));
                if (type == null) throw new ParsingException(message: "Could not find specified type pointer!");
                return type;
            });
            return new Tuple<Function<TContext>, bool>(function, !en.MoveNext());
        }

        /*
         * Compiles a constant number down to a symbol and returns to caller.
         */
        private static Tuple<Function<TContext>, bool> CompileNumber<TContext>(
            IEnumerator<string> en)
        {
            // Holds our actual number, which might be double or long.
            object numericConstant = null;

            // Checking if this is a floating point value.
            if (en.Current.IndexOfAny(new[] {'.', 'e', 'E'}) != -1)
            {
                // Notice, all floating point numbers are treated as double.
                var success = double.TryParse(en.Current, NumberStyles.Any, CultureInfo.InvariantCulture,
                    out var dblResult);
                if (!success)
                    throw new EvaluationException(typeof(Lexer),
                        $"Sorry, I tried my best to parse '{en.Current}' as a double, but I failed.");
                numericConstant = dblResult;
            }
            else
            {
                // Notice, all integer numbers are treated as long.
                var success = long.TryParse(en.Current, NumberStyles.Any, CultureInfo.InvariantCulture,
                    out var longResult);
                if (!success)
                    throw new EvaluationException(typeof(Lexer),
                        $"Sorry, I tried my best to parse '{en.Current}' as a long, but I failed.");
                numericConstant = longResult;
            }

            // Creates a function that evaluates to the actual constant number.
            var function =
                new Function<TContext>((ctx, binder, arguments) => { return numericConstant; });
            return new Tuple<Function<TContext>, bool>(function, !en.MoveNext());
        }

        /*
         * Compiles a symbolic reference down to a function invocation and returns
         * that function to caller.
         */
        private static Tuple<Function<TContext>, bool> CompileSymbol<TContext>(
            IEnumerator<string> en)
        {
            // Retrieving symbol's name and sanity checking it.
            var symbolName = en.Current;
            SanityCheckSymbolName(symbolName);

            // Discarding "(" token and checking if we're at EOF.
            var eof = !en.MoveNext();

            // Checking if this is a function invocation.
            if (!eof && en.Current == "(")
                // Method invocation, making sure we apply arguments,
                return ApplyArguments<TContext>(symbolName, en);
            return new Tuple<Function<TContext>, bool>(
                (_, binder, _) => binder[symbolName], eof);
        }


        /*
         * Applies arguments to a function invoction, such that they're evaluated at runtime.
         */
        private static Tuple<Function<TContext>, bool> ApplyArguments<TContext>(
            string symbolName,
            IEnumerator<string> en)
        {
            // Used to hold arguments before they're being applied inside of function evaluation.
            var arguments = new List<Function<TContext>>();

            // Sanity checking tokenizer's content.
            if (!en.MoveNext())
                throw new ParsingException(message: "Unexpected EOF while parsing function invocation.");

            // Looping through all arguments, if there are any.
            while (en.Current != ")")
            {
                // Compiling current argument.
                var tuple = CompileStatement<TContext>(en);
                arguments.Add(tuple.Item1);
                if (en.Current == ")")
                    break; // And we are done parsing arguments.

                // Sanity checking tokenizer's content, and discarding "," token.
                if (en.Current != ",")
                    throw new ParsingException(message: 
                        $"Syntax error in arguments to '{symbolName}', expected ',' separating arguments and found '{en.Current}'.");
                if (!en.MoveNext())
                    throw new ParsingException(message:
                        "Unexpected EOF while parsing arguments to function invocation.");
            }

            /*
             * Creates a function invocation that evaluates its arguments at runtime.
             */
            return new Tuple<Function<TContext>, bool>((ctx, binder, args) =>
            {
                // Applying arguments.
                var appliedArguments =
                    new Parameters(
                        arguments.Select(ix =>
                            ix(ctx, binder, new Parameters())));
                if (appliedArguments.Count == 1 && appliedArguments.Get(0) is Parameters explicitlyApplied)
                    appliedArguments = explicitlyApplied;

                // Retrieving symbol's value and doing some basic sanity checks.
                var symbol = binder[symbolName];
                return symbol switch
                {
                    null => throw new EvaluationException(typeof(Lexer), $"Symbol '{symbolName}' is null."),
                    Function<TContext> functor => functor(ctx, binder, appliedArguments),
                    Func<TContext, ContextBinder<TContext>, Parameters, object> funct => funct(ctx, binder,
                        appliedArguments),
                    _ => throw new EvaluationException(typeof(Lexer),
                        $"'{symbolName}' is not a function, but a '{symbol.GetType().FullName}'")
                };
            }, !en.MoveNext());
        }

        /*
         * Returns true if this is a numeric value, which might be floating point
         * value, or an integer value.
         */
        private static bool IsNumeric(string symbol)
        {
            // To support scientific notation, split the number into the base and exponent parts, 
            // which will be seperated by an 'E' character.
            var parts = symbol.Split('e', 'E');

            // All valid numbers will either have one or two parts, the coefficient will always be the first part.
            var coefficient = SanitizeSign(parts[0]);

            // If we have two parts we need to check the exponent.
            if (parts.Length == 2)
            {
                var exponent = SanitizeSign(parts[1]);
                if (exponent.Any(ix => !char.IsDigit(ix)))
                    return false;
            }
            else if (parts.Length != 1)
            {
                return false;
            }

            // If we are this far then we just need to check that the coefficient.
            return coefficient.All(ix => char.IsDigit(ix) || ix == '.');
        }

        /*
         * Returns the string with the first character removed if the first character is 
         * a '+' or '-', as long as that is not the only character in the string.
         */
        private static string SanitizeSign(string number)
        {
            if (number.Length > 1 && (number[0] == '-' || number[0] == '+')) number = number.Substring(1);

            return number;
        }
    }
}