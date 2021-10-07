using System;
using System.Drawing;
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
    /// Represents modifier for biome colors
    /// </summary>
    public struct BiomeColorModifier
    {
        /// <summary>
        /// No color modifier
        /// </summary>
        public static readonly BiomeColorModifier None = new("none");
        /// <summary>
        /// Darker colors like in dark forest
        /// </summary>
        public static readonly BiomeColorModifier DarkForest = new("dark_forest");
        /// <summary>
        /// Greener colors with other water colors
        /// </summary>
        public static readonly BiomeColorModifier Swamp = new("swamp");
        
        /// <summary>
        /// Value stored inside struct
        /// </summary>
        public readonly string Value;

        internal BiomeColorModifier(string value)
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

    /// <summary>
    /// Extra modifier for biome temperature
    /// </summary>
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
    /// Represents effects for biome
    /// </summary>
    public struct BiomeEffects
    {
        private int ColorToHex(Color clr) => clr.ToArgb();

        private Color HexToColor(int hex)
        {
            var byteArray = BitConverter.GetBytes(hex);
   
            return Color.FromArgb(byteArray[3], byteArray[0], byteArray[1], byteArray[2]);
        }
        
        [JsonProperty("fog_color")] private int fogColor;
        [JsonProperty("foliage_color")] private int foliageColor;
        [JsonProperty("grass_color")] private int grassColor;
        [JsonProperty("sky_color")] private int skyColor;
        [JsonProperty("water_color")] private int waterColor;
        [JsonProperty("water_fog_color")] private int waterFogColor;
        [JsonProperty("grass_color_modifier")] private string grassColorModifier;

        /// <summary>
        /// Hex value representing biome's fog color
        /// </summary>
        [JsonIgnore]
        public Color FogColor
        {
            get => HexToColor(fogColor);
            set => fogColor = ColorToHex(value);
        }

        /// <summary>
        /// Color of tree foliage in biome
        /// </summary>
        [JsonIgnore]
        public Color FoliageColor
        {
            get => HexToColor(foliageColor);
            set => foliageColor = ColorToHex(value);
        }
        
        /// <summary>
        /// Color of grass in biome
        /// </summary>
        [JsonIgnore]
        public Color GrassColor
        {
            get => HexToColor(grassColor);
            set => grassColor = ColorToHex(value);
        }
        
        /// <summary>
        /// Color of sky in biome
        /// </summary>
        [JsonIgnore]
        public Color SkyColor
        {
            get => HexToColor(skyColor);
            set => skyColor = ColorToHex(value);
        }
        
        /// <summary>
        /// Color of water in biome
        /// </summary>
        [JsonIgnore]
        public Color WaterColor
        {
            get => HexToColor(waterColor);
            set => waterColor = ColorToHex(value);
        }
        
        /// <summary>
        /// Color of fog in water in biome
        /// </summary>
        [JsonIgnore]
        public Color WaterFogColor
        {
            get => HexToColor(waterFogColor);
            set => waterFogColor = ColorToHex(value);
        }

        /// <summary>
        /// Extra color modifier for biome
        /// </summary>
        [JsonIgnore]
        public BiomeColorModifier GrassColorModifier
        {
            get => new(grassColorModifier);
            set => grassColorModifier = value.Value;
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
        /// Controls how hot will it be in the biome.<br/>
        /// Values over 0.85 also speed up fire spread.
        /// </summary>
        [JsonProperty("downfall")] public float Downfall { get; set; }
        /// <summary>
        /// Whether can players spawn in this biome
        /// </summary>
        [JsonProperty("player_spawn_friendly")] public bool CanPlayersSpawn { get; set; }
        /// <summary>
        /// Chance for creature to spawn in the biome. Must be between 0 and 1
        /// </summary>
        [JsonProperty("creature_spawn_probability")] public float CreatureSpawnChance { get; set; }
        
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