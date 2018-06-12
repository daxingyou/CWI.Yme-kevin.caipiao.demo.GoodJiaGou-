using Yme.Util;
using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model
{
    /// <summary>
    /// 菜品报表查询参数
    /// </summary>
    public class DishRptViewModel : ViewModel
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        [Required(ErrorMessage = "开始日期不能为空！")]
        [RegularExpression(RegexConsts.IS_DATE, ErrorMessage = "开始日期格式不正确！")]
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Required(ErrorMessage = "结束日期不能为空！")]
        [RegularExpression(RegexConsts.IS_DATE, ErrorMessage = "结束日期格式不正确！")]
        public string EndDate { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public int orderBy { get; set; }
    }
}

