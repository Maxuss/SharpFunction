using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpFunction.Addons.Skyblock;

namespace SFExample.Skyblock.Items
{
    public sealed class Items
    {
        public Items()
        {
            SkyblockItem item = new(ItemType.Sword, ItemRarity.Legendary, "Axe of the Shredded", "diamond_axe");
            item.AttackSpeed = 100;
            item.Damage = 500;
            item.Ferocity = 70;
            item.Defense = 650;
            item.CritChance = 77;
            item.DungeonStats = new();
            item.DungeonStats.GearScore = 9999;
            item.DungeonStats.IsDungeon = true;
            item.DungeonStats.Quality = 100000;

            item.HasGlint = true;
            item.HasCrafts = true;
            item.Health = 99;
            item.AddDescription(
                new AdvancedDescription().Append("Epic line", SharpFunction.Universal.Color.DarkGray));

            // output the command used to create it
            Console.WriteLine(item.Compile());
        }
    }
}
