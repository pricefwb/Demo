
using System;
using GameFramework.DataTable;
using Game.Hotfix.Common;
using UnityGameFramework.Runtime;

public static class DataTableExtension
{
    private const string DataRowClassPrefixName = "Game.Hotfix.Common.DR";

    public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName, string dataTableAssetName, object userData = null)
    {
        if (string.IsNullOrEmpty(dataTableName))
        {
            Log.Warning("Data table name is invalid.");
            return;
        }

        string[] splitedNames = dataTableName.Split('_');
        if (splitedNames.Length > 2)
        {
            Log.Warning("Data table name is invalid.");
            return;
        }

        string dataRowClassName = DataRowClassPrefixName + splitedNames[0];
        Type dataRowType = Type.GetType(dataRowClassName);
        if (dataRowType == null)
        {
            Log.Warning("Can not get data row type with class name '{0}'.", dataRowClassName);
            return;
        }

        string name = splitedNames.Length > 1 ? splitedNames[1] : null;
        DataTableBase dataTable = dataTableComponent.CreateDataTable(dataRowType, name);
        dataTable.ReadData(dataTableAssetName, Constant.AssetPriority.DataTableAsset, userData);
    }
}