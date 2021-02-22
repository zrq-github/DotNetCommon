using DotNetCommon.FileCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DotNetCommon.Config
{
    public class XMLConfigManage<T>
    {
        public virtual string ConfigFilePath { get; set; }

        /// <summary>
        /// 加载xml对象
        /// </summary>
        /// <returns></returns>
        /// <remarks>返回的xml取决于<see cref="ConfigFilePath"/></remarks>
        public virtual T Load()
        {
            string filePath = ConfigFilePath;
            return Load(filePath);
        }
        /// <summary>
        /// 从指定的文件加载对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public virtual T Load(string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            xmlSerializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

            FileStream fs = null;
            T obj = default(T);
            try
            {
                fs = new FileStream(filePath, FileMode.Open);
                obj = (T)xmlSerializer.Deserialize(fs);
            }
            catch
            { }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return obj;
        }

        /// <summary>
        /// 保存xml对象在<see cref="ConfigFilePath"/>文件中
        /// </summary>
        /// <remarks>保存本类</remarks>
        public virtual void Save()
        {
            string filePath = ConfigFilePath;
            Save(ConfigFilePath,this);
        }
        /// <summary>
        /// 保存xml文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="createFile">如果文件不存在是否自动创建文件</param>
        /// <param name="obj">需要保存的可序列化对象</param>
        public virtual void Save(string filePath, object obj, bool createFile = true)
        {
            try
            {
                if (createFile)
                {
                    if (!System.IO.File.Exists(filePath))
                    {
                        FolderHelper.CreateFile(filePath);
                    }
                }

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                TextWriter writer = new StreamWriter(filePath);
                xmlSerializer.Serialize(writer, obj);
                writer.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        protected virtual void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }
        protected virtual void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }
    }
}
