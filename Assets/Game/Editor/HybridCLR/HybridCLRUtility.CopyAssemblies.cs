using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static partial class HybridCLRUtility
    {
        public static class CopyAssemblies
        {
            [MenuItem("MyTools/HybridCLR/Copy Hotfix And AOT metadata Assemblies")]
            public static void CopyHotfixAndAOTAssemblies()
            {
                CopyHotfixAndAOTAssemblies(EditorUserBuildSettings.activeBuildTarget);
            }

            public static void CopyHotfixAndAOTAssemblies(BuildTarget target)
            {
                CopyHotfixAssemblies(target);
                CopyAOTMetaAssemblies(target);
                Debug.Log("Copy Hotfix And AOT metadata Assemblies Done.");
            }

            public static void CopyHotfixAssemblies(BuildTarget target)
            {
                var srcDir = HybridCLR.Editor.SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
                var targetDir = UtilityEditor.Settings.HybridCLRSettings.HotfixAssembliesDirectory;

                UtilityEditor.IO.CreateDirectory(targetDir);

                var assemblies = HybridCLR.Editor.SettingsUtil.HotUpdateAssemblyFilesIncludePreserved;
                foreach (var assembly in assemblies)
                {
                    var srcPath = Path.Combine(srcDir, assembly);
                    var targetPath = Path.Combine(targetDir, $"{assembly}{UtilityEditor.Settings.HybridCLRSettings.AssemblyAssetExtension}");
                    var result = UtilityEditor.IO.CopyFile(srcPath, targetPath);
                    if (!result)
                    {
                        Debug.LogError($"[CopyHotfixDlls] Copy {srcPath} to {targetPath} failed.");
                    }
                }

                AssetDatabase.Refresh();

                Debug.Log($"[CopyHotfixDlls] Copy {assemblies.Count} DLL files to {targetDir}");
            }

            public static void CopyAOTMetaAssemblies(BuildTarget target)
            {
                var srcDir = HybridCLR.Editor.SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
                if (!UtilityEditor.IO.IsDirectoryExists(srcDir))
                {
                    Debug.LogError($"AOTMetaAssemblies文件夹不存在，因此需要你先在菜单栏中(HybridCLR>>Generate>>All)操作。FolderPath:{srcDir}");
                    return;
                }
                var targetDir = UtilityEditor.Settings.HybridCLRSettings.AOTMetaAssembliesDirectory;

                UtilityEditor.IO.CreateDirectory(targetDir);

                var aotAssemblies = UtilityEditor.Settings.HybridCLRSettings.AOTMetaAssemblies;
                foreach (var assembly in aotAssemblies)
                {
                    var srcPath = Path.Combine(srcDir, assembly.EndsWith(".dll") ? assembly : $"{assembly}.dll");
                    var targetPath = Path.Combine(targetDir, $"{assembly}{UtilityEditor.Settings.HybridCLRSettings.AssemblyAssetExtension}");
                    var result = UtilityEditor.IO.CopyFile(srcPath, targetPath);
                    if (!result)
                    {
                        Debug.LogError($"[CopyAOTMetaAssemblies] Copy {srcPath} to {targetPath} failed.");
                    }
                }

                AssetDatabase.Refresh();

                Debug.Log($"[CopyAOTMetadataDlls] Copy {aotAssemblies.Count} DLL files to {targetDir}");
            }

            private static void ClearDirectory(string directory)
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }

            private static void EnsureDirectory(string directory)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }
    }
}