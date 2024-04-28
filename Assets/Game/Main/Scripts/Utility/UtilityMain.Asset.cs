using GameFramework;

namespace Game
{
    public static partial class UtilityMain
    {
        public static partial class Asset
        {
            public static string GetHybridCLRSettingsAsset()
            {
                return "Assets/Game/Hotfix/Configs/HybridCLRSettings.asset";
            }

            public static string GetBuildInfoSettingsAsset()
            {
                return "Assets/Game/Main/Configs/BuildInfoSettings.asset";
            }
        }
    }
}