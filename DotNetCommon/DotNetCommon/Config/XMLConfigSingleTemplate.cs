﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCommon.Config
{
    public class XMLConfigSingleTemplate<T> : XMLConfigTemplate<T> where T : new()
    {
        public override string ConfigFilePath { get; set; } = Path.Combine(System.IO.Directory.GetCurrentDirectory(), typeof(T).Name + ".xml");

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
                            var fact = new XMLConfigTemplate<T>();
                            string filePath = fact.ConfigFilePath;
                            _instance = fact.Load();
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
        public override T Load(string filePath)
        {
            throw new NotSupportedException("单例类不支持指定路径加载");
        }
    }
}