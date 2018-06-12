using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.ViewModels.Waimai.Eleme
{
    /// <summary>
    /// 饿了么消息实体
    /// </summary>
    public class ElemeApiPushMsgModel
    {
        /// <summary>
        /// 消息的唯一id，用于唯一标记每个消息
        /// </summary>
        public string requestId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 应用id
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// 商户的店铺id
        /// </summary>
        public string shopId { get; set; }

        /// <summary>
        /// 消息发送的时间戳【单位毫秒】
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        /// 授权商户的账号id
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 消息签名
        /// </summary>
        public string signature { get; set; }
    }
}
