using Evt.Framework.Common;
using System;
using System.Collections.Generic;
using Yme.Data.Repository;
using Yme.Mcp.BLL.OrderManage;
using Yme.Mcp.BLL.SystemManage;
using Yme.Mcp.BLL.WeChatManage;
using Yme.Mcp.Entity;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Model;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Service.BaseManage;
using Yme.Util;
using Yme.Util.Extension;
using Yme.Util.Log;

namespace Yme.Mcp.BLL.BaseManage
{
    public class ShopBLL
    {
        #region 私有变量

        private IShopService service = new ShopService();

        #endregion

        #region 公有方法

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ShopEntity DoLogin(LoginViewModel loginModel, string accessToken)
        {
            ShopEntity shop = null;
            var db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                var isOk = CheckLoginModel(loginModel, accessToken);
                LogUtil.Info(string.Format("登录参数验证{0}", isOk ? "通过" : "失败"));

                if (isOk)
                {
                    var dbNow = TimeUtil.Now;
                    var mobile = loginModel != null ? loginModel.ShopAccount.Trim() : string.Empty;
                    var vCode = loginModel != null ? loginModel.VerifyCode.Trim() : string.Empty;
                    if (!string.IsNullOrWhiteSpace(mobile))
                    {
                        SingleInstance<VerifyCodeBLL>.Instance.UpdateVerifyCode(ConfigUtil.SystemAppSign, vCode, mobile);
                    }
                    LogUtil.Info(string.Format("mobile:{0},vCode:{1},accessToken:{2}", mobile, vCode, accessToken));
                    shop = GetCurrentInfo(mobile, accessToken, false);
                    if (shop == null)
                    {
                        if (!string.IsNullOrWhiteSpace(mobile) && !string.IsNullOrWhiteSpace(vCode))
                        {
                            //用户不存在，自动注册
                            shop = new ShopEntity();
                            shop.MerchantId = StringUtil.UniqueStr();
                            shop.ShopAccount = mobile;
                            shop.LastLoginIp = NetUtil.Ip;
                            shop.LastLoginTime = dbNow;
                            shop.AccessToken = StringUtil.UniqueStr().ToUpper();
                            shop.ExpiresTime = dbNow.AddHours(12 * 30);
                            shop.CreateDate = dbNow;
                            shop.CreateUserId = mobile;
                            shop.EnabledFlag = EnabledFlagType.Valid.GetHashCode();
                            shop.DeleteFlag = DeleteFlagType.Valid.GetHashCode();
                            service.InsertEntity(shop);
                        }
                        else
                        {
                            throw new AuthenticationException("会话超时，请重新登录！");
                        }
                    }
                    else
                    {
                        //用户存在更新信息及是否更新访问令牌
                        shop.LastLoginIp = NetUtil.Ip;
                        shop.LastLoginTime = dbNow;
                        if (!string.IsNullOrWhiteSpace(mobile) && !string.IsNullOrWhiteSpace(vCode))
                        {
                            //手动登录才更新Token
                            shop.AccessToken = StringUtil.UniqueStr().ToUpper();
                            shop.ExpiresTime = dbNow.AddHours(12 * 30);
                        }
                     
                        service.UpdateEntity(shop);

                        //校验用户信息
                        CheckShopStatus(shop, OperateType.DoLogin);
                    }

                    //更新微信用户门店关联信息
                    if (loginModel != null && !string.IsNullOrWhiteSpace(loginModel.OpenId))
                    {
                        SingleInstance<WeChatBLL>.Instance.UpdateWxUserInfo(shop.ShopId, loginModel.OpenId, EnumWeChatType.Client.GetHashCode());
                    }
                    db.Commit();
                }
            }
            catch (AuthenticationException ex)
            {
                db.Rollback();
                throw new AuthenticationException(ex.Message);
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogUtil.Error(string.Format("登录失败，参考消息：{0}", ex.StackTrace));
                throw new MessageException(ex.Message);
            }
            return shop;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public void DoLogout(ShopEntity currUser)
        {
            //1.清空AccessToken
            UpdateAccessToken(currUser, OperateType.Dologout);
        }

