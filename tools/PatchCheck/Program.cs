// Offline check: loads EncyclopediaBattleRecords.dll next to the game assemblies, applies its patches and checks each one hit its target.
// Usage: PatchCheck.exe [path to EncyclopediaBattleRecords.dll]
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;

using HarmonyLib;

internal static class Program
{
    private static string Setting(string key) =>
        typeof(Program).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(a => a.Key == key)?.Value ?? "";

    private static readonly string Game = Setting("GameFolder");

    private static readonly string[] SearchDirs =
    {
        Path.Combine(Game, @"bin\Win64_Shipping_Client"),
        Path.Combine(Game, @"Modules\Native\bin\Win64_Shipping_Client"),
        Path.Combine(Game, @"Modules\SandBoxCore\bin\Win64_Shipping_Client"),
        Path.Combine(Game, @"Modules\SandBox\bin\Win64_Shipping_Client"),
        Setting("HarmonyFolder"),
        Setting("UIExtenderFolder"),
    };

    private static int _failures;

    private static int Main(string[] args)
    {
        string dll = args.Length > 0 ? args[0] : Path.GetFullPath(@"..\..\dist\EncyclopediaBattleRecords\bin\Win64_Shipping_Client\EncyclopediaBattleRecords.dll");
        AppDomain.CurrentDomain.AssemblyResolve += (_, e) =>
        {
            string name = new AssemblyName(e.Name).Name + ".dll";
            foreach (var dir in SearchDirs.Append(Path.GetDirectoryName(dll)))
            {
                string path = Path.Combine(dir ?? "", name);
                if (!string.IsNullOrEmpty(dir) && File.Exists(path))
                {
                    return Assembly.LoadFrom(path);
                }
            }

            return null;
        };

        var mod = Assembly.LoadFrom(dll);
        const BindingFlags Static = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        var harmony = new Harmony("encyclopediabattlerecords");
        bool applied = (bool)mod.GetType("EncyclopediaBattleRecords.SubModule", true).GetMethod("ApplyPatches", Static).Invoke(null, new object[] { harmony });
        Check(applied, "ApplyPatches reports every patch applied");

        foreach (var (type, prop, owner) in new[]
        {
            ("EncyclopediaBattleRecords.BattlePatches", "AllocateTroopsTarget", "MapEventSide.AllocateTroops"),
            ("EncyclopediaBattleRecords.BattlePatches", "AllocateTroopTarget", "MapEventSide.AllocateTroop"),
            ("EncyclopediaBattleRecords.BattlePatches", "ScoreHitTarget", "MapEventParty.OnTroopScoreHit"),
            ("EncyclopediaBattleRecords.BattlePatches", "KilledTarget", "MapEventParty.OnTroopKilled"),
            ("EncyclopediaBattleRecords.BattlePatches", "WoundedTarget", "MapEventParty.OnTroopWounded"),
            ("EncyclopediaBattleRecords.BattlePatches", "RoutedTarget", "MapEventParty.OnTroopRouted"),
            ("EncyclopediaBattleRecords.EncyclopediaPatch", "Target", "DefaultEncyclopediaUnitPage.InitializeSortControllers"),
            ("EncyclopediaBattleRecords.HeroSortPatch", "Target", "DefaultEncyclopediaHeroPage.InitializeSortControllers"),
        })
        {
            var target = (MethodBase)mod.GetType(type, true).GetProperty(prop, Static).GetValue(null);
            Check(target != null, $"{owner} exists");
            if (target == null)
            {
                continue;
            }

            var info = Harmony.GetPatchInfo(target);
            Check(info != null && info.Postfixes.Count(p => p.owner == "encyclopediabattlerecords") == 1,
                $"{target.DeclaringType.Name}.{target.Name}({string.Join(", ", target.GetParameters().Select(p => p.Name))}) has the postfix");
        }

        var page = mod.GetType("EncyclopediaBattleRecords.EncyclopediaPatch", true);
        var ratio = page.GetMethod("Ratio", Static);
        var perSoldier = page.GetMethod("PerHundred", Static);
        double R(int k, int d) => (double)ratio.Invoke(null, new object[] { k, d });
        double P(int c, int f) => (double)perSoldier.Invoke(null, new object[] { c, f });
        Check(R(9, 2) == 4.5 && R(5, 0) == 5 && R(0, 3) == 0 && R(0, 0) == 0, "K/D ratio: 9/2 = 4.5, 5/0 = 5, 0/3 = 0, 0/0 = 0");
        var perSoldierDamage = page.GetMethod("PerSoldier", Static);
        double D(int a, int f) => (double)perSoldierDamage.Invoke(null, new object[] { a, f });
        Check(D(5000, 100) == 50 && D(5000, 0) == 0, "damage per soldier: 5000 over 100 fielded = 50, none fielded = 0");
        Check(P(30, 120) == 25 && P(5, 0) == 0, "per 100 soldiers: 30 over 120 fielded = 25, none fielded = 0");

        var unitPage = ApplyPrefabs(mod, "EncyclopediaUnitPage", "EncyclopediaBattleRecords.UnitPageMixin", "EncyclopediaBattleRecords.UnitPageStackPrefab", "EncyclopediaBattleRecords.UnitPagePrefab");
        var stack = unitPage?.SelectNodes("descendant::ListPanel[@Id='InnerPanel']/Children/*");
        Check(stack != null && stack.Count == 2
            && stack[0].SelectSingleNode("descendant::EncyclopediaDivider[@Parameter.Title='@EncyclopediaBattleRecordsTitle']") != null
            && stack[1].Attributes?["DataSource"]?.Value == "{Tree}",
            "EncyclopediaUnitPage: the scroll content is a vertical stack with Battle Record first and the upgrade tree below it");
        var heroPage = ApplyPrefabs(mod, "EncyclopediaHeroPage", "EncyclopediaBattleRecords.HeroPageMixin", "EncyclopediaBattleRecords.HeroPagePrefab");
        var afterInfo = heroPage?.SelectSingleNode("descendant::Widget[@Id='InfoContainer']")?.NextSibling;
        Check(afterInfo?.Attributes?["Parameter.Title"]?.Value == "@EncyclopediaBattleRecordsTitle", "EncyclopediaHeroPage: Battle Record follows the hero's info stats");
        CheckField("EncyclopediaUnitPageVM", "_character", "CharacterObject");
        CheckField("EncyclopediaHeroPageVM", "_hero", "Hero");

        // Every method patched at module load: ours via Harmony, and the page VM methods UIExtenderEx patches for the mixins.
        var patched = Harmony.GetAllPatchedMethods().Where(m => Harmony.GetPatchInfo(m).Owners.Contains("encyclopediabattlerecords")).ToList();
        foreach (var mixinName in new[] { "EncyclopediaBattleRecords.UnitPageMixin", "EncyclopediaBattleRecords.HeroPageMixin" })
        {
            var mixin = mod.GetType(mixinName, true);
            var vm = mixin.BaseType.GetGenericArguments()[0];
            patched.AddRange(vm.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly));
            // The nearest declared OnFinalize, inherited if the page doesn't declare its own.
            var finalize = vm.GetMethod("OnFinalize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if (finalize != null && !patched.Contains(finalize))
            {
                patched.Add(finalize);
            }

            var attribute = mixin.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.Name == "ViewModelMixinAttribute");
            var refresh = attribute?.ConstructorArguments.FirstOrDefault(a => a.ArgumentType == typeof(string)).Value as string;
            if (refresh != null)
            {
                patched.Add(vm.GetMethod(refresh, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
            }
        }

        var knownBad = Type.GetType("TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages.EncyclopediaUnitPageVM, TaleWorlds.CampaignSystem.ViewModelCollection", true)
            .GetMethod("RefreshValues", Type.EmptyTypes);
        Check(EagerStaticInitializers(knownBad).Contains("CampaignUIHelper.SkillObjectComparerInstance"),
            "the early-initializer check flags EncyclopediaUnitPageVM.RefreshValues (the method whose patch crashed the game)");

        foreach (var method in patched)
        {
            var early = EagerStaticInitializers(method).ToList();
            Check(early.Count == 0, $"patching {method.DeclaringType.Name}.{method.Name} can't run another type's static initializer early"
                + (early.Count == 0 ? "" : $" (reads {string.Join(", ", early)})"));
        }

        Console.WriteLine(_failures == 0 ? "ALL OK" : $"{_failures} FAILED");
        return _failures == 0 ? 0 : 1;
    }

    /// <summary>
    /// Applies the given prefab patches to the game's own prefab the way UIExtenderEx places them (each XPath must find exactly
    /// one node), checks every snippet is well-formed and binds only properties the mixin has, and returns the patched document.
    /// </summary>
    private static XmlDocument ApplyPrefabs(Assembly mod, string movie, string mixinType, params string[] prefabTypes)
    {
        var doc = new XmlDocument();
        doc.Load(Path.Combine(Game, $@"Modules\SandBox\GUI\Prefabs\Encyclopedia\EncyclopediaSubPages\{movie}.xml"));
        var mixin = mod.GetType(mixinType, true);
        foreach (var prefabType in prefabTypes)
        {
            var type = mod.GetType(prefabType, true);
            var patch = Activator.CreateInstance(type);
            string xpath = (string)type.GetField("XPath").GetValue(null);
            string text = (string)type.GetProperty("Text").GetValue(patch);
            string insert = type.GetProperty("Type").GetValue(patch).ToString();

            var targets = doc.SelectNodes(xpath);
            Check(targets.Count == 1, $"{movie}: {type.Name} \"{xpath}\" matches one node (got {targets.Count})");
            var snippet = new XmlDocument();
            try
            {
                snippet.LoadXml(text);
            }
            catch (XmlException e)
            {
                Check(false, $"{movie}: {type.Name} snippet is well-formed ({e.Message})");
                return null;
            }

            var bound = snippet.SelectNodes("//@*").Cast<XmlAttribute>()
                .Select(a => a.Value)
                .Where(v => v.StartsWith("@EncyclopediaBattleRecords") || v.StartsWith("{EncyclopediaBattleRecords"))
                .Select(v => v.Trim('@', '{', '}'))
                .Distinct()
                .ToList();
            Check(bound.All(p => mixin.GetProperty(p) != null),
                $"{movie}: {type.Name} is well-formed and its bindings ({(bound.Count == 0 ? "none" : string.Join(", ", bound))}) exist on {mixin.Name}");
            if (targets.Count != 1)
            {
                continue;
            }

            var target = targets[0];
            XmlNode previous = null;
            foreach (XmlNode node in snippet.DocumentElement.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                var imported = doc.ImportNode(node, true);
                if (previous != null)
                {
                    previous = previous.ParentNode.InsertAfter(imported, previous);
                    continue;
                }

                switch (insert)
                {
                    case "Prepend":
                        previous = target.ParentNode.InsertBefore(imported, target);
                        break;
                    case "Append":
                        previous = target.ParentNode.InsertAfter(imported, target);
                        break;
                    case "ReplaceKeepChildren":
                        target.ParentNode.ReplaceChild(imported, target);
                        while (target.ChildNodes.Count > 0)
                        {
                            imported.AppendChild(target.ChildNodes[0]);
                        }

                        previous = imported;
                        break;
                    default:
                        Check(false, $"{movie}: {type.Name} uses {insert}, which this check doesn't model");
                        return doc;
                }
            }
        }

        return doc;
    }

    private static void CheckField(string vmName, string field, string fieldType)
    {
        var vm = Type.GetType($"TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages.{vmName}, TaleWorlds.CampaignSystem.ViewModelCollection", true);
        var f = vm.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
        Check(f != null && f.FieldType.Name == fieldType, $"{vmName}.{field} is a {fieldType}");
    }

    /// <summary>
    /// Static fields the method reads or writes on other game types that initialize lazily (beforefieldinit with a static
    /// initializer). The .NET Framework JIT may run such an initializer when it compiles the method, and patching compiles it at
    /// module load, before game texts and managers exist. That's how the RefreshValues patch broke CampaignUIHelper.
    /// </summary>
    private static IEnumerable<string> EagerStaticInitializers(MethodBase method)
    {
        foreach (var instruction in PatchProcessor.GetOriginalInstructions(method))
        {
            if ((instruction.opcode == OpCodes.Ldsfld || instruction.opcode == OpCodes.Ldsflda || instruction.opcode == OpCodes.Stsfld)
                && instruction.operand is FieldInfo field)
            {
                var type = field.DeclaringType;
                if (type != method.DeclaringType && type.TypeInitializer != null
                    && (type.Attributes & TypeAttributes.BeforeFieldInit) != 0
                    && type.Assembly.GetName().Name.StartsWith("TaleWorlds")
                    && !type.Name.StartsWith("<"))
                {
                    yield return $"{type.Name}.{field.Name}";
                }
            }
        }
    }

    private static void Check(bool ok, string what)
    {
        Console.WriteLine($"{(ok ? "OK  " : "FAIL")} {what}");
        if (!ok)
        {
            _failures++;
        }
    }
}
