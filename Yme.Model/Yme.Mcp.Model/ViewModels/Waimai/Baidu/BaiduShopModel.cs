using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.ViewModels.Waimai.Baidu
{
    /// <summary>
    /// 百度门店信息
    /// </summary>
    public class BaiduShopModel
    {
        /// <summary>
        /// 百度门店ID
        /// </summary>
        public string baidu_shop_id { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 供应商ID;如果不存在则为0;
        /// </summary>
        public string supplier_id { get; set; }
    }
}
