using GameFramework;

namespace Game.Hotfix.Common
{
    public static partial class AssetUtility
    {
        public static class DataTable
        {
            public static string GetDataTableAsset(string assetName, bool fromBytes = true)
            {
                return Utility.Text.Format("Assets/Game/Hotfix/DataTables/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
            }
        }
    }
}