using UnityEditor;

namespace Game.Editor.SpriteCollection
{
    public static class SpriteCollectionUtility
    {
        public static void RefreshSpriteCollection()
        {
            string[] guids = AssetDatabase.FindAssets("t:SpriteCollection");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Hotfix.Framework.SpriteCollection collection = AssetDatabase.LoadAssetAtPath<Hotfix.Framework.SpriteCollection>(path);
                collection.Pack();
            }

            AssetDatabase.SaveAssets();
        }
    }
}