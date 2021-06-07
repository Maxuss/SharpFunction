using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents different game rules
    /// </summary>
    public enum GameruleValue
    {
        [EnumValue("announceAdvancements")] AnnounceAdvancements,
        [EnumValue("commandBlocksOutput")] CommandBlockOutput,
        [EnumValue("disableElytraMovementCheck")] DisableElytraMovementCheck,
        [EnumValue("disableRaids")] DisableRaids,
        [EnumValue("doDayLightCycle")] DaylightCycle,
        [EnumValue("doEntityDrops")] EntityDrops,
        [EnumValue("doFireTick")] FireTick,
        [EnumValue("doInsomnia")] SpawnPhantoms,
        [EnumValue("doImmediateRespawn")] InstantRespawn,
        [EnumValue("doLimitedCrafting")] LimitCrafts,
        [EnumValue("doMobLoot")] MobLoot,
        [EnumValue("doMobSpawning")] MobSpawn,
        [EnumValue("doPatrolSpawning")] PatrolSpawn,
        [EnumValue("doTileDrops")] TileDrop,
        [EnumValue("doTraderSpawning")] TraderSpawn,
        [EnumValue("doWeatherCycle")] WeatherCycle,
        [EnumValue("drowningDamage")] DrowningDamage,
        [EnumValue("fallDamage")] FallDamage,
        [EnumValue("fireDamage")] FireDamage,
        [EnumValue("forgiveDeadPlayers")] ForgiveDeadPlayers,
        [EnumValue("keepInventory")] KeepInventory,
        [EnumValue("logAdminCommand")] LogAdminCommands,
        [EnumValue("maxCommandChainLength")] MaxCBChainLength,
        [EnumValue("maxEntityCramming")] MaxEntityCramming,
        [EnumValue("mobGriefing")] MobGriefing,
        [EnumValue("naturalRegeneration")] NaturalRegeneration,
        [EnumValue("randomTickSpeed")] RandomTickSpeed,
        [EnumValue("reducedDebugInfo")] ReducedDebuggingInfo,
        [EnumValue("sendCommandFeedback")] SendCommandFeedback,
        [EnumValue("showDeathMessages")] ShowDeathMessages,
        [EnumValue("spawnRadius")] SpawnRadius,
        [EnumValue("spectatorsGenerateChunks")] SpectatorGenerateChunks,
        [EnumValue("universalAnger")] UniversalAnger,
    }
}
