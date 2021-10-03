using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SFLang.Exceptions;
using SFLang.Language;

namespace SFLang.Lexicon
{
    public class ContextBinder<TContext> : ICloneable
    {
        public static ContextBinder<TContext> InstanceBinder { get; set; } = null;

        private static readonly HashSet<string> BlackListedMethods = new()
        {
            "GetType",
            "ToString",
            "Equals",
            "GetHashCode"
        };
        delegate object DeepFunction(object target, object[] arguments);
        delegate object DeepStaticFunction(object[] arguments);
        
        // Statically bound variables/functions, and root level variables.
        public readonly Dictionary<string, object> StaticContextBinder = new();


        // Stack of dynamically created variables and functions.
        public readonly List<Dictionary<string, object>> StackContextBinder = new();


        // Tracks if an instance context is provided or not.
        bool _contextIsDefault;

        public ContextBinder(TContext context = default)
        {
            _contextIsDefault = EqualityComparer<TContext>.Default.Equals(context, default);
        }
        
        ContextBinder(bool initialize, TContext context = default) { }

        public int MaxStackSize { get; set; } = -1;
        
        public IEnumerable<string> StaticItems => StaticContextBinder.Keys;

        public int StackCount => StackContextBinder.Count;
        
        public bool DeeplyBound => !_contextIsDefault;

        public void RegisterClass(SFClass klass, TContext ctx)
        {
            var properties = klass.GetType().GetProperties();
            var type = klass.GetType();
            foreach(var property in properties)
            {
                this[type.Name+"::"+property.Name] = property.GetValue(klass);
            }

            foreach (var method in klass.GetType().GetMethods())
            {
                if (BlackListedMethods.Contains(method.Name)) continue;
                if (method.Name == "__typeof__")
                {
                    BindDeepMethod(method, $"typeof<{klass.GetType().Name}>", ctx);
                }
                else if (method.Name != "__init__")
                {
                    BindDeepMethod(method, type.Name + "::" + method.Name, ctx);
                }
                else
                {
                    BindDeepMethod(method, type.Name, ctx);
                }
            }
        }

        public object this[string symbolName]
        {
            get {
                /*
                 * Checking if we have a stack level object matching the specified symbol.
                 *
                 * We do this to allow for locally declared symbols to "hide" global symbols.
                 * Locally declared symbols are symbols declared inside of functions, or after the
                 * stack has been "pushed" at least once or more.
                 */
                
                if (StackContextBinder.Count > 0 && StackContextBinder[^1].ContainsKey(symbolName))
                    return StackContextBinder[^1][symbolName];
                if(StaticContextBinder.ContainsKey(symbolName))
                    return StaticContextBinder[symbolName];
                
                // Oops, no such symbol!
                throw new EvaluationException(typeof(ContextBinder<TContext>),$"The '{symbolName}' symbol has not been declared.");
            }
            set {


                /*
                 * Checking if we should set the static (global) symbol, which we
                 * do if the stack has not (yet) been pushed at least once, or the
                 * symbol already exists at the global scope.
                 */
                if (StackContextBinder.Count == 0 || StaticContextBinder.ContainsKey(symbolName)) {
                    StaticContextBinder[symbolName] = value;
                } else {
                    StackContextBinder[StackContextBinder.Count - 1][symbolName] = value;
                }
            }
        }


        /// <summary>
        /// Returns true if the named symbol exists. Notice, the symbol's value might
        /// still be null, even if the symbol exists.
        /// </summary>
        /// <returns><c>true</c>, if symbol exists, <c>false</c> otherwise.</returns>
        /// <param name="symbolName">Symbol name.</param>
        public bool ContainsKey(string symbolName)
        {
            // Checking if our stack contains symbol.
            if (StackContextBinder.Count > 0 && StackContextBinder[StackContextBinder.Count - 1].ContainsKey(symbolName))
                return true;


            // Defaulting to static ContextBinder.
            return StaticContextBinder.ContainsKey(symbolName);
        }


