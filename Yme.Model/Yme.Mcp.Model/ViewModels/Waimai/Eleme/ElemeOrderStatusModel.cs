using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.ViewModels.Waimai.Eleme
{
    /// <summary>
    /// 适用于【type=14,15,17,18】
    /// </summary>
    public class ElemeOrderStatusModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 饿了么门店Id
        /// </summary>
        public string shopId { get; set; }

        /// <summary>
        /// 取消操作时间
        /// </summary>
        public long updateTime { get; set; }

        /// <summary>
        /// 取消操作角色
        /// </summary>
        public int role { get; set; }
    }
}
