using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Hotfix
{
    public static class HotfixEntry
    {
        public static void Start()
        {
            Log.Debug("Hotfix Start");

            GameEntry.Resource.LoadAsset("Assets/Game/Hotfix/Res/Cube.prefab", typeof(GameObject), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    GameObject.Instantiate(asset as GameObject);
                },
                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("LoadAsset Failure");
                    Log.Error(status.ToString());
                    Log.Error(errorMessage);
                },
                (assetName, progress, userData) =>
                {
                    Log.Debug("LoadAsset Progress");
                    Log.Debug(progress.ToString());
                }
            ), null);

            GameEntry.Resource.LoadAsset("Assets/Game/Hotfix/Res/Cube_Red.prefab", typeof(GameObject), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    var redGo = GameObject.Instantiate(asset as GameObject);
                    redGo.transform.position = new Vector3(0, 1, 0);
                },
                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("LoadAsset Failure");
                    Log.Error(status.ToString());
                    Log.Error(errorMessage);
                },
                (assetName, progress, userData) =>
                {
                    Log.Debug("LoadAsset Progress");
                    Log.Debug(progress.ToString());
                }
            ), null);




            // 重置流程组件，初始化热更新流程。
            //GameEntry.Fsm.DestroyFsm<IProcedureManager>();
            //var procedureManager = GameFrameworkEntry.GetModule<IProcedureManager>();
            // ProcedureBase[] procedures =
            // {
            //     new ProcedurePreload(),
            //     new ProcedureChangeScene(),
            //     new ProcedureUI(),
            //     new ProcedureFight(),
            // };

            // procedureManager.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(), procedures);
            // procedureManager.StartProcedure<ProcedurePreload>();
        }
    }
}
