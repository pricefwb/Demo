using UnityEngine;

namespace Game
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntryMain : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
        }
    }
}
