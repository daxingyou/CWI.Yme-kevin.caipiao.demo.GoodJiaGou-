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
    /// 订单菜品信息
    /// </summary>
	public class OrderDishEntity : BaseEntity
    {
		/// <summary>
		/// 订单详细ID
		/// </summary>
		public long OrderDishId { set; get; }

		/// <summary>
		/// 微云打订单ID
		/// </summary>
		public string MorderId { set; get; }

		/// <summary>
		/// 平台ID
		/// </summary>
		public  int PlatformId { set; get; }

		/// <summary>
		/// 菜品名称
		/// </summary>
		public string DishName { set; get; }

        /// <summary>
        /// 菜品特性
        /// </summary>
        public string DishProperty { set; get; }

		/// <summary>
		/// 菜品份数
		/// </summary>
		public decimal DishNum { set; get; }

		/// <summary>
		/// 菜品单价
		/// </summary>
		public decimal DishPrice { set; get; }

		/// <summary>
		/// 菜品总额
		/// </summary>
		public decimal DishAmount { set; get; }

        /// <summary>
        /// 菜品篮子Id
        /// </summary>
        public int? DishGroupId { set; get; }

        /// <summary>
        /// 菜品内部Id
        /// </summary>
        public string DishInnerId { set; get; }

		/// <summary>
		/// 下单时间
		/// </summary>
		public DateTime OrderTime { set; get; }

		/// <summary>
		/// 门店ID
		/// </summary>
		public long ShopId { set; get; }
	}
}
