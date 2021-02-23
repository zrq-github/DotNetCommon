using DotNetCommon.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetCommon.Clone;

namespace Test
{

    public class XMLConfigSingleTestClass : XMLConfigSingleTemplate<XMLConfigSingleTestClass>
    {
        public override string XMLFilePath { get; set; } = "实例";

        public string OrderDate = "OrderDate";
        public XmlDictionary<string, string> KeyValuePairs;
    }

    #region 序列化测试类
    [Serializable]
    public class ObjectCloneTestClass
    {
        public string name;
        public string path;
    }
    #endregion

    public class AAATest
    {
        int a = 0;
        int b = 1;
    }

    class Program
    {
        static void Main(string[] args)
        {
            #region 序列化克隆ObjectClone
            //// 基本类型测试
            //ObjectCloneTestClass objectClone1 = new ObjectCloneTestClass();
            //objectClone1.name = "one";
            //objectClone1.path = "one";
            //ObjectCloneTestClass objectClone2 = objectClone1.ToCloneBySerialize();
            //objectClone1.name = "OneTow";
            //objectClone1.path = "OneTow";
            //System.Console.WriteLine(objectClone1.name + "\n" + objectClone1.path);
            //System.Console.WriteLine(objectClone2.name + "\n" + objectClone2.path);
            #endregion

            #region 保存测试
            //XMLConfigSingleTestClass.Instance.OrderDate = "XMLConfigSingleTestClass";
            //XMLConfigSingleTestClass.Instance.KeyValuePairs = new XmlDictionary<string, string>();
            //XMLConfigSingleTestClass.Instance.KeyValuePairs.Add("1.", "1..");
            //XMLConfigSingleTestClass.Instance.Save();
            #endregion
            #region 读取测试

            var b = XMLConfigSingleTestClass.Instance.Load();
            #endregion

            //XMLConfigManage<XMLConfigSingleTestClass> xMLConfig = new XMLConfigManage<XMLConfigSingleTestClass>();

            //Object obj = null;
            //obj.ToCloneBySerialize();
        }
    }
}
