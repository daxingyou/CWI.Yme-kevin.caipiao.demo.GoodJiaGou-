using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Yme.Util.Extension
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 返回Http请求参数格式
        /// </summary>
        /// <param name="dict">self</param>
        /// <returns>参数字符串</returns>
        public static string ToParamString(this Dictionary<string, string> dict)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> kv in dict)
            {
                list.Add(string.Format("{0}={1}", kv.Key, HttpUtility.UrlEncode(kv.Value)));
            }

            string param = string.Join("&", list);

            return param;
        }
    }
}
