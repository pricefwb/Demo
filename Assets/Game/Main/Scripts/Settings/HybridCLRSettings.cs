using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "HybridCLRSettings", menuName = "MyTool/Settings/HybridCLRSettings")]
    public class HybridCLRSettings : ScriptableObject
    {
        [Header("是否启用热更新")]
        public bool Enable = true;

        [Header("Hotfix")]
        public string HotfixMainAssemblyName = "Game.Hotfix";
        public string HotfixEntryClass = "Game.Hotfix.HotfixEntry";
        public string HotfixEntryMethod = "Entrance";

        [Header("Read Only")]
        public List<string> HotfixAssemblies = new List<string>();
        public List<string> AOTMetaAssemblies = new List<string>();

        [field: SerializeField] public string AssemblyAssetExtension { private set; get; } = ".bytes";
        [field: SerializeField] public string HotfixAssembliesDirectory { private set; get; } = "Assets/Game/Hotfix/Assemblies/Hotfix";
        [field: SerializeField] public string AOTMetaAssembliesDirectory { private set; get; } = "Assets/Game/Hotfix/Assemblies/AOTMeta";

    }
}