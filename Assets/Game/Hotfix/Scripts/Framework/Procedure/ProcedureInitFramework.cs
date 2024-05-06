using System.Collections.Generic;
using System.Linq;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Hotfix.Framework
{
    public class ProcedureInitFramework : ProcedureBase
    {
        readonly string m_CustomGamePrefab = "Assets/Game/Hotfix/Res/Base/HotfixCustomGame.prefab";
        readonly string m_EntranceProcedureTypeName = "Game.Hotfix.Business.ProcedurePreload";
        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();
        ProcedureOwner m_ProcedureOwner;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_ProcedureOwner = procedureOwner;

            LoadHotfixCustomGamePrefab();
        }

        void LoadHotfixCustomGamePrefab()
        {
            GameEntryMain.Resource.LoadAsset(m_CustomGamePrefab, new LoadAssetCallbacks(OnLoadCustomGamePrefabSuccess, OnLoadCustomGamePrefabFail));
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!GameEntry.IsInitiated) return;

            var state = m_ProcedureOwner.GetAllStates().FirstOrDefault(s => s.GetType().FullName == m_EntranceProcedureTypeName);
            if (state != null)
            {
                Log.Info($"Entrance procedure found, typeName: [ {state.GetType()} ]");
                ChangeState(m_ProcedureOwner, state.GetType());
            }
            else
            {
                Log.Error($"Entrance procedure not found, typeName: [ {m_EntranceProcedureTypeName} ]");
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        void OnLoadCustomGamePrefabSuccess(string assetName, object asset, float duration, object userData)
        {
            if (GameObject.Find("GameFramework/HotfixCustomGame") != null)
            {
                return;
            }

            GameObject gameObject = Object.Instantiate((GameObject)asset, GameObject.Find("GameFramework").transform, true);
            gameObject.name = "HotfixCustomGame";
            gameObject.transform.position = Vector3.zero;
        }

        void OnLoadCustomGamePrefabFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Error($"OnLoadCustomGamePrefabFail, assetName: [ {assetName} ], status: [ {status} ], errorMessage: [ {errorMessage} ], userData: [ {userData} ]");
        }
    }
}