using System.Collections.Generic;
using System.IO;

namespace Game.Editor
{
    public static partial class UtilityEditor
    {
        public static class IO
        {
            public static bool IsDirectoryExists(string directoryPath)
            {
                return Directory.Exists(directoryPath);
            }

            public static void ClearDirectory(string directoryPath)
            {
                if (IsDirectoryExists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }
            }

            public static void CreateDirectory(string directoryPath)
            {
                if (!IsDirectoryExists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }

            //保证目录存在 并且清空目录里面的所有文件
            public static void EnsureDirectoryExistsAndEmpty(string directoryPath)
            {
                ClearDirectory(directoryPath);
                CreateDirectory(directoryPath);
            }

            public static bool CopyDirectory(string srcDirectoryPath, string targetDirectoryPath)
            {
                if (!IsDirectoryExists(srcDirectoryPath))
                {
                    return false;
                }

                CreateDirectory(targetDirectoryPath);

                //获得源文件下所有文件
                List<string> files = new List<string>(Directory.GetFiles(srcDirectoryPath));
                files.ForEach(f =>
                {
                    string destFile = Path.Combine(targetDirectoryPath, Path.GetFileName(f));
                    File.Copy(f, destFile, true); //覆盖模式
                });

                //获得源文件下所有目录文件
                List<string> folders = new List<string>(Directory.GetDirectories(srcDirectoryPath));
                folders.ForEach(f =>
                {
                    string destDir = Path.Combine(targetDirectoryPath, Path.GetFileName(f));
                    CopyDirectory(f, destDir); //递归实现子文件夹拷贝
                });
                return true;
            }

            public static bool IsFileExists(string directoryPath)
            {
                return File.Exists(directoryPath);
            }

            public static bool CopyFile(string srcFilePath, string targetFilePath)
            {
                if (!IsFileExists(srcFilePath))
                {
                    return false;
                }
                File.Copy(srcFilePath, targetFilePath, true);
                return true;
            }
        }
    }
}