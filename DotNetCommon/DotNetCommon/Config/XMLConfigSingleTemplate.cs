using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCommon.Config
{
    public class XMLConfigSingleTemplate<T> : XMLConfigManage<T> where T : new()
    {
        public override string XMLFilePath { get; set; } = "XMLConfigSingleTemplate";

        #region Instance
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
                            T tempInst = new T();
                            IXMLConfig<T> iXMLConfig = (IXMLConfig<T>)tempInst;
                            string filePath = iXMLConfig.XMLFilePath;
                            _instance = iXMLConfig.Load();
                            if (_instance == null)
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
        #endregion

        /// <summary>
        /// 单例类不支持指定路径加载
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public new T Load(string filePath)
        {
            throw new NotSupportedException("单例类不支持指定路径加载");
        }
    }
}
