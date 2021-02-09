using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCommon.Config
{
    /// <summary>
    /// xml配置模板类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XMLConfigTemplate<T>: ConfigInterface
    {
        public virtual string ConfigPath { get; set; }

        public virtual void Load()
        {
            throw new NotImplementedException();
        }

        public virtual void Save()
        {
            throw new NotImplementedException();
        }
    }
}
