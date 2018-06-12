using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using Yme.Util.Extension;
using System.Web;

namespace Yme.Util
{
    /// <summary>
    /// 字符串操作 - 工具方法
    /// </summary>
    public sealed partial class StringUtil
    {
        #region Empty(空字符串)

        /// <summary>
        /// 空字符串
        /// </summary>
        public static string Empty
        {
            get { return string.Empty; }
        }

        #endregion

        #region PinYin(获取汉字的拼音简码)
        /// <summary>
        /// 获取汉字的拼音简码，即首字母缩写,范例：中国,返回zg
        /// </summary>
        /// <param name="chineseText">汉字文本,范例： 中国</param>
        public static string PinYin(string chineseText)
        {
            if (string.IsNullOrWhiteSpace(chineseText))
                return string.Empty;
            var result = new StringBuilder();
            foreach (char text in chineseText)
                result.AppendFormat("{0}", ResolvePinYin(text));
            return result.ToString().ToLower();
        }

        /// <summary>
        /// 解析单个汉字的拼音简码
        /// </summary>
        /// <param name="text">单个汉字</param>
        private static string ResolvePinYin(char text)
        {
            byte[] charBytes = Encoding.Default.GetBytes(text.ToString());
            if (charBytes[0] <= 127)
                return text.ToString();
            var unicode = (ushort)(charBytes[0] * 256 + charBytes[1]);
            string pinYin = ResolvePinYinByCode(unicode);
            if (!string.IsNullOrWhiteSpace(pinYin))
                return pinYin;
            return ResolvePinYinByFile(text.ToString());
        }

        /// <summary>
        /// 使用字符编码方式获取拼音简码
        /// </summary>
        private static string ResolvePinYinByCode(ushort unicode)
        {
            if (unicode >= '\uB0A1' && unicode <= '\uB0C4')
                return "A";
            if (unicode >= '\uB0C5' && unicode <= '\uB2C0' && unicode != 45464)
                return "B";
            if (unicode >= '\uB2C1' && unicode <= '\uB4ED')
                return "C";
            if (unicode >= '\uB4EE' && unicode <= '\uB6E9')
                return "D";
            if (unicode >= '\uB6EA' && unicode <= '\uB7A1')
                return "E";
            if (unicode >= '\uB7A2' && unicode <= '\uB8C0')
                return "F";
            if (unicode >= '\uB8C1' && unicode <= '\uB9FD')
                return "G";
            if (unicode >= '\uB9FE' && unicode <= '\uBBF6')
                return "H";
            if (unicode >= '\uBBF7' && unicode <= '\uBFA5')
                return "J";
            if (unicode >= '\uBFA6' && unicode <= '\uC0AB')
                return "K";
            if (unicode >= '\uC0AC' && unicode <= '\uC2E7')
                return "L";
            if (unicode >= '\uC2E8' && unicode <= '\uC4C2')
                return "M";
            if (unicode >= '\uC4C3' && unicode <= '\uC5B5')
                return "N";
            if (unicode >= '\uC5B6' && unicode <= '\uC5BD')
                return "O";
            if (unicode >= '\uC5BE' && unicode <= '\uC6D9')
                return "P";
            if (unicode >= '\uC6DA' && unicode <= '\uC8BA')
                return "Q";
            if (unicode >= '\uC8BB' && unicode <= '\uC8F5')
                return "R";
            if (unicode >= '\uC8F6' && unicode <= '\uCBF9')
                return "S";
            if (unicode >= '\uCBFA' && unicode <= '\uCDD9')
                return "T";
            if (unicode >= '\uCDDA' && unicode <= '\uCEF3')
                return "W";
            if (unicode >= '\uCEF4' && unicode <= '\uD188')
                return "X";
            if (unicode >= '\uD1B9' && unicode <= '\uD4D0')
                return "Y";
            if (unicode >= '\uD4D1' && unicode <= '\uD7F9')
                return "Z";
            return string.Empty;
        }

