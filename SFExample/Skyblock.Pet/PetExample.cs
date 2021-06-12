using SharpFunction.Addons.Skyblock;
using SharpFunction.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFExample.Skyblock.Pet
{
    // this class contains an example on creating a pet
    public sealed class PetExample
    {
        public PetExample()
        {
            SkyblockPet p = new("???", "{Id:[I; 1180736830,2027045052,-1647592044,1801125370],Properties: { textures:[{ Value: \"eyJ0ZXh0dXJlcyI6eyJTS0lOIjp7InVybCI6Imh0dHA6Ly90ZXh0dXJlcy5taW5lY3JhZnQubmV0L3RleHR1cmUvOGM0ZDQ0YTBmZmUwMjM0OWU5OWRhMDYyOTIxMzA2MzExM2U2YmIzYWZjMjU5ZjQ2NjE4YzkwZWRjZTgzMDc4NiJ9fX0=\"}]}}", ItemRarity.Legendary, PetType.Runecrafting);
            p.Defense = 50;
            p.Strength = 50;
            p.Ferocity = 6;
            p.MagicFind = 6;
            p.PetLuck = 6;

            PetAbility abil1 = new("Shhhh!");
            abil1.AbilityDescription = new();
            abil1.AbilityDescription.Append("You can\\'t talk in chat!");

            PetAbility abil2 = new("Crewmate");
            abil2.AbilityDescription = new();
            SuperRawText s1 = new();
            s1.Append("All", Color.Green);
            s1.Append(" your minions work ", Color.Gray);
            s1.Append("10%", Color.Green);
            s1.Append(" faster, if", Color.Gray);
            abil2.AbilityDescription.Append(s1);
            abil2.AbilityDescription.Append("you have more than 4 visitors.");

            PetAbility abil3 = new("Imposter");
            abil3.AbilityDescription = new();
            SuperRawText s2 = new();
            s2.Append("You are naturally ", Color.Gray);
            s2.Append("invisible", Color.Red);
            s2.Append(" to enemies, as", Color.Gray);
            abil3.AbilityDescription.Append(s2);
            abil3.AbilityDescription.Append("long as you don\\'t attack them.");
            SuperRawText s3 = new();
            s3.Append("First hit deals ", Color.Gray);
            s3.Append("33%", Color.Red);
            s3.Append(" more damage!", Color.Gray);
            abil3.AbilityDescription.Append(s3);

            p.Abilities.Add(abil1);
            p.Abilities.Add(abil2);
            p.Abilities.Add(abil3);

            p.MaxLevel = true;
            p.IsPreview = false;

            Console.WriteLine(p.Compile());
        }
    }
}
