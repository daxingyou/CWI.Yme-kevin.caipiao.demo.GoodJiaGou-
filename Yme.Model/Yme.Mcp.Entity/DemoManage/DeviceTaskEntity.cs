using System;

namespace Yme.Mcp.Entity.DemoManage
{
    /// <summary>
    /// 版 本
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2017.11.16 11:15
    /// 描 述：设备任务信息
    /// </summary>
    public class DeviceTaskEntity : BaseEntity
    {
        /// <summary>
        /// 设备任务Id
        /// </summary>
        public long DeviceTaskId { set; get; }

        /// <summary>
        /// 任务Id
        /// </summary>
        public string TaskId { set; get; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public string DeviceId { set; get; }

        /// <summary>
        /// 任务份数
        /// </summary>
        public int Copies { set; get; }

        /// <summary>
        /// 任务状态:1-打印成功,2-打印机缺纸,3-打印机故障
        /// </summary>
        public int TaskStatus { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { set; get; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? ModifyDate { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { set; get; }
    }
}
