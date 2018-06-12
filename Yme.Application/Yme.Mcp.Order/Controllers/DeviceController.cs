using System;
using System.Collections.Generic;

using Yme.Util;
using Yme.Util.Extension;
using Yme.Mcp.BLL.WeChatManage;
using Yme.Mcp.Model.WeChat;
using Evt.Framework.Common;
using Yme.Util.Log;
using Yme.Mcp.Order.Handel;
using System.Web.Mvc;
using Yme.Mcp.Model.Enums;
using Yme.Util.Security;

namespace Yme.Mcp.Order.Controllers
{
    public class DeviceController : Controller
    {
        /// <summary>
        /// 配置WiFi页面
        /// </summary>
        /// <returns></returns>
        [NonAuthorized]
        public object Config()
        {
            ViewBag.ConfigModel = SingleInstance<WeChatBLL>.Instance.GetJsApiParamsModel(EnumWeChatType.Client.GetHashCode(), Request.Url.AbsoluteUri);
            ViewBag.WiFiKey = Base64Util.Base64Encode(ConfigUtil.WiFiKey);
            return View();
        }
    }
}