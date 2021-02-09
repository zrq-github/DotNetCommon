using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HWCommonTool.PathTool;
using HWTransCommon.CommonFunction;

namespace HWTransCommon.RevitTool
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    class ReadHelpInfoXMLTool
    {
        private static ReadHelpInfoXMLTool _instance = null;
        static List<Tuple<string, string, string>> lisHelpInfo = new List<Tuple<string, string, string>>();

        public static void ClearData()
        {
            lisHelpInfo.Clear();
            _instance = null;
        }
        public static bool ReadHelpInfoByCommandId(string strId, out string strToolTip, out string strTipURL)
        {
            if (_instance == null)
            {
                _instance = new ReadHelpInfoXMLTool();
            }

            strToolTip = "";
            strTipURL = "";
            if (lisHelpInfo.Count ==0)
            {
                return false;
            }
            int index = lisHelpInfo.FindIndex(p=>p.Item1.Equals(strId));
            if (index ==-1)
            {
                return false;
            }
            strToolTip = lisHelpInfo[index].Item2;
            strTipURL = lisHelpInfo[index].Item3;
            return true;
        }

        private ReadHelpInfoXMLTool()
        {
            InitData();
        }
        private static void InitData()
        {
            try
            {
                string strConfigPath = HWPathUtility.GetConfigDirectory();
                string strXmlDir = System.IO.Path.Combine(strConfigPath, "HelpInfo.xml");

                //读取节点中的值   
                XmlDocument doc = new XmlDocument();
                doc.Load(strXmlDir);
                 XmlNode xncounts = doc.SelectSingleNode("HelpInfoConfig");
                XmlElement xe = (XmlElement)xncounts;
                XmlNodeList myList = xncounts.ChildNodes;

                int count = myList.Count;
                foreach (XmlNode node in myList)
                {
                     XmlNodeList myList1 = node.ChildNodes;
                     string strid = node.Attributes[0].Value ;
                    List<int> data = new List<int>();
                    string strtooltip = myList1.Item(0).Attributes[0].Value;
                    string strurl = myList1.Item(1).Attributes[0].Value;
                    lisHelpInfo.Add(new Tuple<string, string, string>(strid, strtooltip, strurl));
                }
            }
            catch (Exception ex)
            {
                 log4netTool.Error(ex.ToString());
            }
        }
    }
}
