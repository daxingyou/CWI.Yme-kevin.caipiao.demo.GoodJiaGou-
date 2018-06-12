using Yme.Mcp.Entity.SystemManage;
using Yme.Mcp.IService.SystemManage;
using Yme.Data.Repository;
using Yme.Util.Extension;
using System.Collections.Generic;
using Yme.Util.Enums;
using System;
using Yme.Util;
using Yme.Util.Exceptions;
using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.Service.SystemManage
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.11.12 16:40
    /// 描 述：区域管理
    /// </summary>
    public class SmsService : RepositoryFactory<SmsEntity>, ISmsService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="appSign"></param>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <param name="smsType"></param>
        /// <returns></returns>
        public void SendSmsMessage(string appSign, string mobile, Dictionary<string, object> parms, string content, SmsType smsType, SmsFuncType funcType)
        {
            // 参数检查
            if (string.IsNullOrWhiteSpace(appSign))
            {
                throw new MessageException("应用标识不能为空！");
            }
            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new MessageException("手机号码不能为空！");
            }
            if (parms.Count <= 0)
            {
                throw new MessageException("短信内容参数不能为空！");
            }
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new MessageException("短信内容不能为空！");
            }
            if (!Extensions.IsMobile(mobile))
            {
                throw new MessageException(string.Format("手机号码：{0},格式不正确。" + mobile));
            }

            //创建短信记录
            var entity = new SmsEntity();
            entity.AppSign = appSign;
            entity.ReceiverCode = mobile;
            entity.SmsContent = content;
            entity.SmsType = smsType.GetHashCode();
            entity.CreateUserId = mobile;
            var smsId = InsertEntity(entity);
            switch (smsType)
            {
                case SmsType.ValidateCode:
                    SmsUtil.SendValidCode(mobile, parms["code"].ToString(), Extensions.ToDecimal(parms["code_expire"], 2), funcType);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private long InsertEntity(SmsEntity entity)
        {
            // 写入数据
            entity.CreateDate = DateTime.Now;
            entity.EnabledFlag = 1;

            return this.BaseRepository().Insert(entity);
        }
    }
}