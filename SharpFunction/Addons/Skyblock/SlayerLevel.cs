using System;
using SharpFunction.API;
using SharpFunction.Commands.Minecraft;
using SharpFunction.Exceptions;
using SharpFunction.Universal;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    ///     Represents rewards obtainable after reaching certain level of slayer
    /// </summary>
    public sealed class SlayerLevel
    {
        /// <summary>
        ///     Create new Slayer Level class instance
        /// </summary>
        /// <param name="slayermob">Mob name representing Slayer Boss</param>
        /// <param name="lvl">Level of this slayer level</param>
        /// <param name="skillname">Name describing this level. E.G. "Skilled" or "Novice" etc.</param>
        /// <param name="itemid">ID of item to give</param>
        public SlayerLevel(string slayermob, int lvl, string skillname, string itemid)
        {
            SlayerMob = slayermob;
            Level = lvl;
            SkillName = skillname;
            ItemID = itemid;
        }

        private AdvancedDescription controlledDescription { get; set; } = new();

        /// <summary>
        ///     This description will be accessable only after initializing level
        /// </summary>
        /// <exception cref="InitializationException">If accessed before finishing initialization</exception>
        public AdvancedDescription GeneratedDescription
        {
            get
            {
                if (finishInit) return controlledDescription;
                throw new InitializationException(nameof(GeneratedDescription));
            }
            set => controlledDescription = value;
        }

        /// <summary>
        ///     Name of slayer mob. E.G. Zombie or Enderman
        /// </summary>
        public string SlayerMob { get; set; }

        /// <summary>
        ///     Level required to obtain rewards
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     Name describing this level. E.G. "Skilled" or "Novice" etc.
        /// </summary>
        public string SkillName { get; set; }

        /// <summary>
        ///     ID of item to give
        /// </summary>
        public string ItemID { get; set; }

        // did the initialization began?
        private bool init { get; set; }
        private bool finishInit { get; set; }
        internal RawText name { get; set; }
        internal Item item { get; set; }
        internal ItemNBT nbt { get; set; }

        /// <summary>
        ///     Begins the initialization of compilator. After use of that method <br />you can use all methods until
        ///     <see cref="EndInit()" /> is called.
        /// </summary>
        public void BeginInit()
        {
            controlledDescription = new AdvancedDescription();
            name = new RawText();
            name.AddField($"{SlayerMob} Slayer LVL {Level}", Color.Gold, RawTextFormatting.Straight);
            item = new Item(ItemID);
            nbt = new ItemNBT();
            nbt.Display = new ItemDisplay();
            nbt.Display.AddName(name);
            controlledDescription.Append($"{SkillName}", Color.DarkGray);
            controlledDescription.Append("");
            controlledDescription.Append("Rewards:");
            init = true;
        }

        /// <summary>
        ///     Adds a reward to level's rewards
        /// </summary>
        /// <param name="compiledReward">Compiled reward to add</param>
        /// <exception cref="InitializationException">If accessed before starting initialization</exception>
        public void AddReward(SlayerReward compiledReward)
        {
            if (!init) throw new InitializationException(nameof(AddReward));
            if (compiledReward.Compiled is null) throw new ArgumentNullException(nameof(compiledReward.Compiled));
            controlledDescription.Append(compiledReward.Compiled);
        }

        /// <summary>
        ///     Ends the initialization making it possible to <see cref="GeneratedDescription" />
        /// </summary>
        public void EndInit()
        {
            controlledDescription.Append("");
            controlledDescription.Append("Right-Click to preview items!", Color.Aqua);
            controlledDescription.Append("Rewards claimed!", Color.Green);
            RawText dsc = new();
            foreach (var srt in controlledDescription.Lines) dsc.AddField(srt);
            ;
            nbt.Display.AddLore(dsc);
            item.NBT = nbt;
            init = false;
            finishInit = true;
        }

        /// <summary>
        ///     Compiles item into give command
        /// </summary>
        /// <returns>Compiled give command</returns>
        public string Compile()
        {
            Give g = new(SimpleSelector.Nearest);
            g.Compile(item);
            return g.Compiled;
        }
    }

    /// <summary>
    ///     Class representing simple slayer reward. Inherited by other slayer reward classes
    /// </summary>
    public abstract class SlayerReward
    {
        /// <summary>
        ///     Compiled SRT to add to display
        /// </summary>
        public SuperRawText Compiled { get; set; } = new();
    }

    /// <summary>
    ///     Represents simple slayer reward with just item
    /// </summary>
    public sealed class SlayerItemReward : SlayerReward
    {
        /// <inheritdoc cref="Compile(SkyblockItem)" />
        public SlayerItemReward(SkyblockItem item)
        {
            Compile(item);
        }

        /// <summary>
        ///     Creates a new slayer reward
        /// </summary>
        /// <param name="item">Item to copy data from</param>
        public void Compile(SkyblockItem item)
        {
            Compiled.Append($" {item.DisplayName}", item.Rarity.GetRarityColor());
        }
    }

    /// <summary>
    ///     Slightly complex slayer reward with recipe reward
    /// </summary>
    public sealed class SlayerRecipeReward : SlayerReward
    {
        /// <inheritdoc cref="Compile(SkyblockItem)" />
        public SlayerRecipeReward(SkyblockItem item)
        {
            Compile(item);
        }

        /// <summary>
        ///     Creates a new slayer reward
        /// </summary>
        /// <param name="item">Item to copy data from</param>
        public void Compile(SkyblockItem item)
        {
            Compiled.Append($" {item.DisplayName}", item.Rarity.GetRarityColor());
            Compiled.Append(" Recipe", Color.Gray);
        }
    }

    /// <summary>
    ///     Complex reward, rewarding a permanent stat boost
    /// </summary>
    public sealed class SlayerStatReward : SlayerReward
    {
        /// <inheritdoc cref="Compile(EncapsulatedType{string, int, Color})" />
        public SlayerStatReward(EncapsulatedType<string, int, Color> item)
        {
            Compile(item);
        }

        /// <summary>
        ///     Creates a new slayer reward
        /// </summary>
        /// <param name="item">
        ///     Data to gather from encapsulated type. Value 1: name of stat. Value 2: increase of stat. Value 3:
        ///     color of stat
        /// </param>
        public void Compile(EncapsulatedType<string, int, Color> item)
        {
            var name = item.VALUE1;
            var incr = item.VALUE2;
            var cl = item.VALUE3;
            Compiled.Append($" Permanent +{incr} {name}", cl);
        }
    }

    /// <summary>
    ///     Complex reward, rewarding a permanent perk for player.
    /// </summary>
    public sealed class SlayerPerkReward : SlayerReward
    {
        /// <inheritdoc cref="Compile(RawText)" />
        public SlayerPerkReward(RawText rt)
        {
            Compile(rt);
        }

        /// <summary>
        ///     Creates a new slayer reward
        /// </summary>
        /// <param name="rt">RT containing data of perk</param>
        public void Compile(RawText rt)
        {
            Compiled.Append("Perk: ", Color.DarkPurple);
            Compiled.Append(rt);
        }
    }
}