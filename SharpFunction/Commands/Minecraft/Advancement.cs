using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA = SharpFunction.Commands.Minecraft.VanillaAdvancement;


namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    /// Represents advancement command. Equal to Minecraft's <code>/advancement {grant/revoke} {[only {advancement}]/everything} {criterion}</code>
    /// </summary>
    public sealed class Advancement : ICommand
    {
        public EntitySelector Selector { get; set; }

        public string Compiled { get; private set; }

        dynamic Achievment { get; } = new Dictionary<VA, string>() {
            { VA.ABalancedDiet, "minecraft:husbandry/balanced_diet" },
            { VA.ACompleteCatalogue, "minecraft:husbandry/complete_catalogue" },
            { VA.AcquireHardware, "minecraft:story/smelt_iron" },
            { VA.Adventure, "minecraft:adventure_root" },
            { VA.AdventuringTime, "minecraft:adventure/adventuring_time" },
            { VA.AFuriousCocktail, "minecraft:nether/all_potions" },
            { VA.Arbalistic, "minecraft:adventure/arbalists" },
            { VA.ASeedyPlace, "minecraft:husbandry/plant_seed" },
            { VA.ATerribleFortress, "minecraft:nether/enter_fortress" },
            { VA.AThrowawayJoke, "minecraft:adventure/throw_trident" },
            { VA.Beaconator, "minecraft:nether/create_full_beacon" },
            { VA.BeeOurGuest, "minecraft:husbandry/safely_harvest_honey" },
            { VA.BestFriendsForever, "minecraft:husbandry/tame_an_animal" },
            { VA.BringHomeTheBeacon, "minecraft:nether/create_beacon" },
            { VA.Bullseye, "minecraft:adventure/bullseye" },
            { VA.CountryLodeTakeMeHome, "minecraft:nether/use_lodestone" },
            { VA.CoverMeWithDebris, "minecraft:nether/netherite_armor" },
            { VA.CoverMeWithDiamonds, "minecraft:story/shiny_gear" },
            { VA.Diamonds, "minecraft:story/mine_diamond" },
            { VA.Enchanter, "minecraft:story/enchant_item" },
            { VA.Everything, "everything" },
            { VA.EyeSpy, "minecraft:story/follow_ender_eye" },
            { VA.FishyBusiness, "minecraft:husbandry/fishy_business" },
            { VA.FreeTheEnd, "minecraft:end/kill_dragon" },
            { VA.GettingAnUpgrade, "minecraft:story/upgrade_tools" },
            { VA.GreatViewFromHere, "minecraft:end/levitate" },
            { VA.HeroOfTheVillage, "minecraft:adventure/hero_of_the_village" },
            { VA.HiddenInDepths, "minecraft:nether/obtain_ancient_debris" },
            { VA.HiredHelp, "minecraft:adventure/summon_iron_golem" },
            { VA.HotStuff, "minecraft:story/lava_bucket" },
            { VA.HotTouristDestination, "minecraft:nether/explore_nether" },
            { VA.HowDidWeGetHere, "minecraft:nether/all_effects" },
            { VA.Husbandry, "minecraft:husbandry/root" },
            { VA.IceBucketChallenge, "minecraft:story/form_obsidian" },
            { VA.IntoFire, "minecraft:nether/obtain_blaze_rod" },
            { VA.LocalBrewery, "minecraft:nether/brew_potion" },
            { VA.Minecraft, "minecraft:story/root" },
            { VA.MonsterHunter, "minecraft:adventure/kill_a_mob" },
            { VA.MonstersHunted, "minecraft:adventure/kill_all_mobs" },
            { VA.Nether, "minecraft:nether/root" },
            { VA.NotQuiteNineLives, "minecraft:nether/charge_respawn_anchor" },
            { VA.NotTodayThankYou, "minecraft:story/deflect_arrow" },
            { VA.OhShiny, "minecraft:nether/distract_piglin" },
            { VA.OlBetsy, "minecraft:adventure/ol_betsy" },
            { VA.Postmortal, "minecraft:adventure/totem_of_undying" },
            { VA.RemoteGetaway, "minecraft:end/enter_end_gateway" },
            { VA.ReturnToSender, "minecraft:nether/return_to_sender" },
            { VA.SeriousDedication, "minecraft:husbandry/obtain_netherite_hoe" },
            { VA.SkysTheLimit, "minecraft:end/elytra" },
            { VA.SniperDuel, "minecraft:adventure/sniper_duel" },
            { VA.SpookyScarySkeletons, "minecraft:nether/get_wither_skull" },
            { VA.StickySituation, "minecraft:adventure/honey_block_slide" },
            { VA.StoneAge, "minecraft:story/stone_age" },
            { VA.SubspaceBubble, "minecraft:nether/fast_travel" },
            { VA.SuitUp, "minecraft:story/obtain_armor" },
            { VA.SweetDreams, "minecraft:adventure/sleep_in_bed" }, // (Are Made Of This)
            { VA.TacticalFishing, "minecraft:husbandry/tactical_fishing" },
            { VA.TakeAim, "minecraft:adventure/shoot_arrow" },
            { VA.TheBoatHasLegs, "minecraft:nether/ride_strider" },
            { VA.TheCityAtTheEndOfGame, "minecraft:end/find_end_city" },
            { VA.TheEndAgain, "minecraft:end/respawn_dragon" },
            { VA.TheEnd_q, "minecraft:story/enter_the_end" },
            { VA.TheEnd_q2, "minecraft:end/root" },
            { VA.TheNextGeneration, "minecraft:end/dragon_egg" },
            { VA.TheParrotsAndTheBats, "minecraft:husbandry/breed_an_animal" },
            { VA.ThoseWereTheDays, "minecraft:nether/find_bastion" },
            { VA.TotalBeelocation, "minecraft:husbandry/silk_touch_nest" },
            { VA.TwoBirdsOneArrow, "minecraft:adventure/two_birds_one_arrow" },
            { VA.TwoByTwo, "minecraft:husbandry/breed_all_animals" },
            { VA.UneasyAlliance, "minecraft:nether/uneasy_alliance" },
            { VA.VeryVeryFrightening, "minecraft:adventure/very_very_frightening" },
            { VA.VoluntaryExile, "minecraft:adventure/voluntary_exile" },
            { VA.WarPigs, "minecraft:nether/loot_bastion" },
            { VA.WeNeedToGoDeeper, "minecraft:story/enter_the_nether" },
            { VA.WhatADeal, "minecraft:adventure/trade" },
            { VA.WhoIsCuttingOnions, "minecraft:nether/obtain_crying_obsidian" },
            { VA.WhosThePillagerNow, "minecraft:adventure/whos_the_pillage_now" },
            { VA.WitheringHeights, "minecraft:nether/summon_wither" },
            { VA.YouNeedAMint, "minecraft:end/dragon_breath" },
            { VA.ZombieDoctor, "minecraft:story/cure_zombie_villager" },
        };
        /// <summary>
        /// Initialize Advancement Command class.<br/>
        /// See also: <br/>
        /// <seealso cref="EntitySelector"/>
        /// </summary>
        /// <param name="selector"><see cref="EntitySelector"/> to use</param>
        public Advancement(EntitySelector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Compiles /advancement command with give params
        /// </summary>
        /// <param name="adv">Advancement from <see cref="VA"/> enumerator</param>
        /// <param name="grant">True to grant advancement and False to revoke advancement</param>
        /// <param name="criterion">Criterions (sub-advancement) for advancement. None by default</param>
        public void Compile(VA adv, bool grant, string criterion="")
        {
            string name = GetAdvancement(adv);
            string gr = string.Empty;
            if (grant) gr = "grant";
            else gr = "revoke";
            // Currently does not support 'through' and 'from' keywords.
            // You can add them yourself in file.
            Compiled = $"/advancement {gr} {Selector.String()} {name} {criterion}";
        }

        /// <summary>
        /// Gets advancement name by VanillaAdvancement type. Returns `!undefined!` if provided advancement is wrong.
        /// </summary>
        /// <param name="adv">Type of advancement</param>
        /// <returns>Advancement id for minecraft or '!undefined!' if something is wrong</returns>
        public string GetAdvancement(VA adv)
        {
            string d = string.Empty;
            if (!adv.Equals(VA.Everything)) d += "only ";
            Dictionary<VA, string> s = (Dictionary<VA, string>) Achievment;
            if (s.Keys.Contains(adv)) d += Achievment[adv];
            else d += "!undefined!";
            return d;
        }
    }
}
