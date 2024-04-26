using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BuildInfoSettings", menuName = "MyTool/Settings/BuildInfoSettings")]
    public class BuildInfoSettings : ScriptableObject
    {
        public enum ServerTypeEnum
        {
            Dev = 0,
            Test = 1,
            Release = 2,
        }

        [System.Serializable]
        public class ServerInfo
        {
            [HideInInspector] public ServerTypeEnum ServerType;
            public string HotfixUrl;
            public string ServerUrl;
        }


        [Header("服务器地址")]
        [SerializeField, EnumToggleButtons, OnValueChanged("OnServerTypeChanged")] ServerTypeEnum m_ServerType = ServerTypeEnum.Dev;
        public ServerTypeEnum ServerType => m_ServerType;

        [SerializeField, EnumToggleButtons] ServerInfo m_ServerInfo;

        [SerializeField, HideInInspector] List<ServerInfo> m_HotfixInfoList = new List<ServerInfo>();

        public ServerInfo GetServerInfo()
        {
            return GetServerInfo(m_ServerType);
        }

        ServerInfo GetServerInfo(ServerTypeEnum serverType)
        {
            var serverInfo = m_HotfixInfoList.Find(info => info.ServerType == serverType);
            if (serverInfo == null)
            {
                serverInfo = new ServerInfo
                {
                    ServerType = serverType
                };
                m_HotfixInfoList.Add(serverInfo);
            }
            return serverInfo;
        }

        private void OnServerTypeChanged()
        {
            m_ServerInfo = GetServerInfo();
        }
        public string GetHotfixUrl()
        {
            return $"{GetServerInfo().HotfixUrl}/{UtilityGame.Platform.GetRuntimePlatformName()}";
        }

        [Header("版本文件名")]
        [SerializeField] string m_VersionFile;
        public string VersionFile => m_VersionFile;



        [Header("App Url")]
        [SerializeField] string m_WindowsAppUrl;
        public string WindowsAppUrl => m_WindowsAppUrl;
        [SerializeField] string m_MacOSAppUrl;
        public string MacOSAppUrl => m_MacOSAppUrl;
        [SerializeField] string m_AndroidAppUrl;
        public string AndroidAppUrl => m_AndroidAppUrl;
        [SerializeField] string m_IOSAppUrl;
        public string IOSAppUrl => m_IOSAppUrl;
    }
}