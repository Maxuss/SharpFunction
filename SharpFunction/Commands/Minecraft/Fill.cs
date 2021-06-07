﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;
namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents fill command. Equal to Minecraft's <code>/fill {begin: Vector3} {end: Vector3} {tile: Tile} {extraParams}</code>
    /// </summary>
    public sealed class Fill : ICommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Fill Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Fill(){ }

        /// <summary>
        /// Compiles the /fill command
        /// </summary>
        /// <param name="from">Position to fill from</param>
        /// <param name="to">Position to fill to</param>
        /// <param name="tileName">Name of block used to fill</param>
        /// <param name="params">Extra parameters for command</param>
        public void Compile(Vector3 from, Vector3 to, string tileName, FillParameters @params = FillParameters.None)
        {
            Compiled = $"/fill {from.String()} {to.String()} {tileName} {Universal.EnumHelper.GetStringValue(@params)}";
        }

        /// <summary>
        /// Compile the /fill ... replace command
        /// </summary>
        /// <param name="from">Position to fill from</param>
        /// <param name="to">Position to fill to</param>
        /// <param name="tileName">Name of block used to fill</param>
        /// <param name="filterPredicate">Predicate used to replace blocks</param>
        public void Compile(Vector3 from, Vector3 to, string tileName, string filterPredicate)
        {
            Compiled = $"/fill {from.String()} {to.String()} {tileName} replace {filterPredicate}";
        }
    }

    /// <summary>
    /// Represents extra parameters passed for fill command
    /// </summary>
    public enum FillParameters
    {
        [EnumValue("destroy")] Destroy,
        [EnumValue("hollow")] Hollow,
        [EnumValue("keep")] Keep,
        [EnumValue("outline")] Outline,
        [EnumValue("replace")] Replace,
        [EnumValue("")] None
    }
}
