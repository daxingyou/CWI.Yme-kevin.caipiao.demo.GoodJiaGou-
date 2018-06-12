using System;
using Yme.Util.Attributes;

namespace Yme.Util
{
    /// <summary>
    /// 微信支付接口参数基类
    /// </summary>
    public class WxPayBaseInfo
    {
        /// <summary>
        /// 公众号Id【只读】
        /// </summary>
        [PayDescription("appid")]
        public string AppId { get; set; }

        /// <summary>
        /// 时间戳【只读】
        /// </summary>
        [PayDescription("timestamp")]
        public string TimeStamp { get; set; }

        /// <summary>
        /// 随机字符串【只读】
        /// </summary>
        [PayDescription("noncestr")]
        public string NonceStr { get; set; }

        /// <summary>
        /// 订单详情扩展字符串
        /// </summary>
        [PayDescription("package")]
        public string Package { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [PayDescription("sign")]
        public string Sign { get; set; }
    }
}
