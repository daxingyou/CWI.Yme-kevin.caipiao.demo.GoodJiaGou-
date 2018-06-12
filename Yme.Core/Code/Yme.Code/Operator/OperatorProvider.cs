using Yme.Cache.Factory;
using Yme.Util;
using System;

namespace Yme.Code
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.10.10
    /// 描 述：当前操作者回话
    /// </summary>
    public class OperatorProvider : OperatorIProvider
    {
        #region 静态实例

        /// <summary>
        /// 当前提供者
        /// </summary>
        public static OperatorIProvider Provider
        {
            get { return new OperatorProvider(); }
        }

        /// <summary>
        /// 给app调用
        /// </summary>
        public static string AppUserId
        {
            set;
            get;
        }

        #endregion

        /// <summary>
        /// 秘钥
        /// </summary>
        private string LoginUserKey = "Yme_LoginUserKey_2017";

        /// <summary>
        /// 登陆提供者模式:Session、Cookie 
        /// </summary>
        private string LoginProvider = ConfigUtil.GetValue("LoginProvider");

        /// <summary>
        /// 写入登录信息
        /// </summary>
        /// <param name="user">成员信息</param>
        public virtual void AddCurrent(Operator user)
        {
            try
            {
                if (LoginProvider == "Cookie")
                {
                    WebUtil.WriteCookie(LoginUserKey, DESEncryptUtil.Encrypt(user.ToJson()));
                }
                else
                {
                    WebUtil.WriteSession(LoginUserKey, DESEncryptUtil.Encrypt(user.ToJson()));
                }
                CacheFactory.Cache().WriteCache(user.Token, user.UserId, user.LogTime.AddHours(12));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public virtual Operator Current()
        {
            try
            {
                Operator user = new Operator();
                if (LoginProvider == "Cookie")
                {
                    user = DESEncryptUtil.Decrypt(WebUtil.GetCookie(LoginUserKey).ToString()).ToObject<Operator>();
                }
                else if (LoginProvider == "AppClient")
                {
                    user = CacheFactory.Cache().GetCache<Operator>(AppUserId);
                }
                else
                {
                    user = DESEncryptUtil.Decrypt(WebUtil.GetSession(LoginUserKey).ToString()).ToObject<Operator>();
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除登录信息
        /// </summary>
        public virtual void EmptyCurrent()
        {
            if (LoginProvider == "Cookie")
            {
                WebUtil.RemoveCookie(LoginUserKey.Trim());
            }
            else
            {
                WebUtil.RemoveSession(LoginUserKey.Trim());
            }
        }

        /// <summary>
        /// 是否过期
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOverdue()
        {
            try
            {
                object str = string.Empty;
                if (LoginProvider == "Cookie")
                {
                    str = WebUtil.GetCookie(LoginUserKey);
                }
                else
                {
                    str = WebUtil.GetSession(LoginUserKey);
                }
                if (str != null && str.ToString() != string.Empty)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public virtual int IsOnLine()
        {
            Operator user = new Operator();
            if (LoginProvider == "Cookie")
            {
                user = DESEncryptUtil.Decrypt(WebUtil.GetCookie(LoginUserKey).ToString()).ToObject<Operator>();
            }
            else
            {
                user = DESEncryptUtil.Decrypt(WebUtil.GetSession(LoginUserKey).ToString()).ToObject<Operator>();
            }
            object token = CacheFactory.Cache().GetCache<string>(user.UserId);
            if (token == null)
            {
                return -1;//过期
            }
            if (user.Token == token.ToString())
            {
                return 1;//正常
            }
            else
            {
                return 0;//已登录
            }
        }
    }
}
