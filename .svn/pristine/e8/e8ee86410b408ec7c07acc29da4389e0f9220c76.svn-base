//---------------------------------------------
// 版权信息：版权所有(C) 2015，COOLWI.COM
// 变更历史：
//      姓名          日期              说明
// --------------------------------------------
//      王军锋        2012/02/21         创建
//---------------------------------------------
using Evt.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
using Yme.Util;

namespace Yme.Mcp.Model
{
    /// <summary>
    /// 获取验证码参数
    /// </summary>
    public class VerifyCodeViewModel : ViewModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "手机号码不能为空！")]
        [RegularExpression(RegexConsts.MOBILE_PATTERN, ErrorMessage = "手机号码格式不正确！")]
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        [Required(ErrorMessage = "验证码类型不能为空！")]
        [RegularExpression(RegexConsts.NUM_ONE_TWO, ErrorMessage = "验证码类型不正确！")]
        public string CodeType { get; set; }

        /// <summary>
        /// 终端设备唯一标识
        /// </summary>
        public string TerminalSign { get; set; }
    }
}