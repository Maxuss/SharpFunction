using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using SharpFunction.Addons.Skyblock;
using SharpFunction.Universal;

namespace SharpFunction.API
{
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
            Dictionary<string, object> json = new()
            {
                {"name", item.DisplayName},
                {"rarity", (int) item.Rarity},
                {"description", item.AdvancedDescription.OldValue()},
                {"abilities", item.Abilities.ConvertAll(abil => new AbilityContainable(abil))},
                {"lore", string.Join("\n", item.ItemDescription._deprecated)},
                {"minecraftId", item.ID},
                {"type", item.Type.GetStringValue()},
                {"isLeather", item.IsLeather},
                {"leatherColor", item.LeatherColorHex},
                {
                    "slayer", new Dictionary<string, object>
                    {
                        {"requires", item.Requirement.HasRequirement},
                        {"level", item.Requirement.SlayerLevel},
                        {"name", item.Requirement.SlayerName}
                    }
                },
                {
                    "stats", new Dictionary<string, int?>
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
                    }
                },
                {
                    "dungeons", new Dictionary<string, object>
                    {
                        {"isDungeon", item.DungeonStats.IsDungeon},
                        {"quality", item.DungeonStats.Quality},
                        {"strQuality", string.Join("", item.DungeonStats.GetQuality().Deprecated)},
                        {"gearScore", item.DungeonStats.GearScore},
                        {"strGearScore", string.Join("", item.DungeonStats.GetGearScore().Deprecated)}
                    }
                },
                {"hasCrafts", item.HasCrafts},
                {"hasGlint", item.HasGlint},
                {"hideStats", item.HideStats}
            };
            var str = JsonConvert.SerializeObject(json, Formatting.None);
            var base64 = Convert.ToBase64String(Zip(str));
            Console.WriteLine(Unzip(Convert.FromBase64String(base64)));
            return base64;
        }
    }
}