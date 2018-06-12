using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model
{
    public class PrinterStatusViewModel : PrinterViewModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }
    }
}

