using System;
using System.Collections.Generic;

using Yme.Util;
using Yme.Util.Extension;
using Yme.Mcp.BLL.WeChatManage;
using Yme.Mcp.Model.WeChat;
using Yme.Util.Log;
using Yme.Mcp.Order.Handel;
using System.Web.Http;
using System.Text;
using System.Web;
using System.Net.Http;
using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.Order.Controllers.Api
{
    /// <summary>
    /// 微信控制器
    /// </summary>
    public class WeChatController : ApiBaseController
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        [NonAuthorize]
        [HttpGet]
        public string CreateMenu()
        {
            var wxAppId = ConfigUtil.WechatAppId;
            var backUrl = String.Format("http://{0}/{1}", Request.RequestUri.Authority, "wechat/callback");

            #region 构建自定义菜单

            var menuList = new List<WeChatMenuButton>();

            //1.订单
            var menu1 = new WeChatMenuButton() { key = "order", name = "订单管理", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/order_list.html"), sub_button = new List<WeChatMenuButton>() };

            //2.报表
            var menu2 = new WeChatMenuButton() { key = "report", name = "数据报表", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/data_form_order.html"), sub_button = new List<WeChatMenuButton>() };

            //3.我的
            var menu31 = new WeChatMenuButton() { key = "printer", name = "打印机管理", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/printer_manage.html"), sub_button = new List<WeChatMenuButton>() };
            var menu32 = new WeChatMenuButton() { key = "wifi", name = "Wi-Fi配置", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/device/config"), sub_button = new List<WeChatMenuButton>() };
            var menu33 = new WeChatMenuButton() { key = "platform", name = "平台管理", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/pt_manage.html"), sub_button = new List<WeChatMenuButton>() };
            var menu34 = new WeChatMenuButton() { key = "shop", name = "门店信息", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/store.html"), sub_button = new List<WeChatMenuButton>() };
            var menu35 = new WeChatMenuButton() { key = "config", name = "系统设置", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/setting.html"), sub_button = new List<WeChatMenuButton>() };
            var menu3 = new WeChatMenuButton() { key = "my", name = "我的门店", type = "click", sub_button = new List<WeChatMenuButton>() { menu31, menu32, menu33, menu34, menu35 } };
            menuList.Add(menu1);
            menuList.Add(menu2);
            menuList.Add(menu3);

            #endregion

            return SingleInstance<WeChatBLL>.Instance.CreateMenu(menuList);
        }

        /// <summary>
        /// 接收微信推送信息控制器接口
        /// </summary>
        /// <param name="merchantId"></param>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object ReceiveMessage()
        {
            var queryStrings = WebUtil.UrlDecode(Request.RequestUri.Query.ToString().TrimStart('?'), Encoding.UTF8);
            LogUtil.Debug(string.Format("原始消息：{0}", queryStrings));

            // 判断是否是请求认证
            var requestForm = HttpRequestUtil.GetNameValueCollection(queryStrings);
            var echostr = requestForm["echostr"] as string;
            var retstr = echostr != null ? Extensions.ToString(echostr).Trim() : string.Empty;

            if (SingleInstance<WeChatBLL>.Instance.CheckSignature(ConfigUtil.WechatToken, requestForm))
            {
                if (!String.IsNullOrEmpty(retstr))
                {
                    LogUtil.Debug(string.Format("自动回调：{0}", retstr));
                }
                else
                {
                    var stream = HttpContext.Current.Request.InputStream;
                    retstr = SingleInstance<WeChatBLL>.Instance.HandleXmlCallBackStr(stream, Request.RequestUri.Authority);
                    LogUtil.Debug(string.Format("自动回调：{0}", retstr));

                    if (retstr == EnumWeChatEventType.unsubscribe.ToString())
                    {
                        retstr = string.Empty;

                        //取消关注特殊处理：清除当前会话Session
                        var isClearOk = base.ClearCurrSession();
                        LogUtil.Debug(string.Format("取消关注，清空会话{0}！", isClearOk ? "成功" : "失败"));
                    }
                }
            }

            var responseMessage = new HttpResponseMessage { Content = new StringContent(retstr, Encoding.GetEncoding("UTF-8"), "text/plain") };
            return responseMessage;
        }
    }
}
