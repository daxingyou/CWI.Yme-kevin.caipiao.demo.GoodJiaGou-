using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.ViewModels.Waimai.Eleme
{
    public class ElemeApiRetModel
    {
        /// <summary>
        /// 标示协议版本，固定为 1.0.0
        /// </summary>
        public string nop { get; set; }

        /// <summary>
        /// 本次调用的请求对象的id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 接口调用的返回值
        /// </summary>
        public object result { get; set; }

        /// <summary>
        /// 错误对象
        /// </summary>
        public object error { get; set; }
    }
}
