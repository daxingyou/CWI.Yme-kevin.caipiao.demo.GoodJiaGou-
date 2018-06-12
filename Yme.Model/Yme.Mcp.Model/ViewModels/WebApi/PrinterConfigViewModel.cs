using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model
{
    public class PrinterConfigViewModel : PrinterViewModel
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public string Configs { get; set; }
    }
}

