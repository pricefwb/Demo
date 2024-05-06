using Game.Editor.DataTableTools;
using UnityGameFramework.Editor.ResourceTools;

namespace Game.Editor
{
    public static class GameBuildEventHandlerDataTable
    {
        public static void OnPreprocessAllPlatforms(Platform platforms, bool outputFullSelected)
        {
            DataTableGeneratorMenu.GenerateAll();
        }
        public static void OnPreprocessPlatform(Platform platform)
        {

        }

        public static void OnPostprocessPlatform(Platform platform, bool outputPackageSelected,
            bool outputFullSelected, bool outputPackedSelected, string commitResourcesPath)
        {

        }
    }
}