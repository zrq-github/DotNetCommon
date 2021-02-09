using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HWCommonTool.PathTool;

namespace HWTransCommon
{
    /// <summary>
    /// 配置文件的操作
    /// </summary>
    public class UserConfigTool
    {
        private static string _configXmlDir = null;
        /// <summary>
        /// 读配置文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadConfigXML(string path, string fileName = "UserConfig.xml")
        {
            try
            {
                if (_configXmlDir == null || !_configXmlDir.Contains(fileName))
                {
                    string strConfigPath = HWPathUtility.GetConfigDirectory();
                    _configXmlDir = System.IO.Path.Combine(strConfigPath, fileName);
                }
                
                //读取节点中的值   
                XmlDocument doc = new XmlDocument();
                doc.Load(_configXmlDir);
                XmlNode xncounts = doc.SelectSingleNode(path);//
                return xncounts.InnerText.ToString();
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        /// <summary>
        /// 写配置文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="temp"></param>
        /// <param name="fileName"></param>
        public static void WriteConfigXML(string path, string temp, string fileName = "UserConfig.xml")
        {
            try
            {
                if (_configXmlDir == null || !_configXmlDir.Contains(fileName))
                {
                    string strConfigPath = HWPathUtility.GetConfigDirectory();
                    _configXmlDir = System.IO.Path.Combine(strConfigPath, fileName);
                }

                //读取节点中的值   
                XmlDocument doc = new XmlDocument();
                doc.Load(_configXmlDir);
                XmlNode xncounts = doc.SelectSingleNode(path);
                XmlElement xe = (XmlElement)xncounts;
                xe.InnerText = temp.ToString();
                doc.Save(_configXmlDir);
            }
            catch (Exception ex)
            {
            }
        }
       
        /// <summary>
        /// 读配置文件的方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="source"></param>
        public static void ReadConfigXML(string path, ref Dictionary<int, int> source)
        {
            source.Clear();
            try
            {
                if (_configXmlDir == null)
                {
                    string strConfigPath = HWPathUtility.GetConfigDirectory();
                    _configXmlDir = System.IO.Path.Combine(strConfigPath, "UserConfig.xml");
                }

                //读取节点中的值   
                XmlDocument doc = new XmlDocument();
                doc.Load(_configXmlDir);
                XmlNode xncounts = doc.SelectSingleNode(path);
                XmlElement xe = (XmlElement)xncounts;
                XmlNodeList myList = xncounts.ChildNodes;

                int count = myList.Count;               
                foreach (XmlNode node in myList)
                {
                    XmlNodeList myList1 = node.ChildNodes;
                    List<int> data = new List<int>();
                    foreach (XmlNode node1 in myList1)
                    {
                        XmlElement temp = (XmlElement)node1;
                        data.Add(int.Parse(temp.InnerText));
                    }
                    source.Add(data[0], data[1]);
                }
            }
            catch (Exception ex)
            {
            }
        }
     
        /// <summary>
        /// 写配置文件的方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="source"></param>
        public static void WriteConfigXML(string path, Dictionary<int, int> source)
        {
            try
            {
                if (_configXmlDir == null)
                {
                    string strConfigPath = HWPathUtility.GetConfigDirectory();
                    _configXmlDir = System.IO.Path.Combine(strConfigPath, "UserConfig.xml");
                }

                //创建节点 并写入值   
                XmlDocument doc = new XmlDocument();
                doc.Load(_configXmlDir);
                XmlNode xncounts = doc.SelectSingleNode(path);
                XmlElement root = (XmlElement)xncounts;
                root.RemoveAll(); //先全部删除

                int total = source.Count;
                for (int i = 0; i < total; i++)
                {
                    XmlElement child = doc.CreateElement("Row");
                    child.SetAttribute("dia", i.ToString());

                    XmlElement childsub1 = doc.CreateElement("dia");
                    childsub1.InnerText = source.ElementAt(i).Key.ToString();

                    XmlElement childsub2 = doc.CreateElement("sum");
                    childsub2.InnerText = source.ElementAt(i).Value.ToString();

                    child.AppendChild(childsub1);
                    child.AppendChild(childsub2);

                    root.AppendChild(child);
                }

                doc.Save(_configXmlDir);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 应该被舍弃的方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="source"></param>
        public static void ReadSecondaryStructureXML(string path, ref Dictionary<int, List<string>> source)
        {
            source.Clear();
            try
            {
                string strConfigPath = HWPathUtility.GetConfigDirectory();
                string  configXmlDir = System.IO.Path.Combine(strConfigPath, "SecondaryStructure.xml");
                //读取节点中的值   
                XmlDocument doc = new XmlDocument();
                doc.Load(configXmlDir);
                XmlNode xncounts = doc.SelectSingleNode(path);
                XmlElement xe = (XmlElement)xncounts;
                XmlNodeList myList = xncounts.ChildNodes;

                int count = myList.Count;
                foreach (XmlNode node in myList)
                {
                    int index = int.Parse(node.Attributes[0].Value);
                    XmlNodeList myList1 = node.ChildNodes;
                    List<int> data = new List<int>();
                    List<string> liststr = new List<string>();
                    foreach (XmlNode node1 in myList1)
                    {
                        XmlElement temp = (XmlElement)node1;
                        liststr.Add(temp.InnerText);
                    }
                    source.Add(index, liststr);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 二次结构的xml写入操作，应该被舍弃
        /// </summary>
        /// <param name="path"></param>
        /// <param name="source"></param>
        public static void WriteSecondaryStructureXML(string path, Dictionary<int, List<string>> source)
        {
            try
            {
                string strConfigPath = HWPathUtility.GetConfigDirectory();
                string configXmlDir = System.IO.Path.Combine(strConfigPath, "SecondaryStructure.xml");

                //创建节点 并写入值   
                XmlDocument doc = new XmlDocument();
                doc.Load(configXmlDir);
                XmlNode xncounts = doc.SelectSingleNode(path);
                XmlElement root = (XmlElement)xncounts;
                root.RemoveAll(); //先全部删除

                int total = source.Count;
                foreach(var item in source)
                {
                    XmlElement child = doc.CreateElement("Row");
                    child.SetAttribute("dia", item.Key.ToString());

                    if(item.Value.Count ==4)
                    {
                        XmlElement childsub1 = doc.CreateElement("OpeningMinWidth");
                        childsub1.InnerText = item.Value[0].ToString();

                        XmlElement childsub2 = doc.CreateElement("OpeningMaxWidth");
                        childsub2.InnerText = item.Value[1].ToString();

                        XmlElement childsub3 = doc.CreateElement("LintelWidth");
                        childsub3.InnerText = item.Value[2].ToString();

                        XmlElement childsub4 = doc.CreateElement("LintelHeight");
                        childsub4.InnerText = item.Value[3].ToString();

                        child.AppendChild(childsub1);
                        child.AppendChild(childsub2);
                        child.AppendChild(childsub3);
                        child.AppendChild(childsub4);
                    }
                    else if(item.Value.Count ==2)
                    {
                        XmlElement childsub1 = doc.CreateElement("WallWidth");
                        childsub1.InnerText = item.Key.ToString();

                        XmlElement childsub2 = doc.CreateElement("PeripheryWidth");
                        childsub2.InnerText = item.Value[0].ToString();

                        XmlElement childsub3 = doc.CreateElement("PeripheryHeight");
                        childsub3.InnerText = item.Value[1].ToString();

                        child.AppendChild(childsub1);
                        child.AppendChild(childsub2);
                        child.AppendChild(childsub3);
                    }
                    root.AppendChild(child);
                }
              

                doc.Save(_configXmlDir);
            }
            catch (Exception ex)
            {
            }
        }


    }
}
