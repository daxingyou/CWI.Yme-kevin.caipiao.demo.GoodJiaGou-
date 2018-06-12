using System;
using Yme.Code;

namespace Yme.Mcp.Entity.SystemManage
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2016.1.8 9:56
    /// 描 述：验证码信息
    /// </summary>
    public class VerifyCodeEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 验证码ID
        /// </summary>
        public long VerifyCodeId { set; get; }

        /// <summary>
        /// APP应用标识
        /// </summary>
        public string AppSign { set; get; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { set; get; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { set; get; }

        /// <summary>
        /// 验证失效时间
        /// </summary>
        public DateTime ExpireDate { set; get; }

        /// <summary>
        /// 是否已验证:0-未验证,1-已验证
        /// </summary>
        public int Verified { set; get; }

        /// <summary>
        /// 验证日期
        /// </summary>
        public DateTime VerifiedDate { set; get; }

        /// <summary>
        /// 终端标识
        /// </summary>
        public string TerminalCode { set; get; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>		
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>		
        public DateTime? ModifyDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(long keyValue)
        {
            this.ModifyDate = DateTime.Now;
        }
        #endregion
    }
}
