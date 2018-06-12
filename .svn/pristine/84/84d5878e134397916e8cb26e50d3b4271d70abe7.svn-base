using System.Collections.Generic;
namespace Yme.Mcp.Model.QueryModels
{
    /// <summary>
    /// 订单篮子信息
    /// </summary>
    public class OrderDetailGroupModel
    {
        /// <summary>
        /// 篮子编号
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 篮子明细
        /// </summary>
        public List<OrderDetailQueryModel> Items { get; set; }
    }

    /// <summary>
    /// 订单明细信息
    /// </summary>
    public class OrderDetailQueryModel
    {
        /// <summary>
        /// 品名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 特性
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 篮子编号
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// 平台内部Id
        /// </summary>
        public string InnerId { get; set; }
    }

    /// <summary>
    /// 订单分组
    /// </summary>
    public class OrderGroupQueryModel
    {
        /// <summary>
        /// 篮子编号
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 篮子明细
        /// </summary>
        public List<OrderGroupItemQueryModel> Items { get; set; }

    }

    /// <summary>
    /// 订单分组明细
    /// </summary>
    public class OrderGroupItemQueryModel
    {
        /// <summary>
        /// 品名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string Price { get; set; }
    }
}
