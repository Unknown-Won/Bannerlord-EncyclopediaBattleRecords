using System;

using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;
using Bannerlord.UIExtenderEx.ViewModels;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;

using static TroopKDR.StatNames;

namespace TroopKDR
{
    /// <summary>
    /// The Battle Record section on the encyclopedia's troop and hero pages (UIExtenderEx): Totals, Kills by Target and Averages,
    /// with the same names as the Sort By lists. The prefab snippets are inline, so no GUI file can clash with another mod's of
    /// the same name. Data-source names carry a TroopKDR prefix for the same reason.
    /// </summary>
    internal sealed class RecordPanel
    {
        public string Title => Text(StatNames.Title);

        public string TotalsTitle => Text(StatNames.TotalsTitle);

        public string TargetsTitle => Text(StatNames.TargetsTitle);

        public string AveragesTitle { get; private set; } = "";

        public MBBindingList<StringPairItemVM> Totals { get; } = new MBBindingList<StringPairItemVM>();

        public MBBindingList<StringPairItemVM> Targets { get; } = new MBBindingList<StringPairItemVM>();

        public MBBindingList<StringPairItemVM> Averages { get; } = new MBBindingList<StringPairItemVM>();

        /// <summary>A troop type: soldiers fielded, and deaths, which a hero only has once.</summary>
        public void FillTroop(CharacterObject troop)
        {
            AveragesTitle = Text(TroopAveragesTitle);
            Fill(troop, Fielded, includeDeaths: true);
        }

        /// <summary>A hero, shown even when the player doesn't know the hero's details, as Kill Counter did.</summary>
        public void FillHero(Hero hero)
        {
            AveragesTitle = Text(HeroAveragesTitle);
            Fill(hero?.CharacterObject, Battles, includeDeaths: false);
        }

        private void Fill(CharacterObject c, string fieldedName, bool includeDeaths)
        {
            Totals.Clear();
            Targets.Clear();
            Averages.Clear();
            var r = TroopRecords.Current;
            if (r == null || c == null)
            {
                return;
            }

            int fielded = r.Fielded(c);
            Add(Totals, fieldedName, fielded.ToString(CountFormat));
            Add(Totals, Kills, r.Kills(c).ToString(CountFormat));
            if (includeDeaths)
            {
                Add(Totals, Deaths, r.Deaths(c).ToString(CountFormat));
            }

            Add(Totals, Defeats, r.Defeats(c).ToString(CountFormat));
            Add(Totals, Wounded, r.Wounded(c).ToString(CountFormat));
            Add(Totals, Routed, r.Routed(c).ToString(CountFormat));
            Add(Totals, DmgDealt, r.DamageDealt(c).ToString(CountFormat));
            Add(Totals, DmgTaken, r.DamageTaken(c).ToString(CountFormat));

            Add(Targets, TroopKills, r.Kills(c, TroopRecords.VictimKind.Troop).ToString(CountFormat));
            Add(Targets, HeroKills, r.Kills(c, TroopRecords.VictimKind.Hero).ToString(CountFormat));
            Add(Targets, BanditKills, r.Kills(c, TroopRecords.VictimKind.Bandit).ToString(CountFormat));
            Add(Targets, MercKills, r.Kills(c, TroopRecords.VictimKind.Mercenary).ToString(CountFormat));

            // Same values and formats as the Sort By entries; "-" until there's a battle to average over.
            string Avg(Func<double> value, string format) => fielded == 0 ? "-" : value().ToString(format);
            Add(Averages, AvgKills, Avg(() => EncyclopediaPatch.PerHundred(r.Kills(c), fielded), PerHundredFormat));
            if (includeDeaths)
            {
                Add(Averages, AvgDeaths, Avg(() => EncyclopediaPatch.PerHundred(r.Deaths(c), fielded), PerHundredFormat));
            }

            Add(Averages, AvgDefeats, Avg(() => EncyclopediaPatch.PerHundred(r.Defeats(c), fielded), PerHundredFormat));
            Add(Averages, AvgRouts, Avg(() => EncyclopediaPatch.PerHundred(r.Routed(c), fielded), PerHundredFormat));
            Add(Averages, KdRatio, Avg(() => EncyclopediaPatch.Ratio(r.Kills(c), r.Defeats(c)), RatioFormat));
            Add(Averages, AvgDmg, Avg(() => EncyclopediaPatch.PerSoldier(r.DamageDealt(c), fielded), DamageFormat));
            Add(Averages, AvgTaken, Avg(() => EncyclopediaPatch.PerSoldier(r.DamageTaken(c), fielded), DamageFormat));
        }

