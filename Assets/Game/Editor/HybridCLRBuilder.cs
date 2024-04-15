using System;
using System.IO;
using GameFramework;
using HybridCLR.Editor;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor.ResourceTools;
using HybridCLR.Editor.Commands;

namespace Game.Editor
{
    public class HybridCLRBuilder
    {
        public static BuildTarget GetBuildTarget(Platform platform)
        {
            switch (platform)
            {
                case Platform.Windows:
                    return BuildTarget.StandaloneWindows;

                case Platform.Windows64:
                    return BuildTarget.StandaloneWindows64;

                case Platform.MacOS:
#if UNITY_2017_3_OR_NEWER
                    return BuildTarget.StandaloneOSX;
#else
                    return BuildTarget.StandaloneOSXUniversal;
#endif
                case Platform.Linux:
                    return BuildTarget.StandaloneLinux64;

                case Platform.IOS:
                    return BuildTarget.iOS;

                case Platform.Android:
                    return BuildTarget.Android;

                case Platform.WindowsStore:
                    return BuildTarget.WSAPlayer;

                case Platform.WebGL:
                    return BuildTarget.WebGL;

                default:
                    throw new GameFrameworkException("Platform is invalid.");
            }
        }


        public static void CopyDllAssets(BuildTarget buildTarget)
        {
            // IOUtility.CreateDirectoryIfNotExists(Path.GetDirectoryName(Constant.Dll.DllFormatPath));

            // // Copy HotUpdate Dll
            // string hotUpdateDllPath = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(buildTarget);
            // foreach (var dllName in Constant.Dll.HotfixDllNames)
            // {
            //     string oriFileName = Path.Combine(hotUpdateDllPath, dllName + ".dll");
            //     if (!File.Exists(oriFileName))
            //     {
            //         Debug.LogError($"HotUpdate dll: {oriFileName} 文件不存在。需要生成对应平台的热更dll后才能拷贝。");
            //         continue;
            //     }
            //     string desFileName = Utility.Text.Format(Constant.Dll.DllFormatPath, dllName);
            //     File.Copy(oriFileName, desFileName, true);
            // }

            // // Copy AOT Dll
            // string aotDllPath = SettingsUtil.GetAssembliesPostIl2CppStripDir(buildTarget);
            // foreach (var dllName in Constant.Dll.AOTDllNames)
            // {
            //     string oriFileName = Path.Combine(aotDllPath, dllName + ".dll");
            //     if (!File.Exists(oriFileName))
            //     {
            //         Debug.LogError($"AOT 补充元数据 dll: {oriFileName} 文件不存在。需要构建一次主包后才能生成裁剪后的 AOT dll.");
            //         continue;
            //     }
            //     string desFileName = Utility.Text.Format(Constant.Dll.DllFormatPath, dllName);
            //     File.Copy(oriFileName, desFileName, true);
            // }


            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void CompileHotfixDll(Platform platform)
        {
            var buildTarget = GetBuildTarget(platform);
            CompileDllCommand.CompileDll(buildTarget);
            CopyDllAssets(buildTarget);
        }

        //AOT 只能在对应平台上编译？？

        [MenuItem("HybridCLR/编译 Hotfix dll/Windows", priority = 400)]
        public static void CompileHotfixDllWindows() => CompileHotfixDll(Platform.Windows64);

        [MenuItem("HybridCLR/编译 Hotfix dll/MacOS", priority = 400)]
        public static void CompileHotfixDllMacOS() => CompileHotfixDll(Platform.MacOS);

        [MenuItem("HybridCLR/编译 Hotfix dll/IOS", priority = 400)]
        public static void CompileHotfixDllIOS() => CompileHotfixDll(Platform.IOS);

        [MenuItem("HybridCLR/编译 Hotfix dll/Android", priority = 400)]
        public static void CompileHotfixDllAndroid() => CompileHotfixDll(Platform.Android);

    }
}
