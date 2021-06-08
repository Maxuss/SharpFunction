using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    /// Helper for skyblock addon
    /// </summary>
    public static class SkyblockHelper
    {
        public const string STRENGTH = "❁";
        public const string DAMAGE = "❁";
        public const string HEALTH = "❤";
        public const string DEFENCE = "❈";
        public const string TRUE_DEFENCE = "❂";
        public const string SPEED = "✦";
        public const string INTELLIGENCE = "✎";
        public const string CRIT_CHANCE = "☣";
        public const string CRIT_DAMAGE = "☠";
        public const string ATTACK_SPEED = "⚔";
        public const string MAGIC_FIND = "✯";
        public const string FEROCITY = "⫽";
        public const string SLAYER_REQ = "☠";
        public const string COMBAT_REQ = "❣";
        public const string CATA_REQ = "❣";
        public const string DUNGEON_STAR = "✪";
        public const string SCC = "α";
        public const string PET_LUCK = "♣";
        public const string RUNE_SYMBOL = "◆";
        public const string INCOMPLETE = "✖";
        public const string COMPLETE = "✔";
        public const string FRAGMENT_UPGRADED = "⚚";
        public const string ABILITY_DAMAGE = "๑";
        public const string MINING_SPEED = "⸕";
        public const string FORTUNE = "☘";

        public static RawText NO_NAME = new RawText();
        public static ChestSlot GRAY_GLASS = new ChestSlot("gray_stained_glass_pane", 1);
        public static ChestSlot GREEN_GLASS = new ChestSlot("green_stained_glass_pane", 1);
        public static ChestSlot EXIT_ARROW = new ChestSlot("arrow", 1);

        public static int[] RECIPE_SLOTS_1 = { 11, 12, 13, 
                                               20, 21, 22 };
        public static int[] RECIPE_SLOTS_2 = { 2, 3, 4 };

        public const int RECIPE_OUTPUT = 25;

        public static int[] GRAY_SLOTS_1 =
        {
            1, 2, 3,4,5,6,7,8,9,10,14,15,16,17,18,19,23,24,26,27
        };

        public static int[] GRAY_SLOTS_2 =
        {
            1,5,6,7,8,9,10,11,12,13,14,15,16,17,18
        };

        public static int[] GREEN_SLOTS =
        {
            19,20,21,22,/*ARROW,*/24,25,26,27
        };

        public const int ARROW_SLOT = 23;


        public static ChestNBT[] GenerateEmptyCraftingFields()
        {
            ChestNBT n1 = new ChestNBT();
            ChestNBT n2 = new ChestNBT();
            RawText nn = NO_NAME;
            nn.AddField(" ");
            var _1 = new ItemNBT();
            _1.Display = new ItemDisplay();
            _1.Display.AddName(nn);
            _1.Display.AddLore(nn);
            ChestSlot gray = GRAY_GLASS;
            gray.Tag = _1;
            ChestSlot green = GREEN_GLASS;
            green.Tag = _1;
            ChestSlot arrow = EXIT_ARROW;
            arrow.Tag = _1;
            arrow.SlotID = ARROW_SLOT;

            // Doing stuff with first chest
            foreach(int i in GRAY_SLOTS_1)
            {
                ChestSlot tmp = gray;
                tmp.SlotID = i;
                n1.AddSlotData(tmp);
            }

            // Doing stuff with second chest
            foreach(int i in GRAY_SLOTS_2)
            {
                ChestSlot tmp = gray;
                tmp.SlotID = i;
                n2.AddSlotData(tmp);
            }
            foreach(int i in GREEN_SLOTS)
            {
                ChestSlot tmp = green;
                tmp.SlotID = i;
                n2.AddSlotData(tmp);
            }

            n2.AddSlotData(arrow);

            return new ChestNBT[] { n1, n2};
        }
    }
}
