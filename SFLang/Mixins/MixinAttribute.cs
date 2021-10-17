using System;

#nullable enable
namespace SFLang.Mixins
{
    
    [AttributeUsage(AttributeTargets.Class)]
    public class MixinAttribute : Attribute
    {
        public string? Namespace = null;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class MethodAttribute : Attribute
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();
        public int ExpectedParameters { get; set; } = -1;
    }
}