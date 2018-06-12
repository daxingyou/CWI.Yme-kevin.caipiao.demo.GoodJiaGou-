using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
using Yme.Util;

namespace Yme.Mcp.Model.McpApi
{
    /// <summary>
    /// 打印参数
    /// </summary>
    public class PrintViewModel : PrinterBaseViewModel
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        [Required(ErrorMessage = "应用ID不能为空！")]
        public string app_id { get; set; }

         /// <summary>
        /// 初始化参数
        /// </summary>
        public PrintViewModel()
        {
            this.app_id = ConfigUtil.McpAppId;
        }
    }
}