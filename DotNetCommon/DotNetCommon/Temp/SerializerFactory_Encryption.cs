using HWTransCommon.CommonFunction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HWTransCommon.XMLTool
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    /// <summary>
    /// 
    /// </summary>
    public abstract class EncryptionWrapper
    {
        private string PlaceHolder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="placeHolder"></param>
        public EncryptionWrapper(string placeHolder)
        {
            PlaceHolder = placeHolder;
        }
        /// <summary>
        /// 进行字符串加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public abstract string EncryptString(string inputString);
        /// <summary>
        /// 进行字符串解密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public abstract string DencryptString(string inputString);
    }
   
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    /// <summary>
    /// 
    /// </summary>
    public class Encryption_MD5 : EncryptionWrapper
    {
        //定义密钥
        byte[] MD5_Key = { 102, 16, 93, 156, 78, 4, 218, 32 };
        //定义偏移量       
        byte[] MD5_IV = { 55, 103, 246, 79, 36, 99, 167, 3 };
        /// <summary>
        /// 
        /// </summary>
        public Encryption_MD5() : base("placeHolder")
        {

        }
        /// <summary>  
        /// MD5加密  
        /// </summary>  
        /// <param name="strSource">需要加密的字符串</param>  
        /// <returns>MD5加密后的字符串</returns>  
        string Md5Encrypt(string strSource)
        {
            //把字符串放到byte数组中  
            byte[] bytIn = System.Text.Encoding.UTF8.GetBytes(strSource);

            //实例DES加密类  
            var mobjCryptoService = new DESCryptoServiceProvider();
            mobjCryptoService.Key = MD5_Key;
            mobjCryptoService.IV = MD5_IV;
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            //实例MemoryStream流加密密文件  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            string strOut = System.Convert.ToBase64String(ms.ToArray());
            return strOut;
        }

        /// <summary>  
        /// MD5解密  
        /// </summary>  
        /// <param name="Source">需要解密的字符串</param>  
        /// <returns>MD5解密后的字符串</returns>  
        string Md5Decrypt(string Source)
        {
            //将解密字符串转换成字节数组  
            byte[] bytIn = System.Convert.FromBase64String(Source);
            var mobjCryptoService = new DESCryptoServiceProvider();
            mobjCryptoService.Key = MD5_Key;
            mobjCryptoService.IV = MD5_IV;
            //实例流进行解密  
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader strd = new StreamReader(cs, Encoding.UTF8);
            return strd.ReadToEnd();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public override string EncryptString(string inputString)
        {
            return Md5Encrypt(inputString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public override string DencryptString(string inputString)
        {
            return Md5Decrypt(inputString);
        }
    }

    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="EncryptionClass"></typeparam>
    public class SerializerFactory_Encryption<T, EncryptionClass> where EncryptionClass : EncryptionWrapper, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public T Load(string strFileName)
        {
            try
            {
                string xmlString = File.ReadAllText(strFileName);
                var encryptor = new EncryptionClass();
                var unEncryptorXmlString = encryptor.DencryptString(xmlString);
                return (T)LoadFromString(unEncryptorXmlString);
            }
            catch (Exception ex)
            {
                 log4netTool.Error("SerializerFactory_Encryption 加密序列化失败 : " + strFileName);
            }
            return default(T);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        T LoadFromString(string builder)
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
        string Save(T targe)
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
            var encryptor = new EncryptionClass();
            var encryptorXmlString = encryptor.EncryptString(objectString);
            using (Stream writer = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(encryptorXmlString);
                writer.Write(buffer, 0, buffer.Count());
            }
        }
    }
}
