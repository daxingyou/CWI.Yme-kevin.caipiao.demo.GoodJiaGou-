using log4net;
using System;
using System.IO;
using System.Web;

namespace Yme.Util.Log
{
    /// <summary>
    /// 日志初始化
    /// </summary>
    public class LogFactory
    {
        static LogFactory()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var logFile = "/XmlConfig/log4net.config";
            if (!string.IsNullOrWhiteSpace(path))
            {
                path = string.Format("{0}{1}", path, logFile);
            }
            else
            {
                path = HttpContext.Current.Server.MapPath(logFile);
            }
            FileInfo configFile = new FileInfo(path);

            log4net.Config.XmlConfigurator.Configure(configFile);
        }
        public static Log GetLogger(Type type)
        {
            return new Log(LogManager.GetLogger(type));
        }
        public static Log GetLogger(string str)
        {
            return new Log(LogManager.GetLogger(str));
        }
    }
}
