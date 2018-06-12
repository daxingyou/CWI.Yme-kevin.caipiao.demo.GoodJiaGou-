//---------------------------------------------
// 版权信息：版权所有(C) 2014，COOLWI.COM
// 变更历史：
//      姓名          日期              说明
// --------------------------------------------
//      王军锋        2014/09/22        创建
//---------------------------------------------

using System;
using System.Collections.Generic;
using Yme.Util;
using Yme.Mcp.WinServ.Busy.TimerCtrl;
using Yme.Util.Log;

namespace Yme.Mcp.WinServ.Busy
{
    public class BusyMain : IDisposable
    {
        #region 单例

        private static object _instanceLock = new object();

        /// <summary>
        /// 实例
        /// </summary>
        private static BusyMain _instance = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        private BusyMain()
        {
            #region 添加时间控件
            ListTimers = new List<BaseTimer>();
            //心跳服务
            ListTimers.Add(new MeituanHeartbeatTimer());
            ListTimers.Add(new MeituanUploadDataTimer());

            //定时服务
            ListTimers.Add(new RemindPreOrderTimer());
            ListTimers.Add(new UpdateOrderStatusTimer());
            ListTimers.Add(new UpdateElemeTokenTimer());

            //报表服务
            //订单业绩日报：凌晨1点0分
            ListTimers.Add(new OrderDayRptTimer());
            //平台订单日报：凌晨1点15分
            ListTimers.Add(new PlatformOrderDayRptTimer());
            //菜品日报：    凌晨1点30分
            ListTimers.Add(new DishDayRptTimer());
            //客户日报：    凌晨1点45分
            ListTimers.Add(new CustomerDayRptTimer());
            #endregion
        }

        /// <summary>
        /// 类唯一实例
        /// </summary>
        public static BusyMain Instance
        {
            get 
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BusyMain();
                        }
                    }
                }
                
                return _instance;            
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 时间控件的集合
        /// </summary>
        public List<BaseTimer> ListTimers { get; set; }

        #endregion

        /// <summary>
        /// 开启业务监控
        /// </summary>
        public void Start()
        {
            LogUtil.Debug("启动业务数据监控");

            //启动每一个时间控件
            ListTimers.ForEach(obj =>
            {
                obj.Start();
            });
        }

        /// <summary>
        /// 暂停业务监控
        /// </summary>
        public void Pause()
        {
            //停止每一个时间控件
            ListTimers.ForEach(obj =>
            {
                obj.Stop();
            });
        }

        /// <summary>
        /// 暂停业务监控
        /// </summary>
        public void Continue()
        {
            //启动每一个时间控件
            ListTimers.ForEach(obj =>
            {
                obj.Start();
            });
        }

        /// <summary>
        /// 停止业务监控
        /// </summary>
        public void Stop()
        {
            LogUtil.Debug("停止业务数据监控");
            //停止每一个时间控件
            ListTimers.ForEach(obj =>
            {
                obj.Stop();
            });
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            LogUtil.Debug("开始释放定时器并退出");

            ListTimers.ForEach(obj =>
            {
                obj.Dispose();
                obj = null;
            });

            ListTimers = null;
        }
    }
}

