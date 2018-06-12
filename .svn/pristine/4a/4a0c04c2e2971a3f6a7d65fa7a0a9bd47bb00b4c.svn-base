using System;
using System.Text;

namespace Yme.Util.Security
{
    public class Base64Util
    {
        /// <summary>
        /// Base64编码，使用指定的编码
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="encoding">编码对象</param>
        /// <returns>Base64编码后的字符串</returns>
        public static string Base64Encode(string sourceString, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(sourceString));
        }

        /// <summary>
        /// Base64编码，使用默认的编码
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <returns>Base64编码后的字符串</returns>
        public static string Base64Encode(string sourceString)
        {
            return Base64Encode(sourceString, Encoding.Default);
        }

        /// <summary>
        /// Base64解码，使用指定的编码
        /// </summary>
        /// <param name="base64Encoded">已编码的Base64字符串</param>
        /// <param name="encoding">编码对象</param>
        /// <returns>原字符串</returns>
        public static string Base64Decode(string base64Encoded, Encoding encoding)
        {
            return encoding.GetString(Convert.FromBase64String(base64Encoded));
        }

        /// <summary>
        /// Base64解码,使用默认的编码
        /// </summary>
        /// <param name="base64Encoded">已编码的Base64字符串</param>
        /// <returns>原字符串</returns>
        public static string Base64Decode(string base64Encoded)
        {
            return Base64Decode(base64Encoded, Encoding.Default);
        }
    }
}
