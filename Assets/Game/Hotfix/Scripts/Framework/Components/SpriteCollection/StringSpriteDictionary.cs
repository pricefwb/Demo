#if !ODIN_INSPECTOR
using System;
using UnityEngine;

namespace Game.Hotfix.Framework
{
    [Serializable]
    public class StringSpriteDictionary : SerializableDictionary<string, Sprite> {}
}
#endif