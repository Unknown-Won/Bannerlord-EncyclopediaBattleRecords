# Encyclopedia Battle Records: Nexus Mods page draft

Paste each part into the matching field when you add the mod on Nexus Mods (Upload > Add a mod > Mount & Blade II: Bannerlord).

## Basic info

- **Mod name:** Encyclopedia Battle Records
- **Category:** User Interface
- **Version:** 1.0.0
- **Language:** English
- **Created by:** joemamawon
- **Requirements (add on the Requirements tab):** Harmony, UIExtenderEx.

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
Encyclopedia Battle Records is open source under [url=https://creativecommons.org/licenses/by-nc-sa/4.0/]CC BY-NC-SA 4.0[/url]. Feel free to use, edit and build on it, including in your own mods and modpacks, as long as you credit joemamawon and share your changes under the same license. [b]Paid use is not allowed:[/b] no selling it or anything built from it, and no putting it behind a paywall, Patreon tier or early access. Link to this page instead of re-uploading it unchanged.

[size=4][b]Source[/b][/size]
[url=https://github.com/Unknown-Won/Bannerlord-EncyclopediaBattleRecords]Encyclopedia Battle Records on GitHub[/url]
```

## Permissions and credits tab

These match the CC BY-NC-SA 4.0 license in the repo: reuse with credit and under the same license, nothing paid.

- **Upload permission:** You are not allowed to re-upload this file unchanged to other sites. Link to this page instead.
- **Modification permission:** You are allowed to modify my files and release bug fixes or improve on the features, as long as you credit me.
- **Conversion permission:** You can convert this file to work with other games as long as you credit me.
- **Asset use permission:** You are allowed to use the assets and code in this file in your own mods and modpacks without asking, as long as you credit me.
- **Asset use in mods/files that are being sold:** You are not allowed to use assets from this file in any mods/files that are being sold, for money, on Steam Workshop or other platforms.
- **Asset use in mods/files that earn donation points:** You are not allowed to earn Donation Points for your mods if they use my assets.
- **Author notes:** "Open source under CC BY-NC-SA 4.0: use, edit and build on Encyclopedia Battle Records with credit to joemamawon, and share your changes under the same license. No paid use: no selling, paywalls, Patreon tiers or early access. Source: https://github.com/Unknown-Won/Bannerlord-EncyclopediaBattleRecords"
- **Credits:** add Kill Counter (mod 3793) by LesserScholar as a mod this one is based on.

## Images

Hosted on postimages.org (gallery: https://postimg.cc/gallery/MMcWfb1/795a68e3) and already placed in the description:
1. Troop page with the Battle Record section: https://i.postimg.cc/15R16tSJ/1.png (under "A Battle Record on every troop and hero page").
2. Hero page with the Battle Record section: https://i.postimg.cc/yY6C9dKM/2.png (after the troop page image).
3. Troop list sorted by a battle record: https://i.postimg.cc/NfGvTMYt/3.png (under "Sort troops and heroes by their record").
4. Hero list sorted by a battle record: https://i.postimg.cc/Wbpcr3jP/4.png (after the troop list image).

Upload the same four to the mod's Images tab too, in this order, so the first becomes the thumbnail.

## Building and uploading the release (by hand)

1. Build `src\EncyclopediaBattleRecords.csproj` as **Stable_Release | x64**. It writes `dist\EncyclopediaBattleRecords-1.0.0.7z` (and `dist\EncyclopediaBattleRecords`); it doesn't touch your MO2 folder.
2. Check the .7z holds an `EncyclopediaBattleRecords` folder with `SubModule.xml` and `bin\Win64_Shipping_Client\EncyclopediaBattleRecords.dll`.
3. Test the .7z once: install it through MO2 (or extract into a clean Modules folder), start the game and load a campaign.
4. On Nexus Mods, go to **Upload > Add a mod**, fill in the fields above, and upload the .7z on the Files tab as the Main file, version 1.0.0.
5. Add the images, set permissions, then publish.
