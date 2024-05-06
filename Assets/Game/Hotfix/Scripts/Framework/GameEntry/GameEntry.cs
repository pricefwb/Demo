using Game.Hotfix.Framework.Await;
using UnityEngine;

// namespace Game.Hotfix.Framework
// {
/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameEntry : MonoBehaviour
{
    public static bool IsInitiated { get; private set; } = false;
    void Start()
    {
        InitBuiltinComponents();
        InitCustomComponents();

        AwaitableExtensions.SubscribeEvent();

        IsInitiated = true;
    }
}



