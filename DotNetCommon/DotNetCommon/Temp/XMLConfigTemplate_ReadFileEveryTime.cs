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
    /// 序列化模版 每次都需要序列化读取文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XMLConfigTemplate_ReadFileEveryTime<T> where T : new()
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
                if (System.IO.File.Exists(ConfigPath))
                {
                    var fact = new SerializerFactory<T>();
                    _instance = fact.Load(ConfigPath);
                }
                else
                {
                    _instance = new T();
                    var fact = new SerializerFactory<T>();
                    fact.Save(_instance, ConfigPath);
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        #endregion
    }
}
