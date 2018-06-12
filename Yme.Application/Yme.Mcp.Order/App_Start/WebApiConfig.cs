using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Yme.Mcp.Order.Handel;

namespace Yme.Mcp.Order
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{app_sign}/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //    );

            config.Routes.MapHttpRoute("WebAPI", "{app_sign}/{controller}/{action}/{id}", new { id = RouteParameter.Optional });

            config.Services.Replace(typeof(IHttpControllerSelector), new AppSignHttpControllerSelector(config));
        }
    }
}
