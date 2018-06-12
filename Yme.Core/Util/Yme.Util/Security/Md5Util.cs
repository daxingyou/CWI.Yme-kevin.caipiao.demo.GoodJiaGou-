//=====================================================================================
// All Rights Reserved , Copyright © Yme 2013
//=====================================================================================
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Yme.Util.Log;
using System.Collections.Generic;

namespace Yme.Util
{
    /// <summary>
    /// MD5加密帮助类
    /// 版本：2.0
    /// <author>
    ///		<name>kevin</name>
    ///		<date>2013.09.27</date>
    /// </author>
    /// </summary>
    public class MD5Util
    {
        /// <summary>
        /// MD5 签名
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="inputCharset">编码格式, 默认utf-8 格式</param>
        /// <returns>签名后字符串</returns>
        public static string Sign(string prestr, string key, string inputCharset = "utf-8")
        {
            StringBuilder sb = new StringBuilder(32);
            MD5 md5 = new MD5CryptoServiceProvider();

            prestr = prestr + key;
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取MD5签名
        /// </summary>
        /// <param name="str">签名字符串</param>
        /// <param name="key">签名密钥</param>
        /// <returns>MD5签名</returns>
        public static string GetSign(string str, string key)
        {
            str += "&key=" + key;
            LogUtil.Info("sign str:" + str);
            return Sign(str, "").ToUpper();
        }

        /// <summary>
        /// 获取MD5签名
        /// </summary>
        /// <param name="str">签名字符串</param>
        /// <param name="key">签名密钥</param>
        /// <returns>MD5签名</returns>
        public static string GetSign(SortedDictionary<string, object> sortDics, string key)
        {
            var sb = new StringBuilder();
            foreach (var s in sortDics)
            {
                sb.AppendFormat("{0}={1}&", s.Key, s.Value);
            }
            if (sb.ToString().Length > 0)
            {
                sb.AppendFormat("key={0}", key);
            }
            LogUtil.Info("sign str:" + sb.ToString());
            return Sign(sb.ToString(), "").ToUpper();
        }

        /// <summary>
        /// 获取MD5签名
        /// </summary>
        /// <param name="str">签名字符串</param>
        /// <param name="key">签名密钥</param>
        /// <returns>MD5签名</returns>
        public static string GetParmsSign(SortedDictionary<string, object> sortDics, string key, string keyName = "")
        {
            var sb = new StringBuilder();
            foreach (var s in sortDics)
            {
                sb.AppendFormat("{0}={1}&", s.Key, s.Value);
            }
            if (sb.ToString().Length > 0)
            {
                keyName = string.IsNullOrWhiteSpace(keyName) ? "key" : keyName;
                sb.AppendFormat("{0}={1}", keyName, key);
            }
            LogUtil.Info("sign str:" + sb.ToString());
            return Sign(sb.ToString(), "");
        }

        /// <summary>
        /// MD5 验证签名
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="sign">MD5签名字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="inputCharset">编码格式，默认utf-8</param>
        /// <returns>验证成功 or 失败</returns>
        public static bool Verify(string prestr, string sign, string key, string inputCharset = "utf-8")
        {
            string mysign = Sign(prestr, key, inputCharset).ToUpper();
            return mysign == sign.ToUpper();
        }

        /// <summary>
        /// 获取文件的md5摘要
        /// </summary>
        /// <param name="sFile">文件流</param>
        /// <returns>MD5摘要结果</returns>
        public static string GetAbstractToMD5(Stream sFile)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(sFile);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取文件的md5摘要
        /// </summary>
        /// <param name="dataFile">文件流</param>
        /// <returns>MD5摘要结果</returns>
        public static string GetAbstractToMD5(byte[] dataFile)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(dataFile);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取MD5签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5Sign(string str)
        {
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string MD5Hash(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }

            return strEncrypt;
        }
    }
}
