# Encyclopedia Battle Records

Tracks a battle record for every troop type and hero in the campaign. It covers everything [Kill Counter](https://www.nexusmods.com/mountandblade2bannerlord/mods/3793) showed, so that mod isn't needed alongside it.

## Troop page

Sort By adds:

- **Fielded**: soldiers of this type sent into battle so far. Low counts mean the other numbers are based on few soldiers.
- **Kills**: total kills.
- **Avg Kills**, **Avg Deaths**, **Avg Defeats**, **Avg Routs**: per 100 soldiers fielded. Defeats are deaths, wounded and routs together.
- **K/D Ratio**: kills / defeats. A troop never defeated shows its kill count.
- **Avg Dmg**, **Avg Taken**: damage dealt and taken per soldier fielded.

A Battle Record section above the upgrade tree shows three groups:

- **Totals**: Fielded, Kills, Deaths, Defeats, Wounded, Routed, Dmg Dealt, Dmg Taken.
- **Kills by Target**: Troop, Hero, Bandit and Merc Kills.
- **Averages**: the Sort By averages and K/D Ratio, with the unit in the heading.

## Hero page

The same, with battles in place of soldiers fielded: Sort By adds **Battles**, **Kills**, **Avg Kills**, **Avg Defeats**, **Avg Routs** (per 100 battles), **K/D Ratio**, **Avg Dmg** and **Avg Taken** (per battle). The Battle Record section under the hero's info has the same three groups. Deaths and Avg Deaths are left out, since a hero dies once (a death in battle still counts as a defeat). Both show for every hero, including ones whose details you don't know yet.

Every name is the same in the Sort By lists and the Battle Record sections. A troop or hero with no battles shows "-" and stays at the bottom of a sort in either order.

## How it counts

- Every campaign battle, fought or auto-resolved, including battles the player isn't in. Tournaments, arenas and brawls are not counted.
- Soldiers fielded: each soldier sent into a battle, once per battle. In a fought battle that's each soldier that spawns, so reserves that never take the field don't count. For a hero it's battles joined.
- Troop and hero records are kept apart: a hero's kills never count toward a troop type, but a hero a troop takes out counts as that troop's kill.
- A kill is a blow that takes an enemy out, killed or knocked out (the battle scoreboard's rule). Team kills don't count. Kill categories follow Kill Counter: any hero, then bandits and mercenaries by occupation, everything else a troop.
- Damage is every hit on a person (not horses), team hits excluded. A blow counts in full even past the target's remaining health, and in auto-resolve it's the simulation's damage number.
- A death is a troop that died. Wounded and routed troops are defeats, not deaths. A retreat order is not a rout.
- A troop never fielded shows "-" and stays at the bottom of the list in either sort order.
- Each save keeps its own record, starting when the mod is added. The mod saves only string-to-number tables, so it can be removed from a save at any time.

## Credits

Based on ideas from LesserScholar's [Kill Counter](https://www.nexusmods.com/mountandblade2bannerlord/mods/3793) ([source](https://github.com/LesserScholar/KillCounters)): per-troop kill counts, kills by target, and the encyclopedia sort and stats sections. This mod expands on them.

Requires Harmony and UIExtenderEx. Built against Bannerlord v1.4.8.

## Building

Copy `EncyclopediaBattleRecords.local.props.example` to `EncyclopediaBattleRecords.local.props` and set the paths, then build `src\EncyclopediaBattleRecords.csproj` (`Stable_Debug` or `Stable_Release`, x64). The module is staged in `dist\EncyclopediaBattleRecords`. Pass `-p:DeployToMods=true` to also copy it to `ModsDeployFolder`. Release builds also pack `dist\EncyclopediaBattleRecords-<version>.7z`.

`tools\PatchCheck` applies the patches offline and checks each hits its target, and checks the encyclopedia panels against the game's prefabs.
