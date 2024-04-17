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

namespace Game.Main
{
    public class ProcedureLoadAssembly : ProcedureBase
    {
        HybridCLRSettings m_HybridCLRSettings = null;
        Assembly m_MainHotfixAssembly = null;
        int m_LoadingAssemblyNum = -1;
        bool m_IsStarted = false;

        void Clear()
        {
            m_LoadingAssemblyNum = -1;
            m_MainHotfixAssembly = null;
            m_HybridCLRSettings = null;
            m_IsStarted = false;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Resource.LoadAsset(UtilityGame.Asset.GetSettingAsset("HybridCLRSettings"), typeof(HybridCLRSettings), new LoadAssetCallbacks(OnLoadHybridCLRSettingsSuccess, OnLoadAssetFail));
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_HybridCLRSettings == null) return;

            if (m_MainHotfixAssembly == null) return;

            if (m_LoadingAssemblyNum != 0) return;

            Start();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            Clear();
        }


        bool NeedLoadAssemblies()
        {
#if UNITY_EDITOR
            return false;
#else
            return m_HybridCLRSettings.Enable;
#endif
        }

        void LoadAssemblies()
        {
            m_LoadingAssemblyNum = 0;

            if (!NeedLoadAssemblies())
            {
                m_MainHotfixAssembly = System.AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == m_HybridCLRSettings.HotfixMainAssemblyName);
            }
            else
            {
                foreach (string name in m_HybridCLRSettings.HotfixAssemblies)
                {
                    m_LoadingAssemblyNum++;
                    string assetName = Utility.Text.Format("{0}/{1}.bytes", name);
                    GameEntry.Resource.LoadAsset(assetName, new LoadAssetCallbacks(OnLoadHotfixAssemblySuccess, OnLoadAssetFail), name);
                }

                foreach (string name in m_HybridCLRSettings.AOTMetaAssemblies)
                {
                    m_LoadingAssemblyNum++;
                    string assetName = Utility.Text.Format("{0}/{1}.bytes", name);
                    GameEntry.Resource.LoadAsset(assetName, new LoadAssetCallbacks(OnLoadAOTMetaAssemblySuccess, OnLoadAssetFail), name);
                }

            }
        }

        private void OnLoadHybridCLRSettingsSuccess(string assetName, object asset, float duration, object userData)
        {
            m_HybridCLRSettings = (HybridCLRSettings)asset;

            LoadAssemblies();
        }

        private void OnLoadAOTMetaAssemblySuccess(string assetName, object asset, float duration, object userData)
        {
            Log.Debug($"LoadAOTDllSuccess, assetName: [ {assetName} ], duration: [ {duration} ], userData: [ {userData} ]");

            try
            {
                TextAsset dll = (TextAsset)asset;
                // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                var err = RuntimeApi.LoadMetadataForAOTAssembly(dll.bytes, HomologousImageMode.SuperSet);
                Log.Info($"LoadMetadataForAOTAssembly:{assetName}. ret:{err}");
                m_LoadingAssemblyNum--;
            }
            catch (System.Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }
        private void OnLoadHotfixAssemblySuccess(string assetName, object asset, float duration, object userData)
        {
            Log.Debug($"LoadHotfixDllSuccess, assetName: [ {assetName} ], duration: [ {duration} ], userData: [ {userData} ]");

            try
            {
                TextAsset dll = (TextAsset)asset;
                Assembly assembly = Assembly.Load(dll.bytes);
                if (string.Compare(Utility.Text.Format("{0}.bytes", m_HybridCLRSettings.HotfixMainAssemblyName), userData as string, StringComparison.Ordinal) == 0)
                {
                    m_MainHotfixAssembly = assembly;
                }
                m_LoadingAssemblyNum--;
            }
            catch (System.Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        private void OnLoadAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Error($"LoadAssetFailure, assetName: [ {assetName} ], status: [ {status} ], errorMessage: [ {errorMessage} ], userData: [ {userData} ]");
        }

        private void Start()
        {
            if (m_MainHotfixAssembly == null)
            {
                Log.Error("MainHotfixAssembly is null");
                return;
            }
            if (m_IsStarted)
            {
                return;
            }
            m_IsStarted = true;

            var hotfixMain = m_MainHotfixAssembly.GetType("Game.Hotfix.HotfixEntry");
            Debug.Log(hotfixMain);
            var main = hotfixMain.GetMethod("Start");
            Debug.Log(main);
            main?.Invoke(null, null);
        }
    }
}
