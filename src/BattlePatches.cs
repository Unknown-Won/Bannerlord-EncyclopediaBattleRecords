using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;

namespace TroopKDR
{
    /// <summary>
    /// Counts the battle record from the methods every campaign battle reports through, fought (via PartyGroupAgentOrigin and
    /// PartyGroupTroopSupplier) or simulated (MapEvent.SimulateSingleTroopHit). Tournaments, arenas and brawls have no map event,
    /// so they never reach these.
    /// - Fielded: MapEventSide.AllocateTroops/AllocateTroop hand a troop to the battle. In a fought battle that's when it spawns
    ///   (reserves that never spawn aren't fielded); a simulation re-allocates every round, so each troop is counted once per
    ///   MapEventSide, which lasts for the whole battle.
    /// - Damage and kills: MapEventParty.OnTroopScoreHit, called for every hit on a character, team hits excluded. The damage is
    ///   the whole blow (it can exceed the victim's remaining health); in a simulation it's the simulated hit. isFatal means the
    ///   blow took the victim out, whether it died or was knocked out, the same rule as the battle scoreboard. Horses have no
    ///   character and are never reported.
    /// - Death, wounded, routed: MapEventParty.OnTroopKilled, OnTroopWounded, OnTroopRouted. MapEventSide only calls
    ///   OnTroopRouted for a real rout, not for a retreat order, and also for the troops still standing when a side flees.
    /// All targets are large methods, so callers can't have them inlined past the patch. Postfixes run even if another mod's
    /// prefix skips the original.
    /// </summary>
    internal static class BattlePatches
    {
        private static readonly ConditionalWeakTable<MapEventSide, HashSet<UniqueTroopDescriptor>> FieldedBySide =
            new ConditionalWeakTable<MapEventSide, HashSet<UniqueTroopDescriptor>>();

        public static MethodInfo AllocateTroopsTarget => AccessTools.Method(typeof(MapEventSide), nameof(MapEventSide.AllocateTroops));

        public static MethodInfo AllocateTroopTarget => AccessTools.Method(typeof(MapEventSide), "AllocateTroop");

        public static MethodInfo ScoreHitTarget => AccessTools.Method(typeof(MapEventParty), nameof(MapEventParty.OnTroopScoreHit));

        public static MethodInfo KilledTarget => AccessTools.Method(typeof(MapEventParty), nameof(MapEventParty.OnTroopKilled));

        public static MethodInfo WoundedTarget => AccessTools.Method(typeof(MapEventParty), nameof(MapEventParty.OnTroopWounded));

        public static MethodInfo RoutedTarget => AccessTools.Method(typeof(MapEventParty), nameof(MapEventParty.OnTroopRouted));

        public static void ApplyAllocateTroops(Harmony harmony) => Postfix(harmony, AllocateTroopsTarget, nameof(AllocateTroopsPostfix));

        public static void ApplyAllocateTroop(Harmony harmony) => Postfix(harmony, AllocateTroopTarget, nameof(AllocateTroopPostfix));

        public static void ApplyScoreHit(Harmony harmony) => Postfix(harmony, ScoreHitTarget, nameof(ScoreHitPostfix));

        public static void ApplyKilled(Harmony harmony) => Postfix(harmony, KilledTarget, nameof(KilledPostfix));

        public static void ApplyWounded(Harmony harmony) => Postfix(harmony, WoundedTarget, nameof(WoundedPostfix));

        public static void ApplyRouted(Harmony harmony) => Postfix(harmony, RoutedTarget, nameof(RoutedPostfix));

        private static void Postfix(Harmony harmony, MethodInfo target, string postfix) =>
            harmony.Patch(target, postfix: new HarmonyMethod(typeof(BattlePatches), postfix));

        private static void AllocateTroopsPostfix(MapEventSide __instance, ref List<UniqueTroopDescriptor> troopsList)
        {
            var records = TroopRecords.Current;
            if (records == null || troopsList == null || troopsList.Count == 0)
            {
                return;
            }

            try
            {
                var seen = FieldedBySide.GetValue(__instance, _ => new HashSet<UniqueTroopDescriptor>());
                foreach (var descriptor in troopsList)
                {
                    if (seen.Add(descriptor))
                    {
                        records.AddFielded(__instance.GetAllocatedTroop(descriptor));
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("AllocateTroopsPostfix", e);
            }
        }

        private static void AllocateTroopPostfix(MapEventSide __instance, bool __result, ref UniqueTroopDescriptor troopDescriptor)
        {
            var records = TroopRecords.Current;
            if (!__result || records == null)
            {
                return;
            }

            try
            {
                if (FieldedBySide.GetValue(__instance, _ => new HashSet<UniqueTroopDescriptor>()).Add(troopDescriptor))
                {
                    records.AddFielded(__instance.GetAllocatedTroop(troopDescriptor));
                }
            }
            catch (Exception e)
            {
                Log.Error("AllocateTroopPostfix", e);
            }
        }

        private static void ScoreHitPostfix(MapEventParty __instance, UniqueTroopDescriptor attackerTroopDesc, CharacterObject attackedTroop,
            int damage, bool isFatal, bool isTeamKill)
        {
            var records = TroopRecords.Current;
            if (isTeamKill || records == null)
            {
                return;
            }

            try
            {
                var attacker = __instance.Troops[attackerTroopDesc].Troop;
                records.AddDamage(attacker, attackedTroop, damage);
                if (isFatal)
                {
                    records.AddKill(attacker, attackedTroop);
                }
            }
            catch (Exception e)
            {
                Log.Error("ScoreHitPostfix", e);
            }
        }

        private static void KilledPostfix(MapEventParty __instance, UniqueTroopDescriptor troopSeed)
        {
            try
            {
                TroopRecords.Current?.AddDeath(__instance.Troops[troopSeed].Troop);
            }
            catch (Exception e)
            {
                Log.Error("KilledPostfix", e);
            }
        }

        private static void WoundedPostfix(MapEventParty __instance, UniqueTroopDescriptor troopSeed)
        {
            try
            {
                TroopRecords.Current?.AddWounded(__instance.Troops[troopSeed].Troop);
            }
            catch (Exception e)
            {
                Log.Error("WoundedPostfix", e);
            }
        }

        private static void RoutedPostfix(MapEventParty __instance, UniqueTroopDescriptor troopSeed)
        {
            try
            {
                TroopRecords.Current?.AddRouted(__instance.Troops[troopSeed].Troop);
            }
            catch (Exception e)
            {
                Log.Error("RoutedPostfix", e);
            }
        }
    }
}
