using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCommon.Config
{
    public interface IXMLConfig<T>
    {
        /// <summary>
        /// Xml文件路径
        /// </summary>
        string XMLFilePath { get; set; }

        T Load();
        T Load(string filePath);

        void Save();
        void Save(string filePath, object obj, bool createFile = true);
    }
}
