using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace TroopKDR
{
    /// <summary>
    /// Battle record per character, keyed by CharacterObject.StringId: one entry per troop type, and one per hero (each hero has
    /// its own CharacterObject). The troop page sorts only list troop types, so heroes never mix into troop numbers.
    /// Saved with the campaign as Dictionary&lt;string, int&gt;, a type the save system already knows, so every save keeps its own
    /// record, the mod adds no saveable types and it can be removed from a save at any time.
    /// </summary>
    internal sealed class TroopRecords : CampaignBehaviorBase
    {
        /// <summary>What a kill was against, for the kill breakdown on the encyclopedia pages.</summary>
        public enum VictimKind
        {
            Troop,
            Hero,
            Bandit,
            Mercenary,
        }

        /// <summary>The running campaign's records, or null outside a campaign.</summary>
        public static TroopRecords Current { get; private set; }

        private Dictionary<string, int> _fielded = new Dictionary<string, int>();
        private Dictionary<string, int> _kills = new Dictionary<string, int>();
        private Dictionary<string, int> _killsOfHeroes = new Dictionary<string, int>();
        private Dictionary<string, int> _killsOfBandits = new Dictionary<string, int>();
        private Dictionary<string, int> _killsOfMercenaries = new Dictionary<string, int>();
        private Dictionary<string, int> _deaths = new Dictionary<string, int>();
        private Dictionary<string, int> _wounded = new Dictionary<string, int>();
        private Dictionary<string, int> _routed = new Dictionary<string, int>();
        private Dictionary<string, int> _damageDealt = new Dictionary<string, int>();
        private Dictionary<string, int> _damageTaken = new Dictionary<string, int>();

        public TroopRecords()
        {
            Current = this;
        }

        public static void Clear(TroopRecords records)
        {
            if (Current == records)
            {
                Current = null;
            }
        }

        public override void RegisterEvents()
        {
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("TroopKDR_fielded", ref _fielded);
            dataStore.SyncData("TroopKDR_kills", ref _kills);
            dataStore.SyncData("TroopKDR_killsOfHeroes", ref _killsOfHeroes);
            dataStore.SyncData("TroopKDR_killsOfBandits", ref _killsOfBandits);
            dataStore.SyncData("TroopKDR_killsOfMercenaries", ref _killsOfMercenaries);
            dataStore.SyncData("TroopKDR_deaths", ref _deaths);
            dataStore.SyncData("TroopKDR_wounded", ref _wounded);
            dataStore.SyncData("TroopKDR_routed", ref _routed);
            dataStore.SyncData("TroopKDR_damageDealt", ref _damageDealt);
            dataStore.SyncData("TroopKDR_damageTaken", ref _damageTaken);

            // A save made before the mod (or a field) was added has no entries; keep empty dictionaries rather than null.
            _fielded ??= new Dictionary<string, int>();
            _kills ??= new Dictionary<string, int>();
            _killsOfHeroes ??= new Dictionary<string, int>();
            _killsOfBandits ??= new Dictionary<string, int>();
            _killsOfMercenaries ??= new Dictionary<string, int>();
            _deaths ??= new Dictionary<string, int>();
            _wounded ??= new Dictionary<string, int>();
            _routed ??= new Dictionary<string, int>();
            _damageDealt ??= new Dictionary<string, int>();
            _damageTaken ??= new Dictionary<string, int>();
        }

        public void AddFielded(CharacterObject troop) => Increment(_fielded, troop);

        /// <summary>A kill, also counted by what it was against. Troop kills are the total minus the other three.</summary>
        public void AddKill(CharacterObject attacker, CharacterObject victim)
        {
            Increment(_kills, attacker);
            switch (KindOf(victim))
            {
                case VictimKind.Hero:
                    Increment(_killsOfHeroes, attacker);
                    break;
                case VictimKind.Bandit:
                    Increment(_killsOfBandits, attacker);
                    break;
                case VictimKind.Mercenary:
                    Increment(_killsOfMercenaries, attacker);
                    break;
            }
        }

        public void AddDeath(CharacterObject troop) => Increment(_deaths, troop);

        public void AddWounded(CharacterObject troop) => Increment(_wounded, troop);

        public void AddRouted(CharacterObject troop) => Increment(_routed, troop);

        public void AddDamage(CharacterObject attacker, CharacterObject victim, int damage)
        {
            Increment(_damageDealt, attacker, damage);
            Increment(_damageTaken, victim, damage);
        }

        /// <summary>Soldiers of this type sent into a battle, each counted once per battle; for a hero, battles joined.</summary>
        public int Fielded(CharacterObject troop) => Get(_fielded, troop);

        public int Kills(CharacterObject troop) => Get(_kills, troop);

        public int Kills(CharacterObject troop, VictimKind kind)
        {
            switch (kind)
            {
                case VictimKind.Hero:
                    return Get(_killsOfHeroes, troop);
                case VictimKind.Bandit:
                    return Get(_killsOfBandits, troop);
                case VictimKind.Mercenary:
                    return Get(_killsOfMercenaries, troop);
                default:
                    return Get(_kills, troop) - Get(_killsOfHeroes, troop) - Get(_killsOfBandits, troop) - Get(_killsOfMercenaries, troop);
            }
        }

        public int Deaths(CharacterObject troop) => Get(_deaths, troop);

        public int Wounded(CharacterObject troop) => Get(_wounded, troop);

        public int Routed(CharacterObject troop) => Get(_routed, troop);

        /// <summary>Taken out of a battle in any way: killed, wounded or routed.</summary>
        public int Defeats(CharacterObject troop) => Get(_deaths, troop) + Get(_wounded, troop) + Get(_routed, troop);

        public int DamageDealt(CharacterObject troop) => Get(_damageDealt, troop);

        public int DamageTaken(CharacterObject troop) => Get(_damageTaken, troop);

        /// <summary>Kill Counter's categories: any hero, then bandits and mercenaries by occupation, everything else a troop.</summary>
        private static VictimKind KindOf(CharacterObject victim)
        {
            if (victim == null)
            {
                return VictimKind.Troop;
            }

            if (victim.IsHero)
            {
                return VictimKind.Hero;
            }

            switch (victim.Occupation)
            {
                case Occupation.Bandit:
                    return VictimKind.Bandit;
                case Occupation.Mercenary:
                    return VictimKind.Mercenary;
                default:
                    return VictimKind.Troop;
            }
        }

        /// <summary>Adds to a character's count, stopping at int.MaxValue rather than wrapping (damage totals grow fastest).</summary>
        private static void Increment(Dictionary<string, int> map, CharacterObject troop, int amount = 1)
        {
            if (troop == null || amount <= 0)
            {
                return;
            }

            map.TryGetValue(troop.StringId, out int count);
            map[troop.StringId] = count > int.MaxValue - amount ? int.MaxValue : count + amount;
        }

        private static int Get(Dictionary<string, int> map, CharacterObject troop) =>
            troop != null && map.TryGetValue(troop.StringId, out int count) ? count : 0;
    }
}
