using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model
{
    public class PrinterViewModel : ViewModel
    {
        /// <summary>
        /// 设备号
        /// </summary>
        [Required(ErrorMessage = "设备号不能为空！")]
        public string PrinterCode { get; set; }
    }
}
