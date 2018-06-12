using Yme.Mcp.Service.SystemManage;
using Yme.Mcp.IService.SystemManage;
using System.Collections.Generic;
using Yme.Util.Enums;
using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.BLL.SystemManage
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.11.12 16:40
    /// 描 述：区域管理
    /// </summary>
    public class SmsBLL
    {
        #region 私有变量

        private ISmsService service = new SmsService();

        #endregion

        #region 公有方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSign"></param>
        /// <param name="mobile"></param>
        /// <param name="parms"></param>
        /// <param name="content"></param>
        /// <param name="smsType"></param>
        /// <param name="funcType"></param>
        public void SendSmsMsg(string appSign, string mobile, Dictionary<string, object> parms, string content, SmsType smsType, SmsFuncType funcType)
        {
            service.SendSmsMessage(appSign, mobile, parms, content, smsType, funcType);
        }

        #endregion
    }
}