using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using TaleWorlds.Localization;

namespace TroopKDR
{
    /// <summary>
    /// Adds Fielded, Kills (totals), Avg Kills, Avg Deaths, Avg Defeats, Avg Routs (per 100 soldiers fielded), K/D Ratio, and Avg
    /// Dmg and Avg Taken (per soldier fielded) to the Sort By list of the encyclopedia's troop page; names come from StatNames. A
    /// troop never fielded shows "-" and stays at the bottom in either sort order.
    /// </summary>
    internal static class EncyclopediaPatch
    {
        public static MethodInfo Target => AccessTools.Method(typeof(DefaultEncyclopediaUnitPage), "InitializeSortControllers");

        public static void Apply(Harmony harmony) =>
            harmony.Patch(Target, postfix: new HarmonyMethod(typeof(EncyclopediaPatch), nameof(Postfix)));

        private static void Postfix(ref IEnumerable<EncyclopediaSortController> __result)
        {
            try
            {
                var list = __result as List<EncyclopediaSortController> ?? new List<EncyclopediaSortController>(__result ?? Enumerable.Empty<EncyclopediaSortController>());
                list.Add(Sort(StatNames.Fielded, StatNames.CountFormat, (r, t) => r.Fielded(t)));
                list.Add(Sort(StatNames.Kills, StatNames.CountFormat, (r, t) => r.Kills(t)));
                list.Add(Sort(StatNames.AvgKills, StatNames.PerHundredFormat, (r, t) => PerHundred(r.Kills(t), r.Fielded(t))));
                list.Add(Sort(StatNames.AvgDeaths, StatNames.PerHundredFormat, (r, t) => PerHundred(r.Deaths(t), r.Fielded(t))));
                list.Add(Sort(StatNames.AvgDefeats, StatNames.PerHundredFormat, (r, t) => PerHundred(r.Defeats(t), r.Fielded(t))));
                list.Add(Sort(StatNames.AvgRouts, StatNames.PerHundredFormat, (r, t) => PerHundred(r.Routed(t), r.Fielded(t))));
                list.Add(Sort(StatNames.KdRatio, StatNames.RatioFormat, (r, t) => Ratio(r.Kills(t), r.Defeats(t))));
                list.Add(Sort(StatNames.AvgDmg, StatNames.DamageFormat, (r, t) => PerSoldier(r.DamageDealt(t), r.Fielded(t))));
                list.Add(Sort(StatNames.AvgTaken, StatNames.DamageFormat, (r, t) => PerSoldier(r.DamageTaken(t), r.Fielded(t))));
                __result = list;
            }
            catch (Exception e)
            {
                Log.Error("EncyclopediaPatch.Postfix", e);
            }
        }

        private static EncyclopediaSortController Sort(string name, string format, Func<TroopRecords, CharacterObject, double> value) =>
            new EncyclopediaSortController(new TextObject(name), new RecordComparer(format, value));

        /// <summary>Per 100 soldiers fielded, so typical values read as 10.5 rather than 0.105.</summary>
        internal static double PerHundred(int count, int fielded) => fielded == 0 ? 0 : 100.0 * count / fielded;

        /// <summary>Per soldier fielded, for damage, which is already in the tens.</summary>
        internal static double PerSoldier(int amount, int fielded) => fielded == 0 ? 0 : (double)amount / fielded;

        /// <summary>Kills per defeat; a troop that was never defeated counts each kill as 1.</summary>
        internal static double Ratio(int kills, int defeats) => defeats == 0 ? kills : (double)kills / defeats;

        private sealed class RecordComparer : DefaultEncyclopediaUnitPage.EncyclopediaListUnitComparer
        {
            private readonly string _format;
            private readonly Func<TroopRecords, CharacterObject, double> _value;
            private readonly Func<CharacterObject, CharacterObject, int> _comparison;

            public RecordComparer(string format, Func<TroopRecords, CharacterObject, double> value)
            {
                _format = format;
                _value = value;
                _comparison = (a, b) => Value(a).CompareTo(Value(b));
            }

            private static bool HasRecord(object item) =>
                item is CharacterObject troop && TroopRecords.Current != null && TroopRecords.Current.Fielded(troop) > 0;

            private double Value(CharacterObject troop) => TroopRecords.Current == null ? 0 : _value(TroopRecords.Current, troop);

            public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
            {
                bool hasX = HasRecord(x.Object);
                if (hasX != HasRecord(y.Object))
                {
                    return hasX ? -1 : 1;
                }

                return CompareUnits(x, y, _comparison);
            }

            public override string GetComparedValueText(EncyclopediaListItem item) =>
                HasRecord(item.Object) ? Value((CharacterObject)item.Object).ToString(_format) : _emptyValue.ToString();
        }
    }
}
