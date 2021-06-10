using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;
using SharpFunction.Universal;
namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents /execute command. Equal to Minecraft's <code>/execute {tons of params}</code>
    /// </summary>
    public sealed class Execute : ICommand
    {

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
        /// Too complex so it won't fit here. See wiki for details.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cond"></param>
        /// <param name="oper"></param>
        /// <param name="subcond"></param>
        /// <param name="store"></param>
        /// <param name="cont"></param>
        /// <param name="extraParameters"></param>
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
                                tmpOper += $" {stmp22}";
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
                                        var comptmp = (string)extraParameters[i];
                                        i++;
                                        fstmp = $" matches {comptmp}";
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
                                BossbarOverwrite tmpbo = (BossbarOverwrite)extraParameters[i];
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
        [EnumValue("align")] Align,
        [EnumValue("anchored")] Anchored,
        [EnumValue("as")] As,
        [EnumValue("at")] At,
        [EnumValue("facing")] Facing,
        [EnumValue("in")] In,
        [EnumValue("positoned")] Positioned,
        [EnumValue("positoned as")] PositionedAs,
        [EnumValue("rotated")] Rotated,
        [EnumValue("rotated as")] RotatedAs,
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
