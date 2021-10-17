#nullable enable
using System;
using System.Linq;
using System.Reflection;
using SFLang.Exceptions;
using SFLang.Language;
using SFLang.Lexicon;

namespace SFLang.Mixins
{
    public static class MixinManager
    {
        public static void LoadClassMixins(Type type, ContextBinder<Lambdas.Unit> binder)
        {
            var mixin = (MixinAttribute) type.GetCustomAttributes(typeof(MixinAttribute), false).First();
            foreach (var method in type.GetMethods())
            {
                if (!method.GetCustomAttributes(typeof(MethodAttribute), false).Any()) continue;
                if (!method.IsStatic)
                    throw new BindingException(typeof(MixinAttribute),
                        "Could not resolve mixin because it is not static! Please mark all your mixin methods as static!");
                var param = method.GetParameters();
                var ret = method.ReturnType;
                ParameterInfo firstParam;
                ParameterInfo secondParam;
                try
                {
                    firstParam = param[0];
                    secondParam = param[1];
                }
                catch (Exception)
                {
                    throw new BindingException(typeof(MixinManager),
                        "Could not resolve mixin because of it's member not having enough parameters");
                }
                if (firstParam.ParameterType != typeof(ContextBinder<Lambdas.Unit>))
                    throw new BindingException(typeof(MixinManager),
                        "Could not resolve mixin because of it's member not having ContextBinder<Lambdas.Unit> as first parameter");
                if (secondParam.ParameterType != typeof(Parameters))
                    throw new BindingException(typeof(MixinManager),
                        "Could not resolve mixin because of it's member not having Parameters as second parameter");

                var attr = (MethodAttribute) method.GetCustomAttributes(typeof(MethodAttribute), false).First();

                Function<Lambdas.Unit> fn = (ctx, contextBinder, args) =>
                {
                    if (attr.ExpectedParameters != -1 && args.Count > attr.ExpectedParameters)
                        throw new CoreException(
                            $"Method expected {attr.ExpectedParameters} parameters but instead got {args.Count}");
                    Parameters newParams = new();
                    if (attr.ExpectedParameters != -1)
                    {
                        for (var i = 0; i < attr.ExpectedParameters; i++)
                        {
                            var para = args.Count < i ? null : args[i];
                            newParams.Add(para);
                        }
                    }

                    if (ret != typeof(void)) return method.Invoke(null, new object?[] {contextBinder, newParams});

                    try
                    {
                        method.Invoke(null, new object[] {contextBinder, newParams});
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                };
                
                var fullName = mixin.Namespace == null
                    ? attr.Name
                    : $"{mixin.Namespace}:{attr.Name}";
                binder.StaticContextBinder[fullName] = fn;
            }
        }

        public static void ResolveMixins(ContextBinder<Lambdas.Unit> binder, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(MixinAttribute)).Any()))
            {
                LoadClassMixins(type, binder);
            }
        }

        public static void LoadProjectMixins(Project project, ContextBinder<Lambdas.Unit> binder)
        {
            foreach (var mixin in project.Mixins.Stored)
            {
                try
                {
                    var asm = Assembly.LoadFile(mixin.Assembly);
                    var type = asm.GetType(mixin.Class) ?? throw new Exception("EXIT");
                    LoadClassMixins(type, binder);
                }
                catch (Exception)
                {
                    throw new BindingException(typeof(MixinManager),
                        $"Could not load mixins from assembly {mixin.Assembly} and class {mixin.Class}!");
                }
            }
        }
    }
}