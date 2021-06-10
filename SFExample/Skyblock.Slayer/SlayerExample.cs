using SharpFunction.Addons.Skyblock;
using SharpFunction.Universal;
using SharpFunction.Writer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = SharpFunction.Universal.Color;

namespace SFExample.Skyblock.Slayer
{
    // This class showcases an example of creation a slayer
    // by creating a simple revenant horror with some extra items
    public class SlayerExample
    {
        public SlayerExample(FunctionWriter w)
        {
            SkyblockSlayer slayer = new();

            string[] hands = new[] { "\"minecraft:diamond_hoe\"" };
            string[] armor = new[] { "\"minecraft:diamond_boots\"", "\"minecraft:chainmail_leggings\"", "\"minecraft:diamond_chestplate\",tag:{Enchantments:[{}]}", "\"minecraft:player_head\",tag:{SkullOwner:{Id:[I;-275785545,676744624,-1504261926,894305861],Properties:{textures:[{Value:\"eyJ0ZXh0dXJlcyI6eyJTS0lOIjp7InVybCI6Imh0dHA6Ly90ZXh0dXJlcy5taW5lY3JhZnQubmV0L3RleHR1cmUvZDhiZWUyM2I1YzcyNmFlOGUzZDAyMWU4YjRmNzUyNTYxOWFiMTAyYTRlMDRiZTk4M2I2MTQxNDM0OWFhYWM2NyJ9fX0=\"}]}}}" };
            string[] miniArmor = new[] { "\"minecraft:diamond_boots\"", "\"minecraft:chainmail_leggings\"", "\"minecraft:diamond_chestplate\",tag:{Enchantments:[{}]}" };

            SlayerAbility drain = new();
            drain.AbilityColor = Color.Red;
            drain.Name = "Life Drain";
            drain.Description.Append("Drains health every few seconds.");

            SlayerAbility pest = new();
            pest.AbilityColor = Color.Green;
            pest.Name = "Pestilence";
            pest.Description.Append("Deals AOE damage every second,");
            pest.Description.Append("shredding armor by 25%.");

            SlayerAbility enrage = new();
            enrage.AbilityColor = Color.Red;
            enrage.Name = "Enrage";
            enrage.Description.Append("Gets really mad once in a while.");

            ArmorItems slayerItems = new(hands, armor);


            // create all tiers
            SlayerTier tier1 = new("rotten_flesh", "Revenant Horror", 1, "Beginner", 500, 15, 2000, "zombie", 5, slayerItems, drain);
            SlayerTier tier2 = new("rotten_flesh", "Revenant Horror", 2, "Strong", 20000, 25, 7250, "zombie", 25, slayerItems, drain, pest);
            SlayerTier tier3 = new("rotten_flesh", "Revenant Horror", 3, "Challenging", 400000, 120, 20000, "zombie", 100, slayerItems, drain, pest, enrage);
            SlayerTier tier4 = new("rotten_flesh", "Revenant Horror", 4, "Deadly", 1500000, 400, 50000, "zombie", 500, slayerItems, drain, pest, enrage);

            tier1.LVL = 50;
            tier2.LVL = 100;
            tier3.LVL = 210;
            tier4.LVL = 420;

            Console.WriteLine("Finished creating tiers");
            // items

            AdvancedDescription cliche = new();
            cliche.Append("");

            // rev flesh
            SkyblockItem revFlesh = new(ItemType.None, ItemRarity.Uncommon, "Revenant flesh", "rotten_flesh");
            revFlesh.HasGlint = true;
            revFlesh.HasCrafts = true;
            revFlesh.HideStats = true;
            revFlesh.AddDescription(cliche);
            SlayerDrop rf = new(revFlesh, DropRarity.Guaranteed);
            rf.MinimumTier = 1;
            rf.TierAmounts["Tier I"] = "1 to 3";
            rf.TierAmounts["Tier II"] = "9 to 18";
            rf.TierAmounts["Tier III"] = "30 to 50";
            rf.TierAmounts["Tier IV"] = "50 to 64";

            // foul flesh
            SkyblockItem foulFlesh = new(ItemType.None, ItemRarity.Rare, "Foul Flesh", "charcoal");
            foulFlesh.HideStats = true;
            foulFlesh.AddDescription(cliche);
            SlayerDrop ff = new(foulFlesh, DropRarity.Occasional);
            ff.MinimumTier = 2;
            ff.TierAmounts["Tier II"] = "1";
            ff.TierAmounts["Tier III"] = "1 to 2";
            ff.TierAmounts["Tier IV"] = "2 to 3";

            // smite 6
            SkyblockItem smite = new(ItemType.None, ItemRarity.Rare, "Enchanted Book", "enchanted_book");
            smite.HasGlint = true;
            smite.HideStats = true;
            AdvancedDescription ds = new();
            ds.Append("Deal blah blah blah damage to undead monsters");
            smite.AddDescription(ds);
            SlayerDrop s = new(smite, DropRarity.Extraordinary);
            s.ItemName = "Smite VI";
            s.RequiredLVL = 4;
            s.MinimumTier = 3;
            s.TierAmounts["Tier III"] = "1";


            // scythe blade
            SkyblockItem blade = new(ItemType.None, ItemRarity.Legendary, "Scythe Blade", "diamond");
            blade.HasGlint = true;
            blade.HasCrafts = true;
            blade.HideStats = true;
            blade.AddDescription(cliche);

            SlayerDrop sb = new(blade, DropRarity.PrayRNGesus);
            sb.MinimumTier = 4;
            sb.RequiredLVL = 7;
            sb.TierAmounts["Tier IV"] = "1";


            Console.WriteLine("Finished generating items");
            // minibosses

            // sycophant
            SkyblockEntity sycophant = new("zombie", "Revenant Sycophant", 24000, 30);
            sycophant.NoAI = true;
            sycophant.Equipment = new ArmorItems(new[] { "air" }, miniArmor);

            // champion
            SkyblockEntity champion = new("zombie", "Revenant Champion", 90000, 60);
            champion.NoAI = true;
            champion.Equipment = new ArmorItems(new[] { "air" }, miniArmor);

            // deformed
            SkyblockEntity deformed = new("zombie", "Deformed Revenant", 360000, 120);
            deformed.NoAI = true;
            deformed.Equipment = new ArmorItems(new[] { "air" }, miniArmor);
            deformed.NameColor = Color.DarkRed;

            Console.WriteLine("Finished generating mobs");
            
            // everything together
            SlayerTier[] tiers = new[] { tier1, tier2, tier3, tier4 };
            SkyblockItem[] items = new[] { revFlesh, foulFlesh, smite, blade };
            SlayerDrop[] drops = new[] { rf, ff, s, sb };
            SkyblockEntity[] minibosses = new[] { sycophant, champion, deformed };

            // initializing slayer

            slayer.Writer = w;
            slayer.SlayerTiers = tiers;
            slayer.SlayerItems = items;
            slayer.SlayerDrops = drops;
            slayer.Minibosses = minibosses;
            slayer.SlayerBossName = "Revenant Horror";
            Console.WriteLine("Finished Initializing slayer...");
            // generate datapack

            slayer.Generate();
        }
    }
}
