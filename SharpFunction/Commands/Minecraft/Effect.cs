using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents effect command. Equal to Minecraft's <code>/effect (clear|give) {targets} {effect}</code>
    /// </summary>
    public sealed class Effect : ICommand, ISelectorCommand
    {
        /// <inheritdoc cref="ISelectorCommand.Selector"/>
        public EntitySelector Selector { get; set; }
        /// <inheritdoc cref="ICommand.Compiled"/>
        public string Compiled { get; private set; }

        /// <summary>
        /// Initialize Effect Command class.<br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public Effect(EntitySelector selector)
        {
            Selector = selector;
        }
        /// <summary>
        /// Compiles the /effect *clear* command
        /// </summary>
        /// <param name="effect">Effect to clear</param>
        public void Compile(StatusEffect effect)
        {
            Compiled = $"/effect clear {Selector.String()} minecraft:{EnumHelper.GetStringValue(effect)}";
        }
        /// <summary>
        /// Compiles the /effect *give* command
        /// </summary>
        /// <param name="effect">Effect to give</param>
        /// <param name="timeSeconds">Time for this effect to last in seconds</param>
        /// <param name="amplifier">Amplifier of the effect starting from 0 index</param>
        /// <param name="hideParticles">Whether the particles will be hidden</param>
        public void Compile(StatusEffect effect, uint timeSeconds=10, uint amplifier=1, bool hideParticles=false)
        {
            Compiled = $"/effect give {Selector.String()} minecraft:{EnumHelper.GetStringValue(effect)} {timeSeconds} {amplifier} {Helper.CommandHelper.BoolToString(hideParticles)}";
        }
    }
    /// <summary>
    /// Status effect to put on player
    /// </summary>
    public enum StatusEffect
    {
        /// <summary><code>speed</code></summary>
        [EnumValue("speed")] Speed,
        /// <summary><code>slowness</code></summary>
        [EnumValue("slowness")] Slowness,
        /// <summary><code>haste</code></summary>
        [EnumValue("haste")] Haste,
        /// <summary><code>mining_fatigue</code></summary>
        [EnumValue("mining_fatigue")] MiningFatigue,
        /// <summary><code>strength</code></summary>
        [EnumValue("strength")] Strength,
        /// <summary><code>instant_health</code></summary>
        [EnumValue("instant_health")] InstantHealth,
        /// <summary><code>instant_damage</code></summary>
        [EnumValue("instant_damage")] InstantDamage,
        /// <summary><code>jump_boost</code></summary>
        [EnumValue("jump_boost")] JumpBoost,
        /// <summary><code>nausea</code></summary>
        [EnumValue("nausea")] Nausea,
        /// <summary><code>regeneration</code></summary>
        [EnumValue("regeneration")] Regeneration,
        /// <summary><code>resistance</code></summary>
        [EnumValue("resistance")] Resistance,
        /// <summary><code>fire_resistance</code></summary>
        [EnumValue("fire_resistance")] FireResistance,
        /// <summary><code>water_breathing</code></summary>
        [EnumValue("water_breathing")] WaterBreathing,
        /// <summary><code>invisibility</code></summary>
        [EnumValue("invisibility")] Invisibility,
        /// <summary><code>blindness</code></summary>
        [EnumValue("blindness")] Blindness,
        /// <summary><code>night_vision</code></summary>
        [EnumValue("night_vision")] NightVision,
        /// <summary><code>hunger</code></summary>
        [EnumValue("hunger")] Hunger,
        /// <summary><code>weakness</code></summary>
        [EnumValue("weakness")] Weakness,
        /// <summary><code>poison</code></summary>
        [EnumValue("poison")] Poison,
        /// <summary><code>wither</code></summary>
        [EnumValue("wither")] Wither,
        /// <summary><code>health_boost</code></summary>
        [EnumValue("health_boost")] HealthBoost,
        /// <summary><code>absorption</code></summary>
        [EnumValue("absorption")] Absorption,
        /// <summary><code>saturation</code></summary>
        [EnumValue("saturation")] Saturation,
        /// <summary><code>glowing</code></summary>
        [EnumValue("glowing")] Glowing,
        /// <summary><code>levitation</code></summary>
        [EnumValue("levitation")] Levitation,
        /// <summary><code>luck</code></summary>
        [EnumValue("luck")] Luck,
        /// <summary><code>unluck</code></summary>
        [EnumValue("unluck")] BadLuck,
        /// <summary><code>slow_falling</code></summary>
        [EnumValue("slow_falling")] SlowFalling,
        /// <summary><code>conduit_power</code></summary>
        [EnumValue("conduit_power")] ConduitPower,
        /// <summary><code>dolphins_grace</code></summary>
        [EnumValue("dolphins_grace")] DolphinGrace,
        /// <summary><code>bad_omen</code></summary>
        [EnumValue("bad_omen")] BadOmen,
        /// <summary><code>hero_of_the_village</code></summary>
        [EnumValue("hero_of_the_village")] VillageHero
    }
}
