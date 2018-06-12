using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Yme.Util
{
    public class XmlUtil
    {
        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <param name="filePath">xml文件路径</param>
        /// <returns>返回LoadXmlFile</returns>
        public static XmlDocument LoadXmlFile(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);

            return xmlDoc;
        }

        /// <summary>
        /// 加载XML片段
        /// </summary>
        /// <param name="xml">xml片段</param>
        /// <returns>返回XmlDocument</returns>
        public static XmlDocument LoadXmlString(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            return xmlDoc;
        }

        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch
            {
                throw new Exception("反序列化失败");
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化XML文件
        /// <summary>
        /// 序列化XML文件
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            //创建序列化对象
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();
            return str;
        }

        #endregion

        #region 将XML转换为DATATABLE
        /// <summary>
        /// 将XML转换为DATATABLE
        /// </summary>
        /// <param name="FileURL"></param>
        /// <returns></returns>
        public static DataTable XmlAnalysisArray()
        {
            try
            {
                string FileURL = System.Configuration.ConfigurationManager.AppSettings["Client"].ToString();
                DataSet ds = new DataSet();
                ds.ReadXml(FileURL);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// 将XML转换为DATATABLE
        /// </summary>
        /// <param name="FileURL"></param>
        /// <returns></returns>
        public static DataTable XmlAnalysisArray(string FileURL)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(FileURL);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message.ToString());
                return null;
            }
        }
        #endregion

        #region 获取对应XML节点的值
        /// <summary>
        /// 摘要:获取对应XML节点的值
        /// </summary>
        /// <param name="stringRoot">XML节点的标记</param>
        /// <returns>返回获取对应XML节点的值</returns>
        public static string XmlAnalysis(string stringRoot, string xml)
        {
            if (stringRoot.Equals("") == false)
            {
                try
                {
                    XmlDocument XmlLoad = new XmlDocument();
                    XmlLoad.LoadXml(xml);
                    return XmlLoad.DocumentElement.SelectSingleNode(stringRoot).InnerXml.Trim();
                }
                catch
                {
                    throw new Exception("解析XML失败");
                }
            }
            return "";
        }
        #endregion
    }

    /// <summary>
    /// XML操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class XmlDAL<T> where T : new()
    {
        //编码类型
        private static Encoding code = Encoding.GetEncoding("gb2312");

        /// <summary>
        /// 获取XML格式数据信息
        /// </summary>
        /// <param name="obj">数据类型</param>
        public static string GetXmlString(T obj)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Encoding = code;
            setting.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(stream, setting))
            {
                xs.Serialize(writer, obj);
            }
            return code.GetString(stream.ToArray());
        }

        #region 序列化&反序列化对象成字符串

        /// <summary> 
        /// 序列化对象 
        /// </summary> 
        /// <typeparam name=\"T\">对象类型</typeparam> 
        /// <param name=\"t\">对象</param> 
        /// <returns></returns> 
        public static string SerializeXml<T>(T t)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(t.GetType());
                xz.Serialize(sw, t);
                return sw.ToString();
            }
        }

        /// <summary> 
        /// 反序列化为对象 
        /// </summary> 
        /// <param name=\"type\">对象类型</param> 
        /// <param name=\"s\">对象序列化后的Xml字符串</param> 
        /// <returns></returns> 
        public static object DeserializeXml<T>(T t, string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                XmlSerializer xz = new XmlSerializer(t.GetType());
                return xz.Deserialize(sr);
            }
        }

        /// <summary> 
        /// 反序列化为对象 
        /// </summary> 
        /// <param name=\"type\">对象类型</param> 
        /// <param name=\"s\">对象序列化后的Xml字符串</param> 
        /// <returns></returns> 
        public static object DeserializeXml(Type type, string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                XmlSerializer xz = new XmlSerializer(type);
                return xz.Deserialize(sr);
            }
        }

        #endregion

        #region 读写XML文件

        /// <summary>
        /// 写XML
        /// </summary>
        /// <param name="path">包含文件名的XML文件完整路径</param>
        /// <param name="obj">数据类型</param>
        public static void WriteXml(string path, T obj)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            StreamWriter myWriter = new StreamWriter(path, false, code);
            mySerializer.Serialize(myWriter, obj);
            myWriter.Close();
        }

        /// <summary>
        /// 读XML
        /// </summary>
        /// <param name="path">包含文件名的XML文件完整路径</param>
        /// <returns>数据类型</returns>
        public static T ReadXml(string path)
        {
            T obj;
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            TextReader myReader = new StreamReader(path, code);
            obj = (T)mySerializer.Deserialize(myReader);
            myReader.Close();
            return obj;
        }

        #endregion
    } 
}
