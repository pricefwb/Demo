
using System.Collections.Generic;
using Game.Hotfix.Common;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System.Threading.Tasks;
using Game.Hotfix.Framework.Await;
using UnityGameFramework.Runtime;

namespace Game.Hotfix.Business
{
    public class ProcedurePreload : ProcedureCustomBase
    {
        private bool m_ResourceInitComplete = false;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("ProcedurePreload OnEnter =========>>>>>>>>>>>>");

            PreloadResourcesAsync();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_ResourceInitComplete) return;

            ToGame();
        }

        private void ToGame()
        {
            Log.Info("Preload complete, change to game procedure");
            //ChangeScene(Constant.Scene.UI);
        }


        private async void PreloadResourcesAsync()
        {
            await UtilityGame.Settings.Initialize();

            var tasks = new List<Task>();
            tasks.AddRange(LoadDataTables());

            await Task.WhenAll(tasks);

            m_ResourceInitComplete = true;
        }

        List<Task> LoadDataTables()
        {
            List<Task> tasks = new List<Task>();

            foreach (string dataTableName in UtilityGame.Settings.DataTableSettings.GetAllDataTableNames())
            {
                string dataTableAssetName = AssetUtility.DataTable.GetDataTableAsset(dataTableName, false);
                tasks.Add(GameEntry.DataTable.PreLoadDataTableAsync(dataTableName, dataTableAssetName, this));
            }

            return tasks;
        }
    }
}
