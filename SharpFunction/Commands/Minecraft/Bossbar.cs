using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Universal;
namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents bossbar command. Equal to Minecraft's <code>/bossbar {params}</code>
    /// </summary>
    public sealed class Bossbar : ICommand
    {
        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Bossbar Command class.<br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        public Bossbar(){ }

        /// <summary>
        /// Compile command as "/bossbar add {params}"
        /// </summary>
        /// <param name="id">ID-name of boss bar</param>
        /// <param name="name">Shown name of boss bar</param>
        public void Compile(string id, string name)
        {
            Compiled = $"/bossbar add {id} {name}";
        }
        
        /// <summary>
        /// Compile command as "/bossbar get {params}"
        /// </summary>
        /// <param name="id">ID-name of boss bar</param>
        /// <param name="get">Type of data to get from boss bar</param>
        public void Compile(string id, BossbarGet get)
        {
            switch (get)
            {
                case BossbarGet.MaxValue:
                    Compiled = $"/bossbar get {id} max";
                    break;
                case BossbarGet.Players:
                    Compiled = $"/bossbar get {id} player";
                    break;
                case BossbarGet.Value:
                    Compiled = $"/bossbar get {id} value";
                    break;
                case BossbarGet.Visible:
                    Compiled = $"/bossbar get {id} visible";
                    break;
            }
        }

        /// <summary>
        /// Compile command as "/bossbar list"
        /// </summary>
        public void Compile()
        {
            Compiled = "/bossbar list";
        }

        /// <summary>
        /// Compile command as "/bossbar remove {params}"
        /// </summary>
        /// <param name="id">ID-name of boss bar</param>
        public void Compile(string id)
        {
            Compiled = $"/bossbar remove {id}";
        }

        /// <summary>
        /// Compile command as "/bossbar set {params}"
        /// </summary>
        /// <param name="id">ID-name of boss bar</param>
        /// <param name="set">Type of data (T) to set to boss bar</param>
        /// <param name="extra">
        /// Extra data, depending on <paramref name="set"/><br/>
        /// 1. <see cref="BossbarSet.Color"/> > color string (<see cref="BossbarColor"/> fields are recommended)<br/>
        /// 2. <see cref="BossbarSet.MaxValue"/> > max value 32 bit integer<br/>
        /// 3. <see cref="BossbarSet.Name"/> > show name string<br/>
        /// 4. <see cref="BossbarSet.Players"/> > selector <see cref="EntitySelector"/>
        /// 5. <see cref="BossbarSet.Style"/> > style string. Allowed values: (notched_6|notched_10|notched_12|notched_20|progress)
        /// 6. <see cref="BossbarSet.Value"/> > value short. Value should be less than 100 and more than 0<br/>
        /// 7. <see cref="BossbarSet.Visible"/> > visibility bool.<br/>
        /// </param>
        public void Compile(string id, BossbarSet set, object extra)
        {
            try
            {
                switch (set)
                {
                    case BossbarSet.Color:
                        string color = (string)extra;
                        Compiled = $"/bossbar set {id} color {color}";
                        break;
                    case BossbarSet.MaxValue:
                        int max = (int)extra;
                        Compiled = $"/bossbar set {id} max {max}";
                        break;
                    case BossbarSet.Name:
                        string name = (string)extra;
                        Compiled = $"/bossbar set {id} name {name}";
                        break;
                    case BossbarSet.Players:
                        EntitySelector targets = (EntitySelector)extra;
                        Compiled = $"/bossbar set {id} players {targets.String()}";
                        break;
                    case BossbarSet.Style:
                        string[] possible = new[] { "notched_6", "notched_10", "notched_12", "notched_20", "progress" };
                        string style = (string)extra;
                        if (!possible.Contains(style)) throw new ArgumentException($"{nameof(extra)} is not allowed style. Allowed styles: (notched_6|notched_10|notched_12|notched_20|progress)");
                        Compiled = $"/bossbar set {id} style {style}";
                        break;
                    case BossbarSet.Value:
                        short val = (short)extra;
                        if (val > 100 || val < 0) throw new ArgumentException($"{nameof(extra)} is more than 100 or less than 0.");
                        Compiled = $"/bossbar set {id} value {val}";
                        break;
                    case BossbarSet.Visible:
                        bool visible = (bool)extra;
                        Compiled = $"/bossbar set {id} visible {visible}";
                        break;
                }
            }
            catch (InvalidCastException e) { throw new ArgumentException($"{nameof(extra)} is wrong type. More info: {e.Message}"); }
        }
    }

    /// <summary>
    /// Represents color of Bossbar.<br/>
    /// NOT to be misled with <see cref="Universal.Color"/>
    /// </summary>
    public readonly struct BossbarColor 
    {
        public readonly string Blue { get => "blue"; }
        public readonly string Green { get => "green"; }
        public readonly string Pink { get => "pink"; }
        public readonly string Purple { get => "purple"; }
        public readonly string Red { get => "red"; }
        public readonly string White { get => "white"; }
        public readonly string Yellow { get => "yellow"; }
    }
}
