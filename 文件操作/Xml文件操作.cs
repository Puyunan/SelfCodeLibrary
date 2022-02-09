 /*
 * XML文件的保存与读取
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace XML
{
    /// <summary>
    /// 二进制xml文件
    /// </summary>
    class XMLFileBinary
    {
        /// <summary>
        /// 存储数据为xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">文件名</param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool Save<T>(string filename, T c)
        {
            Stream stream = null;
            try
            {
                stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            catch
            {
                return false;
            }
            BinaryFormatter f = new BinaryFormatter();
            try
            {
                //序列化
                f.Serialize(stream, c);
            }
            catch
            {
                stream.Close();
                return false;
            }
            stream.Close();
            return true;
        }

        /// <summary>
        /// 打开xml文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FileNmae">文件名</param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool Open<T>(string FileNmae, ref T c)
        {
            //文件是否存在
            if (!File.Exists(FileNmae))
                return false;
            Stream stream = null;
            try
            {
                stream = new FileStream(FileNmae, FileMode.Open);
            }
            catch
            {
                return false;
            }
            BinaryFormatter df = new BinaryFormatter();
            try
            {
                object obj = df.Deserialize(stream);
                c = (T)obj;
            }
            catch
            {
                stream.Close();
                return false;
            }
            stream.Close();
            return true;
        }

        /// <summary>
        /// 打开stream文件流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">文件流</param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool Open<T>(Stream stream, ref T c)
        {
            BinaryFormatter df = new BinaryFormatter();
            try
            {
                object obj = df.Deserialize(stream);
                c = (T)obj;
            }
            catch
            {
                stream.Close();
                return false;
            }
            stream.Close();
            return true;
        }
    }
    /// <summary>
    /// xml文件
    /// </summary>
    class XMLFile
    {
        /// <summary>
        /// 将数据存为xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">文件名</param>
        /// <param name="c">c</param>
        /// <returns>
        /// 
        /// </returns>
        public static bool Save<T>(string filename, T c)
        {
            Stream stream = null;

            try
            {
                stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            catch
            {
                return false;
            }
            try
            {

                XmlSerializer f = new XmlSerializer(typeof(T));
                f.Serialize(stream, c);
            }
            catch
            {
                stream.Close();
                return false;
            }

            stream.Close();
            return true;
        }

        /// <summary>
        /// 打开xml文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FileName">文件名</param>
        /// <param name="c">c</param>
        /// <returns>
        /// 
        /// </returns>
        public static bool Open<T>(string FileName, ref T c)
        {
            if (!File.Exists(FileName))
                return false;

            Stream stream = null;

            try
            {
                stream = new FileStream(FileName, FileMode.Open);
            }
            catch
            {
                return false;
            }

            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                object obj = deserializer.Deserialize(stream);
                c = (T)obj;
            }
            catch
            {
                stream.Close();
                return false;
            }

            stream.Close();
            return true;
        }

        /// <summary>
        /// 打开文件流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">文件流</param>
        /// <param name="c">c</param>
        /// <returns>
        /// 
        /// </returns>
        public static bool Open<T>(Stream stream, ref T c)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                object obj = deserializer.Deserialize(stream);
                c = (T)obj;
            }
            catch
            {
                stream.Close();
                return false;
            }

            stream.Close();
            return true;
        }
    }
}
