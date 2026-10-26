using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using TaleWorlds.Localization;

namespace EncyclopediaBattleRecords
{
    /// <summary>
    /// Adds the troop page's sorts that apply to a hero to the Sort By list of the encyclopedia's hero page: Battles, Kills (total,
    /// as Kill Counter had), Avg Kills, Avg Defeats, Avg Routs (per 100 battles), K/D Ratio, and Avg Dmg and Avg Taken (per
    /// battle). A hero's battles play the part of a troop's soldiers fielded, so the same formulas apply. Avg Deaths is left out:
    /// a hero dies once.
    /// Every hero is sorted by value, including ones whose details the player doesn't know (the game hides those in its own hero
    /// sorts). A hero with no battles shows "-" and stays at the bottom in either sort order.
    /// </summary>
    internal static class HeroSortPatch
    {
        public static MethodInfo Target => AccessTools.Method(typeof(DefaultEncyclopediaHeroPage), "InitializeSortControllers");

        public static void Apply(Harmony harmony) =>
            harmony.Patch(Target, postfix: new HarmonyMethod(typeof(HeroSortPatch), nameof(Postfix)));

        private static void Postfix(ref IEnumerable<EncyclopediaSortController> __result)
        {
            try
            {
                var list = __result as List<EncyclopediaSortController> ?? new List<EncyclopediaSortController>(__result ?? Enumerable.Empty<EncyclopediaSortController>());
                list.Add(Sort(StatNames.Battles, StatNames.CountFormat, (r, c) => r.Fielded(c)));
                list.Add(Sort(StatNames.Kills, StatNames.CountFormat, (r, c) => r.Kills(c)));
                list.Add(Sort(StatNames.AvgKills, StatNames.PerHundredFormat, (r, c) => EncyclopediaPatch.PerHundred(r.Kills(c), r.Fielded(c))));
                list.Add(Sort(StatNames.AvgDefeats, StatNames.PerHundredFormat, (r, c) => EncyclopediaPatch.PerHundred(r.Defeats(c), r.Fielded(c))));
                list.Add(Sort(StatNames.AvgRouts, StatNames.PerHundredFormat, (r, c) => EncyclopediaPatch.PerHundred(r.Routed(c), r.Fielded(c))));
                list.Add(Sort(StatNames.KdRatio, StatNames.RatioFormat, (r, c) => EncyclopediaPatch.Ratio(r.Kills(c), r.Defeats(c))));
                list.Add(Sort(StatNames.AvgDmg, StatNames.DamageFormat, (r, c) => EncyclopediaPatch.PerSoldier(r.DamageDealt(c), r.Fielded(c))));
                list.Add(Sort(StatNames.AvgTaken, StatNames.DamageFormat, (r, c) => EncyclopediaPatch.PerSoldier(r.DamageTaken(c), r.Fielded(c))));
                __result = list;
            }
            catch (Exception e)
            {
                Log.Error("HeroSortPatch.Postfix", e);
            }
        }

        private static EncyclopediaSortController Sort(string name, string format, Func<BattleRecords, CharacterObject, double> value) =>
            new EncyclopediaSortController(new TextObject(name), new RecordComparer(format, value));

        private sealed class RecordComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
        {
            private readonly string _format;
            private readonly Func<BattleRecords, CharacterObject, double> _value;

            public RecordComparer(string format, Func<BattleRecords, CharacterObject, double> value)
            {
                _format = format;
                _value = value;
            }

            private static bool HasRecord(object item) =>
                item is Hero hero && BattleRecords.Current != null && BattleRecords.Current.Fielded(hero.CharacterObject) > 0;

            private double Value(Hero hero) => BattleRecords.Current == null ? 0 : _value(BattleRecords.Current, hero.CharacterObject);

            /// <summary>Not CompareHeroes, which sorts heroes the player doesn't know last.</summary>
            public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
            {
                bool hasX = HasRecord(x.Object);
                if (hasX != HasRecord(y.Object))
                {
                    return hasX ? -1 : 1;
                }

                if (!hasX || x.Object is not Hero a || y.Object is not Hero b)
                {
                    return ResolveEquality(x, y);
                }

                int result = Value(a).CompareTo(Value(b)) * (IsAscending ? 1 : -1);
                return result != 0 ? result : ResolveEquality(x, y);
            }

            public override string GetComparedValueText(EncyclopediaListItem item) =>
                HasRecord(item.Object) ? Value((Hero)item.Object).ToString(_format) : _emptyValue.ToString();
        }
    }
}
