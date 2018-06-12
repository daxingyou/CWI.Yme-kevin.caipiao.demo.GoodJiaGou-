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

namespace Yme.Mcp.Entity.OrderManage
{
    /// <summary>
    /// 订单发票信息
    /// </summary>
    public class OrderInvoiceEntity : BaseEntity
    {
		/// <summary>
		/// 微云打订单ID
		/// </summary>
		public string MorderId { set; get; }

		/// <summary>
		/// 平台订单ID
		/// </summary>
		public string OrderId { set; get; }

		/// <summary>
        /// 发票类型:1-个人,2-企业
		/// </summary>
        public int InvoiceType { set; get; }

		/// <summary>
        /// 发票抬头
		/// </summary>
        public string InvoiceHeader { set; get; }

		/// <summary>
        /// 纳税人识别号
		/// </summary>
        public string TaxPayerId { set; get; }

		/// <summary>
        /// 是否开具:0-否,1-是
		/// </summary>
        public int IsOpen { set; get; }

		/// <summary>
        /// 备注
		/// </summary>
        public string Description { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { set; get; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? ModifyDate { set; get; }
	}
}
