namespace Yme.Mcp.Model.QueryModels
{
    /// <summary>
    /// 订单列表查询参数
    /// </summary>
    public class OrderLsitQueryModel : PageViewModel
    {
        /// <summary>
        /// 查询状态或打印状态
        /// </summary>
        public int Status { get; set; }
    }
}
