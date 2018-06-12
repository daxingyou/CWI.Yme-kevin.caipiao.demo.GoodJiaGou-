using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.ViewModels.Waimai.Eleme
{
    public class ElemeRetTokenModel
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 令牌的类型，固定值
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// 令牌有效时间，单位秒，在令牌有效期内可以重复使用。有效期限制（access_token：沙箱环境为1天、正式环境为30天，refresh_token：沙箱环境为15天、正式环境为35天）
        /// </summary>
        public long expires_in { get; set; }

        /// <summary>
        /// 更新令牌，用来在令牌即将到期时延续访问令牌的有效期，获取一个新的访问令牌和更新令牌refresh_token
        /// </summary>
        public string refresh_token { get; set; }
    }
}
