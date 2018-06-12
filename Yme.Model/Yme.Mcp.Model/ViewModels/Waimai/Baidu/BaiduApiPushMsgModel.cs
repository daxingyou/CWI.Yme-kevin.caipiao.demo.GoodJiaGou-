using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.ViewModels.Waimai.Baidu
{
    /// <summary>
    /// 百度外卖消息实体
    /// </summary>
    public class BaiduApiPushMsgModel
    {
        /// <summary>
        /// 消息发送的时间戳【单位毫秒】
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 请求流水号
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 加密方式
        /// </summary>
        public string encrypt { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string cmd { get; set; }

        /// <summary>
        /// 百度商户帐号
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 消息签名
        /// </summary>
        public string sign { get; set; }
    }
}
