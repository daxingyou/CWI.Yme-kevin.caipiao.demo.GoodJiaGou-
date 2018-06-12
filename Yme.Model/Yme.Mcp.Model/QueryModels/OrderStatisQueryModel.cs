
namespace Yme.Mcp.Model.QueryModels
{
    /// <summary>
    /// 订单状态统计信息
    /// </summary>
    public class OrderStatisQueryModel
    {
        /// <summary>
        /// 进行中
        /// </summary>
        public int DoingTotal { get; set; }

        /// <summary>
        /// 已完成
        /// </summary>
        public int CompletedTotal { get; set; }

        /// <summary>
        /// 已取消
        /// </summary>
        public int CanceledTotal { get; set; }
    }
}
