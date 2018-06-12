using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.WinServ.Common;
using Yme.Util;
using Yme.Util.Log;

namespace Yme.Mcp.WinServ.Busy.TimerCtrl
{
    public class UpdateElemeTokenTimer : BaseTimer
    {
        //执行分钟
        public int runMinute = 45;

        /// <summary>
        /// 更新饿了么令牌服务
        /// </summary>
        public UpdateElemeTokenTimer()
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
        /// 更新饿了么令牌服务
        /// </summary>
        public override void Timer_Elapsed()
        {
            DateTime now = SysDateTime.Now;
            DateTime runTimer = new DateTime(now.Year, now.Month, now.Day, ConfigHelper.DayReportAutoRunTime, runMinute, 0);

            if (now == runTimer)
            {
                LogUtil.Info("开始更新饿了么令牌...");
                SingleInstance<PlatformBLL>.Instance.UpdateElemeToken();
            }
        }
    }
}
