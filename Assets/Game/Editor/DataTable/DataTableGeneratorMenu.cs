//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using Game.Hotfix.Business;
using GameFramework;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        //[MenuItem("MyTools/DataTable/Generate DataTables", false, 1)]
        private static void GenerateDataTables()
        {
            // SyncDataTableSettings();

            foreach (string dataTableName in UtilityEditor.Settings.DataTableSettings.GetAllDataTableNames())
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }


        //[MenuItem("MyTools/DataTable/Generate DataTable Code", false, 2)]
        private static void GenerateDataTableCode()
        {
            //SyncDataTableSettings();

            foreach (string dataTableName in UtilityEditor.Settings.DataTableSettings.GetAllDataTableNames())
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }


            AssetDatabase.Refresh();
        }

        //[MenuItem("MyTools/DataTable/Generate DataTable Enum", false, 3)]
        private static void GenerateDataTableEnum()
        {
            //SyncDataTableSettings();

            foreach (string dataTableName in UtilityEditor.Settings.DataTableSettings.GetNeedEnumDataTableNames())
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableEnumGenerator.GenerateEnumFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }

        //  [MenuItem("MyTools/DataTable/Sync DataTable Settings", false, 4)]
        public static void SyncDataTableSettings()
        {
            var dataTableSettings = UtilityEditor.Settings.DataTableSettings;
            var oldDataTableInfos = new List<DataTableSettings.DataTableInfo>(dataTableSettings.DataTableInfos);
            dataTableSettings.DataTableInfos.Clear();

            var names = DataTableGenerator.GetDataTableNames();
            names.Sort();
            foreach (var name in names)
            {
                var dataTableInfo = oldDataTableInfos.Find(info => info.Name == name);
                if (dataTableInfo == null)
                {
                    dataTableInfo = new DataTableSettings.DataTableInfo
                    {
                        Name = name,
                        NeedEnum = false
                    };
                }

                dataTableSettings.DataTableInfos.Add(dataTableInfo);
            }

            EditorUtility.SetDirty(UtilityEditor.Settings.DataTableSettings);
            AssetDatabase.Refresh();
        }

        [MenuItem("MyTools/DataTable/Generate", false, 100)]
        public static void GenerateAll()
        {
            SyncDataTableSettings();
            GenerateDataTables();
            GenerateDataTableCode();
            GenerateDataTableEnum();
        }
    }
}
