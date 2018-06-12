using Yme.Util.Attributes;
using Yme.Util.Enums;

namespace Yme.Util
{
    /// <summary>
    /// 支付宝接口参数基类
    /// </summary>
    public class PayBaseInfo
    {
        /// <summary>
        /// 接口名称【只读】
        /// </summary>
        [PayDescription("service")]
        public string Service { get; protected set; }

        /// <summary>
        /// 合作者ID【只读】
        /// </summary>
        [PayDescription("partner")]
        public string Partner
        {
            get
            {
                return PayConfigUtil.PARTNER;
            }
        }

        /// <summary>
        /// 编码字符【只读】
        /// </summary>
        [PayDescription("_input_charset")]
        public string InputCharset
        {
            get
            {
                return PayConfigUtil.INPUTCHARSET;
            }
        }

        /// <summary>
        /// 签名方式【只读】
        /// </summary>
        [PayDescription("sign_type")]
        public SignType SignType { get; protected set; }

        /// <summary>
        /// 签名【有默认值】
        /// </summary>
        [PayDescription("sign")]
        public string Sign { get; set; }

        /// <summary>
        /// 服务器异步通知回调接口
        /// </summary>
        [PayDescription("notify_url")]
        public string NotifyUrl { get; set; }
    }
}
