using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.BLL.SystemManage;
using Yme.Mcp.Order.Controllers;
using Yme.Mcp.Order.Handel;
using Yme.Util;
using Yme.Util.Log;
using Yme.Util.Extension;
using Yme.Util.Security;
using Yme.Mcp.Model.Definitions;
using System;

namespace Yme.Mcp.Order.Controllers.Api
{
    /// <summary>
    /// 系统控制器
    /// </summary>
    public class SystemController : ApiBaseController
    {
        [NonAuthorize]
        [HttpGet]
        public object GetSecondTicks()
        {
            return OK(TimeUtil.SecondTicks_1970);
        }

        [NonAuthorize]
        [HttpGet]
        public object GetWeekDateTime(long timestamp)
        {
            return OK(TimeUtil.GetWeekDateTime(timestamp));
        }

        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object cache()
        {
            var queryStrings = WebUtil.UrlDecode(Request.RequestUri.Query.TrimStart('?'), Encoding.UTF8);
            LogUtil.Info(string.Format("请求参数：{0}", queryStrings));

            var requestForms = HttpRequestUtil.GetNameValueCollection(queryStrings);
            var key = requestForms["key"] as string;
            var ret = SingleInstance<SystemBLL>.Instance.GetCache(key);
            return OK(ret);
        }

        [NonAuthorize]
        [HttpGet]
        public object GetMeituanGetUrl()
        {
            //1.校验请求参数
            var errMsg = string.Empty;
            var parms = new SortedDictionary<string, object>();

            parms.Add("appAuthToken", "5eb0b7509b2148e2a5ba4a4c671e9750208bb59ef8e3c7f4349cb88aa8d63384224cc76585b83f9f112b3480b715fd52");
            parms.Add("charset", "utf-8");
            parms.Add("timestamp", TimeUtil.SecondTicks_1970);
            //parms.Add("ePoiId", "10004");
            var sign = SignUtil.GetMeituanSign(parms, ConfigUtil.MeiSignKey);

            var parmStr = new StringBuilder();
            var url = "http://api.open.cater.meituan.com/waimai/dish/queryCatList";//"http://api.open.cater.meituan.com/waimai/dish/queryBaseListByEPoiId";
            foreach (var parm in parms)
            {
                parmStr.AppendFormat("{0}={1}&", parm.Key, parm.Value);
            }
            var pars = string.Format("{0}sign={1}", parmStr.ToString(), sign);

            try
            {
                var result = HttpRequestUtil.HttpGet(url, pars);
                LogUtil.Info(string.Format("调用接口,返回：{0}", result));

                errMsg = result;
            }
            catch (Exception ex)
            {
                errMsg = ex.InnerException.ToString();
            }
            return errMsg;
        }
    }
}
