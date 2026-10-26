# Encyclopedia Battle Records: Nexus Mods page draft

Paste each part into the matching field when you add the mod on Nexus Mods (Upload > Add a mod > Mount & Blade II: Bannerlord).

## Basic info

- **Mod name:** Encyclopedia Battle Records
- **Category:** User Interface
- **Version:** 1.0.0
- **Language:** English
- **Created by:** joemamawon
- **Requirements:** see the Requirements part below.

## Summary (short description, max 350 characters)

```
An expanded take on Kill Counter: tracks kills, deaths, wounds, routs and damage for every troop type and hero in your campaign. Adds new Sort By options to the encyclopedia's troop and hero lists, and a Battle Record section to every troop and hero page. Find out which troops really pull their weight.
```

## Description (Nexus uses BBCode)

```
[center][size=5][b]Encyclopedia Battle Records[/b][/size]
Find out which troops really pull their weight[/center]

Requires [url=https://www.nexusmods.com/mountandblade2bannerlord/mods/2006]Harmony[/url] and [url=https://www.nexusmods.com/mountandblade2bannerlord/mods/2102]UIExtenderEx[/url] | War Sails not required | Safe to add or remove mid-campaign | Changes no gameplay: it only records battles and shows the results

Ever wondered whether your Legionaries actually earn their wages, or which lord in your kingdom has the most kills? Encyclopedia Battle Records keeps a battle record for every troop type and every hero in your campaign, and puts it in the encyclopedia. It expands on [url=https://www.nexusmods.com/mountandblade2bannerlord/mods/3793]Kill Counter[/url] by LesserScholar with deaths, wounds, routs, damage and averages, for heroes as well as troops.

[size=4][b]Sort troops and heroes by their record[/b][/size]
The Sort By list on the encyclopedia's troop list gets 9 new options: Fielded, Kills, Avg Kills, Avg Deaths, Avg Defeats, Avg Routs, K/D Ratio, Avg Dmg and Avg Taken. The hero list gets the same, with Battles instead of Fielded and no Avg Deaths. Averages are per 100 soldiers fielded (per 100 battles for a hero), and damage is per soldier (per battle).
[img]https://i.postimg.cc/NfGvTMYt/3.png[/img]

[img]https://i.postimg.cc/Wbpcr3jP/4.png[/img]

[size=4][b]A Battle Record on every troop and hero page[/b][/size]
Each troop page shows a Battle Record section above the upgrade tree, and each hero page shows one under the hero's info, in three groups:
[list]
[*][b]Totals:[/b] soldiers fielded (battles for a hero), kills, deaths, defeats, wounded, routed, damage dealt and damage taken.
[*][b]Kills by Target:[/b] troops, heroes, bandits and mercenaries.
[*][b]Averages:[/b] the same averages and K/D ratio as the Sort By list.
[/list]
[img]https://i.postimg.cc/15R16tSJ/1.png[/img]

[img]https://i.postimg.cc/yY6C9dKM/2.png[/img]

[size=4][b]How it counts[/b][/size]
[list]
[*]Every campaign battle, fought or auto-resolved, including battles you're not part of. Tournaments, arenas and brawls don't count.
[*][b]Fielded:[/b] each soldier sent into a battle, once per battle. In battles you fight, reserves that never take the field don't count.
[*][b]Kill:[/b] a blow that takes an enemy out, killed or knocked out (the battle scoreboard's rule). Team kills don't count.
[*][b]Defeat:[/b] killed, wounded or routed. A retreat order isn't a rout. Comparing Avg Deaths with Avg Defeats shows which troops go down without dying, often a sign of better armor.
[*][b]K/D Ratio:[/b] kills per defeat.
[*][b]Damage:[/b] every hit on a person (not horses), team hits excluded. In auto-resolve it's the simulation's damage number.
[*]Troops and heroes are tracked separately, so a lord's kills never count toward a troop type. Anyone with no battles yet shows "-" and stays at the bottom of a sort.
[/list]

[size=4][b]Manual Installation[/b][/size]
[list=1]
[*]Install [url=https://www.nexusmods.com/mountandblade2bannerlord/mods/2006]Harmony[/url] and [url=https://www.nexusmods.com/mountandblade2bannerlord/mods/2102]UIExtenderEx[/url] if you don't have them.
[*]Extract the EncyclopediaBattleRecords folder from the download into Mount & Blade II Bannerlord\Modules (or install with your mod manager).
[*]If Windows blocked the download, unblock the DLL: right-click EncyclopediaBattleRecords\bin\Win64_Shipping_Client\EncyclopediaBattleRecords.dll, choose Properties and tick Unblock.
[*]Enable Encyclopedia Battle Records in the launcher, below Harmony, UIExtenderEx and the game's own modules.
[/list]

[size=4][b]Does this work with game version x.x.x?[/b][/size]
[b]Likely, unless the update reworks battles or the encyclopedia.[/b] The mod hooks a few small game methods (how troops enter a battle and how hits, deaths, wounds and routs are recorded) and adds sections to the encyclopedia pages. Those have been stable across recent versions. If an update does change one, that part turns itself off and writes a note to the log (see Troubleshooting) instead of crashing; the rest keeps working.

[size=4][b]FAQ[/b][/size]
[list]
[*][b]Can I add it mid-campaign?[/b] Yes. Records start from the moment you add it.
[*][b]Can I remove it mid-campaign?[/b] Yes. Each save only holds a few simple number tables from this mod, which the game ignores once it's gone.
[*][b]Does it work with total conversion mods?[/b] It should: it records whatever troops and heroes the game has.
[*][b]If I switch from Kill Counter, do its kill counts show up in this mod's records?[/b] No. Kill Counter stores its counts in its own format, so this mod's records start from zero.
[/list]

[size=4][b]Compatibility[/b][/size]
[list]
[*][url=https://www.nexusmods.com/mountandblade2bannerlord/mods/13792]Encyclopedia Filters Extended[/url] - Compatible - adds filters and list columns; both add Sort By options and work together.
[*][url=https://www.nexusmods.com/mountandblade2bannerlord/mods/13623]Encyclopedia Extender Reforged[/url] - Compatible - adds its own hero page sections alongside the Battle Record section.
[*][url=https://www.nexusmods.com/mountandblade2bannerlord/mods/3793]Kill Counter[/url] - Partially compatible - with both installed, both show kill counts and a Kills sort, and their numbers differ because Kill Counter also counts tournaments, arenas and team kills. Nothing breaks, but it's redundant: disable Kill Counter.
[/list]

[size=4][b]Recommended[/b][/size]
[list]
[*][url=https://www.nexusmods.com/mountandblade2bannerlord/mods/13792]Encyclopedia Filters Extended[/url]: narrow the troop or hero list first (by culture, type and more), then sort it by a battle record.
[/list]

[size=4][b]Troubleshooting and bug reports[/b][/size]
[list]
[*][b]No Battle Record section on troop or hero pages:[/b] UIExtenderEx is missing, disabled or loaded after this mod. Enable it above Encyclopedia Battle Records in the launcher.
[*][b]Every troop shows "-":[/b] no battles have been recorded since you added the mod. Records build up as battles happen anywhere on the map, usually within a few in-game days.
[*][b]"Cannot load EncyclopediaBattleRecords.dll":[/b] Windows blocked the download. Unblock the DLL (see Manual Installation).
[/list]
For anything else, post a comment or bug report with your game version, your mod list, the crash report if there was one, and EncyclopediaBattleRecords.log from Documents\Mount and Blade II Bannerlord\Configs\ModLogs.

[size=4][b]Credits[/b][/size]
[list]
[*][b]LesserScholar[/b], for [url=https://www.nexusmods.com/mountandblade2bannerlord/mods/3793]Kill Counter[/url] ([url=https://github.com/LesserScholar/KillCounters]source on GitHub[/url]). This mod takes its ideas (per-troop kill counts, kills by target, the encyclopedia sort and stats sections) and expands on them.
[*]The authors of [url=https://www.nexusmods.com/mountandblade2bannerlord/mods/2006]Harmony[/url] and [url=https://www.nexusmods.com/mountandblade2bannerlord/mods/2102]UIExtenderEx[/url].
[/list]

[size=4][b]Permissions[/b][/size]
Encyclopedia Battle Records is open source under [url=https://creativecommons.org/licenses/by-nc-sa/4.0/]CC BY-NC-SA 4.0[/url]. Feel free to use, edit and build on it, including in your own mods and modpacks, as long as you credit joemamawon and share your changes under the same license. [b]Paid use is not allowed:[/b] no selling it or anything built from it, and no putting it behind a paywall, Patreon tier or early access. If you share it, please link to this page rather than re-uploading it unchanged.

[size=4][b]Source[/b][/size]
[url=https://github.com/Unknown-Won/Bannerlord-EncyclopediaBattleRecords]Encyclopedia Battle Records on GitHub[/url]
```

