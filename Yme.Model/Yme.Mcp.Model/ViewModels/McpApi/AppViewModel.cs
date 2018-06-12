using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
using Yme.Util;

namespace Yme.Mcp.Model.McpApi
{
    /// <summary>
    /// 获取访问令牌参数
    /// </summary>
    public class AppViewModel : ViewModel
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        [Required(ErrorMessage = "应用ID不能为空！")]
        public string app_id { get; set; }

        /// <summary>
        /// 应用Key
        /// </summary>
        [Required(ErrorMessage = "应用Key不能为空！")]
        public string app_key { get; set; }

        /// <summary>
        /// 初始化参数
        /// </summary>
        public AppViewModel()
        {
            this.app_id = ConfigUtil.McpAppId;
            this.app_key = ConfigUtil.McpAppKey;
        }
    }
}

