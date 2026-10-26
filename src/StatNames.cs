using TaleWorlds.Localization;

namespace TroopKDR
{
    /// <summary>
    /// One name per stat, shared by the Sort By lists and the Battle Record panels so both always say the same thing. Kept short
    /// enough for the Sort By dropdown (about 11 characters). "Avg" stats are per 100 soldiers fielded (per 100 battles for a
    /// hero), except damage, which is per soldier (per battle). The panels' Averages heading names only the per-100 unit.
    /// </summary>
    internal static class StatNames
    {
        public const string Fielded = "{=TroopKDR_fielded}Fielded";
        public const string Battles = "{=TroopKDR_battles}Battles";
        public const string Kills = "{=TroopKDR_kills}Kills";
        public const string Deaths = "{=TroopKDR_deaths}Deaths";
        public const string Defeats = "{=TroopKDR_defeats}Defeats";
        public const string Wounded = "{=TroopKDR_wounded}Wounded";
        public const string Routed = "{=TroopKDR_routed}Routed";
        public const string DmgDealt = "{=TroopKDR_dmgDealt}Dmg Dealt";
        public const string DmgTaken = "{=TroopKDR_dmgTaken}Dmg Taken";

        public const string TroopKills = "{=TroopKDR_troopKills}Troop Kills";
        public const string HeroKills = "{=TroopKDR_heroKills}Hero Kills";
        public const string BanditKills = "{=TroopKDR_banditKills}Bandit Kills";
        public const string MercKills = "{=TroopKDR_mercKills}Merc Kills";

        public const string AvgKills = "{=TroopKDR_avgKills}Avg Kills";
        public const string AvgDeaths = "{=TroopKDR_avgDeaths}Avg Deaths";
        public const string AvgDefeats = "{=TroopKDR_avgDefeats}Avg Defeats";
        public const string AvgRouts = "{=TroopKDR_avgRouts}Avg Routs";
        public const string KdRatio = "{=TroopKDR_kdRatio}K/D Ratio";
        public const string AvgDmg = "{=TroopKDR_avgDmg}Avg Dmg";
        public const string AvgTaken = "{=TroopKDR_avgTaken}Avg Taken";

        public const string Title = "{=TroopKDR_title}Battle Record";
        public const string TotalsTitle = "{=TroopKDR_totalsTitle}Totals";
        public const string TargetsTitle = "{=TroopKDR_targetsTitle}Kills by Target";
        public const string TroopAveragesTitle = "{=TroopKDR_troopAveragesTitle}Averages (per 100 soldiers fielded)";
        public const string HeroAveragesTitle = "{=TroopKDR_heroAveragesTitle}Averages (per 100 battles)";

        public static string Text(string name) => new TextObject(name).ToString();

        /// <summary>The Sort By list's number formats, also used by the panels.</summary>
        public const string CountFormat = "N0";
        public const string PerHundredFormat = "0.0";
        public const string RatioFormat = "0.00";
        public const string DamageFormat = "0";
    }
}
