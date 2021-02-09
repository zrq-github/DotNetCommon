using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HWTransCommon.XMLTool
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    /// <summary>
    /// 配置文件模板类 王耀祖 19/05/22
    /// </summary>
    /// <typeparam name="T">配置文件名(类名), 类必须是public</typeparam>
    public class XMLConfigTemplate<T> where T : new()
    {
        #region Instance
        public static string ConfigPath
        {
            get
            {
                var configPath = HWCommonTool.PathTool.HWPathUtility.GetConfigDirectory();
                return System.IO.Path.Combine(configPath, typeof(T).Name + ".xml");
            }
        }
        static T _instance = default(T);
        static object _locker = new object();
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            if (System.IO.File.Exists(ConfigPath))
                            {
                                var fact = new SerializerFactory<T>();
                                _instance = fact.Load(ConfigPath);
                            }
                            else
                            {
                                _instance = new T();
                            }
                        }
                    }
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public virtual void Save()
        {
            var log = log4net.LogManager.GetLogger(typeof(XMLConfigTemplate<T>));
            try
            {
                var fact = new SerializerFactory<T>();
                fact.Save(XMLConfigTemplate<T>.Instance, ConfigPath);
            }
            catch (Exception ex)
            {
                log.Error($"XMLConfigTemplate<{nameof(T)}>.Instance.{nameof(this.Save)}" + ex);
            }
        }

        /// <summary>
        /// 2019/12/25 李马元 从指定目录载入对象
        /// 从 strErrorMessage判断是否载入成功。
        /// 如果strErrorMessage 为空，则说明载入成功
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns>不会返回null</returns>
        public static T LoadFrom(string strFilePath, out string strErrorMessage)
        {
            strErrorMessage = "";
            try
            {
                if (System.IO.File.Exists(strFilePath))
                {
                    var fact = new SerializerFactory<T>();
                    return fact.Load(strFilePath);

                }
                else
                {
                    strErrorMessage = "文件不存在";
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = "载入失败 ： 异常信息：" + ex.Message;
            }
            strErrorMessage = "载入失败";
            return new T();
        }

        /// <summary>
        /// 增加一个序列化对象到指定路径的接口 2019/12/25 李马元 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="strFilePath"></param>
        ///  <returns>导出结果提示语。成功-"保存成功！"失败-保存失败加异常信息  "保存失败！失败原因：" + ex.Message</returns>
        ///  <remarks>20201215 更改 当文件不存在的时候, 相应的路径创建一个对应的文件</remarks>
        public static string SaveAs(T obj, string strFilePath)
        {
            string strRet = "保存成功！";
            try
            {
                var fact = new SerializerFactory<T>();
                fact.Save(obj, strFilePath);
                return strRet;
            }
            catch (Exception ex)
            {
                strRet = "保存失败！失败原因：" + ex.Message;
                return strRet;
            }
        }

        /// <summary>
        /// 保存在指定对象 如果文件不存在则创建文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public static bool SaveAndCreateFile(T obj, string strFilePath)
        {
            bool result = true;
            try
            {
                if (!System.IO.File.Exists(strFilePath))
                {
                    HWCommonTool.PathTool.FolderHelper.CreateFile(strFilePath);
                }
                var fact = new SerializerFactory<T>();
                fact.Save(obj, strFilePath);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        #endregion
    }
}
