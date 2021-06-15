using System;
using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    /// Represents *fake* Hypixel Skyblock entity
    /// </summary>
    public sealed class SkyblockEntity
    {
        /// <summary>
        /// Represents the entity to be summoned
        /// </summary>
        public Entity TheEntity { get; set; }
        /// <summary>
        /// Max health of mob
        /// </summary>
        public int MaxHP { get; set; } = 0;
        /// <summary>
        /// Current health of the mob
        /// </summary>
        public int CurrentHP { get; set; } = 0;
        /// <summary>
        /// Name of the mob
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Represents level of the mob
        /// </summary>
        public int Level { get; set; } = 0;
        /// <summary>
        /// Represents vanilla entity to be summoned
        /// </summary>
        public string VanillaEntity { get; set; } = "minecraft:zombie";
        /// <summary>
        /// Represents armor and items of the entity. None by default.
        /// </summary>
        public ArmorItems Equipment { get; set; } = null;
        /// <summary>
        /// Entity will not move
        /// </summary>
        public bool NoAI { get; set; } = true;
        /// <summary>
        /// Command that will be available after <see cref="Compile()"/>
        /// </summary>
        public Summon Command { get; private set; }
        /// <summary>
        /// Color of entity's name
        /// </summary>
        public Color NameColor { get; set; } = Color.Red;

        /// <summary>
        /// Whether the entity is armor stand
        /// </summary>
        /// <value></value>
        public bool IsArmorStand { get; set; } = false;

        /// <summary>
        /// NBT of armor stand if the enity is AS
        /// </summary>
        /// <value></value>
        public ArmorStandNBT StandNBT { get; set; } = null;

        /// <summary>
        /// Creates a simple entity with base params
        /// </summary>
        /// <param name="mob">Vanilla mob to spaw</param>
        /// <param name="name">Name of the skyblock mob</param>
        /// <param name="maxHP">Max HP of the mob</param>
        /// <param name="level">Level of the mob</param>
        public SkyblockEntity(string mob, string name, int maxHP, int level)
        {
            Name = name;
            VanillaEntity = mob.StartsWith("minecraft:") ? mob : $"minecraft:{mob}";
            IsArmorStand = mob == "armor_stand";
            MaxHP = maxHP;
            Level = level;
        }

        /// <summary>
        /// Compiles data to <see cref="Command"/>, which can then be invoked.
        /// </summary>
        /// <returns>Compiled string of command</returns>
        public string Compile()
        {
            var srt = new SuperRawText();
            srt.Append("[", Color.DarkGray);
            srt.Append($"Lv.{Level}", Color.Gray);
            srt.Append("]", Color.DarkGray);
            srt.Append($" {Name}", NameColor);
            srt.Append($" {CurrentHP}", Color.Green);
            srt.Append("/", Color.White);
            srt.Append($"{MaxHP}", Color.Green);
            srt.Append("❤", Color.Red);
            EntityNBT nbt = new EntityNBT();
            nbt.CustomName = srt;
            nbt.CustomNameVisible = true;
            nbt.NoAI = NoAI;
            nbt.Silent = true;
            nbt.Invulnerable = true;

            string mainNBT = nbt.Compile();

            if(IsArmorStand && !NullChecker.IsNull(StandNBT))
            {
                TheEntity = NullChecker.IsNull(Equipment) 
                ? new Entity(VanillaEntity, NBTWrapper.Wrap(mainNBT, StandNBT.Compile())) 
                : new Entity(VanillaEntity, NBTWrapper.Wrap(mainNBT, StandNBT.Compile(),Equipment.Compile()));
            }
            else 
            {
                TheEntity = NullChecker.IsNull(Equipment) 
                    ? new Entity(VanillaEntity, NBTWrapper.Wrap(mainNBT)) 
                    : new Entity(VanillaEntity, NBTWrapper.Wrap(mainNBT, Equipment.Compile()));
            }
            Command = new Summon();
            Command.Compile(TheEntity, SimpleVector.Current);
            return Command.Compiled;
        }
    }
}
