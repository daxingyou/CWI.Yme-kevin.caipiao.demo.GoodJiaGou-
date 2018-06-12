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
using Yme.Util.Extension;

namespace Yme.Mcp.WinServ.Busy.TimerCtrl
{
    /// <summary>
    /// 平台订单日报定时服务
    /// </summary>
    public class PlatformOrderDayRptTimer : BaseTimer
    {
        //执行分钟
        public int runMinute = 15;

        /// <summary>
        /// 平台订单日报服务
        /// </summary>
        public PlatformOrderDayRptTimer()
        {
            DateTime now = SysDateTime.Now;
            if (now == beginDateTime)
            {
                //周期1小时
                this.Interval = hourInterval;
            }
            else
            {
                //周期1秒
                this.Interval = sencondsInterval;
                DateTime zeroTime = now.AddHours(1);
                beginDateTime = new DateTime(zeroTime.Year, zeroTime.Month, zeroTime.Day, zeroTime.Hour, runMinute, 0);
            }
        }

        /// <summary>
        /// 执行报表服务
        /// </summary>
        public override void Timer_Elapsed()
        {
            //每天定点生成前一天的数据报表
            DateTime now = SysDateTime.Now;
            DateTime runTimer = new DateTime(now.Year, now.Month, now.Day, ConfigHelper.DayReportAutoRunTime, runMinute, 0);

            if (now == runTimer)
            {
                LogUtil.Info("开始生成平台订单统计日报...");

                SingleInstance<ReportBLL>.Instance.CreatePlatformOrderDayReport(now.AddDays(-1).Date);
            }

            //var dateList = TimeUtil.GetDateList("2017-05-19".ToDate(), now.AddDays(-1).Date);
            //foreach (var d in dateList)
            //{
            //    LogUtil.Info("开始生成平台订单统计日报...");

            //    SingleInstance<ReportBLL>.Instance.CreatePlatformOrderDayReport(d.ToDate());
            //}
        }
    }
}
