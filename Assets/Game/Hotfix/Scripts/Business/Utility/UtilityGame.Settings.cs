using System.Threading.Tasks;
using Game.Hotfix.Framework.Await;

namespace Game.Hotfix.Business
{
    public static partial class UtilityGame
    {
        public static class Settings
        {
            public const string DataTableSettingsPath = "Assets/Game/Hotfix/Configs/DataTableSettings.asset";

            private static DataTableSettings m_DataTableSettings;
            public static async Task Initialize()
            {
                await Task.WhenAll(
                    GetDataTableSettings()
                );
            }

            static async Task<DataTableSettings> GetDataTableSettings()
            {
                if (m_DataTableSettings == null)
                {
                    m_DataTableSettings = await GameEntry.Resource.LoadAssetAsync<DataTableSettings>(DataTableSettingsPath);
                }
                return m_DataTableSettings;
            }

            public static DataTableSettings DataTableSettings
            {
                get
                {
                    if (m_DataTableSettings == null)
                    {
                        throw new System.Exception("Please call UtilityGame.Settings.Initialize method first");
                    }
                    return m_DataTableSettings;
                }
            }
        }
    }
}