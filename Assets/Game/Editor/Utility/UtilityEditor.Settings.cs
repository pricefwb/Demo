using GameFramework;

namespace Game.Editor
{
    public static partial class UtilityEditor
    {
        public static class Settings
        {
            private static HybridCLRSettings _HybridCLRSettings;
            public static HybridCLRSettings HybridCLRSettings
            {
                get
                {
                    if (_HybridCLRSettings == null)
                    {
                        _HybridCLRSettings = GetHybridCLRSettings();
                    }
                    return _HybridCLRSettings;
                }
            }
            private static HybridCLRSettings GetHybridCLRSettings()
            {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<HybridCLRSettings>(UtilityMain.Asset.GetHybridCLRSettingsAsset());
                if (asset == null)
                {
                    throw new System.Exception("HybridCLRSettings.asset not found");
                }
                return asset;
            }

            private static BuildInfoSettings _BuildInfoSettings;
            public static BuildInfoSettings BuildInfoSettings
            {
                get
                {
                    if (_BuildInfoSettings == null)
                    {
                        _BuildInfoSettings = GetBuildInfoSettings();
                    }
                    return _BuildInfoSettings;
                }
            }
            private static BuildInfoSettings GetBuildInfoSettings()
            {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<BuildInfoSettings>(UtilityMain.Asset.GetBuildInfoSettingsAsset());
                if (asset == null)
                {
                    throw new System.Exception("BuildInfoSettings.asset not found");
                }
                return asset;
            }
        }
    }
}