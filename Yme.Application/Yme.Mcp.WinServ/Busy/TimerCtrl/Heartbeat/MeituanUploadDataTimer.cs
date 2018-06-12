//---------------------------------------------
// 版权信息：版权所有(C) 2017，Yingmei.me
// 变更历史：
//      姓名          日期              说明
// --------------------------------------------
//      王军锋        2017/04/19        创建
//---------------------------------------------
using Yme.Util;
using Yme.Mcp.BLL;
using Yme.Mcp.WinServ.Common;
using Yme.Mcp.BLL.BaseManage;
using Yme.Util.Log;

namespace Yme.Mcp.WinServ.Busy.TimerCtrl
{
    /// <summary>
    /// 美团外卖补充数据服务
    /// </summary>
    public class MeituanUploadDataTimer : BaseTimer
    {
        /// <summary>
        /// 美团外卖补充数据服务
        /// </summary>
        public MeituanUploadDataTimer()
        {
            this.Interval = ConfigHelper.MeiUploadDataInterval;
        }

        /// <summary>
        /// 执行心跳服务
        /// </summary>
        public override void Timer_Elapsed()
        {
            LogUtil.Info("向美团平台发送补充数据...");

            //SingleInstance<PlatformBLL>.Instance.SendUploadData();
        }
    }
}
