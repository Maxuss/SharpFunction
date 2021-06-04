using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Commands.Minecraft
{
    public enum AttributeAction
    {
        Get,
        SetBase,
        GetBase,
        AddModifier,
        RemoveModifier,
        GetModifierValue
    }

    public enum ModifierAction
    {
        Add,
        Multiply,
        MultiplyBase
    }
}
