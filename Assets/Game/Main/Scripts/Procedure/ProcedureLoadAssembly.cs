using System.Linq;
using System.Reflection;
using UnityEngine;
using HybridCLR;
using GameFramework;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;
using System.Collections.Generic;

namespace Game.Main
{
    public class ProcedureLoadAssembly : ProcedureBase
    {
        HybridCLRSettings m_HybridCLRSettings = null;
        Dictionary<string, TextAsset> m_AssemblyAssetDict = new Dictionary<string, TextAsset>();
        void Clear()
        {
            m_AssemblyAssetDict.Clear();
            m_HybridCLRSettings = null;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            LoadHybridCLRSettings();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            Clear();
        }

        void OnLoadAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Error($"LoadAssetFailure, assetName: [ {assetName} ], status: [ {status} ], errorMessage: [ {errorMessage} ], userData: [ {userData} ]");
        }

        bool IsHybridCLREnable()
        {
#if UNITY_EDITOR
            return false;
#else
            return m_HybridCLRSettings.Enable;
#endif
        }

        //STEP1
        void LoadHybridCLRSettings()
        {
            GameEntryMain.Resource.LoadAsset(UtilityMain.Asset.GetHybridCLRSettingsAsset(), typeof(HybridCLRSettings), new LoadAssetCallbacks(OnLoadHybridCLRSettingsSuccess, OnLoadAssetFail));
        }

        void OnLoadHybridCLRSettingsSuccess(string assetName, object asset, float duration, object userData)
        {
            m_HybridCLRSettings = (HybridCLRSettings)asset;

            LoadAssemblyAssets();
        }

        //STEP2
        void LoadAssemblyAssets()
        {
            if (IsHybridCLREnable())
            {
                foreach (string name in m_HybridCLRSettings.HotfixAssemblies)
                {
                    string assetName = Utility.Text.Format("{0}/{1}{2}", m_HybridCLRSettings.HotfixAssembliesDirectory, name, m_HybridCLRSettings.AssemblyAssetExtension);
                    GameEntryMain.Resource.LoadAsset(assetName, new LoadAssetCallbacks(OnLoadAssemblyAssetSuccess, OnLoadAssetFail), name);
                }

                foreach (string name in m_HybridCLRSettings.AOTMetaAssemblies)
                {
                    string assetName = Utility.Text.Format("{0}/{1}{2}", m_HybridCLRSettings.AOTMetaAssembliesDirectory, name, m_HybridCLRSettings.AssemblyAssetExtension);
                    GameEntryMain.Resource.LoadAsset(assetName, new LoadAssetCallbacks(OnLoadAssemblyAssetSuccess, OnLoadAssetFail), name);
                }
            }
            else
            {
                LoadAssemblies();
            }
        }

        void OnLoadAssemblyAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            Log.Debug($"LoadAssemblyAsset, assetName: [ {assetName} ], duration: [ {duration} ], userData: [ {userData} ]");

            m_AssemblyAssetDict.Add(userData as string, asset as TextAsset);

            if (m_AssemblyAssetDict.Count == m_HybridCLRSettings.HotfixAssemblies.Count + m_HybridCLRSettings.AOTMetaAssemblies.Count)
            {
                LoadAssemblies();
            }
        }

        //STEP3
        void LoadAssemblies()
        {
            Assembly mainAssembly = null;
            var mainAssemblyName = m_HybridCLRSettings.HotfixMainAssemblyName;
            if (!IsHybridCLREnable())
            {
                mainAssembly = System.AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == mainAssemblyName);
            }
            else
            {
                try
                {
                    foreach (var AOTMetaName in m_HybridCLRSettings.AOTMetaAssemblies)
                    {
                        var dll = m_AssemblyAssetDict[AOTMetaName];
                        var err = RuntimeApi.LoadMetadataForAOTAssembly(dll.bytes, HomologousImageMode.SuperSet);
                        Log.Info($"LoadMetadataForAOTAssembly:{AOTMetaName}. ret:{err}");
                    }

                    foreach (var hotfixName in m_HybridCLRSettings.HotfixAssemblies)
                    {
                        var dll = m_AssemblyAssetDict[hotfixName];
                        var assembly = Assembly.Load(dll.bytes);
                        if (string.Compare(Utility.Text.Format("{0}.dll", mainAssemblyName), hotfixName, StringComparison.Ordinal) == 0)
                        {
                            mainAssembly = assembly;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Log.Error(e.Message);
                    throw;
                }
            }

            StartGame(mainAssembly);
        }



        //STEP4
        public static List<Assembly> GetHotfixAssemblies(List<string> hotfixAssemblyNames)
        {
            var hotfixAssemblies = new List<Assembly>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var name = Utility.Text.Format("{0}.dll", assembly.GetName().Name);
                if (hotfixAssemblyNames.Contains(name))
                {
                    hotfixAssemblies.Add(assembly);
                }
            }
            return hotfixAssemblies;
        }

        void StartGame(Assembly mainAssembly)
        {
            if (mainAssembly == null)
            {
                Log.Error("Main logic assembly missing.");
                return;
            }

            var mainClass = mainAssembly.GetType(m_HybridCLRSettings.HotfixEntryClass);
            if (mainClass == null)
            {
                Log.Error($"Main logic type '{m_HybridCLRSettings.HotfixEntryClass}' missing.");
                return;
            }

            var mainMethod = mainClass.GetMethod(m_HybridCLRSettings.HotfixEntryMethod);
            if (mainMethod == null)
            {
                Log.Error($"Main logic entry method '{m_HybridCLRSettings.HotfixEntryMethod}' missing.");
                return;
            }

            var hotfixAssemblies = GetHotfixAssemblies(m_HybridCLRSettings.HotfixAssemblies);
            Log.Info($"HotfixAssemblies count:{hotfixAssemblies.Count}");
            if (hotfixAssemblies.Count != m_HybridCLRSettings.HotfixAssemblies.Count)
            {
                Log.Error("Please check HybridCLRSettings.asset file's HotfixAssemblies collection field to ensure that the hotfix assembly has been collected.");
                return;
            }

            object[] objects = new object[] { new object[] { hotfixAssemblies } };
            mainMethod.Invoke(mainClass, objects);
        }
    }
}
