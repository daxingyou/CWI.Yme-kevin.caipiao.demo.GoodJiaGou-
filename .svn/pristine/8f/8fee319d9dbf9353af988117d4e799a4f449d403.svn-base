using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Util;

namespace Yme.Mcp.Model.ViewModels.Waimai.Eleme
{
    public class ElemeApiRequestModel
    {
        /// <summary>
        /// 商户授权应用以后，开放平台颁发给应用的access_token（访问令牌）
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 标示协议版本，固定为 1.0.0
        /// </summary>
        public string nop { get; set; }

        /// <summary>
        /// 公共参数
        /// </summary>
        public ElemeApiRequestMetasModel metas { get; set; }

        /// <summary>
        /// 参数列表【parms -> params】
        /// </summary>
        public Dictionary<string, object> parms { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// UUID4，用来唯一标记此次调用，响应对象中会包含相同的id
        /// </summary>
        public string id 
        { 
            get
            {
                return StringUtil.UniqueStr();
            }
        }

        /// <summary>
        /// 签名参数
        /// </summary>
        public string signature { get; set; }
    }
}