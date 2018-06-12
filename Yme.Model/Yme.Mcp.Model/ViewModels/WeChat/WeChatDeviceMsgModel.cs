
namespace Yme.Mcp.Model.WeChat
{
    /// <summary>
    /// 图片或文件消息推送的设备信息返回属性
    /// </summary>
    public class WeChatDeviceMsgModel
    {
        /// <summary>
        /// 设备DEVICE_ID编码，唯一标识设备
        /// </summary>
        public string device_id { get; set; }

        /// <summary>
        /// 设备类型(公众账号原始ID   )
        /// </summary>
        public string device_type { get; set; }

        /// <summary>
        /// 消息序列号，用于异步通信，用于异步通信，由微信生成，接收方异步返回的时候带上
        /// </summary>
        public string msg_id { get; set; }

        /// <summary>
        /// 消息类型命令字，get代表设备查询消息
        /// </summary>
        public string msg_type { get; set; }

        /// <summary>
        /// 能力项键集合
        /// </summary>
        public WeChatServicesMsgModel services { get; set; }

        /// <summary>
        /// 微信用户OpenId
        /// </summary>
        public string user { get; set; }
    }
}
