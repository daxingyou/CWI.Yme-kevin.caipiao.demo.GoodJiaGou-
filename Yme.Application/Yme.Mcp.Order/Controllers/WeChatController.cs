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

namespace Yme.Mcp.Order.Controllers
{
    /// <summary>
    /// 微信控制器
    /// </summary>
    public class WeChatController : Controller
    {
        #region 智能设备

        /// <summary>
        /// 回调校验
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        public object Verify()
        {
            LogUtil.Debug(string.Format("校验原始消息：{0}", Request.QueryString.ToString()));

            // 判断是否是请求认证
            var echostr = Request["echostr"] as string;
            var retstr = echostr != null ? Extensions.ToString(Request.QueryString["echostr"]).Trim() : string.Empty;

            var requestQuery = Request.QueryString;
            bool isValid = SingleInstance<WeChatBLL>.Instance.CheckSignature(ConfigUtil.IotToken, requestQuery);
            if (isValid)
            {
                if (!String.IsNullOrEmpty(retstr))
                {
                    LogUtil.Debug(string.Format("{0}自动回调：{1}", "校验", retstr));
                    Response.Write(retstr);
                    Response.End();
                }
                else
                {
                    var stream = System.Web.HttpContext.Current.Request.InputStream;
                    SingleInstance<WeChatBLL>.Instance.HandleJsonCallBackStr(stream);
                }
            }
            return isValid ? retstr : string.Empty;
        }

        #endregion

        #region 基础

        /// <summary>
        /// 创建菜单
        /// </summary>
        [NonAuthorize]
        public string CreateMenu()
        {
            var wxAppId = ConfigUtil.WechatAppId;
            var backUrl = String.Format("http://{0}/{1}", Request.Url.Authority, "wechat/callback");

            #region 构建自定义菜单

            var menuList = new List<WeChatMenuButton>();

            //1.订单
            var menu11 = new WeChatMenuButton() { key = "corder", name = "今日订单", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/order_list.html"), sub_button = new List<WeChatMenuButton>() };
            var menu12 = new WeChatMenuButton() { key = "porder", name = "预 订 单", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/order_pre_list.html"), sub_button = new List<WeChatMenuButton>() };
            var menu13 = new WeChatMenuButton() { key = "rorder", name = "订单补打", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/order_reprint_list.html"), sub_button = new List<WeChatMenuButton>() };
            var menu14 = new WeChatMenuButton() { key = "qorder", name = "订单查询", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/order_search_list.html"), sub_button = new List<WeChatMenuButton>() };

            var menu1 = new WeChatMenuButton() { key = "order", name = "订单管理", type = "click", sub_button = new List<WeChatMenuButton>() { menu11, menu12, menu13, menu14 } };

            //2.报表
            var menu2 = new WeChatMenuButton() { key = "report", name = "数据报表", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/data_form_order.html"), sub_button = new List<WeChatMenuButton>() };

            //3.我的
            var menu31 = new WeChatMenuButton() { key = "printer", name = "打印机管理", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/printer_manage.html"), sub_button = new List<WeChatMenuButton>() };
            var menu32 = new WeChatMenuButton() { key = "wifi", name = "Wi-Fi配置", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/device/config"), sub_button = new List<WeChatMenuButton>() };
            var menu33 = new WeChatMenuButton() { key = "platform", name = "平台管理", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/pt_manage.html"), sub_button = new List<WeChatMenuButton>() };
            var menu34 = new WeChatMenuButton() { key = "shop", name = "门店管理", type = "view", url = String.Format(WeChatConsts.WECHAT_AUTHORIZE, wxAppId, backUrl, "/html/store.html"), sub_button = new List<WeChatMenuButton>() };
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
        public void ReceiveMessage()
        {
            LogUtil.Debug(string.Format("原始消息：{0}", Request.QueryString.ToString()));

            // 判断是否是请求认证
            var echostr = Request["echostr"] as string;
            var retstr = echostr != null ? Extensions.ToString(Request.QueryString["echostr"]).Trim() : string.Empty;

            var requestQuery = Request.QueryString;
            if (SingleInstance<WeChatBLL>.Instance.CheckSignature(ConfigUtil.WechatToken, requestQuery))
            {
                if (!String.IsNullOrEmpty(retstr))
                {
                    LogUtil.Debug(string.Format("{0}自动回调：{1}", "", retstr));
                    Response.Write(retstr);
                    Response.End();
                }
                else
                {
                    var stream = System.Web.HttpContext.Current.Request.InputStream;
                    var retStr = SingleInstance<WeChatBLL>.Instance.HandleXmlCallBackStr(stream, Request.Url.Authority);

                    Response.Write(retStr);
                    Response.End();
                }
            }
        }

        /// <summary>
        /// 微信回调
        /// </summary>
        [NonAuthorize]
        public void CallBack()
        {
            var requestQuery = Request.QueryString;
            LogUtil.Debug(string.Format("接收回调消息：{0}", requestQuery));
            var redirectUrl = SingleInstance<WeChatBLL>.Instance.CallBack(requestQuery);
            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                Response.Redirect(redirectUrl, false);
            }
        }

        /// <summary>
        /// 获取微型AccessToken
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        public object GetWxToken(string k)
        {
            var token = string.Empty;
            if (k == "7758520")
            {
                token = SingleInstance<WeChatBLL>.Instance.GetWeiXinToken(EnumWeChatType.Client.GetHashCode());
            }
            return token;
        }

        #endregion
    }
}
