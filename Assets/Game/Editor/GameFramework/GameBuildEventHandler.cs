//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Game.Editor.ResourceTools;
using Game.Main;
using GameFramework;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor.ResourceTools;

namespace Game.Editor
{
    public sealed class GameBuildEventHandler : IBuildEventHandler
    {
        private readonly string CommitResourcesPath = Application.dataPath + $"/../CommitResources/";
        public bool ContinueOnFailure => false;
        private VersionInfo m_VersionInfo = new VersionInfo();

        /// <summary>
        /// 所有平台生成开始前的预处理事件。
        /// </summary>
        /// <param name="productName">产品名称。</param>
        /// <param name="companyName">公司名称。</param>
        /// <param name="gameIdentifier">游戏识别号。</param>
        /// <param name="gameFrameworkVersion">游戏框架版本。</param>
        /// <param name="unityVersion">Unity 版本。</param>
        /// <param name="applicableGameVersion">适用游戏版本。</param>
        /// <param name="internalResourceVersion">内部资源版本。</param>
        /// <param name="platforms">生成的目标平台。</param>
        /// <param name="assetBundleCompression">AssetBundle 压缩类型。</param>
        /// <param name="compressionHelperTypeName">压缩解压缩辅助器类型名称。</param>
        /// <param name="additionalCompressionSelected">是否进行再压缩以降低传输开销。</param>
        /// <param name="forceRebuildAssetBundleSelected">是否强制重新构建 AssetBundle。</param>
        /// <param name="buildEventHandlerTypeName">生成资源事件处理函数名称。</param>
        /// <param name="outputDirectory">生成目录。</param>
        /// <param name="buildAssetBundleOptions">AssetBundle 生成选项。</param>
        /// <param name="workingPath">生成时的工作路径。</param>
        /// <param name="outputPackageSelected">是否生成单机模式所需的文件。</param>
        /// <param name="outputPackagePath">为单机模式生成的文件存放于此路径。若游戏是单机游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="outputFullSelected">是否生成可更新模式所需的远程文件。</param>
        /// <param name="outputFullPath">为可更新模式生成的远程文件存放于此路径。若游戏是网络游戏，生成结束后应将此目录上传至 Web 服务器，供玩家下载用。</param>
        /// <param name="outputPackedSelected">是否生成可更新模式所需的本地文件。</param>
        /// <param name="outputPackedPath">为可更新模式生成的本地文件存放于此路径。若游戏是网络游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="buildReportPath">生成报告路径。</param>
        public void OnPreprocessAllPlatforms(string productName, string companyName, string gameIdentifier, string gameFrameworkVersion, string unityVersion, string applicableGameVersion, int internalResourceVersion,
            Platform platforms, AssetBundleCompressionType assetBundleCompression, string compressionHelperTypeName, bool additionalCompressionSelected, bool forceRebuildAssetBundleSelected, string buildEventHandlerTypeName, string outputDirectory, BuildAssetBundleOptions buildAssetBundleOptions,
            string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath, string buildReportPath)
        {
            m_VersionInfo.LatestGameVersion = applicableGameVersion;
            m_VersionInfo.InternalGameVersion = internalResourceVersion;

            ResourceRuleEditorUtility.RefreshResourceCollection();
        }

        /// <summary>
        /// 某个平台生成开始前的预处理事件。
        /// </summary>
        /// <param name="platform">生成平台。</param>
        /// <param name="workingPath">生成时的工作路径。</param>
        /// <param name="outputPackageSelected">是否生成单机模式所需的文件。</param>
        /// <param name="outputPackagePath">为单机模式生成的文件存放于此路径。若游戏是单机游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="outputFullSelected">是否生成可更新模式所需的远程文件。</param>
        /// <param name="outputFullPath">为可更新模式生成的远程文件存放于此路径。若游戏是网络游戏，生成结束后应将此目录上传至 Web 服务器，供玩家下载用。</param>
        /// <param name="outputPackedSelected">是否生成可更新模式所需的本地文件。</param>
        /// <param name="outputPackedPath">为可更新模式生成的本地文件存放于此路径。若游戏是网络游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>

        public void OnPreprocessPlatform(Platform platform, string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath)
        {
            if (HybridCLR.Editor.SettingsUtil.Enable)
            {
                HybridCLRUtility.Builder.Build(UtilityEditor.GetBuildTarget(platform));
                ResourceRuleEditorUtility.RefreshResourceCollection();
            }
        }

        /// <summary>
        /// 某个平台生成 AssetBundle 完成事件。
        /// </summary>
        /// <param name="platform">生成平台。</param>
        /// <param name="workingPath">生成时的工作路径。</param>
        /// <param name="outputPackageSelected">是否生成单机模式所需的文件。</param>
        /// <param name="outputPackagePath">为单机模式生成的文件存放于此路径。若游戏是单机游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="outputFullSelected">是否生成可更新模式所需的远程文件。</param>
        /// <param name="outputFullPath">为可更新模式生成的远程文件存放于此路径。若游戏是网络游戏，生成结束后应将此目录上传至 Web 服务器，供玩家下载用。</param>
        /// <param name="outputPackedSelected">是否生成可更新模式所需的本地文件。</param>
        /// <param name="outputPackedPath">为可更新模式生成的本地文件存放于此路径。若游戏是网络游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="assetBundleManifest">AssetBundle 的描述文件。</param>
        public void OnBuildAssetBundlesComplete(Platform platform, string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath, AssetBundleManifest assetBundleManifest)
        {
            Debug.Log("OnBuildAssetBundlesComplete");
        }