        /// <summary>
        /// 更新绑定手机号
        /// </summary>
        /// <param name="appSign"></param>
        /// <param name="curUser"></param>
        /// <param name="bindModel"></param>
        public void BindMobile(string appSign, ShopEntity curUser, BindMobileViewModel bindModel)
        {
            var db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                // 校验参数
                if (bindModel == null)
                {
                    throw new MessageException("更新参数不可为空！");
                }
                if (bindModel.NewMobile == bindModel.Mobile)
                {
                    throw new MessageException("新手机号与原手机号相同！");
                }
                if (bindModel.Mobile != curUser.ShopAccount)
                {
                    throw new MessageException("原手机号不正确请核对！");
                }
                var shop = GetShopByMobile(bindModel.NewMobile, OperateType.GetInfo);
                if (shop != null)
                {
                    //1.校验是否存在订单
                    var isExistsOrder = SingleInstance<OrderBLL>.Instance.CheckShopIsExistsOrder(shop.ShopId);
                    if (isExistsOrder)
                    {
                        throw new MessageException("新手机账号下已有订单，请联系客服！");
                    }
                    //2.校验是否关联打印机
                    var isBindPrinter = SingleInstance<PrinterBLL>.Instance.CheckShopIsBindPrinter(shop.ShopId);
                    if (isBindPrinter)
                    {
                        throw new MessageException("新手机账号下已绑定打印机，请先解除！");
                    }
                    //3.校验是否授权第三方平台
                    var isAuth = SingleInstance<PlatformBLL>.Instance.CheckShopIsAuhtPlatform(shop.ShopId);
                    if (isAuth)
                    {
                        throw new MessageException("新手机账号下已有第三方平台授权，请先取消！");
                    }

                    //throw new MessageException("新手机号已注册！");
                }

                // 更新验证码
                SingleInstance<VerifyCodeBLL>.Instance.UpdateVerifyCode(appSign, bindModel.VerifyCode, bindModel.NewMobile);

                if (shop != null)
                {
                    // 4.删除新手机号对应门店账户信息
                    service.Remove(shop.ShopId);
                }

                // 5.更新手机号
                curUser.ShopAccount = bindModel.NewMobile;
                service.UpdateEntity(curUser);

                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogUtil.Error(ex.StackTrace);
                throw new MessageException(ex.Message);
            }
        }

        /// <summary>
        /// 获取门店信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public object GetInfo(long shopId)
        {
            var entity = service.GetEntity(shopId);
            var info = new
            {
                ShopName = entity != null ? entity.ShopName : string.Empty,
                ShopmanName = entity != null ? entity.ShopmanName : string.Empty,
                ShopAccount = entity != null ? entity.ShopAccount.HideMoblie() : string.Empty,
                CityArea = entity != null ? string.Format("{0}{1}", entity.CityArea, string.IsNullOrWhiteSpace(entity.BusinessDistrict) ? string.Empty : string.Format("({0})", entity.BusinessDistrict)) : string.Empty,
                ShopAddress = entity != null ? entity.ShopAddress : string.Empty,
                BusinessScope = entity != null ? entity.BusinessScope : string.Empty
            };

            return info;
        }

        /// <summary>
        /// 根据门店ID获取门店实体
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public ShopEntity GetShopById(long shopId)
        {
            return service.GetEntity(shopId);
        }

        /// <summary>
        /// 根据手机号获取门店实体
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ShopEntity GetShopByMobile(string mobile, OperateType operateType)
        {
            var shop = service.GetEntityByMobile(mobile);
            if (shop != null)
            {
                CheckShopStatus(shop, operateType);
            }
            return shop;
        }

        /// <summary>
        /// 根据访问令牌获取用户实体
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public ShopEntity GetShopByAccessToken(string accessToken, OperateType operateType)
        {
            var shop = service.GetEntityByAccessToken(accessToken);
            if (shop != null)
            {
                var dbNow = TimeUtil.Now;
                if (dbNow >= shop.ExpiresTime)
                {
                    shop = null;
                }
                else
                {
                    CheckShopStatus(shop, operateType);
                }
            }
            return shop;
        }

