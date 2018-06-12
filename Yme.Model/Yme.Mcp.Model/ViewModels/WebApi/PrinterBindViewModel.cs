using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model
{
    public class PrinterBindViewModel : PrinterViewModel
    {
        /// <summary>
        /// 校验码
        /// </summary>
        [Required(ErrorMessage = "校验码不能为空！")]
        public string CheckCode { get; set; }
    }
}