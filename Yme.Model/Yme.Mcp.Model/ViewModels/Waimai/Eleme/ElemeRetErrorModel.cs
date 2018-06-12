using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.ViewModels.Waimai.Eleme
{
    public class ElemeRetErrorModel
    {
        /// <summary>
        /// 失败信息
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// 失败的详细原因描述
        /// </summary>
        public string error_description { get; set; }
    }
}
