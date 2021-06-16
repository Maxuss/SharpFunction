using System;
using static SharpFunction.Universal.EnumHelper;
using SharpFunction.Universal;
namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents /execute command. Equal to Minecraft's <code>/execute {tons of params}</code>
    /// </summary>
    public sealed class Execute : ICommand
    {
        /// <summary>
        /// Compiled command string
        /// </summary>
        /// <value></value>
        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Execute Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Execute() { }

        /// <summary>
        /// Compiles the /execute command.
        /// </summary>
        /// <param name="params">Parameters for '/execute' command</param>
        public void Compile(ExecuteParameters @params)
        {
            Compiled = @params.String;
        }
    }

    /// <summary>
    /// Represents collection of conditions for /execute command with most of it being metadata
    /// </summary>
    public struct ExecuteParameters
    {
        
        internal string String { get; private set; }
#nullable enable

        /// <summary>
        /// Execute command has a very complex syntax tree<br/>
        /// <b>Execute parameters</b>
        /// <para>
        /// 
        /// <para>
        /// 1. <see cref="ExecuteCondition.Align"/><br/>
        /// Updates the command's execution position, aligning to its current block position.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should have swizzle of coordinates.<br/>
        /// Example: "xzy" or "yz"
        /// </para>
        /// 
        /// <para>
        /// 2. <see cref="ExecuteCondition.Anchored"/><br/>
        /// Sets the execution anchor to the eyes or feet. Defaults to feet.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="AnchorCondition"/><br/>
        /// </para>
        /// 
        /// <para>
        /// 3. <see cref="ExecuteCondition.As"/><br/>
        /// Sets the command's executor to target entity, without 
        /// changing execution position, rotation, dimension, or anchor<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="EntitySelector"/>
        /// </para>
        /// 
        /// <para>
        /// 4. <see cref="ExecuteCondition.At"/><br/>
        /// Sets the execution position, rotation, and dimension to 
        /// match those of an entity; does not change executor.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="EntitySelector"/>
        /// </para>
        /// 
        /// <para>
        /// 5. <see cref="ExecuteCondition.Facing"/><br/>
        /// Sets the execution rotation to face a given point, as 
        /// viewed from its anchor (either the eyes or the feet)<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="FacingCondition"/><br/>
        /// First option (<see cref="FacingCondition.Position"/>): <br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="Vector3"/>)<br/>
        /// Second option (<see cref="FacingCondition.Entity"/>): <br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="EntitySelector"/> entity<br/>
        /// <paramref name="extraParameters"/>[2] should be <see cref="AnchorCondition"/> anchor <br/>
        /// </para>
        /// 
        /// <para>
        /// 6. <see cref="ExecuteCondition.In"/><br/>
        /// Sets the execution dimension and position<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be dimension 
        /// string ("overworld"|"the_nether"|"the_end"|"custom dimension")<br/>
        /// </para>
        /// 
        /// <para>
        /// 7. <see cref="ExecuteCondition.Positioned"/><br/>
        /// Sets the execution position, without changing execution 
        /// rotation or dimension;<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="Vector3"/>
        /// </para>
        /// 
        /// <para>
        /// 8. <see cref="ExecuteCondition.PositionedAs"/><br/>
        /// Works like <see cref="ExecuteCondition.Positioned"/> but anchors at 
        /// entity<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="EntitySelector"/>
        /// </para>
        /// 
        /// <para>
        /// 9. <see cref="ExecuteCondition.Rotated"/><br/>
        /// Sets the execution rotation<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be yaw float<br/>
        /// <paramref name="extraParameters"/>[1] should be pitch float<br/>
        /// </para>
        ///       
        /// <para>
        /// 10. <see cref="ExecuteCondition.RotatedAs"/><br/>
        /// Sets the execution rotation to entity's rotation<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="EntitySelector"/> entity<br/>
        /// </para>
        /// </para>
        /// 
        /// <b>Execute Conditions (if/unless)</b>
        /// <para>
        /// 
        /// <para>
        /// 1. <see cref="ExecuteSubcondition.Block"/><br/>
        /// Compares the block at a given position to 
        /// a given block ID or block tag.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="Vector3"/><br/>
        /// <paramref name="extraParameters"/>[1] should be string block predicate tag in format of namespaced_id[predicate_states]{nbt_tags}<br/>
        /// </para>
        /// 
        /// <para>
        /// 2. <see cref="ExecuteSubcondition.Blocks"/><br/>
        /// Compares the blocks in two equally sized volumes<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="Vector3"/> array, where [0] is beginning of volume,
        /// [1] is end of volume and [2] is destination of volume<br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="ScanningMode"/><br/>
        /// </para>
        /// 
        /// <para>
        /// 3. <see cref="ExecuteSubcondition.NBTData"/><br/>
        /// Checks whether the targeted block, entity or 
        /// storage has any data tag for a given path<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="NBTDataType"/>
        /// and depending on it should be <paramref name="extraParameters"/>[1]<br/>
        /// <paramref name="extraParameters"/>[2] should always be nbt data string to check for.<br/>
        /// 3.1. <see cref="NBTDataType.Block"/><br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="Vector3"/> block pos to check<br/>
        /// 3.2. <see cref="NBTDataType.Entity"/><br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="EntitySelector"/> entity to check<br/>
        /// 3.3. <see cref="NBTDataType.Storage"/><br/>
        /// <paramref name="extraParameters"/>[1] should be string namespaced id to check<br/>
        /// </para>
        /// 
        /// <para>
        /// 4. <see cref="ExecuteSubcondition.Entity"/><br/>
        /// Checks whether one or more entities exist.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="EntitySelector"/> entity to check for.<br/>
        /// </para>
        /// 
        /// <para>
        /// 5. <see cref="ExecuteSubcondition.Predicate"/><br/>
        /// Checks whether the predicate evaluates to a positive result.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be namespaced id for predicate to seek.<br/>
        /// </para>
        /// 
        /// <para>
        /// 6. <see cref="ExecuteSubcondition.Scoreboard"/><br/>
        /// Check whether a score has the specific relation to 
        /// another score, or whether it is in a given range.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="ScoreComparation"/> 
        /// and other params will be dependant on it's value.<br/>
        /// 
        /// 6.1. <see cref="ScoreComparation.Comparation"/><br/>
        /// Other params should go this way:<br/>
        /// <see cref="EntitySelector"/> holder of the objective to be compared.<br/>
        /// <see cref="string"/> name of the objective to be compared the value.<br/>
        /// <see cref="Comparator"/> comparator to compare the value.<br/>
        /// <see cref="EntitySelector"/> holder of the objective to compare.<br/>
        /// <see cref="string"/> name of the objective to compare the value.<br/>
        /// 
        /// 6.2. <see cref="ScoreComparation.Match"/><br/>
        /// Other params should go this way:<br/>
        /// <see cref="EntitySelector"/> holder of the objective to be matched.<br/>
        /// <see cref="string"/> name of the objective to be matched the value.<br/>
        /// </para>
        /// 
        /// </para>
        /// 
        /// <para>
        /// <b>Execute Store (result|success)</b>
        /// 
        /// <para>
        /// 1. <see cref="ExecuteStore.Block"/><br/>
        /// Saves the final command's return value as 
        /// tag data within a block entity. Store as a 
        /// byte, short, int, long, float, or double.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="Vector3"/> of block<br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="NBTPath"/> to store data<br/>
        /// <paramref name="extraParameters"/>[2] should be <see cref="Type"/> of data to be stored<br/>
        /// <paramref name="extraParameters"/>[3] should be <see cref="double"/> scale for value to be rounded if it is decimal<br/>
        /// </para>
        /// 
        /// <para>
        /// 2. <see cref="ExecuteStore.Bossbar"/><br/>
        /// Saves the final command's return value in either 
        /// a bossbar's current value or its maximum value<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="string"/> namespaced id of bossbar to store data<br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="BossbarOverwrite"/> of value to overwrite (current or max)<br/>
        /// </para>
        /// 
        /// <para>
        /// 3. <see cref="ExecuteStore.Entity"/><br/>
        /// Save the final command's return value in one of an entity's data 
        /// tags. Store as a byte, short, int, long, float, or double.
        /// Like the /data command, /execute store entity  cannot modify player's NBT.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="EntitySelector"/> of entity<br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="NBTPath"/> to store data<br/>
        /// <paramref name="extraParameters"/>[2] should be <see cref="Type"/> of data to be stored<br/>
        /// <paramref name="extraParameters"/>[3] should be <see cref="double"/> scale for value to be rounded if it is decimal<br/>
        /// </para>
        /// 
        /// <para>
        /// 4. <see cref="ExecuteStore.Score"/><br/>
        /// Overrides the score held by targets on the given 
        /// objective with the final command's return value.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="EntitySelector"/> of score holder<br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="string"/> namespaced id of scoreboard<br/>
        /// </para>
        /// 
        /// <para>
        /// 5. <see cref="ExecuteStore.Storage"/><br/>
        /// Uses the path within storage target to store the return value in. 
        /// Store as a byte, short, int, long, float, or double.
        /// If the storage does not yet exist, it gets created.<br/>
        /// <i>Syntax:</i><br/>
        /// <paramref name="extraParameters"/>[0] should be <see cref="string"/> namespaced id of storage to store data<br/>
        /// <paramref name="extraParameters"/>[1] should be <see cref="NBTPath"/> to store data<br/>
        /// <paramref name="extraParameters"/>[2] should be <see cref="Type"/> of data to be stored<br/>
        /// <paramref name="extraParameters"/>[3] should be <see cref="double"/> scale for value to be rounded if it is decimal<br/>
        /// </para>        
        /// </para>
        /// </summary>
        /// <remarks>
        /// Most information taken from <a href="https://minecraft.fandom.com/wiki/Commands/execute">Minecraft Wiki page</a>.<br/>
        /// </remarks>
        /// <param name="cmd">Compiled command to be ran</param>
        /// <param name="cond">Extra execute params</param>
        /// <param name="oper">Execute operator (if|unless)</param>
        /// <param name="subcond">Condition to execute the command. See <paramref name="oper"/></param>
        /// <param name="store">Specific data that should be stored after command execution</param>
        /// <param name="cont">What the command should store</param>
        /// <param name="extraParameters">See summary on method</param>
        public ExecuteParameters(
            ICommand cmd,
            ExecuteCondition? cond = null,
            ExecuteOperator? oper = null,
            ExecuteSubcondition? subcond = null,
            ExecuteStore? store = null,
            ExecuteStoreContainer? cont = null,
            params object[]? extraParameters
            )
        {
            String = "/execute";
            // Create base stuff to build templates from later
            string tmp = cmd.Compiled.StartsWith("/") ? cmd.Compiled.Remove(0, 1) : cmd.Compiled;
            string tmpRun = $"run {tmp}";
            tmp = !cond.Equals(null) ? EnumHelper.GetStringValue(cond) : "";
            string tmpCond = $"{tmp}";
            tmp = !oper.Equals(null) ? EnumHelper.GetStringValue(oper) : "";
            string tmpOper = $"{tmp}";
            tmp = !subcond.Equals(null) ? EnumHelper.GetStringValue(subcond) : "";
            tmpOper += $" {tmp}";
            tmp = !cont.Equals(null) ? EnumHelper.GetStringValue(cont) : "";
            string tmpStore = !cont.Equals(null) ? $"store {tmp}" : "";
            tmp = !store.Equals(null) ? EnumHelper.GetStringValue(store) : "";
            tmpStore += !store.Equals(null) ? $" {tmp}" : "";
            
            // Switch through all the enumerators and increase current extraParameters node 
            int i = 0;
            if (extraParameters != null)
            {
                try
                {
                    if (cond != null)
                    {
                        switch (cond)
                        {
                            case ExecuteCondition.Align:
                                var swizzle = (string)extraParameters[i];
                                tmpCond += $" {swizzle}";
                                i++;
                                break;
                            case ExecuteCondition.Anchored:
                                var tmpc = (AnchorCondition)extraParameters[i];
                                tmpCond += $" {EnumHelper.GetStringValue(tmpc)}";
                                i++;
                                break;
                            case ExecuteCondition.As:
                                var tmpt = (EntitySelector)extraParameters[i];
                                tmpCond += $" {tmpt.String()}";
                                i++;
                                break;
                            case ExecuteCondition.At:
                                goto case ExecuteCondition.As;

                            case ExecuteCondition.Facing:
                                var tmpfc = (FacingCondition)extraParameters[i];
                                i++;
                                switch (tmpfc)
                                {
                                    case FacingCondition.Position:
                                        tmpCond += $" {((Vector3)extraParameters[i]).String()}";
                                        i++;
                                        break;
                                    case FacingCondition.Entity:
                                        tmpCond += $" entity {((EntitySelector)extraParameters[i]).String()} {EnumHelper.GetStringValue((AnchorCondition)extraParameters[i + 1])}";
                                        i = i + 2;
                                        break;
                                }
                                break;
                            case ExecuteCondition.In:
                                var tmpdn = (string)extraParameters[i];
                                tmpCond += $" {tmpdn}";
                                i++;
                                break;
                            case ExecuteCondition.Positioned:
                                var tmppv = (Vector3)extraParameters[i];
                                tmpCond += $" {tmppv.String()}";
                                i++;
                                break;
                            case ExecuteCondition.PositionedAs:
                                var tmpes = (EntitySelector)extraParameters[i];
                                tmpCond += $" {tmpes.String()}";
                                i++;
                                break;
                            case ExecuteCondition.Rotated:
                                var tmpy = (float)extraParameters[i];
                                i++;
                                var tmpp = (float)extraParameters[i];
                                tmpCond += $" {VectorHelper.DecimalToMCString(tmpy)} {VectorHelper.DecimalToMCString(tmpp)}";
                                i++;
                                break;
                            case ExecuteCondition.RotatedAs:
                                var tmpes2 = (EntitySelector)extraParameters[i];
                                tmpCond += $" {tmpes2.String()}";
                                i++;
                                break;
                            case ExecuteCondition.None:
                                tmpCond += "";
                                i++;
                                break;
                        }
                    }

                    if (oper != null && subcond != null)
                    {
                        switch (subcond)
                        {
                            case ExecuteSubcondition.Block:
                                var tmpv = (Vector3)extraParameters[i];
                                i++;
                                var tmpbn = (string)extraParameters[i];
                                i++;
                                tmpOper += $" {tmpv.String()} {tmpbn}";
                                break;
                            case ExecuteSubcondition.Blocks:
                                var tmpvs = (Vector3[])extraParameters[i];
                                i++;
                                var scanMode = (ScanningMode)extraParameters[i];
                                i++;
                                tmpOper += $" {tmpvs[0].String()} {tmpvs[1].String()} {tmpvs[2].String()} {EnumHelper.GetStringValue(scanMode)}";
                                break;
                            case ExecuteSubcondition.NBTData:
                                var tmpe = (NBTDataType)extraParameters[i];
                                i++;
                                string stmp22;
                                switch (tmpe)
                                {
                                    case NBTDataType.Block:
                                        var tmp2 = (Vector3)extraParameters[i];
                                        i++;
                                        stmp22 = $" block {tmp2.String()}";
                                        break;
                                    case NBTDataType.Entity:
                                        var tmp22 = (EntitySelector)extraParameters[i];
                                        i++;
                                        stmp22 = $" entity {tmp22.String()}";
                                        break;
                                    case NBTDataType.Storage:
                                        var tmp222 = (string)extraParameters[i];
                                        i++;
                                        stmp22 = $" storage {tmp222}";
                                        break;
                                    default:
                                        stmp22 = "";
                                        break;
                                }
                                tmpOper += $" {stmp22} {(string)extraParameters[i]}";
                                i++;
                                break;
                            case ExecuteSubcondition.Entity:
                                var atmp = (EntitySelector)extraParameters[i];
                                i++;
                                tmpOper += $" {atmp.String()}";
                                break;
                            case ExecuteSubcondition.Predicate:
                                var ptmp = (string)extraParameters[i];
                                i++;
                                tmpOper += $" {ptmp}";
                                break;
                            case ExecuteSubcondition.Scoreboard:
                                string fstmp;
                                var yatmp = (ScoreComparation)extraParameters[i];
                                i++;
                                switch (yatmp)
                                {
                                    case ScoreComparation.Comparation:
                                        var ettmp = (EntitySelector)extraParameters[i];
                                        i++;
                                        var objtmp = (string)extraParameters[i];
                                        i++;
                                        var ctmp = (Comparator)extraParameters[i];
                                        i++;
                                        var srchtmp = (EntitySelector)extraParameters[i];
                                        i++;
                                        var srctmp = (string)extraParameters[i];
                                        i++;
                                        fstmp = $" {ettmp.String()} {objtmp} {EnumHelper.GetStringValue(ctmp)} {srchtmp} {srctmp}";
                                        break;
                                    case ScoreComparation.Match:
                                        var ex1 = (EntitySelector)extraParameters[i];
                                        i++;
                                        var ex2 = (string)extraParameters[i];
                                        i++;
                                        var comptmp = (string)extraParameters[i];
                                        i++;
                                        fstmp = $"{ex1.String()} {ex2} matches {comptmp}";
                                        break;
                                    default:
                                        fstmp = "";
                                        break;
                                }
                                tmpOper += $" {fstmp}";
                                break;
                            case ExecuteSubcondition.None:
                                tmpOper += "";
                                break;
                            default:
                                goto case ExecuteSubcondition.None;
                        }
                    }

                    if (store != null && cont != null)
                    {
                        string fstr = string.Empty;
                        switch (store)
                        {
                            case ExecuteStore.Block:
                                var tmpp = (Vector3)extraParameters[i];
                                i++;
                                var tmpnp = (NBTPath)extraParameters[i];
                                i++;
                                var tmpt = (Type)extraParameters[i];
                                i++;
                                var tmps = (double)extraParameters[i];
                                i++;
                                fstr = $" {tmpp.String()} {tmpnp.JSON} {tmpt} {VectorHelper.DecimalToMCString(tmps)}";
                                break;
                            case ExecuteStore.Bossbar:
                                var tmpid = (string)extraParameters[i];
                                i++;
                                var tmpbo = (BossbarOverwrite)extraParameters[i];
                                i++;
                                fstr = $" {tmpid} {EnumHelper.GetStringValue(tmpbo)}";
                                break;
                            case ExecuteStore.Entity:
                                var tmps2 = (EntitySelector)extraParameters[i];
                                i++;
                                var tmpnp2 = (NBTPath)extraParameters[i];
                                i++;
                                var tmpt2 = (Type)extraParameters[i];
                                i++;
                                var tmpsc = (double)extraParameters[i];
                                i++;
                                fstr = $" {tmps2.String()} {tmpnp2.JSON} {tmpt2} {VectorHelper.DecimalToMCString(tmpsc)}";
                                break;
                            case ExecuteStore.Score:
                                var tmps22 = (EntitySelector)extraParameters[i];
                                i++;
                                var tmpobj = (string)extraParameters[i];
                                i++;
                                fstr = $" {tmps22.String()} {tmpobj}";
                                break;
                            case ExecuteStore.Storage:
                                var tmptr = (string)extraParameters[i];
                                i++;
                                var tmpnp22 = (NBTPath)extraParameters[i];
                                i++;
                                var tmpt222 = (Type)extraParameters[i];
                                i++;
                                var tmpscr = (double)extraParameters[i];
                                i++;
                                fstr = $" {tmptr} {tmpnp22.JSON} {tmpt222} {VectorHelper.DecimalToMCString(tmpscr)}";
                                break;
                            case ExecuteStore.None:
                                fstr = "";
                                break;
                            default:
                                goto case ExecuteStore.None;
                        }
                    }

                    String += $" {tmpCond} {tmpOper} {tmpRun} {tmpStore}".Replace("  ", " ");
                }
                // if wrong cast exception appears it means that provided object is invalid.
                catch (InvalidCastException ice)
                {
                    throw new ArgumentException($"Wrong argument at {nameof(extraParameters)}. More information: {ice.Message}");
                }
                // handle any other exception so the code won't break
                catch (Exception e)
                {
                    throw new ArgumentException($"A fatal error occurred (probably) because of parameter {nameof(extraParameters)}. More information: {e.Message}");
                }
            }
        }
#nullable disable
    }

    /// <summary>
    /// Represents enumerated condition for execute command
    /// </summary>
    public enum ExecuteCondition
    {
        /// <summary>
        /// align as swizzle of coordinates
        /// </summary>
        [EnumValue("align")] Align,
        /// <summary>
        /// anchor to entity/pos
        /// </summary>
        [EnumValue("anchored")] Anchored,
        /// <summary>
        /// execute as entity
        /// </summary>
        [EnumValue("as")] As,
        /// <summary>
        /// execute at entity/pos
        /// </summary>
        [EnumValue("at")] At,
        /// <summary>
        /// execute facing entity/pos
        /// </summary>
        [EnumValue("facing")] Facing,
        /// <summary>
        /// execute in
        /// </summary>
        [EnumValue("in")] In,
        /// <summary>
        /// execute positioned at pos
        /// </summary>
        [EnumValue("positoned")] Positioned,
        /// <summary>
        /// execute positioned as entity
        /// </summary>
        [EnumValue("positoned as")] PositionedAs,
        /// <summary>
        /// execute rotated with certain yaw and pitch
        /// </summary>
        [EnumValue("rotated")] Rotated,
        /// <summary>
        /// execute rotated as entity
        /// </summary>
        [EnumValue("rotated as")] RotatedAs,
        /// <summary>
        /// 
        /// </summary>
        [EnumValue("")] None
    }
    /// <summary>
    /// Represents type of facing
    /// </summary>
    public enum FacingCondition
    {
        [EnumValue("")] Position,
        [EnumValue("entity")] Entity,
    }

    /// <summary>
    /// Represents type of anchor
    /// </summary>
    public enum AnchorCondition
    {
        [EnumValue("eyes")] Eyes,
        [EnumValue("feet")] Feet
    }

    /// <summary>
    /// Represents scanning mode for execute command
    /// </summary>
    public enum ScanningMode
    {
        [EnumValue("all")] All,
        [EnumValue("masked")] NoAir
    }

    /// <summary>
    /// Represents operator for /execute command
    /// </summary>
    public enum ExecuteOperator
    {
        [EnumValue("if")] If,
        [EnumValue("unless")] Unless,
        [EnumValue("")] None
    }

    /// <summary>
    /// Represents subcondition for execute command to be executed with
    /// </summary>
    public enum ExecuteSubcondition
    {
        [EnumValue("block")] Block,
        [EnumValue("blocks")] Blocks,
        [EnumValue("data")] NBTData,
        [EnumValue("entity")] Entity,
        [EnumValue("predicate")] Predicate,
        [EnumValue("score")] Scoreboard,
        [EnumValue("")] None
    }

    /// <summary>
    /// Represents /execute store subcommand
    /// </summary>
    public enum ExecuteStore
    {
        [EnumValue("block")] Block,
        [EnumValue("bossbar")] Bossbar,
        [EnumValue("entity")] Entity,
        [EnumValue("score")] Score,
        [EnumValue("storage")] Storage,
        [EnumValue("")] None,
    }

    /// <summary>
    /// Represents data stored in /execute store command
    /// </summary>
    public enum ExecuteStoreContainer
    {
        [EnumValue("result")] Result,
        [EnumValue("success")] Success,
        [EnumValue("")] None,
    }

    /// <summary>
    /// Represents type of data in execute command
    /// </summary>
    public enum NBTDataType
    {
        [EnumValue("block")] Block,
        [EnumValue("entity")] Entity,
        [EnumValue("storage")] Storage
    }

    /// <summary>
    /// Represents comparator oper to compare values
    /// </summary>
    public enum Comparator
    {
        [EnumValue("<")] Less,
        [EnumValue("<=")] LessOrEqual,
        [EnumValue("==")] Equal,
        [EnumValue("=>")] MoreOrEqual,
        [EnumValue(">")] More
    }

    /// <summary>
    /// Represents type of comparing scoreboards
    /// </summary>
    public enum ScoreComparation
    {
        [EnumValue("")] Comparation,
        [EnumValue("matches")] Match
    }

    /// <summary>
    /// Specifies which value of boss bar to overwrite
    /// </summary>
    public enum BossbarOverwrite
    {
        [EnumValue("max")] MaxValue,
        [EnumValue("value")] CurrentValue
    }
}
