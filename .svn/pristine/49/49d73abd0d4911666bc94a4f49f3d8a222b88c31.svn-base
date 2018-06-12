//---------------------------------------------
// 版权信息：版权所有(C) 2014，COOLWI.COM
// 变更历史：
//      姓名          日期              说明
// --------------------------------------------
//      王军锋        2014/08/18        创建
//---------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using Yme.Util.Log;

namespace Yme.Util
{
    /// <summary>
    /// 内容模版配置助手
    /// </summary>
    public class MsgTemplateUtil
    {
        static string filepath = "";

        /// <summary>
        /// 消息内容
        /// </summary>
        public static MsgContentCollection MsgContentConfig
        {
            get;
            private set;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public static Dictionary<string, string> MsgDict
        {
            get;
            private set;
        }

        static MsgTemplateUtil()
        {
            var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\","");

            if (dir != null) filepath = Path.Combine(dir, @"Resources\MsgContent.xml");

            MsgDict = new Dictionary<string, string>();

            Load();
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private static void Load()
        {
            if (File.Exists(filepath))
            {
                try
                {
                    using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(MsgContentCollection), new Type[] { });
                        MsgContentConfig = ((MsgContentCollection)ser.Deserialize(fs));
                    }

                    if (MsgContentConfig != null)

                        foreach (var item in MsgContentConfig.MsgDescriptions)
                        {
                            MsgDict.Add(item.MsgKey, item.MsgFormat);
                        }
                }
                catch (Exception e)
                {
                    LogUtil.Error("加载消息配置文件错误." + e.ToString());
                }
            }
            else
            {
                LogUtil.Error("加载消息配置文件错误");
            }
        }
    }
}
