namespace SharpFunction.Addons.Skyblock
{
    /// <summary>
    ///     Helper for skyblock addon
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
        public const string ARR = "\\u27A4";


        public static string ParseHP(int hp)
        {
            var mult = "";
            float php;
            // 1b
            if (hp > 1000000000)
            {
                php = hp / 1000000000;
                mult = $"{php}B";
            }
            else if (hp > 1000000)
            {
                php = hp / 1000000;
                mult = $"{php}M";
            }
            else if (hp > 1000)
            {
                php = hp / 1000;
                mult = $"{php}k";
            }
            else
            {
                mult = $"{hp}";
            }

            return mult;
        }
    }
}