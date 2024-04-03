using System;
namespace Game.Main
{
    public static partial class Constant
    {
        /// <summary>
        /// 设置。
        /// </summary>
        public static class Dll
        {
            public const string DllPath = "Assets/Game/Hotfix/Res/Dlls/{0}.dll.bytes";
            public const string HotfixMainDllName = "Game.Hotfix";

            public static readonly String[] HotfixDllNames =
             {
                "Game.Hotfix",
            };

            public static readonly String[] AOTDllNames =
            {
                "mscorlib",
                "System",
                "System.Core", // 如果使用了Linq，需要这个
            };


        }
    }
}
