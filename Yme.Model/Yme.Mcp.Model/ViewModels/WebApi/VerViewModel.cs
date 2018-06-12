﻿//---------------------------------------------
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
    /// 版本信息参数
    /// </summary>
    public class VerViewModel : ViewModel
    {
        /// <summary>
        /// 版本类型
        /// </summary>
        //[RegularExpression(RegexConsts.NUM_ONE_TO_THREE, ErrorMessage = "版本类型格式不正确！")]
        public string Type { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        [Required(ErrorMessage = "当前版本不能为空。")]
        public string Ver { get; set; }

        /// <summary>
        /// 设备标识【Mac地址】
        /// </summary>
        [Required(ErrorMessage = "设备标识不能为空。")]
        public string TerminalCode { get; set; }
    }
}