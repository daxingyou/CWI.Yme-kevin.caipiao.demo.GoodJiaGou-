namespace Yme.Mcp.Model.QueryModels
{
    /// <summary>
    /// 订单搜索查询参数
    /// </summary>
    public class OrderSearchQueryModel : PageViewModel
    {
        /// <summary>
        /// 搜索类型
        /// </summary>
        public int SearchType { get; set; }

        /// <summary>
        /// 订单状态，0-默认全部订单状态
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 打印状态，0-默认全部打印状态
        /// </summary>
        public int PrintStatus { get; set; }

        /// <summary>
        /// 平台ID，0-默认为全部平台，1-美团外卖，2-饿了么，3-百度外卖
        /// </summary>
        public int PlatformId { get; set; }

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string KeyWords { get; set; }
    }
}
