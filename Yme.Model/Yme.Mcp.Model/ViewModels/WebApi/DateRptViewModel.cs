using Yme.Util;
using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model
{
    /// <summary>
    /// 日期报表查询参数
    /// </summary>
    public class DateRptViewModel : ViewModel
    {
        /// <summary>
        /// 查询日期
        /// </summary>
        [Required(ErrorMessage = "查询日期不能为空！")]
        [RegularExpression(RegexConsts.IS_DATE, ErrorMessage = "查询日期格式不正确！")]
        public string QueryDate { get; set; }
    }
}
