using System.Security.Cryptography;
using System.Text;

namespace Yme.Util.Security
{
    public class SHAEncryptUtil
    {
        #region SHA 加解密

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string inputString)
        {
            SHA1Managed sha1 = new SHA1Managed();
            return ByteToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(inputString))).ToLower();
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string SHA256Encrypt(string inputString)
        {
            SHA256Managed sha256 = new SHA256Managed();
            return ByteToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString))).ToLower();
        }

        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
                sbinary += buff[i].ToString("X2");
            /* hex format */
            return sbinary;
        }

        #endregion
    }
}
