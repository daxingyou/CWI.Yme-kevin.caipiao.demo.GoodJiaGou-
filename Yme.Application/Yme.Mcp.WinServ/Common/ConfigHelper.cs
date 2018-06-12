//---------------------------------------------
// 版权信息：版权所有(C) 2017，Yingmei.me
// 变更历史：
//      姓名          日期              说明
// --------------------------------------------
//      王军锋        2017/04/19        创建
//---------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Util;
using Yme.Util.Extension;

namespace Yme.Mcp.WinServ.Common
{
    public class ConfigHelper : Yme.Util.ConfigUtil
    {
        #region 美团外卖

        /// <summary>
        /// 通用心跳频率（单位：秒）
        /// </summary>
        public static int MeiHearbeatInterval
        {
            get
            {
                return Extensions.ToInt(GetValue("MeiHearbeatInterval"), 1) * 1000;
            }
        }

        /// <summary>
        /// 上传数据频率（单位：小时）
        /// </summary>
        public static int MeiUploadDataInterval
        {
            get
            {
                return Extensions.ToInt(GetValue("MeiUploadDataInterval"), 1) * 1000 * 60 * 60;
            }
        }

        #endregion

        #region 饿了么

        #endregion

        #region 百度外卖

        #endregion

        #region 定时报表

        /// <summary>
        /// 日报自动生成时间 （凌晨0点后的小时数，如1代表凌晨1点）
        /// </summary>
        public static int DayReportAutoRunTime
        {
            get
            {
                return Extensions.ToInt(GetValue("DayReportAutoRunTime"), 1);
            }
        }

        /// <summary>
        /// 月报表自动生成日期 （当月的几号，如2代表当月的2日汇总上月的结果）
        /// </summary>
        public static int MonthReportAutoRunDay
        {
            get
            {
                return Extensions.ToInt(GetValue("MonthReportAutoRunDay"), 1);
            }
        }

        /// <summary>
        /// 更新订单状态频率[单位：分钟]
        /// </summary>
        public static int UpdateOrderStatusInterval
        {
            get
            {
                return Extensions.ToInt(GetValue("UpdateOrderStatusInterval"), 1) * 1000 * 60;
            }
        }

        /// <summary>
        /// 强制更新订单状态最大分钟数
        /// </summary>
        public static int ForceUpdateStatusMaxMins
        {
            get
            {
                return Extensions.ToInt(GetValue("ForceUpdateStatusMaxMins"), 60);
            }
        }

        /// <summary>
        /// 自动清理打印任务频率[单位：分钟]
        /// </summary>
        public static int ClearPrintTaskInterval
        {
            get
            {
                return Extensions.ToInt(GetValue("ClearPrintTaskInterval"), 1) * 1000 * 60;
            }
        }

        /// <summary>
        /// 打印任务保留最大小时数 [单位：小时]
        /// </summary>
        public static int PrintTaskExistsMaxHours
        {
            get
            {
                return Extensions.ToInt(GetValue("PrintTaskExistsMaxHours"), 1);
            }
        }

        /// <summary>
        /// 预订单判断间隔时间 [单位：分钟]
        /// </summary>
        public static int RemindPreOrderMaxMins
        {
            get
            {
                return Extensions.ToInt(GetValue("RemindPreOrderMaxMins"), 1);
            }
        }

        /// <summary>
        /// 预订单提醒服务间隔时间 [单位：分钟]
        /// </summary>
        public static int PreOrderRemindInterval
        {
            get
            {
                return Extensions.ToInt(GetValue("PreOrderRemindInterval"), 1);
            }
        }

        #endregion
    }
}