        /// <summary>
        /// 从拼音简码文件获取
        /// </summary>
        /// <param name="text">单个汉字</param>
        private static string ResolvePinYinByFile(string text)
        {
            int index = Const.ChinesePinYin.IndexOf(text, StringComparison.Ordinal);
            if (index < 0)
                return string.Empty;
            return Const.ChinesePinYin.Substring(index + 1, 1);
        }

        #endregion

        #region Splice(拼接集合元素)

        /// <summary>
        /// 拼接集合元素
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="quotes">引号，默认不带引号，范例：单引号 "'"</param>
        /// <param name="separator">分隔符，默认使用逗号分隔</param>
        public static string Splice<T>(IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            if (list == null)
                return string.Empty;
            var result = new StringBuilder();
            foreach (var each in list)
                result.AppendFormat("{0}{1}{0}{2}", quotes, each, separator);
            return result.ToString().TrimEnd(separator.ToCharArray());
        }

        #endregion

        #region FirstUpper(将值的首字母大写)

        /// <summary>
        /// 将值的首字母大写
        /// </summary>
        /// <param name="value">值</param>
        public static string FirstUpper(string value)
        {
            string firstChar = value.Substring(0, 1).ToUpper();
            return firstChar + value.Substring(1, value.Length - 1);
        }

        #endregion

        #region ToCamel(将字符串转成驼峰形式)

        /// <summary>
        /// 将字符串转成驼峰形式
        /// </summary>
        /// <param name="value">原始字符串</param>
        public static string ToCamel(string value)
        {
            return FirstUpper(value.ToLower());
        }

        #endregion

        #region ContainsChinese(是否包含中文)

        /// <summary>
        /// 是否包含中文
        /// </summary>
        /// <param name="text">文本</param>
        public static bool ContainsChinese(string text)
        {
            const string pattern = "[\u4e00-\u9fa5]+";
            return Regex.IsMatch(text, pattern);
        }

        #endregion

        #region ContainsNumber(是否包含数字)

        /// <summary>
        /// 是否包含数字
        /// </summary>
        /// <param name="text">文本</param>
        public static bool ContainsNumber(string text)
        {
            const string pattern = "[0-9]+";
            return Regex.IsMatch(text, pattern);
        }

        #endregion

        #region Distinct(去除重复)

        /// <summary>
        /// 去除重复
        /// </summary>
        /// <param name="value">值，范例1："5555",返回"5",范例2："4545",返回"45"</param>
        public static string Distinct(string value)
        {
            var array = value.ToCharArray();
            return new string(array.Distinct().ToArray());
        }

        #endregion

        #region Truncate(截断字符串)

        /// <summary>
        /// 截断字符串
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="length">返回长度</param>
        /// <param name="endCharCount">添加结束符号的个数，默认0，不添加</param>
        /// <param name="endChar">结束符号，默认为省略号</param>
        public static string Truncate(string text, int length, int endCharCount = 0, string endChar = ".")
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            if (text.Length < length)
                return text;
            return text.Substring(0, length) + GetEndString(endCharCount, endChar);
        }

