using System;
using System.Web.Http;

using Yme.Util;
using Yme.Util.Log;
using Yme.Mcp.Model;
using Yme.Mcp.Entity;
using Yme.Mcp.Model.WebApi;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.BLL.SystemManage;
using Yme.Mcp.Order.Controllers;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.Order.Handel;
using Yme.Util.Extension;
using Yme.Cache.Factory;
using System.Collections.Generic;
using Yme.Mcp.Model.Definitions;

namespace Yme.Mcp.Order.Controllers.Api
{
    /// <summary>
    /// 门店控制器
    /// </summary>
    public class ShopController : ApiBaseController
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="vModel">获取验证码参数</param>
        /// <returns>验证码信息</returns>
        [NonAuthorize]
        [HttpGet]
        public object GetVerifyCode([FromUri]VerifyCodeViewModel vCodeModel)
        {
            LogUtil.Info("执行获取验证码请求...");

            vCodeModel.TerminalSign = vCodeModel.Mobile;
            SingleInstance<VerifyCodeBLL>.Instance.GetSmsVerifyCode(base.AppSign, vCodeModel);
            return OK();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginModel">获取验证码参数</param>
        /// <returns>验证码信息</returns>
        [NonAuthorize]
        [HttpPost]
        public object DoLogin([FromBody]LoginViewModel loginModel)
        {
            LogUtil.Info("执行登录请求...");

            try
            {
                base.CheckRequestHeaderIsValid();
                var accessToken = base.AccessToken;
                var mobile = loginModel != null ? loginModel.ShopAccount.Trim() : string.Empty;

                //单点登录
                base.SingleLoginOn(accessToken, mobile);
                var sysUser = SingleInstance<ShopBLL>.Instance.DoLogin(loginModel, accessToken);

                //校验用户信息
                var currUser = new LoginInfo() { UserId = sysUser.ShopId, Mobile = sysUser.ShopAccount, AccessToken = sysUser.AccessToken };
                Session[ConfigUtil.SystemUserSessionKey] = currUser;

                //返回门店信息
                var shopInfo = new
                {
                    ShopId = sysUser.ShopId,
                    ShopName = sysUser.ShopName,
                    ShopAccount = sysUser.ShopAccount,
                    CityArea = string.Format("{0}{1}", sysUser.CityArea, string.IsNullOrWhiteSpace(sysUser.BusinessDistrict) ? string.Empty : string.Format("({0})", sysUser.BusinessDistrict)),
                    ShopAddress = sysUser.ShopAddress,
                    BusinessScope = sysUser.BusinessScope,
                    AccessToken = sysUser.AccessToken,
                    ExpiresIn = sysUser.ExpiresTime.ToDateTimeString()
                };
                return OK(shopInfo);
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("登录失败，参考消息：{0}", ex.Message));
                return Failed(ex.Message);
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns>退出结果</returns>
        [NonAuthorize]
        [HttpGet]
        public object DoLogout()
        {
            LogUtil.Info("执行退出请求...");

            var currUser = GetCurrUser(OperateType.Dologout);
            //执行登出系列操作
            if (currUser != null)
            {
                SingleInstance<ShopBLL>.Instance.DoLogout(currUser);
            }
            return OK();
        }

        /// <summary>
        /// 更换绑定手机号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UpdateBindMobile([FromBody]BindMobileViewModel bindModel)
        {
            LogUtil.Info("执行更换绑定手机号请求...");

            SingleInstance<ShopBLL>.Instance.BindMobile(base.AppSign, base.CurrUser, bindModel);
            var currUser = GetCurrUser(OperateType.Dologout, bindModel.NewMobile);
            //执行登出系列操作
            if (currUser != null)
            {
                SingleInstance<ShopBLL>.Instance.DoLogout(currUser);
            }
            return OK();
        }

        /// <summary>
        /// 获取门店信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetInfo()
        {
            LogUtil.Info("执行获取门店信息请求...");

            var result = SingleInstance<ShopBLL>.Instance.GetInfo(base.CurrId);
            return OK(result);
        }

