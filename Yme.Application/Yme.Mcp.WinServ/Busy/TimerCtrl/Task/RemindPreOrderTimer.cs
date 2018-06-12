//---------------------------------------------
// 版权信息：版权所有(C) 2017，Yingmei.me
// 变更历史：
//      姓名          日期              说明
// --------------------------------------------
//      王军锋        2017/04/19        创建
//---------------------------------------------
using System;
using Yme.Util;
using Yme.Mcp.BLL;
using Yme.Mcp.WinServ.Common;
using Yme.Mcp.BLL.ReportManage;
using Yme.Util.Log;
using Yme.Mcp.BLL.OrderManage;
using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.WinServ.Busy.TimerCtrl
{
    /// <summary>
    /// 预订单提醒定时服务
    /// </summary>
    public class RemindPreOrderTimer : BaseTimer
    {
        /// <summary>
        /// 预订单提醒服务
        /// </summary>
        public RemindPreOrderTimer()
        {
            this.Interval = ConfigHelper.PreOrderRemindInterval * 1000 * 60;
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        public override void Timer_Elapsed()
        {
            LogUtil.Info("准备执行预订单提醒...");

            SingleInstance<OrderBLL>.Instance.ExecPreOrderRemindTask(ConfigHelper.RemindPreOrderMaxMins, ConfigHelper.PreOrderRemindInterval);
        }
    }
}