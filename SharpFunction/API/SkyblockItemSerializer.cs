using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using SharpFunction.Addons.Skyblock;
using SharpFunction.Universal;

namespace SharpFunction.API
{
    internal class SerializedItem
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("rarity")] public int Rarity { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("abilities")] public List<AbilityContainable> Abilities { get; set; }
        [JsonProperty("lore")] public string Lore { get; set; }
        [JsonProperty("material")] public string Material { get; set; }
        [JsonProperty("type")] public int Type { get; set; }
        [JsonProperty("isLeather")] public bool Leather { get; set; }
        [JsonProperty("leatherColor")] public string LeatherColor { get; set; }
        [JsonProperty("slayer")] public Dictionary<string, object> Slayer { get; set; }
        [JsonProperty("stats")] public Dictionary<string, int?> Stats { get; set; }
        [JsonProperty("dungeons")] public Dictionary<string, object> Dungeons { get; set; }
        [JsonProperty("hasCrafts")] public bool HasCrafts { get; set; }
        [JsonProperty("hasGlint")] public bool HasGlint { get; set; }
        [JsonProperty("hideStats")] public bool HideStats { get; set; }
        [JsonProperty("makeEmptyLines")] public bool MakeEmptyLines { get; set; }
    }
    
    internal class AbilityContainable
    {
        public AbilityContainable(ItemAbility abil)
        {
            Name = abil.Name;
            ManaCost = abil.ManaCost;
            AbilityType = abil.Type.GetStringValue();
            Cooldown = abil.Cooldown;
            AbilityDescription = abil.Description.OldValue();
        }

        [JsonProperty("name")] internal string Name { get; set; }
        [JsonProperty("mana")] internal int ManaCost { get; set; }
        [JsonProperty("type")] internal string AbilityType { get; set; }
        [JsonProperty("cooldown")] internal int Cooldown { get; set; }
        [JsonProperty("description")] internal string AbilityDescription { get; set; }
    }

    /// <summary>
    ///     Serializes SkyblockItems to Maxus & Sketchpad's Skyblock Remake format
    /// </summary>
    public static class SkyblockItemSerializer
    {
        /// <summary>
        ///     Zips string into bytes
        /// </summary>
        /// <param name="str">String to be compressed</param>
        /// <returns>Compressed string</returns>
        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                msi.CopyTo(gs);
            }

            return mso.ToArray();
        }

        /// <summary>
        ///     Decompresses the bytes into string
        /// </summary>
        /// <param name="bytes">Bytes to be decompressed</param>
        /// <returns>Decompressed string</returns>
        public static string Unzip(byte[] bytes)
        {
            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                gs.CopyTo(mso);
            }

            return Encoding.UTF8.GetString(mso.ToArray());
        }

        /// <summary>
        ///     Serializes a <see cref="SkyblockItem" /> into deflated string to be used by SkyblockConcept parser
        /// </summary>
        /// <param name="item">Item to be serialized</param>
        /// <returns>String representing the item</returns>
        public static string Serialize(SkyblockItem item)
        {
            var obj = new SerializedItem
            {
                Name = item.DisplayName,
                Rarity = Enum.GetValues<ItemRarity>().ToList().IndexOf(item.Rarity),
                Description = item.AdvancedDescription.OldValue(),
                Abilities = item.Abilities.ConvertAll(abil => new AbilityContainable(abil)),
                Lore = string.Join("\\n", item.ItemDescription._deprecated),
                Material = item.ID,
                Type = Enum.GetValues<ItemType>().ToList().IndexOf(item.Type),
                Leather = item.IsLeather,
                LeatherColor = item.LeatherColorHex,
                Slayer = new Dictionary<string, object>
                {
                    {"requires", item.Requirement.HasRequirement},
                    {"level", item.Requirement.SlayerName},
                    {"name", item.Requirement.SlayerName}
                },
                Stats = new Dictionary<string, int?>
                {
                    {"health", item.Health},
                    {"defense", item.Defense},
                    {"trueDefense", item.TrueDefense},
                    {"speed", item.Speed},
                    {"intelligence", item.Intelligence},
                    {"ferocity", item.Ferocity},
                    {"damage", item.Damage},
                    {"critChance", item.CritChance},
                    {"critDamage", item.CritDamage},
                    {"attackSpeed", item.AttackSpeed},
                    {"strength", item.Strength},
                    {"magicFind", item.MagicFind},
                    {"petLuck", item.PetLuck},
                    {"seaCreatureChance", item.SeaCreatureChance}
                },
                Dungeons = new Dictionary<string, object>
                {
                    {"isDungeon", item.DungeonStats.IsDungeon},
                    {"quality", item.DungeonStats.Quality},
                    {"strQuality", string.Join("", item.DungeonStats.GetQuality().Deprecated)},
                    {"gearScore", item.DungeonStats.GearScore},
                    {"strGearScore", string.Join("", item.DungeonStats.GetGearScore().Deprecated)}
                },
                HasCrafts = item.HasCrafts,
                HasGlint = item.HasGlint,
                HideStats = item.HideStats,
                MakeEmptyLines = item.MakeEmptyLines
            };
            var str = JsonConvert.SerializeObject(obj, Formatting.None);
            var base64 = Convert.ToBase64String(Zip(str));
            Console.WriteLine(Unzip(Convert.FromBase64String(base64)));
            return base64;
        }
    }
}