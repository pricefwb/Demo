//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

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
        private string m_GameVersion;
        private int m_InternalResourceVersion;
        private string m_OutputDirectory;

        public bool ContinueOnFailure
        {
            get
            {
                return false;
            }
        }

        private string GetHotUpdateUrl()
        {
            string buildInfoPath = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "Game/Configs/Runtime/BuildInfo.txt"));
            if (!File.Exists(buildInfoPath))
            {
                Debug.LogError("Build info can not be found");
                return "";
            }

            BuildInfo buildInfo = LitJson.JsonMapper.ToObject<BuildInfo>(File.ReadAllText(buildInfoPath));
            return buildInfo.HotUpdateUrl;
        }

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
            m_GameVersion = applicableGameVersion;
            m_InternalResourceVersion = internalResourceVersion;
            m_OutputDirectory = outputDirectory;

            // string streamingAssetsPath = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "StreamingAssets"));
            // string[] fileNames = Directory.GetFiles(streamingAssetsPath, "*", SearchOption.AllDirectories);
            // foreach (string fileName in fileNames)
            // {
            //     if (fileName.Contains(".gitkeep"))
            //     {
            //         continue;
            //     }

            //     File.Delete(fileName);
            // }

            // Utility.Path.RemoveEmptyDirectory(streamingAssetsPath);
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
#if ENABLE_HYBRID_CLR_UNITY
        HybridCLRUtility.OnPreprocessPlatform(platform);
#endif
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

        public void OnOutputUpdatableVersionListData(Platform platform, string versionListPath, int versionListLength, int versionListHashCode, int versionListCompressedLength, int versionListCompressedHashCode)
        {
            Debug.Log("OnOutputUpdatableVersionListData");
            string platformPath = GetPlatformPath(platform);
            string gameVersion = m_GameVersion.Replace('.', '_');

            VersionInfo versionInfo = new VersionInfo
            {
                ForceUpdateGame = false,
                UpdatePrefixUri = string.Format("{0}/Full/{1}_{2}/{3}", GetHotUpdateUrl(), gameVersion, m_InternalResourceVersion.ToString(), platformPath),
                LatestGameVersion = m_GameVersion,
                InternalGameVersion = 1,
                InternalResourceVersion = m_InternalResourceVersion,
                VersionListLength = versionListLength,
                VersionListHashCode = versionListHashCode,
                VersionListCompressedLength = versionListCompressedLength,
                VersionListCompressedHashCode = versionListCompressedHashCode,
            };
            string versionJson = LitJson.JsonMapper.ToJson(versionInfo);
            IOUtility.SaveFileSafe(m_OutputDirectory, platformPath + "Version.txt", versionJson);

            Debug.LogFormat("Version save success. \n length is {0} , hash code is {1} . \n compressed length is {2} , compressed hash code is {3} . \n list path is {4} \n ", versionListLength, versionListHashCode, versionListCompressedLength, versionListCompressedHashCode, versionListPath);
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
            Debug.Log("OnPostprocessPlatform");
            //拷贝AB 倒项目的 StreamingAssets 目录
            // if (!outputPackageSelected)
            // {
            //     return;
            // }

            // if (platform != Platform.Windows && platform != Platform.Windows64)
            // {
            //     return;
            // }

            // string streamingAssetsPath = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "StreamingAssets"));
            // string[] fileNames = Directory.GetFiles(outputPackagePath, "*", SearchOption.AllDirectories);
            // foreach (string fileName in fileNames)
            // {
            //     string destFileName = Utility.Path.GetRegularPath(Path.Combine(streamingAssetsPath, fileName.Substring(outputPackagePath.Length)));
            //     FileInfo destFileInfo = new FileInfo(destFileName);
            //     if (destFileInfo.Directory != null && !destFileInfo.Directory.Exists)
            //     {
            //         destFileInfo.Directory.Create();
            //     }

            //     File.Copy(fileName, destFileName);
            // }
        }

        /// <summary>
        /// 由 UnityGameFramework.Editor.ResourceTools.Platform 得到 平台标识符。
        /// </summary>
        /// <param name="platform">UnityGameFramework.Editor.ResourceTools.Platform。</param>
        /// <returns>平台标识符。</returns>
        public string GetPlatformPath(Platform platform)
        {
            // 这里和 ProcedureVersionCheck.GetPlatformPath() 对应。
            // 使用 平台标识符 关联 UnityEngine.RuntimePlatform 和 UnityGameFramework.Editor.ResourceTools.Platform
            switch (platform)
            {
                case Platform.Windows:
                    return "Windows";
                case Platform.Windows64:
                    return "Windows64";

                case Platform.MacOS:
                    return "MacOS";

                case Platform.IOS:
                    return "IOS";

                case Platform.Android:
                    return "Android";

                case Platform.WindowsStore:
                    return "WSA";

                case Platform.WebGL:
                    return "WebGL";

                case Platform.Linux:
                    return "Linux";

                default:
                    throw new GameFrameworkException("Platform is invalid.");
            }
        }
    }
}