        /// <summary>
        /// 登录校验
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpGet]
        public object CheckLogin()
        {
            LogUtil.Info("执行登录校验...");

            try
            {
                //验证是否已经登录
                var isLogin = true;
                base.CheckRequestHeaderIsValid();
                if (Session.ContainsKey(ConfigUtil.SystemUserSessionKey))
                {
                    var currUser = Session[ConfigUtil.SystemUserSessionKey] as LoginInfo;
                    isLogin = currUser != null;
                }
                else
                {
                    isLogin = false;
                }

                if (!isLogin && !string.IsNullOrWhiteSpace(base.AccessToken))
                {
                    var current = ServiceContext.Current;
                    if (current != null && current.RequestTerminal != null && !string.IsNullOrWhiteSpace(current.RequestTerminal.ClientToken) && current.RequestTerminal.ClientToken != "null")
                    {
                        base.SingleLoginOn(current.RequestTerminal.ClientToken);
                        var currUser = SingleInstance<ShopBLL>.Instance.DoLogin(null, current.RequestTerminal.ClientToken);
                        isLogin = currUser != null;
                        if (currUser != null)
                        {
                            var loginInfo = new LoginInfo() { UserId = currUser.ShopId, Mobile = currUser.ShopAccount, AccessToken = currUser.AccessToken };
                            Session[ConfigUtil.SystemUserSessionKey] = loginInfo;
                        }
                    }
                    else
                    {
                        isLogin = false;
                    }
                }

                return isLogin ? OK() : Failed(string.Empty);
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("执行登录校验失败，参考消息：{0}", ex.Message));
                return Failed(ex.Message);
            }
        }

        /// <summary>
        /// 获取接单平台配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetConfirmConfigs()
        {
            LogUtil.Info("执行获取接单平台配置请求...");

            var result = SingleInstance<PlatformBLL>.Instance.GetShopPlatformConfigs(base.CurrId, BusinessType.Waimai);
            return OK(result);
        }

        /// <summary>
        /// 设置门店接单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object DoConfirmConfig()
        {
            var requestForms = base.GetRequestParams(true, "执行门店接单设置请求");
            var platformId = requestForms["PlatformId"] != null ? requestForms["PlatformId"].ToString().Trim() : string.Empty;
            var status = requestForms["Status"] != null ? requestForms["Status"].ToString().Trim() : string.Empty;
            if (string.IsNullOrEmpty(platformId))
            {
                return Failed("平台ID不能为空！");
            }
            if (string.IsNullOrEmpty(status))
            {
                return Failed("启用状态不能为空！");
            }
            if (!status.IsNumBool())
            {
                return Failed("启用状态格式不正确！");
            }

            var result = SingleInstance<PlatformBLL>.Instance.UpadatePlatfromIsAutoCofrim(base.CurrId, platformId.ToInt(), status.ToInt());
            return result >= 0 ? OK(result) : Failed("设置失败,请重试！");
        }

        #region 私有方法

        /// <summary>
        /// 获取去当前门店信息
        /// </summary>
        /// <param name="operate"></param>
        /// <param name="newMobile"></param>
        /// <returns></returns>
        private ShopEntity GetCurrUser(OperateType operate, string newMobile = "")
        {
            //获取当前登录信息
            ShopEntity currUser = null;
            if (Session.ContainsKey(ConfigUtil.SystemUserSessionKey))
            {
                var login = Session[ConfigUtil.SystemUserSessionKey] as LoginInfo;
                currUser = login != null ? login.CurrUserInfo : null;
                if (operate == OperateType.Dologout)
                {
                    if (!string.IsNullOrWhiteSpace(newMobile) && currUser != null && currUser.ShopAccount != newMobile)
                    {
                        currUser.ShopAccount = newMobile;
                    }
                    Session[ConfigUtil.SystemUserSessionKey] = null;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(base.AccessToken))
                {
                    currUser = SingleInstance<ShopBLL>.Instance.GetShopByAccessToken(base.AccessToken, operate);
                }
            }

            return currUser;
        }

        #endregion
    }
}
