using System.Collections.Generic;
namespace Yme.Mcp.Model.QueryModels
{
    /// <summary>
    /// 订单费用明细信息
    /// </summary>
    public class OrderFeeQueryModel
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayDesc { get; set; }

        /// <summary>
        /// 订单总额
        /// </summary>
        public string OrderAmount { get; set; }

        /// <summary>
        /// 实际待付总额
        /// </summary>
        public string PayAmount { get; set; }

        /// <summary>
        /// 费用明细
        /// </summary>
        public List<OrderGroupItemQueryModel> FeeDetail { get; set; }
    }
}
