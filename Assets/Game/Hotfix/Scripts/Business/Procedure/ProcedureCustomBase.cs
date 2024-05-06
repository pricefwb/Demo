
using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using Game.Hotfix.Common;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Hotfix.Business
{
    public abstract class ProcedureCustomBase : ProcedureBase
    {

        protected ProcedureOwner m_ProcedureOwner;

        protected bool m_ChangeScene = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_ProcedureOwner = procedureOwner;
            m_ChangeScene = false;

            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_ChangeScene)
            {
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }

        }

        protected void ChangeScene(string sceneAsset)
        {
            m_ChangeScene = true;
            m_ProcedureOwner.SetData<VarString>(Constant.ProcedureData.NextSceneId, sceneAsset);
        }

        protected void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            ChangeScene(ne.SceneAsset);
        }

    }
}
