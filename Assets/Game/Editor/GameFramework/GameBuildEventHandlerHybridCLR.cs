using Game.Editor.ResourceTools;
using UnityGameFramework.Editor.ResourceTools;

namespace Game.Editor
{
    public static class GameBuildEventHandlerHybridCLR
    {

        public static void OnPreprocessAllPlatforms(Platform platforms, bool outputFullSelected)
        {

        }
        public static void OnPreprocessPlatform(Platform platform)
        {
            if (HybridCLR.Editor.SettingsUtil.Enable)
            {
                HybridCLRUtility.Builder.Build(UtilityEditor.GetBuildTarget(platform));
                ResourceRuleEditorUtility.RefreshResourceCollection();
            }
            else
            {
                HybridCLRUtility.SyncSetting.SyncAll(UtilityEditor.GetBuildTarget(platform));
            }
        }

        public static void OnPostprocessPlatform(Platform platform, bool outputPackageSelected,
            bool outputFullSelected, bool outputPackedSelected, string commitResourcesPath)
        {

        }
    }
}