        private static void Add(MBBindingList<StringPairItemVM> rows, string name, string value) =>
            rows.Add(new StringPairItemVM(Text(name) + ":", value));

        /// <summary>
        /// The section's markup, shared by both pages. Brushes and sizes are the hero page's own stats grid; columns differ per page.
        /// </summary>
        public static string Markup(int columns, int cellWidth) => $@"
  <EncyclopediaDivider MarginTop=""20"" Parameter.Title=""@TroopKDRTitle"" Parameter.ItemList=""..\TroopKDRBody"" />
  <ListPanel Id=""TroopKDRBody"" WidthSizePolicy=""StretchToParent"" HeightSizePolicy=""CoverChildren"" MarginLeft=""15"" StackLayout.LayoutMethod=""VerticalTopToBottom"">
    <Children>
      {Group("TroopKDRTotalsTitle", "TroopKDRTotals", columns, cellWidth)}
      {Group("TroopKDRTargetsTitle", "TroopKDRTargets", columns, cellWidth)}
      {Group("TroopKDRAveragesTitle", "TroopKDRAverages", columns, cellWidth)}
    </Children>
  </ListPanel>";

        private static string Group(string title, string rows, int columns, int cellWidth) => $@"
      <TextWidget WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" MarginTop=""12"" MarginLeft=""15"" Brush=""Encyclopedia.Stat.DefinitionText"" Brush.FontSize=""22"" Text=""@{title}"" />
      <GridWidget DataSource=""{{{rows}}}"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" DefaultCellWidth=""{cellWidth}"" DefaultCellHeight=""30"" HorizontalAlignment=""Left"" ColumnCount=""{columns}"" MarginTop=""2"">
        <ItemTemplate>
          <ListPanel HeightSizePolicy=""CoverChildren"" WidthSizePolicy=""CoverChildren"" MarginLeft=""15"" MarginTop=""3"">
            <Children>
              <AutoHideRichTextWidget HeightSizePolicy=""CoverChildren"" WidthSizePolicy=""CoverChildren"" VerticalAlignment=""Bottom"" Brush=""Encyclopedia.Stat.DefinitionText"" Text=""@Definition"" MarginRight=""5"" />
              <AutoHideRichTextWidget HeightSizePolicy=""CoverChildren"" WidthSizePolicy=""CoverChildren"" VerticalAlignment=""Bottom"" Brush=""Encyclopedia.Stat.ValueText"" Text=""@Value"" />
            </Children>
          </ListPanel>
        </ItemTemplate>
      </GridWidget>";
    }

    /// <summary>
    /// No refresh method: UIExtenderEx would Harmony-patch EncyclopediaUnitPageVM.RefreshValues at module load, and that method
    /// reads CampaignUIHelper.SkillObjectComparerInstance. Compiling the patched copy runs CampaignUIHelper's static initializer
    /// early, before GameTexts exist; it throws, and every later use (the campaign map bar) crashes the game. The panel is filled
    /// once when the page opens, which is all it needs: the record can't change while the encyclopedia is open.
    /// </summary>
    [ViewModelMixin]
    public sealed class UnitPageMixin : BaseViewModelMixin<EncyclopediaUnitPageVM>
    {
        private static readonly AccessTools.FieldRef<EncyclopediaUnitPageVM, CharacterObject> CharacterField =
            AccessTools.FieldRefAccess<EncyclopediaUnitPageVM, CharacterObject>("_character");

        private readonly RecordPanel _panel = new RecordPanel();

        public UnitPageMixin(EncyclopediaUnitPageVM vm) : base(vm)
        {
            try
            {
                _panel.FillTroop(CharacterField(vm));
            }
            catch (Exception e)
            {
                Log.Error("UnitPageMixin", e);
            }
        }

        [DataSourceProperty] public string TroopKDRTitle => _panel.Title;
        [DataSourceProperty] public string TroopKDRTotalsTitle => _panel.TotalsTitle;
        [DataSourceProperty] public string TroopKDRTargetsTitle => _panel.TargetsTitle;
        [DataSourceProperty] public string TroopKDRAveragesTitle => _panel.AveragesTitle;
        [DataSourceProperty] public MBBindingList<StringPairItemVM> TroopKDRTotals => _panel.Totals;
        [DataSourceProperty] public MBBindingList<StringPairItemVM> TroopKDRTargets => _panel.Targets;
        [DataSourceProperty] public MBBindingList<StringPairItemVM> TroopKDRAverages => _panel.Averages;
    }

