using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Yme.Util.Enums;
using Yme.Util.Log;

namespace Yme.Util.Security
{
    public class SignUtil
    {
        /// <summary>
        /// 获取MD5签名
        /// </summary>
        /// <param name="key">签名密钥</param>
        /// <param name="str">签名字符串</param>
        /// <returns>MD5签名</returns>
        public static string GetMD5Sign(string key, string str)
        {
            str += "&key=" + key;
            LogUtil.Info("sign str:" + str);
            return MD5Util.Sign(str, "").ToUpper();
        }

        /// <summary>
        /// 获取签名字符串
        /// </summary>
        /// <param name="dic">待签名参数</param>
        /// <param name="key">签名密钥</param>
        /// <param name="signType">签名加密方法</param>
        /// <returns></returns>
        public static string GetSign(SortedDictionary<string, object> dic, string key, SignType signType)
        {
            var strSign = string.Empty;
            var strRequestData = string.Empty;
            StringBuilder sbRequest = new StringBuilder();
            foreach (var item in dic)
            {
                if (item.Key.ToLower() != "sign" && item.Key.ToLower() != "sign_type" && item.Value != "" && item.Value != null)
                {
                    sbRequest.AppendFormat("{0}={1}&", item.Key, item.Value);
                }
            }

            if (sbRequest.Length > 0)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    strRequestData = string.Format("{0}&key={1}", sbRequest.ToString(), key.Trim());
                }
                else
                {
                    strRequestData = sbRequest.ToString().TrimEnd('&');
                }
            }

            switch (signType)
            {
                case SignType.MD5:
                    {
                        strSign = MD5Util.Sign(strRequestData, "").ToUpper();
                        break;
                    };
                case SignType.SHA1:
                    {
                        strSign = SHAEncryptUtil.SHA1Encrypt(strRequestData);
                        break;
                    };
                case SignType.SHA256:
                    {
                        strSign = SHAEncryptUtil.SHA256Encrypt(strRequestData);
                        break;
                    }
                default:
                    break;
            }

            return strSign;
        }

        /// <summary>
        /// 获取【美团】签名字符串
        /// </summary>
        /// <param name="dic">待签名参数</param>
        /// <param name="key">签名密钥</param>
        /// <returns></returns>
        public static string GetMeituanSign(SortedDictionary<string, object> dic, string key)
        {
            var sbRequest = new StringBuilder();

            //签名密钥
            if (!string.IsNullOrEmpty(key))
            {
                sbRequest.AppendFormat(key.Trim());
            }

            //请求参数
            foreach (var item in dic)
            {
                if (item.Key.ToLower() != "sign" && item.Key.ToLower() != "sign_type" && item.Value != "" && item.Value != null)
                {
                    sbRequest.AppendFormat("{0}{1}", item.Key, item.Value);
                }
            }

            var strSign = SHAEncryptUtil.SHA1Encrypt(sbRequest.ToString()).ToLower();
            return strSign;
        }

        /// <summary>
        /// 获取【饿了么】签名字符串
        /// </summary>
        /// <param name="dic">待签名参数</param>
        /// <param name="secret">应用密钥</param>
        /// <param name="action">接口方法</param>
        /// <param name="token">访问令牌</param>
        /// <param name="secret">应用密钥</param>
        /// <returns></returns>
        public static string GetEleSign(SortedDictionary<string, object> dic, string secret, bool isToJson = false, string action = "", string token = "")
        {
            var strSign = string.Empty;
            var strRequestData = string.Empty;
            StringBuilder sbRequest = new StringBuilder();
            foreach (var item in dic)
            {
                sbRequest.AppendFormat("{0}={1}", item.Key, isToJson ? item.Value.ToJson() : item.Value);
            }

            //拼接加密字符串串
            strRequestData = string.Format("{0}{1}{2}{3}", action, token, sbRequest.ToString(), secret);
            strSign = MD5Util.Sign(strRequestData, "").ToUpper();

            return strSign;
        }

        /// <summary>
        /// 获取【百度外卖】签名字符串
        /// </summary>
        /// <param name="dic">待签名参数</param>
        /// <param name="key">签名密钥</param>
        /// <returns></returns>
        public static string GetBaiduSign(SortedDictionary<string, object> dic, string key = "")
        {
            var sbRequest = new StringBuilder();

            //签名密钥
            if (!string.IsNullOrEmpty(key) && !dic.Keys.Contains("secret"))
            {
                dic.Add("secret", key);
            }

            //请求参数
            foreach (var item in dic)
            {
                if (item.Key.ToLower() != "sign")
                {
                    sbRequest.AppendFormat("{0}={1}&", item.Key, item.Value);
                }
            }

            var strSign = MD5Util.GetMD5Sign(sbRequest.ToString().TrimEnd('&')).ToUpper();
            return strSign;
        }
    }
}
