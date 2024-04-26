using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "HybridCLRSettings", menuName = "MyTool/Settings/HybridCLRSettings")]
    public class HybridCLRSettings : ScriptableObject
    {
        [Header("热更新入口")]
        [SerializeField] string m_HotfixMainAssemblyName;
        public string HotfixMainAssemblyName => m_HotfixMainAssemblyName;
        [SerializeField] string m_HotfixEntryClass;
        public string HotfixEntryClass => m_HotfixEntryClass;
        [SerializeField] string m_HotfixEntryMethod;
        public string HotfixEntryMethod => m_HotfixEntryMethod;


        [Header("程序集资源")]
        [SerializeField] string m_AssemblyAssetExtension;
        public string AssemblyAssetExtension => m_AssemblyAssetExtension;
        [SerializeField] string m_HotfixAssembliesDirectory;
        public string HotfixAssembliesDirectory => m_HotfixAssembliesDirectory;
        [SerializeField] string m_AOTMetaAssembliesDirectory;
        public string AOTMetaAssembliesDirectory => m_AOTMetaAssembliesDirectory;


        [Header("Build的时候自动同步")]
        [SerializeField, ReadOnly] public bool Enable = true;
        [SerializeField, ReadOnly] public List<string> HotfixAssemblies = new List<string>();
        [SerializeField, ReadOnly] public List<string> AOTMetaAssemblies = new List<string>();
    }
}