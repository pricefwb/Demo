using Game.Hotfix.Framework.SpriteCollection;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Hotfix.Framework
{
    public static partial class SetSpriteExtensions
    {
        /// <summary>
        /// 设置精灵
        /// </summary>
        /// <param name="image"></param>
        /// <param name="collectionPath">精灵所在收集器地址</param>
        /// <param name="spritePath">精灵名称</param>
        public static void SetSprite(this Image image, string collectionPath, string spritePath)
        {
            GameEntry.SpriteCollection.SetSprite(WaitSetImage.Create(image, collectionPath, spritePath));
        }
    }
}