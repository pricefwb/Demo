using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Hotfix.Framework
{
    public class ProcedureGameEntry : ProcedureBase
    {
        static readonly string s_GameEntryPrefab = "Assets/Game/Hotfix/Res/Base/GameEntry.prefab";
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            LoadGameEntryPrefab();
        }

        void LoadGameEntryPrefab()
        {
            GameEntryMain.Resource.LoadAsset(s_GameEntryPrefab, new LoadAssetCallbacks(OnLoadGameEntryPrefabSuccess, OnLoadAssetFail));
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        void OnLoadGameEntryPrefabSuccess(string assetName, object asset, float duration, object userData)
        {
            if (GameObject.Find("GameFramework/GameEnter") != null)
            {
                return;
            }

            GameObject gameObject = Object.Instantiate((GameObject)asset, GameObject.Find("GameFramework").transform, true);
            gameObject.name = "GameEnter";
            gameObject.transform.position = Vector3.zero;
        }

        void OnLoadAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            //Log.Error($"LoadAssetFailure, assetName: [ {assetName} ], status: [ {status} ], errorMessage: [ {errorMessage} ], userData: [ {userData} ]");
        }
    }
}