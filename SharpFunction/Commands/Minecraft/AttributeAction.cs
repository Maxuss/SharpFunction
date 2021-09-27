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