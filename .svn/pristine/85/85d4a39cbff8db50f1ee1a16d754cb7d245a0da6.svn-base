using Yme.Mcp.IService.SystemManage;
using Yme.Mcp.Service.SystemManage;
using System;
using System.Collections.Generic;
using Yme.Mcp.Model;
using Yme.Util.Exceptions;
using Yme.Util.Extension;
using Yme.Util.Enums;
using Yme.Util;
using Yme.Mcp.Model.Enums;
using Yme.Util.Log;

namespace Yme.Mcp.BLL.SystemManage
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.11.12 16:40
    /// 描 述：区域管理
    /// </summary>
    public class VerifyCodeBLL
    {
        #region 私有变量

        private IVerifyCodeService service = new VerifyCodeService();

        #endregion

        #region 公有方法

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="vCodeModel">验证码类型</param>
        public void GetSmsVerifyCode(string appSign, VerifyCodeViewModel vCodeModel)
        {
            try
            {
                if (vCodeModel == null)
                {
                    throw new MessageException("获取验证码参数不可为空！");
                }
                if (vCodeModel.TerminalSign == null)
                {
                    throw new MessageException("终端标识不可为空！");
                }
                VerifyCodeType verifyCodeType = (VerifyCodeType)Extensions.ToInt(vCodeModel.CodeType, 1);
                SmsFuncType funcType = verifyCodeType == VerifyCodeType.BindMobile ? SmsFuncType.BindMobile : SmsFuncType.Login;
                var verifyCode = service.GetVerifyCode(appSign, vCodeModel.Mobile, vCodeModel.TerminalSign, verifyCodeType);

                // 3.调用发送邮件接口,发送验证码
                string title = string.Empty;
                string content = string.Empty;
                SmsType smsType = SmsType.UnKnown;
                decimal validateCodeExpire = ConfigUtil.ValidatecodeExpire / 60;
                switch (verifyCodeType)
                {
                    case VerifyCodeType.Login:
                    case VerifyCodeType.BindMobile:
                        {
                            smsType = SmsType.ValidateCode;
                            content = string.Format(MsgTemplateUtil.MsgDict["SmsValidCode"], verifyCode, validateCodeExpire);
                            break;
                        }
                    default:
                        {
                            throw new MessageException("未知的验证码类型！");
                        }
                }

                if (!string.IsNullOrWhiteSpace(content))
                {
                    //发送短信
                    if (verifyCodeType == VerifyCodeType.Login || verifyCodeType == VerifyCodeType.BindMobile)
                    {
                        var parms = new Dictionary<string, object>();
                        parms.Add("code", verifyCode);
                        parms.Add("code_expire", validateCodeExpire);
                        SingleInstance<SmsBLL>.Instance.SendSmsMsg(appSign, vCodeModel.Mobile, parms, content, smsType, funcType);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.StackTrace);
                throw new MessageException(ex.Message);
            }
        }

        /// <summary>
        /// 更新验证码
        /// </summary>
        /// <param name="appSign"></param>
        /// <param name="verifyCode"></param>
        /// <param name="mobile"></param>
        public void UpdateVerifyCode(string appSign, string verifyCode, string mobile)
        {
            // 1.校验验证码
            long verifyCodeId = -1;
            if (!ConfigUtil.IsTestModel)
            {
                if (!string.IsNullOrWhiteSpace(mobile))
                {
                    verifyCodeId = service.CheckEntity(appSign, verifyCode, mobile);
                }
            }
            else
            {
                if (verifyCode != string.Empty.PadRight(Const.SMS_VALIDATECODE_LENGTH, '0') && !string.IsNullOrWhiteSpace(mobile))
                {
                    verifyCodeId = service.CheckEntity(appSign, verifyCode, mobile);
                }
            }

            // 2.更新验证码状态
            if (verifyCodeId != -1 && !string.IsNullOrWhiteSpace(mobile))
            {
                service.UpdateEntityStatus(appSign, verifyCodeId);
            }
        }

        #endregion
    }
}