using Yme.Mcp.Entity.SystemManage;
using Yme.Mcp.IService.SystemManage;
using Yme.Data.Repository;
using Yme.Util.Extension;
using System.Linq;
using System;
using Yme.Util;
using System.Data.Common;
using Yme.Data;
using Yme.Util.Exceptions;
using Yme.Code;
using System.Text;
using Yme.Mcp.Model.Enums;
using Yme.Util.Log;

namespace Yme.Mcp.Service.SystemManage
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.11.12 16:40
    /// 描 述：区域管理
    /// </summary>
    public class VerifyCodeService : RepositoryFactory<VerifyCodeEntity>, IVerifyCodeService
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>已有的验证码</returns>
        public string GetVerifyCode(string appSign, string mobile, string terminalSign, VerifyCodeType verifyCodeType)
        {
            // 1.校验请求参数
            CheckGetUserVerifyCode(appSign, verifyCodeType, terminalSign, mobile);

            // 2.获取新验证码
            return BuildVerifyCode(appSign, terminalSign, mobile);
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns>验证码ID</returns>
        public long CheckEntity(string appSign, string verifyCode, string mobile)
        {
            var dbNow = DateTime.Now;
            var parms = new DbParameter[]
            {
               DbParameters.CreateDbParameter("appSign", appSign),
               DbParameters.CreateDbParameter("verifyCode", verifyCode),
               DbParameters.CreateDbParameter("mobile", mobile)
            };

            string sql = @" SELECT *
                            FROM sys_verifycode 
                            WHERE AppSign = @appSign AND Mobile = @mobile AND VerifyCode = @verifyCode
                            ORDER BY ModifyDate DESC 
                            LIMIT 1";

            var list = this.BaseRepository().FindList(sql, parms);
            if (list == null || list.Count() <= 0)
            {
                throw new BusinessException(VerifyCodeStatusType.Fail.GetRemark());
            }
            else
            {
                var entity = list.FirstOrDefault();
                if (entity != null)
                {
                    if (entity.Verified == 1)
                    {
                        throw new BusinessException(VerifyCodeStatusType.Used.GetRemark());
                    }

                    if (entity.ExpireDate < dbNow)
                    {
                        throw new BusinessException(VerifyCodeStatusType.Expired.GetRemark());
                    }
                }
                else
                {
                    throw new BusinessException(VerifyCodeStatusType.Fail.GetRemark());
                }

                return entity.VerifyCodeId;
            }
        }

        /// <summary>
        /// 使用验证码
        /// </summary>
        /// <param name="verifyCodeId">验证码Id</param>
        public void UpdateEntityStatus(string appSign, long verifyCodeId)
        {
            var entity = new VerifyCodeEntity();
            entity.VerifyCodeId = verifyCodeId;
            entity.AppSign = appSign;
            entity.Verified = 1;
            entity.VerifiedDate = DateTime.Now;
            long vid = this.BaseRepository().Update(entity);
            if (vid <= 0)
            {
                throw new MessageException("系统错误，更新验证码状态失败！");
            }
        }

        #region 私有

        /// <summary>
        /// 校验获取验证码请求参数
        /// </summary>
        /// <param name="verifyCodeType">验证码类型</param>
        /// <param name="email">邮箱地址</param>
        /// <param name="mobile">手机号码</param>
        private void CheckGetUserVerifyCode(string appSign, VerifyCodeType verifyCodeType, string terminalSign, string mobile)
        {
            // 1.校验验证码类型
            int codeType = verifyCodeType.GetHashCode();

            // 2.校验获取验证码方式
            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new MessageException("手机号码不能为空！");
            }

            //验证是否频繁发送
            terminalSign = mobile;
            bool isExist = this.IsExistVerifycodeBlackList(appSign, terminalSign, ConfigUtil.VarifyCodeFailCount);
            if (isExist)
            {
                throw new MessageException("您操作太频繁了，请稍候再试！");
            }
        }

        /// <summary>
        /// 获取当前用户已有的验证码（有效期内）
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="mobile">手机号码</param>
        /// <returns>已有的验证码</returns>
        private string GetUserVerifyCode(string appSign, string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new MessageException("手机号码不能为空！");
            }

            var dbNow = DateTime.Now;
            var parms = new DbParameter[]
            {
               DbParameters.CreateDbParameter("appSign", appSign),
               DbParameters.CreateDbParameter("expireDate", dbNow),
               DbParameters.CreateDbParameter("mobile", mobile)
            };
            string sql = @" SELECT * FROM sys_verifycode WHERE AppSign = @appSign AND Mobile = @mobile AND Verified = 0 AND ExpireDate > @expireDate ORDER BY ModifyDate DESC  LIMIT 1";
            var entity = this.BaseRepository().FindList(sql, parms).FirstOrDefault();
            if (entity != null)
            {
                //强制验证码过期
                CancelVerifyCode(appSign, mobile);

                //产生新的验证码记录，验证码相同
                entity.CreateDate = dbNow;
                entity.ExpireDate = dbNow.AddSeconds(ConfigUtil.ValidatecodeExpire);
                this.BaseRepository().Insert(entity);

                //返回验证码
                return entity.VerifyCode;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>验证码</returns>
        private string BuildVerifyCode(string appSign, string terminalSign, string mobile)
        {
            var db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                //1.强制手机号对应验证码失效
                CancelVerifyCode(appSign, mobile);

                //1.生成验证码及失效日期
                var dbNow = DateTime.Now;
                string verifyCode = VerifyCode.GenNumCode(Const.SMS_VALIDATECODE_LENGTH);
                DateTime expireDate = dbNow.AddSeconds(ConfigUtil.ValidatecodeExpire);

                //2.登记用户验证码信息
                var entity = new VerifyCodeEntity();
                entity.AppSign = appSign;
                entity.Mobile = mobile;
                entity.Email = string.Empty;
                entity.VerifyCode = verifyCode;
                entity.ExpireDate = expireDate;
                entity.Verified = 0;
                entity.VerifiedDate = DateTime.MinValue;
                entity.TerminalCode = terminalSign;
                entity.Description = string.Empty;
                entity.CreateUserId = mobile;
                entity.CreateDate = dbNow;
                db.Insert(entity);
                db.Commit();

                //3.返回产生的验证码
                return verifyCode;
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogUtil.Error(ex.StackTrace);
                throw new MessageException("系统错误，生成验证码失败！");
            }
        }

        /// <summary>
        /// 强制验证码过期
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="mobile">手机号码</param>
        private void CancelVerifyCode(string appSign, string mobile)
        {
            var sql = @"UPDATE sys_verifycode SET ExpireDate=NOW() WHERE AppSign = @appSign AND Mobile = @mobile AND ExpireDate>NOW()";
            var parms = new DbParameter[]
            {
               DbParameters.CreateDbParameter("appSign", appSign),
               DbParameters.CreateDbParameter("mobile", mobile)
            };

            this.BaseRepository().ExecuteBySql(sql, parms);
        }

        /// <summary>
        /// 累计统计用户验证验证码次数，如果失败 limitcount，设置验证码为黑名单
        /// </summary>
        /// <param name="terminalCode">终端标识</param>
        /// <param name="limitCount">获取验证码次数</param>
        private void UpdateVerifycodeBlackList(string appSign, string terminalCode, int limitCount)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(@"CALL proc_sys_blacklist('{0}','{1}','{2}') ", appSign, terminalCode, limitCount);
            this.BaseRepository().ExecuteBySql(strSql.ToString());
        }

        /// <summary>
        /// 校验终端是否存在于黑名单中
        /// </summary>
        /// <param name="terminalCode"></param>
        /// <returns></returns>
        private bool IsExistVerifycodeBlackList(string appSign, string terminalCode)
        {
            var parms = new DbParameter[]
            {
               DbParameters.CreateDbParameter("appSign", appSign),
               DbParameters.CreateDbParameter("terminalcode", terminalCode)
            };
            string sql = "SELECT COUNT(BlackId) FROM sys_blacklist WHERE AppSign=@appSign AND TerminalCode=@terminalCode AND LockExpireDate>CURRENT_TIMESTAMP() LIMIT 1;";

            var isExist = this.BaseRepository().FindObject(sql, parms);
            return Extensions.ToInt(isExist, 0) > 0;
        }

        /// <summary>
        /// 验证码错误limitcount内，判定为黑名单用户
        /// </summary>
        /// <param name="terminalCode">硬件标示符</param>
        /// <param name="limitCount">失败次数</param>
        /// <returns></returns>
        private bool IsExistVerifycodeBlackList(string appSign, string terminalCode, int limitCount)
        {
            UpdateVerifycodeBlackList(appSign, terminalCode, limitCount);

            return IsExistVerifycodeBlackList(appSign, terminalCode);
        }

        #endregion
    }
}