        /// <summary>
        /// 获取结束字符串
        /// </summary>
        private static string GetEndString(int endCharCount, string endChar)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < endCharCount; i++)
                result.Append(endChar);
            return result.ToString();
        }

        #endregion

        #region ToSimplifiedChinese(转换为简体中文)

        /// <summary>
        /// 转换为简体中文
        /// </summary>
        /// <param name="text">繁体中文</param>
        public static string ToSimplifiedChinese(string text)
        {
            return Strings.StrConv(text, VbStrConv.SimplifiedChinese);
        }

        #endregion

        #region ToSimplifiedChinese(转换为繁体中文)

        /// <summary>
        /// 转换为繁体中文
        /// </summary>
        /// <param name="text">简体中文</param>
        public static string ToTraditionalChinese(string text)
        {
            return Strings.StrConv(text, VbStrConv.TraditionalChinese);
        }

        #endregion

        #region GUID(获取全局唯一值)

        /// <summary>
        /// 获取不带"-"的GUID字符串
        /// </summary>
        public static string UniqueStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 获取GUID字符串
        /// </summary>
        public static string GuidStr()
        {
            return Guid.NewGuid().ToString();
        }

        #endregion

        #region GetLastProperty(获取最后一个属性)

        /// <summary>
        /// 获取最后一个属性
        /// </summary>
        /// <param name="propertyName">属性名，范例，A.B.C,返回"C"</param>
        public static string GetLastProperty(string propertyName)
        {
            if (propertyName.IsEmpty())
                return string.Empty;
            var lastIndex = propertyName.LastIndexOf(".", StringComparison.Ordinal) + 1;
            return propertyName.Substring(lastIndex);
        }

        #endregion

        #region 字符串处理

        /// <summary>
        /// 获取字符串长度（中文算两个字符）
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns>字符串长度</returns>
        public static int GetStringLen(string content)
        {
            return content.CnLength();
        }

        /// <summary>
        /// 获取字符串的字节数
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>字符串字节长度</returns>
        public static int GetStringByteLength(string str)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            int length = 0; // 表示当前的字节数

            for (int i = 0; i < bytes.GetLength(0); i++)
            {
                // 偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
                if (i % 2 == 0)
                {
                    length++;   // 在UCS2第一个字节时n加1
                }
                else
                {
                    // 当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    if (bytes[i] > 0)
                    {
                        length++;
                    }
                }
            }
            return length;
        }

        /// <summary>
        /// 获取加星号的字符串(邮箱地址/手机号均使用)
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="startIndex">开始位数</param>
        /// <param name="length">加星号位数</param>
        /// <returns>加星号的字符串</returns>
        public static string GetStarString(string str, int startIndex, int length)
        {
            if (str.Length < (startIndex + length))
            {
                return str;
            }
            else
            {
                string retStr = string.Empty;
                if (str.IndexOf("@") > 0)
                {
                    string[] mail = str.Split('@');
                    if (mail[0].Length < (startIndex + length))
                    {
                        return str;
                    }
                    else
                    {
                        retStr = mail[0].Substring(0, startIndex) + string.Empty.PadLeft(length, '*') + mail[0].Substring(startIndex + length, mail[0].Length - (startIndex + length)) + "@" + mail[1];
                    }
                }
                else
                {
                    retStr = str.Substring(0, startIndex) + string.Empty.PadLeft(length, '*') + str.Substring(startIndex + length, str.Length - (startIndex + length));
                }
                return retStr;
            }
        }

        /// <summary>
        /// 获取字符串省略(当超出最大字节数时截取字符串，根据isEllipsis参数判断是否添加省略号)
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="maxByteCount">字节数最大值</param>
        /// <param name="isEllipsis">是否添加省略号</param>
        /// <returns>处理后的字符串</returns>
        public static string GetPrifxString(string str, int maxByteCount, bool isEllipsis = true)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);   //Unicode编码，产生的字节数组
            int length = str.Length;                         //字符串的字符数量
            int lenBytes = length;                              //字符串真正的字符串
            int[] byteArray = new int[length];                  //字节位数组，汉字为2,非汉字为1

            for (int i = 0; i < length; i++)
            {
                if (bytes[i * 2 + 1] > 0)
                {
                    byteArray[i] = 2;
                    lenBytes++;
                }
                else
                {
                    byteArray[i] = 1;
                }
            }

            if (maxByteCount >= lenBytes)
            {
                return str;
            }
            else
            {
                //int iByteCutOut = lenBytes - maxByteCount + (isEllipsis ? 3 : 0);   //需要截取字节数，省略号在maxByteCount内
                int iByteCutOut = lenBytes - maxByteCount;   //需要截取字节数，省略号不在maxByteCount内
                int iCur = 0;                       //当前截取字节数
                int iCharCutOut = 0;                      //截取字节数
                for (int j = byteArray.Length - 1; j >= 0; j--)
                {
                    iCharCutOut++;
                    iCur += byteArray[j];
                    if (iCur >= iByteCutOut)
                    {
                        break;
                    }
                }
                string strDest = str.Substring(0, length - iCharCutOut) + (isEllipsis ? "..." : string.Empty);
                return strDest;
            }
        }

        #endregion
    }
}
