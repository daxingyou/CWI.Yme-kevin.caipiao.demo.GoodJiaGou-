//---------------------------------------------
// 版权信息：版权所有(C) 2017，Yingmei.me
// 变更历史：
//      姓名          日期              说明
// --------------------------------------------
//      王军锋        2017/04/20         创建
//---------------------------------------------
using System;
using Yme.Code;
using Yme.Util;

namespace Yme.Mcp.Entity.BaseManage
{
    /// <summary>
    /// 打印机配置信息
    /// </summary>
	public class PrinterConfigEntity : BaseEntity
    {
		/// <summary>
		/// 打印机配置ID
		/// </summary>
		public long PrinterConfigId { set; get; }

		/// <summary>
		/// 门店ID
		/// </summary>
		public long ShopId { set; get; }

		/// <summary>
		/// 打印机唯一标识
		/// </summary>
		public string PrinterCode { set; get; }

        /// <summary>
        /// 业务类型:1-外卖,2-餐饮,3-商城,4-物流
        /// </summary>
        public int BusinessType { set; get; }

		/// <summary>
		/// 票据ID
		/// </summary>
		public int BillId { set; get; }

		/// <summary>
		/// 打印份数
		/// </summary>
		public  int Copies { set; get; }

		/// <summary>
		/// 有效标记
		/// </summary>
		public  int EnabledFlag { set; get; }

		/// <summary>
		/// 删除标记
		/// </summary>
		public  int DeleteFlag { set; get; }

		/// <summary>
		/// 备注
		/// </summary>
		public string Description { set; get; }

		/// <summary>
		/// 创建人ID
		/// </summary>
		public string CreateUserId { set; get; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateDate { set; get; }

		/// <summary>
		/// 更新人ID
		/// </summary>
		public string ModifyUserId { set; get; }

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? ModifyDate { set; get; }
	}
}
