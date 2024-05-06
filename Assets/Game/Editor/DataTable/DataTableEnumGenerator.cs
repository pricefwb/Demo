using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEditor;
using GameFramework;

namespace Game.Editor.DataTableTools
{
    public static class DataTableEnumGenerator
    {
        private readonly static string EnumTemplateFileName = "Assets/Game/Editor/Configs/EnumTemplate.txt";
        private readonly static string GeneratePath = "Assets/Game/Hotfix/Scripts/Common/DataTable/Enum";
        private readonly static string CSharpCodeNameSpace = "Game.Hotfix.Common";

        public static void GenerateEnumFile(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            dataTableProcessor.SetCodeTemplate(EnumTemplateFileName, Encoding.UTF8);
            dataTableProcessor.SetCodeGenerator(DataTableCodeGenerator);

            string csharpCodeFileName = Utility.Path.GetRegularPath(Path.Combine(GeneratePath, "Enum" + dataTableName + ".cs"));
            if (!dataTableProcessor.GenerateCodeFile(csharpCodeFileName, Encoding.UTF8, dataTableName) && File.Exists(csharpCodeFileName))
            {
                File.Delete(csharpCodeFileName);
            }
        }

        private static void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
        {
            string dataTableName = (string)userData;

            codeContent.Replace("__DATA_TABLE_CREATE_TIME__", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            codeContent.Replace("__DATA_TABLE_NAME_SPACE__", CSharpCodeNameSpace);
            codeContent.Replace("__DATA_TABLE_ENUM_NAME__", "Enum" + dataTableName);
            //codeContent.Replace("__DATA_TABLE_COMMENT__", dataTableProcessor.GetValue(0, 1) + "。");
            codeContent.Replace("__DATA_TABLE_ENUM_ITEM__", GenerateEnumItems(dataTableProcessor));
        }

        private static string GenerateEnumItems(DataTableProcessor dataTableProcessor)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool firstProperty = true;

            int startRow = 4;

            stringBuilder
             .AppendLine("        /// <summary>")
             .AppendFormat("        /// {0}", "无").AppendLine()
             .AppendLine("        /// </summary>")
             .AppendFormat("        {0} = {1},", "None", "0").AppendLine().AppendLine();

            for (int i = startRow; i < dataTableProcessor.RawRowCount; i++)
            {
                int index = i - startRow;

                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    stringBuilder.AppendLine().AppendLine();
                }

                stringBuilder
                    .AppendLine("        /// <summary>")
                    .AppendFormat("        /// {0}", dataTableProcessor.GetValue(i, 2)).AppendLine()
                    .AppendLine("        /// </summary>")
                    .AppendFormat("        {0} = {1},", dataTableProcessor.GetValue(i, 3), dataTableProcessor.GetValue(i, 1));
            }
            return stringBuilder.ToString();
        }
    }
}


