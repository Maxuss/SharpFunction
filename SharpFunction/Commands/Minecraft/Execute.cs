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
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Execute Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Execute() { }

        public void Compile()
        {

        }
    }

    /// <summary>
    /// Represents collection of conditions for /execute command with most of it being metadata
    /// </summary>
    public struct ExecuteParameters
    {
#nullable enable

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
            AsString = "/execute";
            // Create base stuff to build templates from later
            string tmp = cmd.Compiled.StartsWith("/") ? cmd.Compiled.Remove(0, 1) : cmd.Compiled;
            string tmpRun = $"run {tmp}";
            tmp = !cond.Equals(null) ? EnumHelper.GetStringValue(cond) : "";
            string tmpCond = $"{tmp}";
            tmp = !oper.Equals(null) ? EnumHelper.GetStringValue(oper) : "";
            string tmpOper = $"{tmp}";
            tmp = !subcond.Equals(null) ? EnumHelper.GetStringValue(subcond) : "";
            tmpOper += $" {tmp}";
            tmp = !store.Equals(null) ? EnumHelper.GetStringValue(store) : "";
            string tmpStore = $"store {tmp}";
            tmp = !cont.Equals(null) ? EnumHelper.GetStringValue(cont) : "";
            tmpStore += $" {tmp}";
            
            // Switch through all the enumerators and increase current extraParameters node 
            int i = 0;

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

            if(oper != null && subcond != null)
            {
                switch(subcond)
                {
                    case ExecuteSubcondition.Block:
                        var tmpv = (Vector3)extraParameters[i];
                        i++;
                        var tmpbn = (string)extraParameters[i];
                        i++;
                        tmpOper += $" block {tmpv.String()} {tmpbn}";
                        break;
                    case ExecuteSubcondition.Blocks:
                        var tmpvs = (Vector3[])extraParameters[i];
                        i++;
                        var scanMode = (ScanningMode)extraParameters[i];
                        i++;
                        tmpOper += $" blocks {tmpvs[0].String()} {tmpvs[1].String()} {tmpvs[2].String()} {EnumHelper.GetStringValue(scanMode)}";
                        break;
                    case ExecuteSubcondition.NBTData:
                        var tmpe = (NBTDataType)extraParameters[i];
                        i++;
                        string stmp22;
                        switch(tmpe)
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
                        tmpOper += $" data {stmp22}";
                        break;
                    case ExecuteSubcondition.Entity:
                        var atmp = (EntitySelector)extraParameters[i];
                        i++;
                        tmpOper += $" entity {atmp.String()}";
                        break;
                    case ExecuteSubcondition.Predicate:
                        var ptmp = (string)extraParameters[i];
                        i++;
                        tmpOper += $" predicate {ptmp}";
                        break;
                    case ExecuteSubcondition.Scoreboard:
                        string fstmp;
                        var yatmp = (ScoreComparation)extraParameters[i];
                        i++;
                        switch(yatmp)
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
                        tmpOper += $" score {fstmp}";
                        break;
                    case ExecuteSubcondition.None:
                        tmpOper += "";
                        break;
                    default:
                        goto case ExecuteSubcondition.None;
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
}
