using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCommon.Clone
{
    /// <summary>
    /// 对类进行Clone
    /// </summary>
    /// <remarks>序列化克隆</remarks>
    public static class ObjectClone
    {
        /// <summary>
        /// 序列化克隆类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Thrown when object is not be serializable</exception>
        public static T ToCloneBySerialize<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
                // 如果这里出现错误,尝试在类上加[Serializable]
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
