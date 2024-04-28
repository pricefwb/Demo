using GameFramework;
using UnityEngine;

namespace Game
{
    public static partial class UtilityMain
    {
        public static partial class Platform
        {
            public static string GetRuntimePlatformName()
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                        //return "Windows";
                        //TODO
                        return "Windows64";
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                        return "MacOS";

                    case RuntimePlatform.IPhonePlayer:
                        return "IOS";

                    case RuntimePlatform.Android:
                        return "Android";

                    default:
                        throw new System.NotSupportedException(Utility.Text.Format("Platform '{0}' is not supported.", Application.platform));
                }
            }
        }
    }
}