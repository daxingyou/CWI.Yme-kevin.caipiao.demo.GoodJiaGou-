using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Yme.Mcp.Demo.Handel;

namespace Yme.Mcp.Demo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("WebAPI", "{app_sign}/{controller}/{action}/{id}", new { id = RouteParameter.Optional });

            config.Services.Replace(typeof(IHttpControllerSelector), new AppSignHttpControllerSelector(config));
        }
    }
}
