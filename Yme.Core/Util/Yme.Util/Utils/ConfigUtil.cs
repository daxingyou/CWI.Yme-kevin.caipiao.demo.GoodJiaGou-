using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Linq;
using Yme.Util.Extension;

namespace Yme.Util
{
    /// <summary>
    /// Config文件操作
    /// </summary>
    public class ConfigUtil
    {
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }
        /// <summary>
        /// 根据Key修改Value
        /// </summary>
        /// <param name="key">要修改的Key</param>
        /// <param name="value">要修改为的值</param>
        public static void SetValue(string key, string value)
        {
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(HttpContext.Current.Server.MapPath("~/XmlConfig/sys.config"));
            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", value);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", key);
                xElem2.SetAttribute("value", value);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(HttpContext.Current.Server.MapPath("~/XmlConfig/sys.config"));
        }

        #region 系统通用配置

        /// <summary>
        /// 系统应用标识
        /// </summary>
        public static string SystemAppSign
        {
            get
            {
                return GetValue("SystemAppSign");
            }
        }

        /// <summary>
        /// 系统用户Session标识
        /// </summary>
        public static string SystemUserSessionKey
        {
            get
            {
                return string.Format("{0}_login_user", GetValue("SystemAppSign"));
            }
        }

        /// <summary>
        /// 系统终端Session标识
        /// </summary>
        public static string SystemTerminalSessionKey
        {
            get
            {
                return string.Format("{0}_terminal_info", GetValue("SystemAppSign"));
            }
        }

        /// <summary>
        /// 是否测试模式
        /// </summary>
        public static bool IsTestModel
        {
            get
            {
                bool isTestModel = false;
                if (GetValue("IsTestModel").ToInt(0) == 1)
                {
                    isTestModel = true;
                }

                return isTestModel;
            }
        }

        /// <summary>
        /// 验证码失败次数 DEBUG模式 3 ， Release 模式 10
        /// </summary>
        public static int VarifyCodeFailCount
        {
            get
            {
#if DEBUG
                int defaultCount = 3;
#else
                int defaultCount = 10;
#endif
                return GetValue("VarifyCodeFailCount").ToInt(defaultCount);
            }
        }

        /// <summary>
        /// 验证码有效时间，单位：秒 DEBUG模式 60 ， Release 模式 1800
        /// </summary>
        public static int ValidatecodeExpire
        {
            get
            {
#if DEBUG
                int defaultCount = 60;
#else
                int defaultCount = 1800;
#endif
                return GetValue("ValidatecodeExpire").ToInt(defaultCount);
            }
        }

        /// <summary>
        /// 系统部署服务器IP
        /// </summary>
        public static string McpOrderServerIp
        {
            get
            {
                return GetValue("McpOrderServerIp");
            }
        }

        #endregion

        #region 发送邮件服务器配置

        /// <summary>
        /// 邮件帐号名称
        /// </summary>
        public static string EmailName
        {
            get
            {
                return GetValue("EmailName");
            }
        }

        /// <summary>
        /// 邮件帐号
        /// </summary>
        public static string EmailAccount
        {
            get
            {
                return GetValue("EmailAccount");
            }
        }

        /// <summary>
        /// 邮件帐号密码
        /// </summary>
        public static string EmailAccountPwd
        {
            get
            {
                return GetValue("EmailAccountPwd");
            }
        }

        /// <summary>
        /// 邮件服务器域名
        /// </summary>
        public static string EmailServerHost
        {
            get
            {
                return GetValue("EmailServerHost");
            }
        }

        /// <summary>
        /// 邮件服务器端口
        /// </summary>
        public static int EmailServerHostPort
        {
            get
            {
                return GetValue("EmailServerHostPort").ToInt(25);
            }
        }

        /// <summary>
        /// 是否启用ssl加密
        /// </summary>
        public static bool EmailServerEnableSSL
        {
            get
            {
                return GetValue("EmailServerEnableSSL").ToInt(0) > 0;
            }
        }

        /// <summary>
        /// 是否系统错误自动发送邮件
        /// </summary>
        public static bool EmailAutoSendError
        {
            get
            {
                return GetValue("EmailAutoSendError").ToInt(0) > 0;
            }
        }

        #endregion

        #region 短信服务配置

        /// <summary>
        /// 发送渠道
        /// </summary>
        public static int SmsWay
        {
            get
            {
                return GetValue("SmsWay").ToInt(1);
            }
        }

        /// <summary>
        /// 【电信】短信AppId
        /// </summary>
        public static string SmsAppId_dx
        {
            get
            {
                return GetValue("SmsAppId_dx");
            }
        }

        /// <summary>
        /// 【电信】短信AppSecret
        /// </summary>
        public static string SmsAppSecret_dx
        {
            get
            {
                return GetValue("SmsAppSecret_dx");
            }
        }

        /// <summary>
        /// 【荣联云】短信AppId
        /// </summary>
        public static string SmsAppId_rly
        {
            get
            {
                return GetValue("SmsAppId_rly");
            }
        }

        /// <summary>
        /// 【荣联云】短信AppSecret
        /// </summary>
        public static string SmsAppSecret_rly
        {
            get
            {
                return GetValue("SmsAppSecret_rly");
            }
        }

        /// <summary>
        /// 【荣联云】短信帐号
        /// </summary>
        public static string SmsAccountSid_rly
        {
            get
            {
                return GetValue("SmsAccountSid_rly");
            }
        }

        /// <summary>
        /// 【荣联云】短信帐号令牌
        /// </summary>
        public static string SmsAuthToken_rly
        {
            get
            {
                return GetValue("SmsAuthToken_rly");
            }
        }

        #endregion

        #region 推送服务端配置

        /// <summary>
        /// IOS证书文件路径
        /// </summary>
        public static string AppleCertificateFilePath
        {
            get
            {
                return GetValue("AppleCertificateFilePath");
            }
        }

        /// <summary>
        /// IOS证书文件密码
        /// </summary>
        public static string AppleCertificatePwd
        {
            get
            {
                return GetValue("AppleCertificatePwd");
            }
        }

        /// <summary>
        /// Android APIKey
        /// </summary>
        public static string AndroidApiKey
        {
            get
            {
                return GetValue("AndroidApiKey");
            }
        }

        /// <summary>
        /// Android SecretKey
        /// </summary>
        public static string AndroidSecretKey
        {
            get
            {
                return GetValue("AndroidSecretKey");
            }
        }

        /// <summary>
        /// 消息推送提醒方式是否带声音：0-无声音,1-有声音
        /// </summary>
        public static bool IsWithSound
        {
            get
            {
                return GetValue("IsWithSound").ToInt(0) > 0;
            }
        }

        /// <summary>
        /// 推送消息标题
        /// </summary>
        public static string PushTitle
        {
            get
            {
                return GetValue("PushTitle");
            }
        }

        #endregion

        #region 微信接入配置

        /// <summary>
        /// 关注自动回复
        /// </summary>
        public static string WechatSubscribeAutoMessage
        {
            get
            {
                var sbStr = new StringBuilder();
                var value = GetValue("WechatSubscribeAutoMessage") ?? string.Empty;
                sbStr.AppendLine(value);
                sbStr.AppendLine("更多服务,请回复序号:");
                sbStr.AppendLine("[1]查看打印机Wi-Fi配网指南");
                sbStr.AppendLine("[2]查看平台使用指南");
                return sbStr.ToString();
            }
        }

        /// <summary>
        /// 自动回复关键词1
        /// </summary>
        public static List<string> WechatAutoMsgKeyWord1
        {
            get
            {
                var list = new List<string>();
                var value = GetValue("WechatAutoMsgKeyWord1");
                if (!string.IsNullOrWhiteSpace(value))
                {
                    list = value.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                return list;
            }
        }

        /// <summary>
        /// 自动回复关键词2
        /// </summary>
        public static List<string> WechatAutoMsgKeyWord2
        {
            get
            {
                var list = new List<string>();
                var value = GetValue("WechatAutoMsgKeyWord2");
                if (!string.IsNullOrWhiteSpace(value))
                {
                    list = value.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                return list;
            }
        }

        /// <summary>
        /// 自动回复内容1
        /// </summary>
        public static string WechatAutoMsgKeyUrl1
        {
            get
            {
                var value = GetValue("WechatAutoMsgKeyUrl1");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 自动回复内容2
        /// </summary>
        public static string WechatAutoMsgKeyUrl2
        {
            get
            {
                var value = GetValue("WechatAutoMsgKeyUrl2");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 自动回复图1
        /// </summary>
        public static string WechatAutoMsgKeyPicUrl1
        {
            get
            {
                var value = GetValue("WechatAutoMsgKeyPicUrl1");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 自动回复图2
        /// </summary>
        public static string WechatAutoMsgKeyPicUrl2
        {
            get
            {
                var value = GetValue("WechatAutoMsgKeyPicUrl2");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 自动回复消息描述1
        /// </summary>
        public static string WechatAutoMsgKeyDesc1
        {
            get
            {
                var value = GetValue("WechatAutoMsgKeyDesc1");
                return value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// 自动回复消息描述2
        /// </summary>
        public static string WechatAutoMsgKeyDesc2
        {
            get
            {
                var value = GetValue("WechatAutoMsgKeyDesc2");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WechatAccount
        {
            get
            {
                var value = GetValue("WechatAccount");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WechatToken
        {
            get
            {
                var value = GetValue("WechatToken");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WechatAppId
        {
            get
            {
                var value = GetValue("WechatAppId");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WechatAppSecret
        {
            get
            {
                var value = GetValue("WechatAppSecret");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WxPayAppId
        {
            get
            {
                var value = GetValue("WxPayAppId");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WechatPayAppSecret
        {
            get
            {
                var value = GetValue("WxPayAppSecret");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string MerchantWechatAppSecret
        {
            get
            {
                var value = GetValue("MerchantWechatAppSecret");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WechatMerchantIdForApp
        {
            get
            {
                var value = GetValue("WechatMerchantIdForApp");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WechatPayKeyForApp
        {
            get
            {
                var value = GetValue("WechatPayKeyForApp");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WechatMerchantIdForJSAPI
        {
            get
            {
                var value = GetValue("WechatMerchantIdForJSAPI");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WechatPayKeyForJSAPI
        {
            get
            {
                var value = GetValue("WechatPayKeyForJSAPI");
                return value == null ? string.Empty : value.ToString();
            }
        }

        #endregion

        #region 微信设备功能配置

        public static string IotToken
        {
            get
            {
                var value = GetValue("IotToken");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string ProductId
        {
            get
            {
                var value = GetValue("ProductId");
                return value == null ? string.Empty : value.ToString();
            }
        }

        public static string WiFiKey
        {
            get
            {
                var value = GetValue("WiFiKey");
                return value == null ? string.Empty : value.ToString();
            }
        }

        #endregion

        #region 支付配置

        /// <summary>
        /// 是否开启接收支付回调
        /// </summary>
        public static bool EnablePayCallBack
        {
            get
            {
                var value = GetValue("EnablePayCallBack");
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
                var value = GetValue("PayedCallBackUrl");
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
                var value = GetValue("PaySubject");
                return value == null ? "支付订单描述" : value.ToString();
            }
        }

        /// <summary>
        /// 支付订单描述
        /// </summary>
        public static string ItBPay
        {
            get
            {
                var value = GetValue("ItBPay");
                return value == null ? "29m" : value.ToString();
            }
        }

        #endregion

        #region 集成平台

        #region 美团外卖

        /// <summary>
        /// 是否跳转至测试环境
        /// </summary>
        public static bool MeiIsToTest
        {
            get
            {
                bool isToTest = false;
                if (GetValue("MeiIsToTest").ToInt(0) == 1)
                {
                    isToTest = true;
                }

                return isToTest;
            }
        }

        /// <summary>
        /// 跳转至测试环境门店Id列表
        /// </summary>
        public static List<string> MeiToTestShopIds
        {
            get
            {
                var list = new List<string>();
                var value = GetValue("MeiToTestShopIds");
                if (!string.IsNullOrWhiteSpace(value))
                {
                    list = value.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                return list;
            }
        }

        /// <summary>
        /// 开发者Id
        /// </summary>
        public static string MeiDeveloperId
        {
            get
            {
                return GetValue("MeiDeveloperId");
            }
        }

        /// <summary>
        /// 签名密钥
        /// </summary>
        public static string MeiSignKey
        {
            get
            {
                return GetValue("MeiSignKey");
            }
        }

        /// <summary>
        /// 门店映射回调
        /// </summary>
        public static string MeiMapCallBack
        {
            get
            {
                return GetValue("MeiMapCallBack");
            }
        }

        /// <summary>
        /// 门店解绑回调
        /// </summary>
        public static string MeiUnMapCallBack
        {
            get
            {
                return GetValue("MeiUnMapCallBack");
            }
        }

        #endregion

        #region 饿了么

        /// <summary>
        /// 【饿了么】应用ID
        /// </summary>
        public static string EleAppId
        {
            get
            {
                return GetValue("EleAppId");
            }
        }

        /// <summary>
        /// 【饿了么】应用Key
        /// </summary>
        public static string EleAppKey
        {
            get
            {
                return GetValue("EleAppKey");
            }
        }

        /// <summary>
        /// 【饿了么】应用Secret
        /// </summary>
        public static string EleAppSecret
        {
            get
            {
                return GetValue("EleAppSecret");
            }
        }

        /// <summary>
        /// 【饿了么】授权回调
        /// </summary>
        /// <returns></returns>
        public static string EleAuthCallBack
        {
            get
            {
                return GetValue("EleAuthCallBack");
            }
        }

        /// <summary>
        /// 【饿了么】更新令牌有效期天数
        /// </summary>
        /// <returns></returns>
        public static int EleReTokenExpiresDays
        {
            get
            {
                return GetValue("EleReTokenExpiresDays").ToInt(15);
            }
        }

        #endregion

        #region 百度外卖

        /// <summary>
        /// 【百度外卖】应用SourceId
        /// </summary>
        public static string BaiduSourceId
        {
            get
            {
                return GetValue("BaiduSourceId");
            }
        }

        /// <summary>
        /// 【百度外卖】应用SourceSecret
        /// </summary>
        public static string BaiduSourceSecret
        {
            get
            {
                return GetValue("BaiduSourceSecret");
            }
        }

        /// <summary>
        /// 【百度外卖】授权SourceKey
        /// </summary>
        public static string BaiduAuthSourceKey
        {
            get
            {
                return GetValue("BaiduAuthSourceKey");
            }
        }

        #endregion

        #endregion

        #region 打印配置

        /// <summary>
        /// 【微云打】最大打印份数
        /// </summary>
        public static int MaxCopies
        {
            get
            {
                var value = GetValue("MaxCopies");
                return value.ToInt();
            }
        }

        /// <summary>
        /// 【微云打】签名密钥
        /// </summary>
        public static string McpSignKey
        {
            get
            {
                return GetValue("McpSignKey");
            }
        }

        /// <summary>
        /// 【微云打】API接口域名
        /// </summary>
        public static string McpApiDomain
        {
            get
            {
                return GetValue("McpApiDomain");
            }
        }

        /// <summary>
        /// 【微云打】订单应用AppId
        /// </summary>
        public static string McpAppId
        {
            get
            {
                return GetValue("McpAppId");
            }
        }

        /// <summary>
        /// 【微云打】订单应用AppKey
        /// </summary>
        public static string McpAppKey
        {
            get
            {
                return GetValue("McpAppKey");
            }
        }

        /// <summary>
        /// 预订单判断间隔时间 [单位：分钟]
        /// </summary>
        public static int PreOrderInterval
        {
            get
            {
                var value = GetValue("PreOrderInterval");
                return value.ToInt();
            }
        }

        #endregion

        #region 彩票

        /// <summary>
        /// 微云打任务密钥
        /// </summary>
        public static string TaskSecrcetKey
        {
            get
            {
                return GetValue("TaskSecrcetKey");
            }
        }

        /// <summary>
        /// 私有云服务回调地址
        /// </summary>
        public static string CallBackUrl
        {
            get
            {
                return GetValue("CallBackUrl");
            }
        }

         /// <summary>
        /// 彩票Demo口令
        /// </summary>
        public static string LoginCmd
        {
            get
            {
                return GetValue("LoginCmd");
            }
        }

        /// <summary>
        /// 彩票打印机设备Id串
        /// </summary>
        public static string LotDeviceIds
        {
            get
            {
                return GetValue("LotDeviceIds");
            }
        }

        #endregion
    }
}
