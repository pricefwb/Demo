using Game.Hotfix.Business;
using Game.Main;

namespace Game.Editor
{
    public static partial class UtilityEditor
    {
        public static class Settings
        {
            private static HybridCLRSettings m_HybridCLRSettings;
            public static HybridCLRSettings HybridCLRSettings
            {
                get
                {
                    if (m_HybridCLRSettings == null)
                    {
                        m_HybridCLRSettings = GetSettings<HybridCLRSettings>(UtilityMain.Asset.GetHybridCLRSettingsAsset());
                    }
                    return m_HybridCLRSettings;
                }
            }

            private static BuildInfoSettings m_BuildInfoSettings;
            public static BuildInfoSettings BuildInfoSettings
            {
                get
                {
                    if (m_BuildInfoSettings == null)
                    {
                        m_BuildInfoSettings = GetSettings<BuildInfoSettings>(UtilityMain.Asset.GetBuildInfoSettingsAsset());
                    }
                    return m_BuildInfoSettings;
                }
            }

            private static DataTableSettings m_DataTableSettings;
            public static DataTableSettings DataTableSettings
            {
                get
                {
                    if (m_DataTableSettings == null)
                    {
                        m_DataTableSettings = GetSettings<DataTableSettings>(UtilityGame.Settings.DataTableSettingsPath);
                    }
                    return m_DataTableSettings;
                }
            }

            private static T GetSettings<T>(string path) where T : UnityEngine.Object
            {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null)
                {
                    throw new System.Exception($"{typeof(T).Name}.asset not found");
                }
                return asset;
            }

        }
    }
}