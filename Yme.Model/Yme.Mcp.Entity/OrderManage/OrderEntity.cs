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
    /// 订单信息
    /// </summary>
	public class OrderEntity : BaseEntity
    {
        /// <summary>
        /// 记录ID
        /// </summary>		
        public int Id { get; set; }

		/// <summary>
		/// 微云打订单ID
		/// </summary>
		public string MorderId { set; get; }

        /// <summary>
        /// 平台内部订单ID
        /// </summary>
        public string OrderId { set; get; }

		/// <summary>
        /// 平台显示订单ID
		/// </summary>
		public string OrderViewId { set; get; }

        /// <summary>
        /// 平台订单类型:1-即时订单,2-预订单
        /// </summary>
        public int OrderType { set; get; }

        /// <summary>
        /// 平台门店当天的订单流水号
        /// </summary>
        public int DaySeq { set; get; }

		/// <summary>
		/// 平台类型ID
		/// </summary>
		public int PlatformId { set; get; }

		/// <summary>
		/// 平台业务ID
		/// </summary>
		public int BussinessType { set; get; }

		/// <summary>
		/// 门店ID
		/// </summary>
		public long ShopId { set; get; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string ShopName { set; get; }

		/// <summary>
		/// 收货人姓名
		/// </summary>
		public string RecipientName { set; get; }

		/// <summary>
		/// 收货人电话
		/// </summary>
		public string RecipientPhone { set; get; }

		/// <summary>
		/// 收货地址
		/// </summary>
		public string RecipientAddress { set; get; }

		/// <summary>
		/// 下单时间
		/// </summary>
		public DateTime OrderTime { set; get; }

		/// <summary>
		/// 送达时间
		/// </summary>
		public string DeliveryTime { set; get; }

		/// <summary>
		/// 用餐人数
		/// </summary>
		public  int DinnersNum { set; get; }

		/// <summary>
		/// 订单备注
		/// </summary>
		public string Caution { set; get; }

		/// <summary>
		/// 配送服务
		/// </summary>
		public string DeliveryService { set; get; }

		/// <summary>
		/// 配送费
		/// </summary>
		public decimal DeliveryFee { set; get; }

		/// <summary>
		/// 订单总额
		/// </summary>
		public decimal OrderAmount { set; get; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { set; get; }

        /// <summary>
        /// 订单数据
        /// </summary>
        public string OrderData { set; get; }

        /// <summary>
        /// 订单明细总数
        /// </summary>
        public decimal OrderItemTotal { set; get; }

		/// <summary>
		/// 订单状态:1-待接单,2-已接单,3-配送中,4-已完成,5-已取消
		/// </summary>
		public  int OrderStatus { set; get; }

        /// <summary>
        /// 打印时间
        /// </summary>
        public DateTime? PrintTime { set; get; }

        /// <summary>
        /// 打印订单ID
        /// </summary>
        public string PrintOrderId { set; get; }

        /// <summary>
        /// 打印结果编码
        /// </summary>
        public string PrintResultCode { set; get; }

        /// <summary>
        /// 打印状态:1-待打印,2-已打印,3-打印失败,4-超时未打印
        /// </summary>
        public int PrintStatus { set; get; }

        /// <summary>
        /// 打印次数
        /// </summary>
        public int PrintTotal { set; get; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime? CancelTime { set; get; }

        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason { set; get; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { set; get; }

        /// <summary>
        /// 是否预订单:0-否,1-是
        /// </summary>
        public int IsPreOrder { set; get; }

        /// <summary>
        /// 预计送达时间
        /// </summary>
        public DateTime? PreDeliveryTime { set; get; }

        /// <summary>
        /// 是否已提醒:0-否,1-是
        /// </summary>
        public int IsRemind { set; get; }

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
