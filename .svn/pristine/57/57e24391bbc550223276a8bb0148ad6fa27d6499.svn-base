using Yme.Util.Attributes;

namespace Yme.Util
{
    /// <summary>
    /// 支付宝接口参数基类
    /// </summary>
    public class RequestModelForJSAPI
    {
        /// <summary>
        /// 公众号Id【只读】
        /// </summary>
        [PayDescription("appId")]
        public string appId { get; set; }

        /// <summary>
        /// 随机字符串【只读】
        /// </summary>
        [PayDescription("nonceStr")]
        public string nonceStr { get; set; }

        /// <summary>
        /// 订单详情扩展字符串
        /// </summary>
        [PayDescription("package")]
        public string package { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [PayDescription("paySign")]
        public string paySign { get; set; }


        /// <summary>
        /// 订单详情扩展字符串
        /// </summary>
        [PayDescription("signType")]
        public string signType { get; set; }        
        
        /// <summary>
        /// 时间戳【只读】
        /// </summary>
        [PayDescription("timeStamp")]
        public string timeStamp { get; set; }

    }
}
