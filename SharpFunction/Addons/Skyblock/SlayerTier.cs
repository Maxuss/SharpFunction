using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    /// Represents tier of skyblock slayer
    /// </summary>
    public sealed class SlayerTier
    {
        /// <summary>
        /// Represents item to give with command. E.G. Rotten flesh <br/>for revenant horror and ender pearl for voidgloom seraph
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// Tier of slayer. E.G. 1 or 4. Goes up to 6
        /// </summary>
        public int Tier { get; set; }
        /// <summary>
        /// Represents difficulty of slayer. E.G. "Beginner" or "Deadly"
        /// </summary>
        public string SlayerLevel { get; set; }
        /// <summary>
        /// Name of the slayer. E.G. "Revenant Horror"
        /// </summary>
        public string SlayerName { get; set; }
        /// <summary>
        /// Represents HP of slayer
        /// </summary>
        public int HP { get; set; }
        /// <summary>
        /// Damage per second of the boss
        /// </summary>
        public int DPS { get; set; }
        /// <summary>
        /// Level of slayer boss
        /// </summary>
        public int LVL { get; set; } = 100;
        /// <summary>
        /// Experience this tier gives on kill
        /// </summary>
        public int XP { get; set; }
        /// <summary>
        /// Cost to start slayer
        /// </summary>
        public int Cost { get; set; }
        /// <summary>
        /// Enemies required to kill to spawn this boss. E.G. Enderman for Enderman Slayer
        /// </summary>
        public string SlayerEnemy { get; set; }
        /// <summary>
        /// Abilities of the slayer
        /// </summary>
        public List<SlayerAbility> Abilities { get; set; } = new();
        /// <summary>
        /// Command to summon slayer boss
        /// </summary>
        public Summon Summon { get; set; }
        /// <summary>
        /// Command to give item representing the boss
        /// </summary>
        public Give Give { get; set; }
        /// <summary>
        /// Compiled give command
        /// </summary>
        public string GiveCommand { get; set; }
        /// <summary>
        /// Raw name of the mob
        /// </summary>
        public string RawMobName { get; set; }
        /// <summary>
        /// Compiled summon command
        /// </summary>
        public string SummonCommand { get; set; }
        /// <summary>
        /// Equipment of slayer boss
        /// </summary>
        public ArmorItems Equipment { get; set; } = null;

        private RawText giveRaw;
        private SuperRawText summonRaw;
        private dynamic num = new Dictionary<int, string>()
        {
            {1, "I"},
            {2, "II"},
            {3, "III"},
            {4, "IV"},
            {5, "V"},
            {6, "VI"}
        };

        private dynamic col = new Dictionary<int, Color>()
        {
            {1,Color.Green},
            {2, Color.Yellow},
            {3, Color.Red},
            {4, Color.DarkRed},
            {5, Color.DarkPurple},
            {6, Color.DarkAqua}
        };


        /// <summary>
        /// Initialize a new tier of slayer
        /// </summary>
        /// <param name="item">Item of slayer</param>
        /// <param name="name">Name of slayer</param>
        /// <param name="tier">Tier of slayer. Goes from 1 to 6 including</param>
        /// <param name="difficulty">Difficulty tier of slayer</param>
        /// <param name="hp">HP boss deals</param>
        /// <param name="dps">DPS boss deals</param>
        /// <param name="cost">Cost to summon mob in coins</param>
        /// <param name="enemy">Enemy to be summoned. E.G. enderman</param>
        /// <param name="abilities">Abilities of the slayer</param>
        public SlayerTier(string item, string name, int tier, string difficulty,
                          int hp, int dps, int cost, string enemy, int xp, ArmorItems items = null, params SlayerAbility[] abilities)
        {
            Item = item.StartsWith("minecraft:") ? item : $"minecraft:{item.ToLower().Replace(" ", "_")}";
            SlayerName = name;
            if(tier > 6 || tier < 1)
            {
                throw new ArgumentException("Tier should be more or equal to 1 and less or equal to 6", nameof(tier));
            }
            Tier = tier;
            SlayerLevel = difficulty;
            HP = hp;
            DPS = dps;
            Cost = cost;
            XP = xp;
            SlayerEnemy = enemy.StartsWith("minecraft:") ? enemy : $"minecraft:{enemy.ToLower().Replace(" ", "_")}";
            RawMobName = enemy.StartsWith("minecraft:") ? enemy.Replace("_", " ") : enemy;
            Abilities = abilities.ToList();
            Equipment = items;
        }

        /// <summary>
        /// Compiles 
        /// </summary>
        public void Compile()
        {
            #region Give
            RawText upper = new();
            SuperRawText hp = new();
            SuperRawText dps = new();
            RawText name = new();
            upper.AddField(SlayerLevel, Color.DarkGray, RawTextFormatting.Straight);
            upper.AddField(" ");
            hp.Append($"Health: ", Color.Gray, RawTextFormatting.Straight);
            hp.Append($"{HP.ToString("N0", new CultureInfo("en-US", false).NumberFormat)}{SkyblockHelper.HEALTH}", Color.Red, RawTextFormatting.Straight);
            dps.Append($"Damage: ", Color.Gray, RawTextFormatting.Straight);
            dps.Append($"{DPS.ToString("N0", new CultureInfo("en-US", false).NumberFormat)}", Color.Red, RawTextFormatting.Straight);
            dps.Append($" per second", Color.Gray, RawTextFormatting.Straight);
            upper.AddField(hp);
            upper.AddField(dps);
            upper.AddField(" ");
            foreach(SlayerAbility ability in Abilities)
            {
                if (ability != null)
                {
                    ability.Compile(upper);
                    upper.AddField(" ");
                }
            }
            SuperRawText reward = new();
            reward.Append("Reward: ", Color.Gray, RawTextFormatting.Straight);
            reward.Append($" {XP} {RawMobName.First().ToString().ToUpper() + string.Concat(RawMobName.Skip(1))} Slayer XP", Color.LightPurple);
            upper.AddField(reward);
            upper.AddField(" + Boss drops", Color.DarkGray, RawTextFormatting.Straight);
            upper.AddField(" ");
            SuperRawText cost = new();
            cost.Append("Cost to start: ", Color.Gray, RawTextFormatting.Straight);
            cost.Append($"{Cost.ToString("N0", new CultureInfo("en-US", false).NumberFormat)} coins", Color.Gold);
            upper.AddField(cost);
            upper.AddField(" ");
            upper.AddField("Click to slay!", Color.Yellow, RawTextFormatting.Straight);

            name.AddField($"{SlayerName} {num[Tier]}", col[Tier], RawTextFormatting.Straight);

            ItemNBT nbt = new ItemNBT();
            nbt.Display = new ItemDisplay();
            nbt.Display.AddLore(upper);
            nbt.Display.AddName(name);

            Item give = new Item(Item, nbt);
            Give = new Give(SimpleSelector.@p);
            Give.Compile(give, 1);
            GiveCommand = Give.Compiled;
            #endregion give
            #region Summon
            Summon = new();
            EntityNBT en = new();
            en.CustomNameVisible = true;
            SuperRawText enm = new();
            enm.Append("[", Color.DarkGray);
            enm.Append($"Lv.{LVL}", Color.Gray);
            enm.Append("]", Color.DarkGray);
            enm.Append($" {SkyblockHelper.SLAYER_REQ} ", Color.Red);
            enm.Append($"{SlayerName} ", col[Tier]);
            enm.Append($"{SkyblockHelper.ParseHP(HP)}", Color.Green);
            enm.Append("/", Color.White);
            enm.Append($"{SkyblockHelper.ParseHP(HP)}", Color.Green);
            enm.Append($"{SkyblockHelper.HEALTH}", Color.Red);
            en.CustomName = enm;
            en.NoAI = true;
            string wrappedNbt;
            if(Equipment is not null)
            {
                wrappedNbt = NBTWrapper.Wrap(en.Compile(), Equipment.Compile());
            }
            else
            {
                wrappedNbt = NBTWrapper.Wrap(en.Compile());
            }

            Entity e = new(SlayerEnemy, wrappedNbt);
            Summon.Compile(e, SimpleVector.Current);
            SummonCommand = Summon.Compiled;

            #endregion Summon
        }
    }
}
