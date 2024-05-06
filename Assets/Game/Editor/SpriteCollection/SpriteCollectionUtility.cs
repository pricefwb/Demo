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
                Game.Hotfix.Framework.SpriteCollection.SpriteCollection collection = AssetDatabase.LoadAssetAtPath<Game.Hotfix.Framework.SpriteCollection.SpriteCollection>(path);
                collection.Pack();
            }

            AssetDatabase.SaveAssets();
        }
    }
}