using Yme.Util;
using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yme.Mcp.Model
{
    /// <summary>
    /// 平台报表查询参数
    /// </summary>
    public class PlatformRptViewModel
    {
        /// <summary>
        /// 平台ID
        /// </summary>
        public int PlatformId { get; set; }

        /// <summary>
        /// 统计类型
        /// </summary>
        [Required(ErrorMessage = "统计类型不能为空！")]
        public string StatisticsType { get; set; }

        ///// <summary>
        ///// 开始日期
        ///// </summary>
        //[Required(ErrorMessage = "开始日期不能为空！")]
        //[RegularExpression(RegexConsts.IS_DATE, ErrorMessage = "开始日期格式不正确！")]
        //public string BeginDate { get; set; }

        ///// <summary>
        ///// 结束日期
        ///// </summary>
        //[Required(ErrorMessage = "结束日期不能为空！")]
        //[RegularExpression(RegexConsts.IS_DATE, ErrorMessage = "结束日期格式不正确！")]
        //public string EndDate { get; set; }
    }
}