//---------------------------------------------
// 版权信息：版权所有(C) 2014，COOLWI.COM
// 变更历史：
//      姓名          日期              说明
// --------------------------------------------
//      王军锋        2014/08/16        创建
//---------------------------------------------
using Evt.Framework.Mvc;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Yme.Util;

namespace Yme.Mcp.Model
{
    public class PageViewModel : ViewModel
    {
        /// <summary>
        /// 页面大小
        /// </summary>
        [RegularExpression(RegexConsts.INT_FOR_GREAT_ZERO, ErrorMessage = "页面大小必须为大于零的正整数！")]
        public int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [RegularExpression(RegexConsts.INT_FOR_GREAT_ZERO, ErrorMessage = "页码必须为大于零的正整数！")]
        public int PageIndex { get; set; }
    }
}