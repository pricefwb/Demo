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
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<HybridCLRSettings>(UtilityGame.Asset.GetSettingAsset("HybridCLRSettings"));
                if (asset == null)
                {
                    throw new System.Exception("HybridCLRSettings.asset not found");
                }
                return asset;
            }
        }
    }
}