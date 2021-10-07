using System.Collections.Generic;
using Newtonsoft.Json;

namespace SharpFunction.Universal
{
    /// <summary>
    /// Particle that can spawn in the world
    /// </summary>
    public interface Particle
    {
        /// <summary>
        /// Probability for the particle to spawn each tick
        /// </summary>
        [JsonProperty("probability")] public float Probability { get; set; }
        /// <summary>
        /// Type of particle
        /// </summary>
        [JsonProperty("type")] public string Type { get; }
    }

    /// <summary>
    /// Particle that spawns when a block is broken
    /// </summary>
    public class BlockParticle : Particle
    {
        /// <inheritdoc cref="Particle.Probability"/>
        public float Probability { get; set; }
        
        /// <inheritdoc cref="Particle.Type"/>
        public string Type => "block";

        /// <summary>
        /// ID of block to use
        /// </summary>
        [JsonProperty("Name")] public string BlockID { get; set; } = "minecraft:stone";
        
        /// <summary>
        /// Properties of block particle
        /// </summary>
        [JsonProperty("Properties")] public Dictionary<string, string> Properties { get; set; }
    }
}