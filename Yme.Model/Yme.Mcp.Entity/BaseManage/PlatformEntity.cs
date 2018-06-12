using System;
using Yme.Code;

namespace Yme.Mcp.Entity.BaseManage
{
    /// <summary>
    /// 版 本
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.11.03 10:58
    /// 描 述：平台信息
    /// </summary>
    public class PlatformEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 平台ID
        /// </summary>		
        public int PlatformId { get; set; }
        /// <summary>
        /// 平台名称
        /// </summary>
        public string PlatformName { get; set; }
        /// <summary>
        /// 1-外卖,2-餐饮,3-商城,4-物流
        /// </summary>		
        public int BusinessType { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>		
        public int? EnabledFlag { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int? DeleteFlag { get; set; }
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
        /// 更新人ID
        /// </summary>		
        public string ModifyUserId { get; set; }
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
            this.EnabledFlag = 1;
            this.DeleteFlag = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int keyValue)
        {
            this.PlatformId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }
}