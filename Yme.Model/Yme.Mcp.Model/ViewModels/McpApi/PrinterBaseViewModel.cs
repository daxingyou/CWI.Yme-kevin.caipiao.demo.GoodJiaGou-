using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model.McpApi
{
    /// <summary>
    /// 打印机基础参数
    /// </summary>
    public class PrinterBaseViewModel : ViewModel
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        [Required(ErrorMessage = "访问令牌不能为空！")]
        public string access_token { get; set; }

        /// <summary>
        /// 商家编码
        /// </summary>
        [Required(ErrorMessage = "商家编码不能为空！")]
        public string merchant_code { get; set; }

        /// <summary>
        /// 打印机编码串（以逗号隔开）
        /// </summary>
        [Required(ErrorMessage = "打印机编码不能为空！")]
        public string printer_codes { get; set; }
    }
}