    /// <summary>No refresh method, for the same reason as UnitPageMixin.</summary>
    [ViewModelMixin]
    public sealed class HeroPageMixin : BaseViewModelMixin<EncyclopediaHeroPageVM>
    {
        private static readonly AccessTools.FieldRef<EncyclopediaHeroPageVM, Hero> HeroField =
            AccessTools.FieldRefAccess<EncyclopediaHeroPageVM, Hero>("_hero");

        private readonly RecordPanel _panel = new RecordPanel();

        public HeroPageMixin(EncyclopediaHeroPageVM vm) : base(vm)
        {
            try
            {
                _panel.FillHero(HeroField(vm));
            }
            catch (Exception e)
            {
                Log.Error("HeroPageMixin", e);
            }
        }

        [DataSourceProperty] public string TroopKDRTitle => _panel.Title;
        [DataSourceProperty] public string TroopKDRTotalsTitle => _panel.TotalsTitle;
        [DataSourceProperty] public string TroopKDRTargetsTitle => _panel.TargetsTitle;
        [DataSourceProperty] public string TroopKDRAveragesTitle => _panel.AveragesTitle;
        [DataSourceProperty] public MBBindingList<StringPairItemVM> TroopKDRTotals => _panel.Totals;
        [DataSourceProperty] public MBBindingList<StringPairItemVM> TroopKDRTargets => _panel.Targets;
        [DataSourceProperty] public MBBindingList<StringPairItemVM> TroopKDRAverages => _panel.Averages;
    }

    /// <summary>
    /// Troop page, step 1: the right side's scroll content (a Widget holding the upgrade tree) becomes a vertical ListPanel with
    /// the same Id, so the Battle Record section can sit above the tree and scroll with it. The tree container is kept as is.
    /// </summary>
    [PrefabExtension("EncyclopediaUnitPage", XPath)]
    public sealed class UnitPageStackPrefab : PrefabExtensionInsertPatch
    {
        public const string XPath = "descendant::Widget[@Id='InnerPanel']";

        public override InsertType Type => InsertType.ReplaceKeepChildren;

        [PrefabExtensionText(true)]
        public string Text => @"<DummyRoot>
  <ListPanel Id=""InnerPanel"" HeightSizePolicy=""CoverChildren"" WidthSizePolicy=""CoverChildren"" HorizontalAlignment=""Center"" IsHidden=""@HasErrors"" StackLayout.LayoutMethod=""VerticalTopToBottom"" />
</DummyRoot>";
    }

    /// <summary>Troop page, step 2: the Battle Record section, above the upgrade tree, at a fixed width (four columns).</summary>
    [PrefabExtension("EncyclopediaUnitPage", XPath)]
    public sealed class UnitPagePrefab : PrefabExtensionInsertPatch
    {
        public const string XPath = "descendant::Widget[@DataSource='{Tree}']";

        public override InsertType Type => InsertType.Prepend;

        [PrefabExtensionText(true)]
        public string Text => @"<DummyRoot>
  <ListPanel WidthSizePolicy=""Fixed"" SuggestedWidth=""980"" HeightSizePolicy=""CoverChildren"" HorizontalAlignment=""Center"" MarginLeft=""20"" MarginRight=""20"" StackLayout.LayoutMethod=""VerticalTopToBottom"">
    <Children>" + RecordPanel.Markup(4, 235) + @"
    </Children>
  </ListPanel>
</DummyRoot>";
    }

    /// <summary>Hero page: the Battle Record section after the hero's info stats, two columns like the game's own stats grid.</summary>
    [PrefabExtension("EncyclopediaHeroPage", XPath)]
    public sealed class HeroPagePrefab : PrefabExtensionInsertPatch
    {
        public const string XPath = "descendant::Widget[@Id='InfoContainer']";

        public override InsertType Type => InsertType.Append;

        [PrefabExtensionText(true)]
        public string Text => @"<DummyRoot>" + RecordPanel.Markup(2, 275) + @"
</DummyRoot>";
    }
}
