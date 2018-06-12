using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.BLL.OrderManage;
using Yme.Mcp.Model.Enums;
using Yme.Util;

namespace Yme.Mcp.Order.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 订单业务服务
        /// </summary>
        private OrderBLL obServ = SingleInstance<OrderBLL>.Instance;

        //
        // GET: /Home/

        public void Index()
        {
            var loginUrl = "/html/login.html";
            Response.Redirect(loginUrl, false);
        }
    }
}
