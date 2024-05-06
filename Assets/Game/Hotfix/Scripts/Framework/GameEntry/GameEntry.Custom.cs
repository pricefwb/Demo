using Game.Hotfix.Framework.SpriteCollection;
using UnityEngine;

// namespace Game.Hotfix.Framework
// {
/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameEntry : MonoBehaviour
{
    public static SpriteCollectionComponent SpriteCollection
    {
        get;
        private set;
    }

    private static void InitCustomComponents()
    {
        SpriteCollection = UnityGameFramework.Runtime.GameEntry.GetComponent<SpriteCollectionComponent>();
    }
}
//}