## Images

### Header (Media step, optional)

None. A 1300×372 banner can be added later if you want one.

### Gallery (Media step, Images, required)

Upload the screenshot files directly. Suggested titles, in gallery order:
1. Hero page: "Battle Record on a hero page"
2. Troop page: "Battle Record on a troop page" (mark as thumbnail)
3. Troop list sorted by K/D Ratio: "Troops sorted by K/D Ratio"
4. Hero list sorted by Avg Kills: "Heroes sorted by Avg Kills"

### Description images (already placed)

Hosted on postimages.org (gallery: https://postimg.cc/gallery/MMcWfb1/795a68e3), full-size links:
- Under "Sort troops and heroes by their record": https://i.postimg.cc/NfGvTMYt/3.png (troop list), then https://i.postimg.cc/Wbpcr3jP/4.png (hero list).
- Under "A Battle Record on every troop and hero page": https://i.postimg.cc/15R16tSJ/1.png (troop page), then https://i.postimg.cc/yY6C9dKM/2.png (hero page).

## Files

One file, `EncyclopediaBattleRecords-1.0.0.7z` (Claude sends it):
- **File category:** Main
- **Display name:** Encyclopedia Battle Records
- **File version:** 1.0.0, with "Update mod version to match this file's version" ticked
- **Description:**
  ```
  Requires Harmony and UIExtenderEx. No DLC required. Safe to add or remove mid-campaign.
  ```
- **Downloads:** allow mod manager download, and set as primary file for download.
- **Changelog:**
  ```
  Initial release.
  Battle records (kills, deaths, wounds, routs, damage) for every troop type and hero, from all campaign battles.
  New Sort By options on the encyclopedia's troop and hero lists, and a Battle Record section on every troop and hero page.
  ```

## Requirements

- **Method:** Mod requirements (legacy), so each requirement can carry a load order note on the Description tab.
- **Nexus Mods:**
  - Harmony, note: `Load above Encyclopedia Battle Records.`
  - UIExtenderEx, note: `Load above Encyclopedia Battle Records. Needed for the Battle Record sections on troop and hero pages.`
- **External resources:** none.
- **DLC:** none. War Sails stays unticked; it isn't required.

## Permissions

Matches the CC BY-NC-SA 4.0 license in the repo: reuse with credit and under the same license, nothing paid.
- **Third-party content:** No (all content is mine). Kill Counter only inspired the ideas; none of its code is included.
- **Credits:**
  ```
  Based on ideas from Kill Counter by LesserScholar (https://www.nexusmods.com/mountandblade2bannerlord/mods/3793, source: https://github.com/LesserScholar/KillCounters): per-troop kill counts, kills by target, and the encyclopedia sort and stats sections. No code from it is included.
  ```
- **Permission settings:** Pick your own
  - Re-upload: Allowed (Credit required)
  - Conversion (other games): Allowed (Credit required)
  - Modification: Allowed (Credit required)
  - Asset use: Allowed (Credit required)
  - Earning donation points (using assets): Not allowed
  - Monetisation (third-party): Not allowed
- **Additional instructions:** tick "I would like to write additional instructions":
  ```
  Licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/): credit joemamawon and share your changes under the same license. No paid use: no selling it or anything built from it, and no paywalls, Patreon tiers or early access. Source: https://github.com/Unknown-Won/Bannerlord-EncyclopediaBattleRecords
  ```

## Uploading

Claude builds the release, checks it and sends you `EncyclopediaBattleRecords-1.0.0.7z` (from `dist`). It holds one `EncyclopediaBattleRecords` folder with `SubModule.xml` and `bin\Win64_Shipping_Client\EncyclopediaBattleRecords.dll`. To build it yourself, build `src\EncyclopediaBattleRecords.csproj` as **Stable_Release | x64**.

On Nexus Mods, **Upload > Add a mod**, then the form's steps:
1. **General:** the basic info, summary and description above.
2. **Media:** the header (if any) and gallery images listed under Images.
3. **Files:** upload `EncyclopediaBattleRecords-1.0.0.7z` and fill in its fields as listed under Files.
4. **Requirements:** as listed under Requirements.
5. **Permissions:** as listed under Permissions, then publish.
