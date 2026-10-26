using System;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace TroopKDR
{
    public class SubModule : MBSubModuleBase
    {
        public const string HarmonyId = "troopkdr";

        private bool _patched;
        private TroopRecords _records;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            _patched = ApplyPatches(new Harmony(HarmonyId));
            _patched &= TryPatch("encyclopedia panels", EnablePanels);
            Log.Info(_patched ? "Troop KDR loaded." : "Troop KDR loaded, but some patches failed (see errors above).");
        }

        /// <summary>Kept apart from ApplyPatches so tools\PatchCheck doesn't need UIExtenderEx running.</summary>
        private static void EnablePanels()
        {
            var extender = Bannerlord.UIExtenderEx.UIExtender.Create("TroopKDR");
            extender.Register(typeof(SubModule).Assembly);
            extender.Enable();
        }

        /// <summary>One patch at a time, so one failure doesn't stop the rest. Also called by tools\PatchCheck.</summary>
        internal static bool ApplyPatches(Harmony harmony)
        {
            bool ok = true;
            ok &= TryPatch("allocate troops", () => BattlePatches.ApplyAllocateTroops(harmony));
            ok &= TryPatch("allocate troop", () => BattlePatches.ApplyAllocateTroop(harmony));
            ok &= TryPatch("score hit", () => BattlePatches.ApplyScoreHit(harmony));
            ok &= TryPatch("troop killed", () => BattlePatches.ApplyKilled(harmony));
            ok &= TryPatch("troop wounded", () => BattlePatches.ApplyWounded(harmony));
            ok &= TryPatch("troop routed", () => BattlePatches.ApplyRouted(harmony));
            ok &= TryPatch("encyclopedia sort", () => EncyclopediaPatch.Apply(harmony));
            ok &= TryPatch("hero sort", () => HeroSortPatch.Apply(harmony));
            return ok;
        }

        private static bool TryPatch(string name, Action apply)
        {
            try
            {
                apply();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Patch " + name, e);
                return false;
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            if (game.GameType is Campaign && gameStarterObject is CampaignGameStarter starter)
            {
                _records = new TroopRecords();
                starter.AddBehavior(_records);
            }
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            if (_records != null)
            {
                TroopRecords.Clear(_records);
                _records = null;
            }
        }
    }
}
