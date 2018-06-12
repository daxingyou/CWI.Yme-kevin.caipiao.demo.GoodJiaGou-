using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Yme.Util.Log;

namespace Yme.Mcp.Demo
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 网站启动后第一个请求到达时执行
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// 捕获发生在应用程序中的错误
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Application_Error(object sender, EventArgs e)
        {
#if DEBUG
            Exception ex = Server.GetLastError();
            string message = string.Empty;
            if (ex != null)
            {
                if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
                {
                    LogUtil.Info(ex.ToString());
                }
                else
                {
                    LogUtil.Error("Application_Error:" + ex.ToString());
                }
                message = ex.ToString();
            }
            Server.ClearError();
            HttpContext.Current.Response.StatusCode = 500;
            HttpContext.Current.Response.TrySkipIisCustomErrors = true;

#else
            Exception exception = Server.GetLastError();
            Response.Clear();

            if (exception is HttpException && ((HttpException)exception).GetHttpCode() == 404)
            {
                Redirect404();
                LogUtil.Info(exception.ToString());
            }
            else
            {
                Redirect500();
                LogUtil.Error(exception.ToString());
            }

            //清除Exception,避免继续传递给上一级处理
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;
#endif
        }

        #region 私有

        /// <summary>
        /// 跳转404
        /// </summary>
        private void Redirect404()
        {
            Response.Redirect("/html/404.html");
        }

        /// <summary>
        /// 跳转500
        /// </summary>
        private void Redirect500()
        {
            Response.Redirect("/html/500.html");
        }

        #endregion
    }
}