namespace Yme.Util
{
    /// <summary>
    /// 支付相关配置
    /// </summary>
    public class PayConfigUtil
    {
        #region 支付宝配置

        /// <summary>
        /// 支付宝合作商家ID
        /// </summary>
        public const string PARTNER = "2088221601143626"; //"2088801350036778";

        /// <summary>
        /// 交易安全检验码，由数字和字母组成的32位字符串
        /// </summary>
        public const string KEY = "4qj7g64g3pvzk4takpb46u30rdrqntav";//"xgnm0oha4pehih4yxmnyuwvayxvrfqsg";

        /// <summary>
        /// 默认编码
        /// </summary>
        public const string INPUTCHARSET = "utf-8";

        /// <summary>
        /// 支付网关接口地址
        /// </summary>
        public const string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";

        /// <summary>
        /// 卖家支付宝账号
        /// </summary>
        public const string SELLEREMAIL = "gm1@jolimark.com";//"2817774769@qq.com";

        /// <summary>
        /// 卖家支付宝用户号
        /// </summary>
        public const string SELLEREID = "2088221601143626";//"20888013500367780156";

        /// <summary>
        /// 支付账户名称
        /// </summary>
        public const string SELLERACCOUNTNAME = "深圳映美卡莫网络有限公司";//"新会江裕信息产业有限公司";

        /// <summary>
        /// RSA 密钥
        /// </summary>
        public static string RSA_PRIVATEKEY
        {
            get
            {
                string key = string.Empty;

                key += @"MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAJk8zwet6BY7p6vN";
                key += @"7cWpFUUkPt7hSa0rXL6dSkZ8Ejq1xJdtQ2hopGJmvNCfN/xwm4Drmnfa2UBRVRsh";
                key += @"BJNjjKw57cgP6Q9Vy/GOeXsZQAcxAf5kUJbaT7awURZVy4VvsVqzHOYbtXFJHx2o";
                key += @"mxRVs4aC9NN76tSJQwMSnBEs2xKhAgMBAAECgYBll3wGMlSxEMG71F3z8oJIgZww";
                key += @"9Zl79kiZkvwgOLd5NvLsFaNgACgjmMtLYJOkiB+AXDLfjTcFPia3Qq+e65vhuBxb";
                key += @"2Uz2ezlp7cykBWveHAFZdUqYia9I+vNiBtOUGhGI3g8zO/BJrR+qlMKUBV2o5FpL";
                key += @"3z11Ac8ER0C8pcq9AQJBAMs7m3Q/NLdoYFDSkHGybleXzfqnKVQ7MOVoAkr73j4V";
                key += @"QV7XFE9F1RbvVlla8Y4+8LroR3XuTSFo/AljswxIGbECQQDBBiKO8aATNaT7ycxR";
                key += @"ADRWyAWWDrYzEb5cKDR26E+3xZ64Q88Cx8efkCS03MfrYW9HAgpPAO3D5VPoKooy";
                key += @"LtPxAkEAuF6E8np8mn5oEipTu7GBhJlE1cVcKvvfqd4nZlsEDRI/UYiQ9jGF1N6f";
                key += @"WUS1qrPSs90RJx1ef5a8PtBhmBy4oQJBAImDt2cjGSnWU/3V3Kvwe1lAE0q+nGtx";
                key += @"vq6lLgtwoiqIrfUUygxvC/bPmHcJu5wjxEVQXMvAG6QuY0LJ2764weECQBQGsKBL";
                key += @"HlkIs2MbF83MAA70yI8lskZa6Ecr5mCNHf4eNt5uKu78n1hRAde6eoZkPZyyBXRZ";
                key += @"ALijGORasDkOq2Y=";

                return key;
            }
        }

        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public static string ALIPAY_PUBLICKKEY
        {
            get
            {
                return @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
            }
        }

        /// <summary>
        /// 加密Salt种子
        /// </summary>
        public static string ENCRYPT_SALT = "@~#$%^&*()_+!";

        /// <summary>
        /// 是否开启接收支付回调
        /// </summary>
        public static bool EnablePayCallBack
        {
            get
            {
                var value = ConfigUtil.GetValue("EnablePayCallBack");
                return value == null ? true : value.Equals("1");
            }
        }

        /// <summary>
        /// 支付回调地址
        /// </summary>
        public static string PayedCallBackUrl
        {
            get
            {
                var value = ConfigUtil.GetValue("PayedCallBackUrl");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 支付订单描述
        /// </summary>
        public static string PaySubject
        {
            get
            {
                var value = ConfigUtil.GetValue("PaySubject");
                return value == null ? "映美MeO2O打印费用" : value.ToString();
            }
        }

        /// <summary>
        /// 支付订单描述
        /// </summary>
        public static string ItBPay
        {
            get
            {
                var value = ConfigUtil.GetValue("ItBPay");
                return value == null ? "29m" : value.ToString();
            }
        }

        /// <summary>
        /// 转账回调地址
        /// </summary>
        public static string TransferedCallBackUrl
        {
            get
            {
                var value = ConfigUtil.GetValue("TransferedCallBackUrl");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 退款回调地址
        /// </summary>
        public static string RefundCallBackUrl
        {
            get
            {
                var value = ConfigUtil.GetValue("RefundCallBackUrl");
                return value == null ? string.Empty : value.ToString();
            }
        }

        #endregion

        #region 微信支付配置

        /// <summary>
        /// 微信支付回调地址
        /// </summary>
        public static string WechatPayedCallBackUrl
        {
            get
            {
                var value = ConfigUtil.GetValue("WechatPayedCallBackUrl");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 微信退款回调地址
        /// </summary>
        public static string WechatRefundCallBackUrl
        {
            get
            {
                var value = ConfigUtil.GetValue("WechatRefundCallBackUrl");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 商户系统后台机器IP
        /// </summary>
        public static string WechatPayMerchantIP
        {
            get
            {
                var value = ConfigUtil.GetValue("WechatPayMerchantIP");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 【代理服务器设置】默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        /// </summary>
        public static string WechatProxyUrl
        {
            get
            {
                var value = ConfigUtil.GetValue("WechatProxyUrl");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 【证书路径设置：仅退款、撤销订单时需要】证书路径,注意应该填写绝对路径
        /// </summary>
        public static string WechatPaySslcertPathForApp
        {
            get
            {
                var value = ConfigUtil.GetValue("WechatPaySslcertPathForApp");
                return value == null ? string.Empty : value.ToString();
            }
        }
        /// <summary>
        /// 【证书路径设置：仅退款、撤销订单时需要】证书路径,注意应该填写绝对路径
        /// </summary>
        public static string WechatPaySslcertPathForJSAPI
        {
            get
            {
                var value = ConfigUtil.GetValue("WechatPaySslcertPathForJSAPI");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 【上报信息配置】测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        /// </summary>
        public const int REPORT_LEVENL = 1;

        /// <summary>
        /// 【日志级别】日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        /// </summary>
        public const int LOG_LEVENL = 0;

        #endregion
    }
}
