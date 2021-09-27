using AA = SharpFunction.Commands.Minecraft.AttributeAction;
using static SharpFunction.Universal.VectorHelper;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents and attribute command. Equal to Minecraft's
    ///     <code>/attribute {target: String} {attribute: String} {get/set}</code>
    /// </summary>
    public sealed class Attribute : ICommand, ISelectorCommand
    {
        /// <summary>
        ///     Initialize Attribute Command class.<br />
        ///     See also: <br />
        ///     <seealso cref="EntitySelector" />
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector" /> to use</param>
        public Attribute(EntitySelector selector)
        {
            Selector = selector;
        }

        public string Compiled { get; private set; }
        public EntitySelector Selector { get; set; }

        /// <summary>
        ///     Compiles the /attribute command.<br />
        ///     <b>USAGE:</b><br />
        ///     <paramref name="extra" /> should have different values depending on <paramref name="action" /><br />
        ///     1. <see cref="AA.Get" />: <paramref name="extra" />[0] -> <see cref="double" /> scale<br />
        ///     2. <see cref="AA.GetBase" />: <paramref name="extra" />[0] -> <see cref="double" /> scale<br />
        ///     3. <see cref="AA.SetBase" />: <paramref name="extra" />[0] -> <see cref="int" /> value<br />
        ///     4. <see cref="AA.AddModifier" />:<br />
        ///     <paramref name="extra" />[0] -> <see cref="string" /> uuid,<br />
        ///     <paramref name="extra" />[1] -> <see cref="string" /> name,<br />
        ///     <paramref name="extra" />[2] -> <see cref="int" /> value,<br />
        ///     <paramref name="extra" />[3] -> <see cref="ModifierAction" /> modifier_action,<br />
        ///     5. <see cref="AA.RemoveModifier" />: <paramref name="extra" />[0] -> <see cref="string" /> uuid<br />
        ///     6. <see cref="AA.GetModifierValue" />: <br />
        ///     <paramref name="extra" />[0] -> <see cref="string" /> uuid<br />
        ///     <paramref name="extra" />[1] -> <see cref="double" /> scale<br />
        /// </summary>
        /// <param name="attribute">Name of attribute</param>
        /// <param name="action">Action to perform with attribute</param>
        /// <param name="extra">External data required to perform actions with attribute</param>
        public void Compile(string attribute, AA action, params object[] extra)
        {
            string full;
            switch (action)
            {
                case AA.Get:
                    full = $"{attribute} get {DecimalToMCString((double) extra[0])}";
                    break;
                case AA.GetBase:
                    full = $"{attribute} base get {DecimalToMCString((double) extra[0])}";
                    break;
                case AA.SetBase:
                    full = $"{attribute} base set {(int) extra[0]}";
                    break;
                case AA.AddModifier:
                    string modif;
                    switch ((ModifierAction) extra[3])
                    {
                        case ModifierAction.Add:
                            modif = "add";
                            break;
                        case ModifierAction.Multiply:
                            modif = "multiply";
                            break;
                        case ModifierAction.MultiplyBase:
                            modif = "multiply_base";
                            break;
                        default: goto case ModifierAction.Add;
                    }

                    full =
                        $"{attribute} modifier add {(string) extra[0]} {(string) extra[1]} {DecimalToMCString((double) extra[2])} {modif}";
                    break;
                case AA.RemoveModifier:
                    full = $"{attribute} modifier remove {(string) extra[0]}";
                    break;
                case AA.GetModifierValue:
                    full = $"{attribute} modifier value get {(string) extra[0]} {DecimalToMCString((double) extra[1])}";
                    break;
                default: goto case AA.Get;
            }

            var f = $"/attribute {Selector.String()} {full}";
            Compiled = f;
        }
    }
}