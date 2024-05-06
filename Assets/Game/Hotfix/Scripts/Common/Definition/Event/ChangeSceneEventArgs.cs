using GameFramework.Event;
using GameFramework;

namespace Game.Hotfix.Common
{
    public class ChangeSceneEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeSceneEventArgs).GetHashCode();

        public ChangeSceneEventArgs()
        {
            SceneAsset = null;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }


        public string SceneAsset
        {
            get;
            private set;
        }
        public object UserData
        {
            get;
            private set;
        }

        public static ChangeSceneEventArgs Create(string sceneAsset, object userData = null)
        {
            ChangeSceneEventArgs changeSceneEventArgs = ReferencePool.Acquire<ChangeSceneEventArgs>();
            changeSceneEventArgs.SceneAsset = sceneAsset;
            changeSceneEventArgs.UserData = userData;
            return changeSceneEventArgs;
        }

        public override void Clear()
        {
            SceneAsset = null;
        }
    }

}

