using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HWTransCommon.XMLTool
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SerializerFactory<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public T Load(string strFileName)
        {
            //             using (Stream reader = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            //             {
            //                 XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            //                 return (T)xmlSerializer.Deserialize(reader);
            //             }
            string xmlString = File.ReadAllText(strFileName);
            return (T)LoadFromString(xmlString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public T LoadFromString(string builder)
        {
            using (StringReader reader = new StringReader(builder))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(reader);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targe"></param>
        /// <returns></returns>
        public string Save(T targe)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, targe);
                return writer.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targe"></param>
        /// <param name="strFileName"></param>
        public void Save(T targe, string strFileName)
        {
            var objectString = this.Save(targe);
            using (Stream writer = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(objectString);
                writer.Write(buffer, 0, buffer.Count());
                //                 XmlSerializer serializer = new XmlSerializer(typeof(T));
                //                 serializer.Serialize(writer, targe);
            }
        }
    }
}
