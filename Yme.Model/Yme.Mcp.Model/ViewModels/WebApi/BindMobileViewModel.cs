using Evt.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Util;

namespace Yme.Mcp.Model
{
    public class BindMobileViewModel : ViewModel
    {
        /// <summary>
        /// 原手机号
        /// </summary>
        [Required(ErrorMessage = "原手机号不能为空！")]
        [RegularExpression(RegexConsts.MOBILE_PATTERN, ErrorMessage = "原手机号格式不正确！")]
        public string Mobile { get; set; }

        /// <summary>
        /// 新手机号
        /// </summary>
        [Required(ErrorMessage = "新手机号不能为空！")]
        [RegularExpression(RegexConsts.MOBILE_PATTERN, ErrorMessage = "新手机号格式不正确！")]
        public string NewMobile { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空！")]
        public string VerifyCode { get; set; }
    }
}
