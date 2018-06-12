using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.Model.Definitions;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Order.Handel;
using Yme.Util;
using Yme.Util.Extension;
using Yme.Util.Log;

namespace Yme.Mcp.Order.Controllers
{
    public class CallBackController : Controller
    {
        #region 常量

        /// <summary>
        /// 尝试延长执行间隔，单位：毫秒
        /// </summary>
        private const int delayTryInterval = 3000;

        /// <summary>
        /// 失败后尝试次数
        /// </summary>
        private const int tryMax = 5;

        #endregion

        #region 饿了么

        /// <summary>
        /// 饿了么门店解除授权
        /// </summary>
        public void UnMapElemeShopAuth()
        {
            var key = Request["key"] ?? string.Empty;
            var shopId = key.ToInt();
            SingleInstance<PlatformBLL>.Instance.SyncUnMapShopPlatformData(shopId, PlatformType.Eleme, AuthBussinessType.Waimai);
            var authListUrl = "/html/pt_manage.html";
            Response.Redirect(authListUrl, false);
        }

        /// <summary>
        /// 【饿了么】授权回调【同意授权】
        /// </summary>
        /// <returns></returns>
        public void EleShopAuthCallBack()
        {
            var actionDesc = "饿了么门店授权回调";
            var queryStrings = WebUtil.UrlDecode(Request.QueryString.ToString(), Encoding.UTF8);
            LogUtil.Info(string.Format("{0},请求参数：{1}", actionDesc, queryStrings));

            var errMsg = string.Empty;
            var url = "pt_manage.html";
            var eUrl = "/html/error_page.html?msg={0}&url={1}";
            var sUrl = "/html/success_page.html?msg={0}&url={1}";
            var eShopIds = new List<string>();
            var requestForm = Request.QueryString;
            var successMsg = JsonUtil.ToJson(new { message = ElemeConsts.RETURN_SUCCESS });
            if (requestForm.Keys.Count > 0)
            {
                //正常授权
                var code = requestForm["code"] ?? string.Empty;
                var shopId = DESEncryptUtil.Decrypt(requestForm["state"] ?? string.Empty);
                if (!string.IsNullOrWhiteSpace(code))
                {
                    //获取AccessToken
                    var elemeToken = SingleInstance<PlatformBLL>.Instance.GetEleAccessTokenByCode(code, out eShopIds, out errMsg);
                    var accessToken = elemeToken != null ? elemeToken.access_token : string.Empty;
                    if (!string.IsNullOrWhiteSpace(errMsg) || elemeToken == null || string.IsNullOrWhiteSpace(accessToken))
                    {
                        //错误页跳转
                        LogUtil.Error(errMsg);
                        errMsg = string.IsNullOrWhiteSpace(errMsg) ? "获取访问令牌失败！" : errMsg;
                        Response.Redirect(string.Format(eUrl, WebUtil.UrlEncode(errMsg), url), false);
                    }
                    else
                    {
                        //执行门店解除授权数据同步【重复执行】
                        var handleResult = false;
                        var authType = AuthBussinessType.Waimai;
                        var pShopId = eShopIds != null && eShopIds.Count > 0 ? eShopIds[0] : string.Empty;
                        var shopPlatform = SingleInstance<PlatformBLL>.Instance.GetShopPlatformInfo(PlatformType.Eleme.GetHashCode(), pShopId);
                        if (shopPlatform != null)
                        {
                            //错误页跳转
                            LogUtil.Error(errMsg);
                            errMsg = string.IsNullOrWhiteSpace(errMsg) ? "此饿了么门店已授权，不可重复授权！" : errMsg;

                            shopPlatform.AuthToken = accessToken;
                            shopPlatform.ExpiresIn = elemeToken.expires_in;
                            shopPlatform.RefreshToken = elemeToken.refresh_token ?? string.Empty;
                            shopPlatform.RefreshExpireTime = TimeUtil.Now.AddDays(ConfigUtil.EleReTokenExpiresDays);
                            shopPlatform.ModifyDate = TimeUtil.Now;
                            SingleInstance<PlatformBLL>.Instance.UpdateEntity(shopPlatform);

                            Response.Redirect(string.Format(eUrl, WebUtil.UrlEncode(errMsg), url), false);
                        }
                        else
                        {
                            for (var t = 1; t <= tryMax; t++)
                            {
                                try
                                {
                                    handleResult = SyncShopPlatformData(shopId, accessToken, elemeToken.expires_in, elemeToken.refresh_token, PlatformType.Eleme, authType, t, pShopId);
                                }
                                catch (Exception ex)
                                {
                                    handleResult = false;
                                    LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                                }

                                if (handleResult)
                                {
                                    break;
                                }
                                else
                                {
                                    Thread.Sleep(delayTryInterval);
                                    continue;
                                }
                            }

                            if (handleResult)
                            {
                                LogUtil.Info(string.Format("{0}成功！", actionDesc));
                                Response.Write(successMsg);
                                Response.Redirect(string.Format(sUrl, WebUtil.UrlEncode("授权成功！"), url), false);
                            }
                            else
                            {
                                errMsg = string.Format("{0}同步失败！", actionDesc);
                                Response.Redirect(string.Format(eUrl, WebUtil.UrlEncode(errMsg), url), false);
                            }
                        }
                    }
                }
                else
                {
                    //失败
                    var error = requestForm["error"] ?? string.Empty;
                    var error_description = requestForm["error_description"] ?? string.Empty;
                    LogUtil.Error(string.Format("{0}错误, 返回信息：{1}", actionDesc, queryStrings));
                    Response.Redirect(string.Format(eUrl, WebUtil.UrlEncode(error_description), url), false);
                }
            }
            else
            {
                errMsg = "参数为空！";
                LogUtil.Error(string.Format("执行{0}{1}, 返回信息：{2}", actionDesc, errMsg, queryStrings));
            }
        }

        #endregion

        #region 基础通用

        /// <summary>
        /// 执行门店授权数据同步
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="appAuthToken"></param>
        /// <param name="expiresIn"></param>
        /// <param name="refreshToken"></param>
        /// <param name="pType"></param>
        /// <param name="authType"></param>
        /// <param name="tryNum"></param>
        /// <returns></returns>
        private bool SyncShopPlatformData(string shopId, string appAuthToken, long expiresIn, string refreshToken, PlatformType pType, AuthBussinessType authType, int tryNum, string pShopId = "")
        {
            LogUtil.Info(string.Format("第{0}次尝试执行门店授权数据同步。", tryNum));
            return SingleInstance<PlatformBLL>.Instance.SyncShopPlatformData(shopId, appAuthToken, expiresIn, refreshToken, pType, authType, BusinessType.Waimai, pShopId) > 0;
        }

        #endregion
    }
}
