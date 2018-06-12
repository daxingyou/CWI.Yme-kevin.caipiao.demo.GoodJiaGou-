using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.QueryModels
{
    /// <summary>
    /// 门店平台
    /// </summary>
    public class ShopPlatformQueryModel
    {
        /// <summary>
        /// 平台Id
        /// </summary>
        public int PlatformId { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        public string PlatformName { get; set; }

        /// <summary>
        /// 门店平台授权ID
        /// </summary>
        public long AuthId { get; set; }

        /// <summary>
        /// 授权状态
        /// </summary>
        public int AuthStasus { get; set; }

        /// <summary>
        /// 是否KA商户
        /// </summary>
        public int IsKaMerchant { get; set; }

        /// <summary>
        /// KA商户授权地址
        /// </summary>
        public string KaMerchantAuthUrl { get; set; }
    }
}
