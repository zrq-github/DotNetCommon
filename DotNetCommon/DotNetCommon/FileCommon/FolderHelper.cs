using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCommon.FileCommon
{
    class FolderHelper
    {
        /// <summary>
        /// 复制文件夹的递归
        /// </summary>
        /// <param name="sourceFolderName"></param>
        /// <param name="destFolderName"></param>
        /// <param name="overwrite"></param>
        /// <param name="arrExtenion">指定复制的文件后缀</param>
        /// <returns>返回提示信息，成功，返回""</returns>
        public static string CopyFun(string sourceFolderName, string destFolderName, bool overwrite = false, string[] arrExtenion = null)
        {
            try
            {
                var listdir = Directory.GetDirectories(sourceFolderName);
                if (listdir != null && listdir.Length > 0)
                {
                    foreach (var item in listdir)
                    {
                        var forlders = item.Split('\\');
                        var lastDirectory = forlders[forlders.Length - 1];
                        var dest = Path.Combine(destFolderName, lastDirectory);
                        if (!Directory.Exists(dest))
                        {
                            Directory.CreateDirectory(dest);
                        }

                        CopyFun(item, dest, overwrite, arrExtenion);
                    }
                }

                var list = Directory.GetFiles(sourceFolderName);
                if (list != null && list.Length > 0)
                {
                    foreach (var item in list)
                    {
                        string strExtenion = Path.GetExtension(item).ToLower();
                        if (arrExtenion != null && !arrExtenion.Contains(strExtenion))
                        {
                            continue;
                        }
                        var sourceFileName = Path.GetFileName(item);
                        File.Copy(item, Path.Combine(destFolderName, sourceFileName), overwrite);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        /// <summary>
        /// 创建文件  并解除占用
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <returns> 根据filePath的位置 自动创建文件夹以及文件</returns>
        public static void CreateFile(string filePath)
        {
            string dicPath = System.IO.Path.GetDirectoryName(filePath);

            try
            {
                if (!Directory.Exists(dicPath))
                {
                    Directory.CreateDirectory(dicPath);
                }
                var fileStream = File.Create(filePath);
                fileStream.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