        /// <summary>
        /// Returns true if the named symbol exists. Notice, the symbol's value might
        /// still be null, even if the symbol exists.
        /// </summary>
        /// <returns><c>true</c>, if symbol exists, <c>false</c> otherwise.</returns>
        /// <param name="symbolName">Symbol name.</param>
        public bool ContainsDynamicKey(string symbolName)
        {
            if (StackContextBinder.Count > 0)
                return StackContextBinder[StackContextBinder.Count - 1].ContainsKey(symbolName);
            return false;
        }


        /// <summary>
        /// Returns true if the named symbol exists. Notice, the symbol's value might
        /// still be null, even if the symbol exists.
        /// </summary>
        /// <returns><c>true</c>, if symbol exists, <c>false</c> otherwise.</returns>
        /// <param name="symbolName">Symbol name.</param>
        public bool ContainsStaticKey(string symbolName)
        {
            return StaticContextBinder.ContainsKey(symbolName);
        }


        /// <summary>
        /// Removes the specified key from the stack.
        /// </summary>
        /// <param name="symbolName">Symbol name of item to remove.</param>
        public void RemoveKey(string symbolName)
        {
            if (StackContextBinder.Count > 0)
                StackContextBinder[StackContextBinder.Count - 1].Remove(symbolName);
            StaticContextBinder.Remove(symbolName);
        }


        /// <summary>
        /// Creates a new stack and makes it become the current stack.
        /// </summary>
        public void PushStack()
        {
            if (StackContextBinder.Count == MaxStackSize)
                throw new EvaluationException(typeof(ContextBinder<TContext>),"Your maximum stack size has been exceeded");
            StackContextBinder.Add(new Dictionary<string, object>());
        }


        /// <summary>
        /// Pops the top item off the stack, and makes the previous stack the current stack.
        /// </summary>
        public void PopStack()
        {
            StackContextBinder.RemoveAt(StackContextBinder.Count - 1);
        }


        /// <summary>
        /// Clones this instance.
        /// 
        /// Since invoking the default CTOR has some overhead due to reflection,
        /// caching instances of this class might improve your performance.
        /// However, since instances of the class is not thread safe and cannot
        /// be safely shared among multiple threads, you can use this method
        /// to clone a "master instance" for each of your threads that needs to
        /// bind to the same type. This method is also useful in case you want
        /// to spawn of new threads, where each thread needs access to a thread
        /// safe copy of the ContextBinder, at the point of creation.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public ContextBinder<TContext> Clone()
        {
            var clone = new ContextBinder<TContext>(false) {
                MaxStackSize = MaxStackSize
            };
            foreach (var ix in StaticContextBinder.Keys) {
                clone[ix] = StaticContextBinder[ix];
            }
            foreach (var ixStack in StackContextBinder) {
                var dictionary = new Dictionary<string, object>();
                foreach (var ixKey in ixStack.Keys) {
                    dictionary[ixKey] = ixStack[ixKey];
                }
                clone.StackContextBinder.Add(dictionary);
            }
            return clone;
        }


        #region [ -- Private helper methods -- ]

        /*
         * Binds a single method as a "shallow" delegate.
         */
        void BindMethod(MethodInfo method, string functionName)
        {
            SanityCheckSignature(method, functionName);
            StaticContextBinder[functionName] = Delegate.CreateDelegate(typeof(Func<TContext>), method);
        }

        /*
         * Binds a Lizzie function "deeply", to make it possible to create a
         * delegate that is able to invoke methods in super classes
         * as Lizzie functions.
         * 
         * Useful in for instance IoC (Dependency Injection) and similar scenarios
         * where you don't have access to the implementing bound type through its
         * generic argument.
         */
        void BindDeepMethod(MethodInfo method, string functionName, TContext context)
        {
            SanityCheckSignature(method, functionName);
            /*
             * Wrapping our "deep" delegate invocation inside a "normal" function invocation.
             * Excactly how, depends upon whether or not the bound method is static or not.
             */
            if (!method.IsStatic) {
                
                var lateBound = CreateInstanceFunction(method);
                StaticContextBinder[functionName] = new Function<TContext>((ctx, binder, arguments) => {
                    return lateBound(ctx, new object[] { binder, arguments });
                });
            } else {
                
                var lateBound = CreateStaticFunction(method);
                StaticContextBinder[functionName] = new Function<TContext>((ctx, binder, arguments) => {
                    return lateBound(new object[] { ctx, binder, arguments });
                });

            }
        }


