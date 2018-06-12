using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model
{
    public class PrinterUpdateViewModel : PrinterViewModel
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        [Required(ErrorMessage = "设备名称不能为空！")]
        public string PrinterName { get; set; }
    }
}
