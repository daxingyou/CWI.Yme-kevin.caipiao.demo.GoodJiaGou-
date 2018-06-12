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
    /// 更新订单状态定时服务
    /// </summary>
    public class UpdateOrderStatusTimer : BaseTimer
    {
        /// <summary>
        /// 更新订单状态服务
        /// </summary>
        public UpdateOrderStatusTimer()
        {
            this.Interval = ConfigHelper.UpdateOrderStatusInterval;
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        public override void Timer_Elapsed()
        {
            LogUtil.Info("准备更新订单状态...");

            SingleInstance<OrderBLL>.Instance.UpdateOrderStatusTask(ConfigHelper.ForceUpdateStatusMaxMins);
        }
    }
}