        /*
         * Creates an instance wrapper for a deeply bound method.
         */
        DeepFunction CreateInstanceFunction(MethodInfo method)
        {
            try
            {
                var instanceParameter = Expression.Parameter(typeof(object), "target");
                var argumentsParameter = Expression.Parameter(typeof(object[]), "arguments");


                var call = Expression.Call(
                    Expression.Convert(instanceParameter, method.DeclaringType),
                    method,
                    method.GetParameters().Select((parameter, index) => Expression.Convert(
                            Expression.ArrayIndex(argumentsParameter, Expression.Constant(index)),
                            parameter.ParameterType)).ToArray());


                var lambda = Expression.Lambda<DeepFunction>(
                    Expression.Convert(call, typeof(object)),
                    instanceParameter,
                    argumentsParameter);


                return lambda.Compile();
            }
            catch (Exception e)
            {
                throw new EvaluationException(typeof(ContextBinder<TContext>), $"{e.Message}");
            }
        }


        /*
         * Creates a static wrapper for a deeply bound method.
         */
        DeepStaticFunction CreateStaticFunction(MethodInfo method)
        {
            var argumentsParameter = Expression.Parameter(typeof(object[]), "arguments");


            var call = Expression.Call(
              method,
              method.GetParameters().Select((parameter, index) =>
                  Expression.Convert(
                    Expression.ArrayIndex(argumentsParameter, Expression.Constant(index)), parameter.ParameterType)).ToArray());


            var lambda = Expression.Lambda<DeepStaticFunction>(
              Expression.Convert(call, typeof(object)),
              argumentsParameter);


            return lambda.Compile();
        }


        /*
         * Sanity checks method.
         */
        void SanityCheckSignature(MethodInfo method, string functionName)
        {
            // Sanity checking function name.
            if (string.IsNullOrEmpty(functionName))
                throw new BindingException(typeof(ContextBinder<TContext>),"Can't bind to functions unless you choose a non-empty function name.");


            // Sanity checking method.
            var methodArgs = method.GetParameters();
            if (method.IsStatic) {
                if (methodArgs.Length != 3)
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it doesn't take the right number of arguments");
                if (methodArgs[0].ParameterType != typeof(TContext))
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it doesn't take a '{nameof(TContext)}' type of argument as its first argument.");
                if (methodArgs[1].ParameterType != typeof(ContextBinder<TContext>))
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it doesn't take a '{nameof(ContextBinder<TContext>)}' type of argument as its second argument.");
                if (methodArgs[2].ParameterType != typeof(Parameters))
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it doesn't take an '{nameof(Parameters)}' type of argument as its third argument.");
                if (method.ContainsGenericParameters)
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it requires a generic argument.");
                if (method.ReturnType != typeof(object))
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it doesn't return '{nameof(Object)}'.");
            } else {
                if (methodArgs.Length != 2)
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it doesn't take the right number of arguments");
                if (methodArgs[0].ParameterType != typeof(ContextBinder<TContext>))
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it doesn't take a '{typeof(ContextBinder<TContext>).FullName}' type of argument as its first argument.");
                if (methodArgs[1].ParameterType != typeof(Parameters))
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it doesn't take an '{nameof(Parameters)}' type of argument as its second argument.");
                if (method.ContainsGenericParameters)
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it requires a generic argument.");
                if (method.ReturnType != typeof(object))
                    throw new BindingException(typeof(ContextBinder<TContext>),$"Can't bind to {method.Name} since it doesn't return '{nameof(Object)}'.");
            }
        }


        /*
         * Explicit IClonable implementation.
         * This type of implementation, requires you to explicitly cast your
         * instance to an ICloneable instance in order to clone it.
         */
        object ICloneable.Clone()
        {
            return Clone();
        }


        #endregion
    }
}