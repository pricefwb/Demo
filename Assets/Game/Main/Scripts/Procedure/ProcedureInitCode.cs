using System.Linq;
using System.Reflection;
using UnityEngine;
using HybridCLR;
using GameFramework;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using System.Collections.Generic;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;

namespace Game.Main
{
    public class ProcedureInitCode : ProcedureBase
    {
        private Assembly m_MainHotfixAssembly = null;

        private int m_AOTDllLoadedCount = 0;

        private int m_HotfixDllLoadedCount = 0;

        private bool m_IsStart = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_MainHotfixAssembly = null;

#if UNITY_EDITOR
            m_MainHotfixAssembly = System.AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == Constant.Dll.HotfixMainDllName);
#else
            LoadAOTDlls();
            LoadHotfixDlls();
#endif
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_MainHotfixAssembly == null)
                return;
            if (!IsLoadAssemblyComplete())
                return;

            if (m_IsStart) return;

            Start(m_MainHotfixAssembly);
            m_IsStart = true;
        }

        private bool IsLoadAssemblyComplete()
        {
#if UNITY_EDITOR
            return true;
#else
            return m_AOTDllLoadedCount == Constant.Dll.AOTDllNames.Length && m_HotfixDllLoadedCount == Constant.Dll.HotfixDllNames.Length;      
#endif
        }

        private void LoadAOTDlls()
        {
            // 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
            // 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行。

            // 可以加载任意aot assembly的对应的dll。但要求dll必须与unity build过程中生成的裁剪后的dll一致，而不能直接使用原始dll。
            // 我们在BuildProcessor里添加了处理代码，这些裁剪后的dll在打包时自动被复制到 {项目目录}/HybridCLRData/AssembliesPostIl2CppStrip/{Target} 目录。

            // 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            // 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误。

            foreach (string dllName in Constant.Dll.AOTDllNames)
            {
                string assetName = Utility.Text.Format(Constant.Dll.DllPath, dllName);
                GameEntry.Resource.LoadAsset(assetName, new LoadAssetCallbacks(OnLoadAOTDllSuccess, OnLoadAssetFail), dllName);
            }
        }

        private void LoadHotfixDlls()
        {

            foreach (string dllName in Constant.Dll.HotfixDllNames)
            {
                string assetName = Utility.Text.Format(Constant.Dll.DllPath, dllName);
                GameEntry.Resource.LoadAsset(assetName, new LoadAssetCallbacks(OnLoadHotfixDllSuccess, OnLoadAssetFail), dllName);
            }
        }

        private void OnLoadAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Error($"LoadAssetFailure, assetName: [ {assetName} ], status: [ {status} ], errorMessage: [ {errorMessage} ], userData: [ {userData} ]");
            //TODO 加载Dll 失败提示
        }

        private void OnLoadAOTDllSuccess(string assetName, object asset, float duration, object userData)
        {
            Log.Debug($"LoadAOTDllSuccess, assetName: [ {assetName} ], duration: [ {duration} ], userData: [ {userData} ]");

            try
            {
                TextAsset dll = (TextAsset)asset;
                // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                var err = RuntimeApi.LoadMetadataForAOTAssembly(dll.bytes, HomologousImageMode.SuperSet);
                Log.Info($"LoadMetadataForAOTAssembly:{assetName}. ret:{err}");
            }
            catch (System.Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
            finally
            {
                m_AOTDllLoadedCount++;
            }

        }
        private void OnLoadHotfixDllSuccess(string assetName, object asset, float duration, object userData)
        {
            Log.Debug($"LoadHotfixDllSuccess, assetName: [ {assetName} ], duration: [ {duration} ], userData: [ {userData} ]");

            try
            {
                TextAsset dll = (TextAsset)asset;
                Assembly assembly = Assembly.Load(dll.bytes);
                if (string.Compare(Constant.Dll.HotfixMainDllName, userData as string, StringComparison.Ordinal) == 0)
                    m_MainHotfixAssembly = assembly;
            }
            catch (System.Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
            finally
            {
                m_HotfixDllLoadedCount++;
            }
        }

        private void Start(Assembly hotfixMainAssembly)
        {
            var hotfixMain = hotfixMainAssembly.GetType("Game.Hotfix.HotfixEntry");
            var main = hotfixMain.GetMethod("Start");
            main?.Invoke(null, null);
        }
    }
}
