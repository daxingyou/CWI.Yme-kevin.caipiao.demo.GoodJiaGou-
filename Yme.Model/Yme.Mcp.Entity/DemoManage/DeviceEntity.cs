using System;

namespace Yme.Mcp.Entity.DemoManage
{
    /// <summary>
    /// 版 本
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2017.11.16 11:15
    /// 描 述：设备信息
    /// </summary>
	public class DeviceEntity : BaseEntity
    {
		/// <summary>
		/// 设备Id
		/// </summary>
		public string DeviceId { set; get; }

		/// <summary>
		/// 设备型号,1:MCP-360
		/// </summary>
		public int DeviceType { set; get; }

        /// <summary>
		/// 设备状态:1-正常,2-缺纸,3-故障
		/// </summary>
		public int DeviceStatus { set; get; }

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