        /// <summary>
        /// 校验用户状态
        /// </summary>
        /// <param name="currObj">用户信息</param>
        /// <param name="action">获取信息分类枚举</param>
        public void CheckShopStatus(ShopEntity currObj, OperateType operateType)
        {
            if (operateType == OperateType.DoLogin)
            {
                if (currObj == null)
                {
                    throw new MessageException("此门店不存在！");
                }

                if (currObj.EnabledFlag == EnabledFlagType.Disabled.GetHashCode())
                {
                    throw new MessageException(EnabledFlagType.Disabled.GetRemark());
                }

                if (currObj.DeleteFlag == DeleteFlagType.Disabled.GetHashCode())
                {
                    throw new MessageException(DeleteFlagType.Disabled.GetRemark());
                }
            }
        }

        /// <summary>
        /// 更新门店信息
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public int UpdateShopInfo(ShopEntity shop)
        {
            return service.UpdateEntity(shop);
        }

        #endregion

        #region 私有方法

        // 1.校验登录参数
        private bool CheckLoginModel(LoginViewModel loginModel, string accessToken)
        {
            bool isOK = true;
            if (loginModel != null)
            {
                if (!string.IsNullOrWhiteSpace(loginModel.ShopAccount))
                {
                    var vCode = loginModel.VerifyCode != null ? loginModel.VerifyCode.Trim() : string.Empty;
                    if (string.IsNullOrWhiteSpace(vCode))
                    {
                        isOK = false;
                        throw new MessageException("验证码不能为空！");
                    }
                }

                if (!loginModel.ShopAccount.IsMobile())
                {
                    isOK = false;
                    throw new MessageException("手机号码格式不正确！");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    isOK = false;
                    throw new MessageException("手机号和验证码不能为空！");
                }
            }
            return isOK;
        }

        /// <summary>
        /// 获取当前对象信息
        /// </summary>
        /// <param name="mobile">用户名</param>
        /// <param name="userPwd">用户密码</param>
        /// <param name="accessToken">用户Token</param>
        /// <returns>当前对象信息</returns>
        private ShopEntity GetCurrentInfo(string mobile, string accessToken = "", bool showMsg = true)
        {
            //1.参数校验
            if (string.IsNullOrWhiteSpace(accessToken) && string.IsNullOrWhiteSpace(mobile) && showMsg)
            {
                throw new MessageException("手机号码不能为空！");
            }

            //2.当前对象查询
            return !string.IsNullOrWhiteSpace(mobile) ? GetShopByMobile(mobile, OperateType.GetInfo) : GetShopByAccessToken(accessToken, OperateType.GetInfo);
        }

        /// <summary>
        /// 更新登录Token
        /// </summary>
        /// <param name="shop">店铺信息</param>
        /// <param name="operateType"></param>
        /// <param name="access_token">客户端Token</param>
        /// <returns>新的AccessToken</returns>
        private string UpdateAccessToken(ShopEntity shop, OperateType operateType, string access_token = "")
        {
            CheckShopStatus(shop, operateType);

            var dbNow = DateTime.Now;
            switch (operateType)
            {
                case OperateType.DoLogin:
                    {
                        if (string.IsNullOrWhiteSpace(shop.AccessToken)
                            || (!shop.AccessToken.Equals(access_token, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(access_token)))
                        {
                            shop.AccessToken = StringUtil.UniqueStr();
                            shop.ExpiresTime = DateTime.MaxValue;//dbNow.AddHours(12);
                            shop.LastLoginIp = NetUtil.Ip;
                            shop.LastLoginTime = dbNow;
                            shop.ModifyDate = dbNow;
                            service.UpdateEntity(shop);
                        }
                        break;
                    }
                case OperateType.Dologout:
                    {
                        shop.AccessToken = string.Empty;
                        shop.ModifyDate = dbNow;
                        shop.ExpiresTime = DateTime.MinValue;
                        service.UpdateEntity(shop);
                        break;
                    }
            }

            return shop.AccessToken;
        }

        #endregion
    }
}
