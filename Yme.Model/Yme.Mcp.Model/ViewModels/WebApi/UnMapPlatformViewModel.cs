//---------------------------------------------
// 版权信息：版权所有(C) 2015，COOLWI.COM
// 变更历史：
//      姓名          日期              说明
// --------------------------------------------
//      王军锋        2012/02/21         创建
//---------------------------------------------
using Yme.Util;
using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model
{
    /// <summary>
    /// 平台解绑参数
    /// </summary>
    public class UnMapPlatformViewModel : ViewModel
    {
        /// <summary>
        /// 平台授权Id
        /// </summary>
        public int AuthId { get; set; }
    }
}
