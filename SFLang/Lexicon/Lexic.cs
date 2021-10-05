using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SFLang.Lexicon
{
    public static class Lexic<TContext>
    {
        public static Function<TContext> Out => (ctx, binder, args) =>
        {
            if (args.Count != 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    $"'out' method expects one argument but got {args.Count} instead!");

            var item = args.Get(0);
            Console.WriteLine(item);
            return item;
        };

        public static Function<TContext> Exit => (ctx, binder, parameters) =>
        {
            if (parameters.Count == 1 && parameters.Get(0) is string str)
                throw new EvaluationException(typeof(Lexic<TContext>), $"Exit error: {str}");

            if (parameters.Count > 0)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "'error' method expects either 0 or 1 string parameters!");

            throw new EvaluationException(typeof(Lexic<TContext>), "Hardcoded error exit! Exit code: -1");
        };

        public static Function<TContext> Let => (ctx, binder, parameters) =>
        {
            // Sanity checking invocation.
            if (parameters.Count == 0)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "No Parameters provided to 'let', provide at least a symbol name, e.g. 'let(@foo)'.");

            // Expecting symbol name as the first argument and doing some basic sanity checking.
            var symbolObject = parameters.Get(0);
            if (symbolObject == null)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "You'll need to supply a symbol name for 'let' to function correctly.");
            if (symbolObject is not string symbolName)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "You'll need to supply a symbol name for 'let' to function correctly. Please use the '@' character in front of your symbol's declaration.");
            Lexer.SanityCheckSymbolName(symbolName);

            if (binder.ContainsDynamicKey(symbolName))
                throw new EvaluationException(typeof(Lexic<TContext>),
                    $"The symbol '{symbolName}' has already been declared in the scope of where you are trying to declare it using the 'let' keyword.");
            if (binder.StackCount == 0 && binder.ContainsStaticKey(symbolName))
                throw new EvaluationException(typeof(Lexic<TContext>),
                    $"The symbol '{symbolName}' has already been declared in the scope of where you are trying to declare it using the 'let' keyword.");

            if (parameters.Count > 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    $"The 'let' keyword can only handle at most two Parameters, and you tried to pass in more than two Parameters as you tried to declare '{symbolName}'.");

            // Setting the symbol's initial value, if any.
            var value = parameters.Get(1);
            binder[symbolName] = value;
            return value;
        };

        public static Function<TContext> Method => (ctx, binder, parameters) =>
        {
            // Sanity checking code.
            if (parameters.Count == 0)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'function' keyword requires at least 1 argument, and you tried to invoke it with fewer.");

            // Retrieving lambda of function and doing some basic sanity checks.
            if (parameters.Get(0) is not Function<TContext> lambda)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "When declaring a function, the first argument must be a lambda object, e.g. '{ ... some code ... }'.");

            // Retrieving argument declarations and sanity checking them.
            var formallyDeclaredParameters = parameters.Skip(1).Select(ix => ix as string).ToList();
            foreach (var ix in formallyDeclaredParameters)
                if (ix is { } ixStr)
                    Lexer.SanityCheckSymbolName(ixStr);
                else
                    throw new EvaluationException(typeof(Lexic<TContext>),
                        "When declaring a function, all argument declarations must be valid symbols, e.g. '@foo'.");

            /*
             * Returning function to caller.
             *
             * NOTICE!
             * A function always pushes the stack.
             */
            return new Function<TContext>((invocationContext, invocationBinder, invocationParameters) =>
            {
                /*
                 * Sanity checking that caller did not supply more Parameters than
                 * the function is declared to at maximum being able to handle.
                 */
                if (invocationParameters.Count > formallyDeclaredParameters.Count)
                    throw new EvaluationException(typeof(Lexic<TContext>),
                        "Method was invoked with too many Parameters.");

                // Pushing stack, making sure we can correctly pop it once we're done.
                invocationBinder.PushStack();
                try
                {
                    /*
                     * Binding all argument declarations for our lambda to whatever
                     * the caller provided as values during invocation.
                     */
                    for (var ix = 0; ix < invocationParameters.Count; ix++)
                        invocationBinder[formallyDeclaredParameters[ix]] = invocationParameters.Get(ix);

                    /*
                     * Applying the rest of our Parameters as null values.
                     *
                     * NOTICE!
                     * This allows us to create functions without forcing the caller
                     * of those functions to supply all Parameters that the function
                     * declares, simply setting the rest of the declared Parameters
                     * to "null" values.
                     */
                    for (var ix = invocationParameters.Count; ix < formallyDeclaredParameters.Count; ix++)
                        invocationBinder[formallyDeclaredParameters[ix]] = null;

                    // Evaluating function.
                    return lambda(invocationContext, invocationBinder, invocationParameters);
                }
                finally
                {
                    // Popping stack.
                    invocationBinder.PopStack();
                }
            });
        };

        public static Function<TContext> Set => (ctx, binder, parameters) =>
        {
            // Sanity checking invocation.
            if (parameters.Count == 0)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "No Parameters provided to 'set', provide at least a symbol name, e.g. 'set(@foo)'.");

            // Retrieving symbol name, and doing some more basic sanity checking.
            var symbolName = parameters.Get<string>(0);
            if (symbolName == null)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "You'll need to supply a symbol name as a string for 'set' to function correctly.");
            if (parameters.Count > 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    $"The 'set' keyword can only handle at most two Parameters, and you tried to pass in more than two Parameters as you tried to change the value of '{symbolName}'.");

            /*
             * Sanity checking that symbol exists from before, since we don't allow 'set'ing a variable that doesn't exist from before.
             *
             * NOTICE!
             * We do allow to change a globally declared symbol, including one declared by the C# code that evaluates our Lizzie code.
             */
            if (!binder.ContainsKey(symbolName))
                throw new EvaluationException(typeof(Lexic<TContext>),
                    $"The symbol '{symbolName}' has not been declared in the scope of where you are trying to 'set' it.");

            // Retrieving the initial value of the variable, setting it, and returning the value to caller.
            var value = parameters.Get(1);
            binder[symbolName] = value;
            return value;
        };

        public static Function<TContext> Add => (ctx, binder, parameters) =>
        {
            // Sanity checking code.
            if (parameters.Count < 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'add' keyword requires at least 2 Parameters, and you tried to invoke it with fewer.");

            // Retrieving the first value, making sure we retrieve it as a "dynamic type".
            dynamic result = parameters.Get(0);
            foreach (dynamic ix in parameters.Skip(1)) // Adding currently iterated argument.
                result += ix;

            // Returning the result of the operation to caller.
            return result;
        };

        public static Function<TContext> Power => (ctx, binder, parameters) =>
        {
            // Sanity checking code.
            if (parameters.Count != 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    $"The 'power' keyword requires 2 parameters, and you tried to invoke it with {parameters.Count}!");

            // Retrieving the first value, making sure we retrieve it as a "dynamic type".
            dynamic num = parameters.Get(0);
            dynamic pow = parameters.Get(1);

            // Returning the result of the operation to caller.
            return Math.Pow(num, pow);
        };

        public static Function<TContext> Subtract => (ctx, binder, Parameters) =>
        {
            // Sanity checking code.
            if (Parameters.Count < 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'subtract' keyword requires at least 1 argument, and you tried to invoke it with fewer.");

            // Retrieving the first value, making sure we retrieve it as a "dynamic type".
            dynamic result = Parameters.Get(0);
            if (Parameters.Count == 1)
                return -result;
            foreach (dynamic ix in Parameters.Skip(1)) result -= ix;

            // Returning the result of the operation to caller.
            return result;
        };

        public static Function<TContext> Multiply => (ctx, binder, Parameters) =>
        {
            // Sanity checking code.
            if (Parameters.Count < 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'multiply' keyword requires at least 2 Parameters, and you tried to invoke it with fewer.");

            // Retrieving the first value, making sure we retrieve it as a "dynamic type".
            dynamic result = Parameters.Get(0);
            foreach (dynamic ix in Parameters.Skip(1)) result *= ix;

            // Returning the result of the operation to caller.
            return result;
        };

        public static Function<TContext> Divide => (ctx, binder, Parameters) =>
        {
            // Sanity checking code.
            if (Parameters.Count < 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'divide' keyword requires at least 2 Parameters, and you tried to invoke it with fewer.");

            // Retrieving the first value, making sure we retrieve it as a "dynamic type".
            dynamic result = Parameters.Get(0);
            foreach (dynamic ix in Parameters.Skip(1)) result /= ix;

            // Returning the result of the operation to caller.
            return result;
        };

        public static Function<TContext> Modulo => (ctx, binder, Parameters) =>
        {
            // Sanity checking code.
            if (Parameters.Count < 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'modulo' keyword requires at least 2 Parameters, and you tried to invoke it with fewer.");

            // Retrieving the first value, making sure we retrieve it as a "dynamic type".
            dynamic result = Parameters.Get(0);
            foreach (dynamic ix in Parameters.Skip(1)) result %= ix;

            // Returning the result of the operation to caller.
            return result;
        };


        /// <summary>
        ///     Converts a 'list' into an Parameters collection, allowing you to explicitly
        ///     apply a bunch of Parameters dynamically during runtime. Can only be
        ///     evaluated with a 'list'. If you invoke it with anything but a 'list',
        ///     it will throw an exception during runtime.
        ///     Will return an "Parameters collection" to caller.
        /// </summary>
        /// <value>The applied Parameters.</value>
        public static Function<TContext> Apply => (ctx, binder, Parameters) =>
        {
            // Sanity checking invocation.
            if (Parameters.Count != 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'apply' keyword expects exactly 1 argument.");

            // Checking type of invocation, which might be 'list' or 'map'.
            if (Parameters.Get(0) is List<object> list) // list of Parameters.
                return new Parameters(list);
            throw new EvaluationException(typeof(Lexic<TContext>),
                "The 'apply' keyword expects a 'list' as its only argument.");
        };

        /// <summary>
        ///     Creates an if condition and returns it to the caller.
        ///     The first argument is expected to be some condition, the second argument
        ///     is expected to be a lambda which will be evaluated if the condition
        ///     evaluates to anything but null, and the third (optional) argument is
        ///     what should be evaluated if the condition evaluates to null.
        ///     When 'if' is evaluated, it will return either null, or the returned
        ///     value from the lambda object that was evaluated as a response to the
        ///     condition's value.
        /// </summary>
        /// <value>The function wrapping the 'if keyword'.</value>
        public static Function<TContext> If => (ctx, binder, Parameters) =>
        {
            // Sanity checking code.
            if (Parameters.Count < 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'if' keyword requires at least 2 Parameters, and you tried to invoke it with fewer.");

            // Retrieving condition, if(true) lambda, and sanity checking invocation.
            var condition = Parameters.Get(0);
            if (condition != null)
            {
                if (Parameters.Get(1) is not Function<TContext> lambdaIf)
                    throw new EvaluationException(typeof(Lexic<TContext>),
                        "The 'if' keyword requires a lambda argument as its second argument.");
                return lambdaIf(ctx, binder, Parameters);
            }

            // Execute the else-clause, if one is present:
            if (Parameters.Count <= 2) return null;
            if (Parameters.Get(2) is not Function<TContext> lambdaElse)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'if' keyword requires a lambda argument as its third (else) argument if you supply a third argument.");
            return lambdaElse(ctx, binder, Parameters);

            // If yields false, and there is no "else lambda".
        };

        /// <summary>
        ///     Creates an equals function, that checks two or more objects for equality.
        ///     This function will return null if any of the objects it is being asked
        ///     to compare does not equal all other objects it is asked to compare.
        /// </summary>
        /// <value>The function wrapping the 'eq keyword'.</value>
        public static Function<TContext> Eq => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count < 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'eq' function must be given at least 2 Parameters.");
            var arg1 = Parameters.Get(0);

            // Comparing all other objects to the first.
            foreach (var ix in Parameters.Skip(1))
            {
                if (arg1 == null && ix != null || arg1 != null && ix == null)
                    return null;
                if (arg1 != null && !arg1.Equals(ix))
                    return null;
            }

            // Equality!
            return true;
        };

        /// <summary>
        ///     Creates a more than function, that checks if one object is more than the other.
        ///     This function will return the LHS object if it is more than the RHS object,
        ///     otherwise it will return the null. This function can be used for any object
        ///     that has overloaded the more than operator.
        /// </summary>
        /// <value>The function wrapping the 'mt keyword'.</value>
        public static Function<TContext> Mt => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count != 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'more than' function must be given exactly 2 Parameters.");
            dynamic arg1 = Parameters.Get(0);
            dynamic arg2 = Parameters.Get(1);
            if (arg1 > arg2)
                return arg1;

            // Failed!
            return null;
        };

        /// <summary>
        ///     Creates a less than function, that checks if one object is less than the other.
        ///     This function will return the LHS object if it is less than the RHS object,
        ///     otherwise it will return the null. This function can be used for any object
        ///     that has overloaded the less than operator.
        /// </summary>
        /// <value>The function wrapping the 'lt keyword'.</value>
        public static Function<TContext> Lt => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count != 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'less than' function must be given exactly 2 Parameters.");
            dynamic arg1 = Parameters.Get(0);
            dynamic arg2 = Parameters.Get(1);
            if (arg1 < arg2)
                return arg1;

            // Failed!
            return null;
        };

        /// <summary>
        ///     Creates a more than equals function, that checks if one object is more
        ///     than or equals to the other.
        ///     This function will return the LHS object if it is more than or equals the RHS object,
        ///     otherwise it will return the null. This function can be used for any object
        ///     that has overloaded the more than equals operator.
        /// </summary>
        /// <value>The function wrapping the 'mte keyword'.</value>
        public static Function<TContext> Mte => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count != 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'more than equals' function must be given exactly 2 Parameters.");
            dynamic arg1 = Parameters.Get(0);
            dynamic arg2 = Parameters.Get(1);
            if (arg1 >= arg2)
                return arg1;

            // Failed!
            return null;
        };

        /// <summary>
        ///     Creates a less than equals function, that checks if one object is less or
        ///     equals the other.
        ///     This function will return the LHS object if it is less than or equals to the
        ///     RHS object, otherwise it will return the null. This function can be used
        ///     for any object that has overloaded the less than equals operator.
        /// </summary>
        /// <value>The function wrapping the 'lte keyword'.</value>
        public static Function<TContext> Lte => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count != 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'less than' function must be given exactly 2 Parameters.");
            dynamic arg1 = Parameters.Get(0);
            dynamic arg2 = Parameters.Get(1);
            if (arg1 <= arg2)
                return arg1;

            // Failed!
            return null;
        };

        /// <summary>
        ///     Negates the given value, whatever it previously was.
        ///     If the argument given to this function is null, this function will
        ///     return true. If it is given a non-null value, the function will return
        ///     null. This negates its argument, allowing you to do the same as the
        ///     not operator normally would do in a conventional programming language.
        /// </summary>
        /// <value>The function wrapping the 'not keyword'.</value>
        public static Function<TContext> Not => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count != 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'not' function must be given exactly 1 argument.");
            var arg = Parameters.Get(0);
            return arg == null ? true : null;
        };

        /// <summary>
        ///     Returns the first object it is being given that is not null.
        ///     If all objects are null, this function will return null.
        /// </summary>
        /// <value>The function wrapping the 'any keyword'.</value>
        public static Function<TContext> Any => (ctx, binder, Parameters) =>
        {
            /*
             * Notice, if there are zero Parameters given to "any", it will return null (false), contrary to "all" that will return true
             * given an empty list of Parameters.
             */
            return Parameters.FirstOrDefault(ix =>
            {
                // Making sure we verify that this is a "delayed verification" of condition.
                if (ix is Function<TContext> function)
                {
                    // Checking if function returns something, and returning predicate value accordingly.
                    var ixContent = function(ctx, binder, new Parameters());
                    if (ixContent == null)
                        return false;
                    return true;
                }

                if (ix is string symbol)
                {
                    // Symbol.
                    var symbolValue = binder[symbol];
                    if (symbolValue == null)
                        return false;
                    return true;
                }

                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'any' function requires you to only pass in conditions as function invocations. Prepend your condition with an '@' sign if this is a problem.");
            });
        };

        /// <summary>
        ///     Returns true if all of its given values yields true.
        ///     This function will only return true if all of its Parameters are not null.
        ///     Otherwise the function will return null.
        /// </summary>
        /// <value>The function wrapping the 'all keyword'.</value>
        public static Function<TContext> All => (ctx, binder, Parameters) =>
        {
            foreach (var arg in Parameters)
                if (arg == null)
                {
                    throw new EvaluationException(typeof(Lexic<TContext>),
                        "The 'all' function requires you to only pass in function invocations. Prepend your condition with an '@' sign if this is a problem.");
                }
                else
                {
                    /*
                     * Checking if this is a function, at which point we evaluate it,
                     * to make sure we support "delayed function invocations".
                     */
                    if (arg is Function<TContext> functor)
                    {
                        // Making sure we verify that this is a "delayed verification" of condition.
                        if (functor(ctx, binder, Parameters) == null)
                            return null; // No reasons to continue ...
                    }
                    else if (arg is string symbol)
                    {
                        // Symbol.
                        var symbolValue = binder[symbol];
                        if (symbolValue == null)
                            return null;
                    }
                    else
                    {
                        throw new EvaluationException(typeof(Lexic<TContext>),
                            "The 'all' function requires you to only pass in function invocations. Prepend your condition with an '@' sign if this is a problem.");
                    }
                }

            // Notice, if "all" is given no Parameters, logically it is true, since there are no "false statements" in it.
            return Parameters.Any() ? Parameters.Last() : true;
        };

        /// <summary>
        ///     Creates a list of objects and returns it to the caller.
        ///     This function will create a list, with the initial Parameters as its
        ///     members, and return that list to the caller.
        /// </summary>
        /// <value>The function wrapping the 'list keyword'.</value>
        public static Function<TContext> List => (ctx, binder, Parameters) => { return new List<object>(Parameters); };

        /// <summary>
        ///     Returns the count of a list previously created.
        ///     This function will return the count of items in the list it is given
        ///     as its first argument.
        /// </summary>
        /// <value>The function wrapping the 'count keyword'.</value>
        public static Function<TContext> Count => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count != 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'count' function must be given exactly 1 argument, and that argument must be a list or a map.");
            if (Parameters.Get(0) is List<object> list)
                return list.Count;
            if (Parameters.Get(0) is Dictionary<string, object> map)
                return map.Count;
            throw new EvaluationException(typeof(Lexic<TContext>),
                "The 'count' function can only handle a 'list' or a 'map'.");
        };

        /// <summary>
        ///     Returns the specified item of a list previously created.
        ///     This function will return the object at the specified index from a previously
        ///     created list. Pass in the index of the item to retrieve as the only
        ///     argument you supply to this function.
        /// </summary>
        /// <value>The function wrapping the 'get keyword'.</value>
        public static Function<TContext> Get => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count != 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'get' function must be given exactly 2 Parameters. The first argument must be a list or a map, and the second argument a numeric value or a key.");
            if (Parameters.Get(1) is string key)
            {
                // Map de-referencing operation.
                var map = Parameters.Get(0) as Dictionary<string, object>;
                if (map == null)
                    throw new EvaluationException(typeof(Lexic<TContext>),
                        "The 'get' function must be given a list or a map as its first argument.");
                return map[Parameters.Get<string>(1)];
            }

            // List de-referencing operation.
            var list = Parameters.Get(0) as List<object>;
            if (list == null)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'get' function must be given a list or a map as its first argument.");
            var index = Parameters.Get<int>(1);
            return list[index];
        };

        /// <summary>
        ///     Adds the specified items to the specified list.
        ///     This function will add all Parameters beyond the first to the list
        ///     specified as the first argument. The function will return the last
        ///     items added to your list.
        /// </summary>
        /// <value>The function wrapping the 'add keyword'.</value>
        public static Function<TContext> AddValue => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count < 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'add' function must be given at least 2 Parameters. The first argument must be a 'list' or a 'map', and the rest of the Parameters objects to add to your 'list' or your 'map'.");
            if (Parameters.Get(0) is List<object> list)
            {
                // List.
                list.AddRange(Parameters.Skip(1));
                return list[list.Count - 1];
            }

            if (Parameters.Get(0) is Dictionary<string, object> map)
            {
                // Map.
                var en = Parameters.GetEnumerator();

                // Skipping symbol.
                en.MoveNext();

                // Adding each value.
                object lastValue = null;
                while (en.MoveNext())
                {
                    var key = (string) en.Current;
                    if (!en.MoveNext())
                        throw new EvaluationException(typeof(Lexic<TContext>),
                            "The 'add' function requires an even number of Parameters when given a 'map' as its destination.");
                    lastValue = en.Current;
                    map.Add(key, lastValue);
                }

                return lastValue;
            }

            // Oops ...!!
            throw new EvaluationException(typeof(Lexic<TContext>),
                "The 'add' function must be given either a 'list' or a 'map' as its first argument.");
        };

        /// <summary>
        ///     Updates And Returns the specified item of a list previously created by a new value.
        ///     This function will update the object at the specified index from a previously
        ///     created list or map. Pass in the index of the item to retrieve and the new value  as the only two
        ///     argument you supply to this function.
        /// </summary>
        /// <value>The function wrapping the 'update keyword'.</value>
        public static Function<TContext> UpdateValue => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count != 3)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'update' function must be given exactly 3 Parameters. The first argument must be a list or a map, and the second argument a numeric value or a key, and the last argument is the new value.");

            var value = Parameters.Get(2);

            if (Parameters.Get(1) is string key)
            {
                // Map de-referencing operation.
                var map = Parameters.Get(0) as Dictionary<string, object>;
                if (map == null)
                    throw new EvaluationException(typeof(Lexic<TContext>),
                        "The 'update' function must be given a list or a map as its first argument.");
                map[Parameters.Get<string>(1)] = value;
                return map[Parameters.Get<string>(1)];
            }

            // List de-referencing operation.
            var list = Parameters.Get(0) as List<object>;
            if (list == null)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'update' function must be given a list or a map as its first argument.");
            var index = Parameters.Get<int>(1);
            list[index] = value;
            return list[index];
        };

        /// <summary>
        ///     Returns a slice from the specified list without modifying the original list.
        ///     This function will return a 'slice' of the list supplied as its first argument.
        ///     It expects at least 1 argument, and that argument must be a list. If it
        ///     is given a second argument, this argument is expected to be a numeric value,
        ///     defining the offset of where to start creating the slice. If it is given
        ///     a third argument, it expects that argument to be the end of where to
        ///     create the slice from.
        /// </summary>
        /// <value>The function wrapping the 'slice keyword'.</value>
        public static Function<TContext> Slice => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count == 0 || Parameters.Count > 3)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'slice' function must be given 1-3 Parameters, the first argument must be a list, and the optional 2nd and 3rd Parameters must be numeric values.");
            var list = Parameters.Get(0) as List<object>;
            if (list == null)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'slice' function must be given a list as its first argument.");
            var start = 0;
            var end = list.Count;
            if (Parameters.Count > 1)
                start = Parameters.Get<int>(1);
            if (Parameters.Count > 2)
                end = Parameters.Get<int>(2);
            if (end < start)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The end index to 'slice' must be larger than the start index.");
            return list.GetRange(start, end - start);
        };

        /// <summary>
        ///     Iterates a list and evaluates the specified lambda once for each of its values.
        ///     The first argument must be a symbol, declaring how you
        ///     intend to reference your iterated list item from within your lambda block.
        ///     The second argument must be a list.
        ///     The third argument is expected to be a lambda block, e.g. "{ ... your code goes here ... }".
        ///     The function will return the list itself.
        /// </summary>
        /// <value>The function wrapping the 'each keyword'.</value>
        public static Function<TContext> Each => (ctx, binder, Parameters) =>
        {
            // Sanity checking code.
            if (Parameters.Count < 3)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'each' keyword requires 3 Parameters, and you tried to invoke it with fewer.");

            // Retrieving lambda and list for function and doing some basic sanity checks.
            var argName = Parameters.Get(0) as string;
            if (argName == null)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'each' function must be given a symbol name as its first argument.");
            if (binder.ContainsDynamicKey(argName))
                throw new EvaluationException(typeof(Lexic<TContext>),
                    $"The '{argName}' is already declared from before, hence you can't use it as an iterator for the 'each' function.");
            var lambda = Parameters.Get(2) as Function<TContext>;
            if (lambda == null)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "When invoking the 'each' function, the third argument must be a lambda object, e.g. '{ ... some code ... }'.");
            var retVal = new List<object>();
            if (Parameters.Get(1) is List<object> list) // List.
                try
                {
                    foreach (var ix in list)
                    {
                        binder[argName] = ix;
                        var current = lambda(ctx, binder, Parameters);
                        retVal.Add(current);
                    }
                }
                finally
                {
                    if (binder.ContainsDynamicKey(argName))
                        binder.RemoveKey(argName);
                }
            else if (Parameters.Get(1) is Dictionary<string, object> map) // Map.
                try
                {
                    foreach (var ix in map.Keys)
                    {
                        binder[argName] = ix;
                        var current = lambda(ctx, binder, Parameters);
                        retVal.Add(current);
                    }
                }
                finally
                {
                    if (binder.ContainsDynamicKey(argName))
                        binder.RemoveKey(argName);
                }
            else // Oops ...!!
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'each' function must be given either a 'map' or a 'list' as its 2nd argument.");

            return retVal;
        };

        /// <summary>
        ///     Creates a dictionary and returns it to the caller.
        ///     This function will create a dictionary, of string/object, and return
        ///     it to caller.
        /// </summary>
        /// <value>The function wrapping the 'map keyword'.</value>
        public static Function<TContext> Map => (ctx, binder, Parameters) =>
        {
            var map = new Dictionary<string, object>();
            var en = Parameters.GetEnumerator();
            while (en.MoveNext())
            {
                var key = (string) en.Current;
                if (!en.MoveNext())
                    throw new EvaluationException(typeof(Lexic<TContext>),
                        "The 'map' function requires an even number of Parametersas a key/value collection.");
                var value = en.Current;
                map.Add(key, value);
            }

            return map;
        };

        /// <summary>
        ///     Gets from string.
        /// </summary>
        /// <value>From string.</value>
        public static Function<TContext> Json => (ctx, binder, parameters) =>
        {
            if (parameters.Count != 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'json' function must be given exactly 1 argument.");
            var json = parameters.Get<string>(0);
            var result = JsonConvert.DeserializeObject(json);
            return ConvertJson(result);
        };

        /// <summary>
        ///     Converts the specified object to a string.
        ///     This function will return the string representation of whatever object
        ///     it is given. The function must be given exactly one argument.
        /// </summary>
        /// <value>The function wrapping the 'string keyword'.</value>
        public static Function<TContext> String => (ctx, binder, parameters) =>
        {
            if (parameters.Count != 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'string' function must be given exactly 1 argument.");
            var builder = new StringBuilder();
            ToString(parameters.Get(0), builder);
            return builder.ToString();
        };

        /// <summary>
        ///     Converts the specified object to a number.
        ///     This function will return the numeric representation of whatever object
        ///     it is given. The function must be given exactly one argument.
        /// </summary>
        /// <value>The function wrapping the 'number keyword'.</value>
        public static Function<TContext> Number => (ctx, binder, Parameters) =>
        {
            if (Parameters.Count != 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'number' function must be given exactly 1 argument.");
            var tmp = Parameters.Get(0);
            if (tmp is long tmpLong)
                return tmpLong;
            if (tmp is double tmpDouble)
                return tmpDouble;
            var tmpString = Parameters.Get<string>(0);
            if (tmpString.Contains("."))
                return double.Parse(tmpString);
            return long.Parse(tmpString);
        };

        /// <summary>
        ///     Returns a substring of the given string value.
        ///     This function will return a substring of the specified string. It
        ///     expects at least two Parameters, the first being a string, the second
        ///     an offset of where to start the returned string from. You can also
        ///     optionally supply a third argument being the length, or the count of characters
        ///     to return. Obviously, the start position + length must be shorter than or equal
        ///     to the entire length of the original string.
        /// </summary>
        /// <value>The function wrapping the 'substr keyword'.</value>
        public static Function<TContext> Substr => (ctx, binder, Parameters) =>
        {
            // Sanity checking.
            if (Parameters.Count < 2)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'substr' function must be given at least 2 Parameters.");
            if (Parameters.Count > 3)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'substr' function cannot be given more than 3 Parameters.");

            // Retrieving string to manipulate.
            var source = Parameters.Get<string>(0);

            // Retrieving start position.
            var offset = Parameters.Get<int>(1);

            // More sanity checking.
            if (offset > source.Length)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The second argument to 'substr' cannot be more than the total length of your string.");

            // Checking if we have an end position.
            if (Parameters.Count > 2)
            {
                var length = Parameters.Get<int>(2);
                if (length == 0)
                    return "";
                if (length < 0)
                    throw new EvaluationException(typeof(Lexic<TContext>),
                        "The third argument to 'substr' must be zero or higher.");
                if (offset + length > source.Length)
                    throw new EvaluationException(typeof(Lexic<TContext>),
                        "The second argument + the third argument to 'substr' cannot be longer than the string's total length.");
                return source.Substring(offset, length);
            }

            return source.Substring(offset);
        };

        /// <summary>
        ///     Returns the length of the given string value.
        ///     This function will return the length of the given string value.
        /// </summary>
        /// <value>The function wrapping the 'length keyword'.</value>
        public static Function<TContext> Length => (ctx, binder, Parameters) =>
        {
            // Sanity checking.
            if (Parameters.Count != 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'length' function must be given exactly 1 argument.");

            // Retrieving string's length.
            var arg1 = Parameters.Get<string>(0);
            return arg1.Length;
        };

        /// <summary>
        ///     Replace occurrencies of one string in a string with another value and
        ///     returns a new string with the replacing having occurred.
        ///     This function will replace all occurrencies of the 2nd argument with the
        ///     value of the 3rd argument, and return a new string to the caller.
        /// </summary>
        /// <value>The function wrapping the 'replace keyword'.</value>
        public static Function<TContext> Replace => (ctx, binder, Parameters) =>
        {
            // Sanity checking.
            if (Parameters.Count != 3)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'replace' function must be given exactly 3 Parameters.");

            // Retrieving string's length.
            var arg1 = Parameters.Get<string>(0);
            var arg2 = Parameters.Get<string>(1);
            var arg3 = Parameters.Get<string>(2);
            return arg1.Replace(arg2, arg3);
        };

        /// <summary>
        ///     Dynamically compiles and evaluates the given code, and returns the result.
        ///     This function will compile and evaluate the given string, assuming it
        ///     contains valid Lizzie code. Notice, the evaluated code will not have
        ///     access to the stack of the code evaluating eval, but it will share
        ///     the same context instance.
        /// </summary>
        /// <value>The function wrapping the 'eval keyword'.</value>
        public static Function<TContext> Eval => (ctx, binder, Parameters) =>
        {
            // Sanity checking.
            if (Parameters.Count != 1)
                throw new EvaluationException(typeof(Lexic<TContext>),
                    "The 'eval' function must be given exactly 1 argument.");

            // Retrieving string's length.
            var arg1 = Parameters.Get<string>(0);
            var lambda = Lambdas.Compile(ctx, arg1);
            return lambda();
        };

        /*
         * Helper for above.
         */
        private static object ConvertJson(object result)
        {
            if (result is JObject jObj)
            {
                // Dictionary/map.
                var map = new Dictionary<string, object>();
                foreach (var ix in jObj) map.Add(ix.Key, ConvertJson(ix.Value));
                return map;
            }

            if (result is JArray jArr)
            {
                // 'list'/List<object>.
                var list = new List<object>();
                foreach (var ix in jArr) list.Add(ConvertJson(ix));
                return list;
            }

            if (result is JValue iVal)
                // Some value of some sort.
                switch (iVal.Type)
                {
                    case JTokenType.Integer:
                        return iVal.Value<long>();

                    case JTokenType.Float:
                        return iVal.Value<double>();

                    case JTokenType.Date:
                        return iVal.Value<DateTime>();

                    case JTokenType.Boolean:
                        return iVal.Value<bool>();

                    case JTokenType.Guid:
                        return iVal.Value<Guid>();

                    case JTokenType.Bytes:
                        return iVal.Value<byte[]>();

                    case JTokenType.TimeSpan:
                        return iVal.Value<TimeSpan>();

                    case JTokenType.Null:
                        return null;

                    default:
                        return iVal.Value<string>();
                }

            throw new EvaluationException(typeof(Lexic<TContext>), "Unsupported JSON type.");
        }

        /*
         * Helper for above.
         * 
         * Basically creates a JSON string if object is "complex", otherwise
         * simply invokes ToString on object, unless object is "null", at which
         * point it creates "null" as its string representation.
         */
        private static void ToString(object value, StringBuilder builder, bool isJson = false)
        {
            if (value == null)
            {
                // Null.
                builder.Append("null");
                return;
            }

            if (value is List<object> list)
            {
                // List.
                builder.Append("[");
                var first = true;
                foreach (var ix in list)
                {
                    if (first)
                        first = false;
                    else
                        builder.Append(",");
                    ToString(ix, builder, true);
                }

                builder.Append("]");
            }
            else if (value is Dictionary<string, object> map)
            {
                // Map.
                builder.Append("{");
                var first = true;
                foreach (var key in map.Keys)
                {
                    if (first)
                        first = false;
                    else
                        builder.Append(",");
                    builder.Append($"\"{key.Replace("\"", "\\\"")}\":");
                    ToString(map[key], builder, true);
                }

                builder.Append("}");
            }
            else
            {
                if (value is string valueStr)
                {
                    // String, making sure we escape it, and wrap it in double quotes.
                    if (isJson)
                        builder.Append("\"" + valueStr.Replace("\"", "\\\"") + "\"");
                    else
                        builder.Append(valueStr);
                }
                else
                {
                    // Everything else.
                    builder.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
                }
            }
        }
    }
}