using Newtonsoft.Json;

namespace SharpFunction.World
{
    /// <summary>
    /// Temperature of the biome
    /// </summary>
    public struct BiomePrecipitation
    {
        /// <summary>
        /// Standard biome temperature
        /// </summary>
        public static readonly BiomePrecipitation None = new("none");
        /// <summary>
        /// Rainforest biome temperature
        /// </summary>
        public static readonly BiomePrecipitation Rainy = new("rain");
        /// <summary>
        /// Low biome temperature
        /// </summary>
        public static readonly BiomePrecipitation Snowy = new("snow");

        /// <summary>
        /// Value stored inside struct
        /// </summary>
        public readonly string Value;

        internal BiomePrecipitation(string value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Category for biome to be sorted inside
    /// </summary>
    public struct BiomeCategory
    {
        /// <summary>
        /// No group
        /// </summary>
        public static readonly BiomeCategory None = new("none");
        /// <summary>
        /// Taiga group
        /// </summary>
        public static readonly BiomeCategory Taiga = new("taiga");
        /// <summary>
        /// Mountain group
        /// </summary>
        public static readonly BiomeCategory ExtremeHills = new("extreme_hills");
        /// <summary>
        /// Jungle group
        /// </summary>
        public static readonly BiomeCategory Jungle = new("jungle");
        /// <summary>
        /// Mesa group
        /// </summary>
        public static readonly BiomeCategory Mesa = new("mesa");
        /// <summary>
        /// Plains group
        /// </summary>
        public static readonly BiomeCategory Plains = new("plains");
        /// <summary>
        /// Savanna group
        /// </summary>
        public static readonly BiomeCategory Savanna = new("savanna");
        /// <summary>
        /// Icy (spikes) group
        /// </summary>
        public static readonly BiomeCategory Icy = new("icy");
        /// <summary>
        /// The End group
        /// </summary>
        public static readonly BiomeCategory Ender = new("the_end");
        /// <summary>
        /// Beach group
        /// </summary>
        public static readonly BiomeCategory Beach = new("beach");
        /// <summary>
        /// Forest group
        /// </summary>
        public static readonly BiomeCategory Forest = new("forest");
        /// <summary>
        /// Ocean group
        /// </summary>
        public static readonly BiomeCategory Ocean = new("ocean");
        /// <summary>
        /// Desert group
        /// </summary>
        public static readonly BiomeCategory Desert = new("desert");
        /// <summary>
        /// River group
        /// </summary>
        public static readonly BiomeCategory River = new("river");
        /// <summary>
        /// Swamp group
        /// </summary>
        public static readonly BiomeCategory Swamp = new("swamp");
        /// <summary>
        /// Mushroom group
        /// </summary>
        public static readonly BiomeCategory Mushroom = new("mushroom");
        /// <summary>
        /// The Nether group
        /// </summary>
        public static readonly BiomeCategory Nether = new("nether");
        
        /// <summary>
        /// Value stored inside struct
        /// </summary>
        public readonly string Value;

        internal BiomeCategory(string value)
        {
            Value = value;
        }
    }

    public struct BiomeTemperatureModifier
    {
        /// <summary>
        /// Default
        /// </summary>
        public static readonly BiomeTemperatureModifier None = new("none");
        /// <summary>
        /// Frozen
        /// </summary>
        public static readonly BiomeTemperatureModifier Frozen = new("frozen");
        
        /// <summary>
        /// Value stored inside struct
        /// </summary>
        public readonly string Value;

        internal BiomeTemperatureModifier(string value)
        {
            Value = value;
        }
    }
    
    /// <summary>
    /// Represents a biome for world generation
    /// </summary>
    public class Biome
    {
        [JsonProperty("precipitation")] private string _precipitation = "none";
        [JsonProperty("category")] private string _category = "none";
        [JsonProperty("temperature_modifier")] public string _temperatureModifier = "none";
        /// <summary>
        /// Depth of biome. Positive = land, Negative = oceans
        /// </summary>
        [JsonProperty("depth")] public float Depth { get; set; }
        /// <summary>
        /// Scale of biome. The less the value, the more flat it will be
        /// </summary>
        [JsonProperty("scale")] public float Scale { get; set; }
        /// <summary>
        /// Temperature of the biome
        /// </summary>
        [JsonProperty("temperature")] public float Temperature { get; set; }
        /// <summary>
        /// Temperature modifier of the biome
        /// </summary>
        [JsonIgnore] public BiomeTemperatureModifier TemperatureModifier
        {
            get => new(_temperatureModifier);
            set => _temperatureModifier = value.Value;
        }
        /// <summary>
        /// Temperature category of biome
        /// </summary>
        [JsonIgnore] public BiomePrecipitation Precipitation
        {
            get => new(_precipitation);
            set => _precipitation = value.Value;
        }
        /// <summary>
        /// Category of the biome
        /// </summary>
        [JsonIgnore] public BiomeCategory Category
        {
            get => new(_category);
            set => _category = value.Value;
        }
    }
}