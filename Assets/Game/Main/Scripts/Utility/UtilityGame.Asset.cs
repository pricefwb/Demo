using GameFramework;

namespace Game
{
    public static partial class UtilityGame
    {
        public static partial class Asset
        {
            public static string GetSettingAsset(string assetName)
            {
                return Utility.Text.Format("Assets/Game/Hotfix/Settings/{0}.asset", assetName);
            }
        }
    }
}