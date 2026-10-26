using TaleWorlds.Localization;

namespace EncyclopediaBattleRecords
{
    /// <summary>
    /// One name per stat, shared by the Sort By lists and the Battle Record panels so both always say the same thing. Kept short
    /// enough for the Sort By dropdown (about 11 characters). "Avg" stats are per 100 soldiers fielded (per 100 battles for a
    /// hero), except damage, which is per soldier (per battle). The panels' Averages heading names only the per-100 unit.
    /// </summary>
    internal static class StatNames
    {
        public const string Fielded = "{=EncyclopediaBattleRecords_fielded}Fielded";
        public const string Battles = "{=EncyclopediaBattleRecords_battles}Battles";
        public const string Kills = "{=EncyclopediaBattleRecords_kills}Kills";
        public const string Deaths = "{=EncyclopediaBattleRecords_deaths}Deaths";
        public const string Defeats = "{=EncyclopediaBattleRecords_defeats}Defeats";
        public const string Wounded = "{=EncyclopediaBattleRecords_wounded}Wounded";
        public const string Routed = "{=EncyclopediaBattleRecords_routed}Routed";
        public const string DmgDealt = "{=EncyclopediaBattleRecords_dmgDealt}Dmg Dealt";
        public const string DmgTaken = "{=EncyclopediaBattleRecords_dmgTaken}Dmg Taken";

        public const string TroopKills = "{=EncyclopediaBattleRecords_troopKills}Troop Kills";
        public const string HeroKills = "{=EncyclopediaBattleRecords_heroKills}Hero Kills";
        public const string BanditKills = "{=EncyclopediaBattleRecords_banditKills}Bandit Kills";
        public const string MercKills = "{=EncyclopediaBattleRecords_mercKills}Merc Kills";

        public const string AvgKills = "{=EncyclopediaBattleRecords_avgKills}Avg Kills";
        public const string AvgDeaths = "{=EncyclopediaBattleRecords_avgDeaths}Avg Deaths";
        public const string AvgDefeats = "{=EncyclopediaBattleRecords_avgDefeats}Avg Defeats";
        public const string AvgRouts = "{=EncyclopediaBattleRecords_avgRouts}Avg Routs";
        public const string KdRatio = "{=EncyclopediaBattleRecords_kdRatio}K/D Ratio";
        public const string AvgDmg = "{=EncyclopediaBattleRecords_avgDmg}Avg Dmg";
        public const string AvgTaken = "{=EncyclopediaBattleRecords_avgTaken}Avg Taken";

        public const string Title = "{=EncyclopediaBattleRecords_title}Battle Record";
        public const string TotalsTitle = "{=EncyclopediaBattleRecords_totalsTitle}Totals";
        public const string TargetsTitle = "{=EncyclopediaBattleRecords_targetsTitle}Kills by Target";
        public const string TroopAveragesTitle = "{=EncyclopediaBattleRecords_troopAveragesTitle}Averages (per 100 soldiers fielded)";
        public const string HeroAveragesTitle = "{=EncyclopediaBattleRecords_heroAveragesTitle}Averages (per 100 battles)";

        public static string Text(string name) => new TextObject(name).ToString();

        /// <summary>The Sort By list's number formats, also used by the panels.</summary>
        public const string CountFormat = "N0";
        public const string PerHundredFormat = "0.0";
        public const string RatioFormat = "0.00";
        public const string DamageFormat = "0";
    }
}
