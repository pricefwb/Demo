using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.Hotfix.Framework
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
        }
    }
}
