using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model.McpApi
{
    /// <summary>
    /// 获取打印票据参数
    /// </summary>
    public class GetBillViewModel : ViewModel
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 订单Key
        /// </summary>
        public string key { get; set; }
    }
}
