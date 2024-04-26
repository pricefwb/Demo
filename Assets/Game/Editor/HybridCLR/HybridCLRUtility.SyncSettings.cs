using System;
using System.Collections.Generic;
using System.Linq;
using HybridCLR.Editor;
using HybridCLR.Editor.AOT;
using HybridCLR.Editor.Meta;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static partial class HybridCLRUtility
    {
        public static class SyncSettings
        {
            [MenuItem("MyTools/HybridCLR/Sync Settings")]
            public static void SyncAll()
            {
                SyncAll(EditorUserBuildSettings.activeBuildTarget);
            }

            public static void SyncAll(BuildTarget target)
            {
                SyncEnable(target);
                SyncHotfixAssemblies(EditorUserBuildSettings.activeBuildTarget);
                SyncAOTMetaAssemblies(EditorUserBuildSettings.activeBuildTarget);

                Debug.Log("Sync Settings Done.");
            }

            public static void SyncEnable(BuildTarget target)
            {
                UtilityEditor.Settings.HybridCLRSettings.Enable = SettingsUtil.Enable;

                EditorUtility.SetDirty(UtilityEditor.Settings.HybridCLRSettings);
                AssetDatabase.Refresh();

                Debug.Log("Sync Enable Done.");
            }

            public static void SyncHotfixAssemblies(BuildTarget target)
            {
                List<string> hotUpdateDllNames = SettingsUtil.HotUpdateAssemblyNamesExcludePreserved;

                UtilityEditor.Settings.HybridCLRSettings.HotfixAssemblies.Clear();
                foreach (var name in hotUpdateDllNames)
                {
                    UtilityEditor.Settings.HybridCLRSettings.HotfixAssemblies.Add($"{name}.dll");
                }

                EditorUtility.SetDirty(UtilityEditor.Settings.HybridCLRSettings);
                AssetDatabase.Refresh();

                Debug.Log("Sync Hotfix Assemblies Done.");
            }

            /// <summary>
            /// Copy from AOTReferenceGeneratorCommand
            /// </summary>
            /// <param name="target"></param>
            public static void SyncAOTMetaAssemblies(BuildTarget target)
            {
                var gs = SettingsUtil.HybridCLRSettings;
                List<string> hotUpdateDllNames = SettingsUtil.HotUpdateAssemblyNamesExcludePreserved;

                string dir = $"{SettingsUtil.GetAssembliesPostIl2CppStripDir(target)}";
                if (!UtilityEditor.IO.IsDirectoryExists(dir))
                {
                    Debug.LogError($"AOTMetaAssemblies文件夹不存在，因此需要你先在菜单栏中(HybridCLR>>Generate>>All)操作。FolderPath:{dir}");
                    return;
                }

                using (AssemblyReferenceDeepCollector collector = new AssemblyReferenceDeepCollector(MetaUtil.CreateHotUpdateAndAOTAssemblyResolver(target, hotUpdateDllNames), hotUpdateDllNames))
                {
                    var analyzer = new Analyzer(new Analyzer.Options
                    {
                        MaxIterationCount = Math.Min(20, gs.maxGenericReferenceIteration),
                        Collector = collector,
                    });

                    analyzer.Run();

                    var types = analyzer.AotGenericTypes;
                    var methods = analyzer.AotGenericMethods;
                    var modules = new HashSet<dnlib.DotNet.ModuleDef>(types.Select(t => t.Type.Module).Concat(methods.Select(m => m.Method.Module))).ToList();
                    modules.Sort((a, b) => a.Name.CompareTo(b.Name));

                    UtilityEditor.Settings.HybridCLRSettings.AOTMetaAssemblies.Clear();
                    foreach (dnlib.DotNet.ModuleDef module in modules)
                    {
                        UtilityEditor.Settings.HybridCLRSettings.AOTMetaAssemblies.Add(module.Name);
                    }

                    EditorUtility.SetDirty(UtilityEditor.Settings.HybridCLRSettings);
                    AssetDatabase.Refresh();
                }

                Debug.Log("Sync AOT metadata Assemblies Done.");
            }
        }
    }
}