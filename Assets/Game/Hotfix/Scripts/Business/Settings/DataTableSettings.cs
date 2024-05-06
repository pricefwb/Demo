using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Hotfix.Business
{
    [CreateAssetMenu(fileName = "DataTableSettings", menuName = "MyTool/Settings/DataTableSettings")]
    public class DataTableSettings : ScriptableObject
    {
        [System.Serializable]
        public class DataTableInfo
        {
            [ReadOnly] public string Name;
            [SerializeField] public bool NeedEnum = false;
        }

        [SerializeField] public List<DataTableInfo> DataTableInfos = new List<DataTableInfo>();

        public List<string> GetAllDataTableNames()
        {
            List<string> names = new List<string>();
            foreach (var dataTableInfo in DataTableInfos)
            {
                names.Add(dataTableInfo.Name);
            }

            return names;
        }

        public List<string> GetNeedEnumDataTableNames()
        {
            List<string> names = new List<string>();

            foreach (var dataTableInfo in DataTableInfos)
            {
                if (dataTableInfo.NeedEnum)
                {
                    names.Add(dataTableInfo.Name);
                }
            }

            return names;
        }
    }
}