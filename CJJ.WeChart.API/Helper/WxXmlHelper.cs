using FastDev.Log;
using System;
using System.IO;
using System.Xml.Serialization;

namespace CJJ.WeChart.API.Helper
{
    public class WxXmlHelper
    {
        //public static WxEventModel DeserilizeXml(string xmldata)
        //{
        //    if (string.IsNullOrEmpty(xmldata))
        //    {
        //        return null;
        //    }
        //    var model = new WxEventModel();
        //    try
        //    {
        //        var xmldoc = new XmlDocument();
        //        xmldoc.Load(xmldata);
        //        //XmlElement rootElement = xmldoc.DocumentElement;
        //        //var namenodel = rootElement.SelectSingleNode(nameof(WxEventModel.FromUserName));
        //        var nodel = xmldoc.SelectSingleNode("xml").SelectSingleNode(nameof(WxEventModel.FromUserName));
        //        model.FromUserName = nodel.SelectSingleNode("CDATA").InnerText;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(ex, "DeserilizeXml");
        //    }

        //    return model;
        //}
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
            catch (Exception e)
            {
                LogHelper.WriteLog(e, "xml反序列化异常");
                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion
        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(stream, obj);
            }
            catch (InvalidOperationException e)
            {
                LogHelper.WriteLog(e, "xml序列化异常");
                return string.Empty;
            }
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            string str = sr.ReadToEnd();
            sr.Dispose();
            stream.Dispose();
            return str;
        }

        #endregion
    }
}
