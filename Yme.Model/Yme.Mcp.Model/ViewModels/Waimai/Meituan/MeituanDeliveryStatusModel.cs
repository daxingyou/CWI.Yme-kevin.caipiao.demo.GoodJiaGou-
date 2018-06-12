using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.Waimai.Meituan
{
    public class MeituanDeliveryStatusModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public long orderId { get; set; }

        /// <summary>
        /// 配送状态【0-配送单发往配送;10-配送单已确认;20-骑手已取餐;40-骑手已送达;100-配送单已取消】
        /// </summary>
        public int shippingStatus { get; set; }

        /// <summary>
        /// 发生时间【单位秒】
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 骑手姓名
        /// </summary>
        public string dispatcherName { get; set; }

        /// <summary>
        /// 骑手电话
        /// </summary>
        public string dispatcherMobile { get; set; }
    }
}