        public void OnOutputUpdatableVersionListData(Platform platform, string versionListPath, int versionListLength,
            int versionListHashCode, int VersionListCompressedLength, int VersionListCompressedHashCode)
        {
            m_VersionInfo.VersionListLength = versionListLength;
            m_VersionInfo.VersionListHashCode = versionListHashCode;
            m_VersionInfo.VersionListCompressedLength = VersionListCompressedLength;
            m_VersionInfo.VersionListCompressedHashCode = VersionListCompressedHashCode;
        }


        /// <summary>
        /// 所有平台生成结束后的后处理事件。
        /// </summary>
        /// <param name="productName">产品名称。</param>
        /// <param name="companyName">公司名称。</param>
        /// <param name="gameIdentifier">游戏识别号。</param>
        /// <param name="gameFrameworkVersion">游戏框架版本。</param>
        /// <param name="unityVersion">Unity 版本。</param>
        /// <param name="applicableGameVersion">适用游戏版本。</param>
        /// <param name="internalResourceVersion">内部资源版本。</param>
        /// <param name="platforms">生成的目标平台。</param>
        /// <param name="assetBundleCompression">AssetBundle 压缩类型。</param>
        /// <param name="compressionHelperTypeName">压缩解压缩辅助器类型名称。</param>
        /// <param name="additionalCompressionSelected">是否进行再压缩以降低传输开销。</param>
        /// <param name="forceRebuildAssetBundleSelected">是否强制重新构建 AssetBundle。</param>
        /// <param name="buildEventHandlerTypeName">生成资源事件处理函数名称。</param>
        /// <param name="outputDirectory">生成目录。</param>
        /// <param name="buildAssetBundleOptions">AssetBundle 生成选项。</param>
        /// <param name="workingPath">生成时的工作路径。</param>
        /// <param name="outputPackageSelected">是否生成单机模式所需的文件。</param>
        /// <param name="outputPackagePath">为单机模式生成的文件存放于此路径。若游戏是单机游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="outputFullSelected">是否生成可更新模式所需的远程文件。</param>
        /// <param name="outputFullPath">为可更新模式生成的远程文件存放于此路径。若游戏是网络游戏，生成结束后应将此目录上传至 Web 服务器，供玩家下载用。</param>
        /// <param name="outputPackedSelected">是否生成可更新模式所需的本地文件。</param>
        /// <param name="outputPackedPath">为可更新模式生成的本地文件存放于此路径。若游戏是网络游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="buildReportPath">生成报告路径。</param>
        public void OnPostprocessAllPlatforms(string productName, string companyName, string gameIdentifier, string gameFrameworkVersion, string unityVersion, string applicableGameVersion, int internalResourceVersion,
            Platform platforms, AssetBundleCompressionType assetBundleCompression, string compressionHelperTypeName, bool additionalCompressionSelected, bool forceRebuildAssetBundleSelected, string buildEventHandlerTypeName, string outputDirectory, BuildAssetBundleOptions buildAssetBundleOptions,
            string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath, string buildReportPath)
        {
            Debug.Log("OnPostprocessAllPlatforms");
        }


        /// <summary>
        /// 某个平台生成结束后的后处理事件。
        /// </summary>
        /// <param name="platform">生成平台。</param>
        /// <param name="workingPath">生成时的工作路径。</param>
        /// <param name="outputPackageSelected">是否生成单机模式所需的文件。</param>
        /// <param name="outputPackagePath">为单机模式生成的文件存放于此路径。若游戏是单机游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="outputFullSelected">是否生成可更新模式所需的远程文件。</param>
        /// <param name="outputFullPath">为可更新模式生成的远程文件存放于此路径。若游戏是网络游戏，生成结束后应将此目录上传至 Web 服务器，供玩家下载用。</param>
        /// <param name="outputPackedSelected">是否生成可更新模式所需的本地文件。</param>
        /// <param name="outputPackedPath">为可更新模式生成的本地文件存放于此路径。若游戏是网络游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="isSuccess">是否生成成功。</param>
        public void OnPostprocessPlatform(Platform platform, string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath, bool isSuccess)
        {
            string platformName = UtilityEditor.GetPlatformName(platform);
            string commitPath = Path.Combine(CommitResourcesPath, platformName);
            UtilityEditor.IO.EnsureDirectoryExistsAndEmpty(commitPath);
            UtilityEditor.IO.CopyDirectory(outputFullPath, commitPath);

            string versionJson = LitJson.JsonMapper.ToJson(m_VersionInfo);
            string versionPath = Path.Combine(commitPath, UtilityEditor.Settings.BuildInfoSettings.VersionFile);
            UtilityEditor.IO.SaveFileSafe(versionPath, versionJson);

            Debug.Log("更新资源文件拷贝完毕！");
        }
    }
}
