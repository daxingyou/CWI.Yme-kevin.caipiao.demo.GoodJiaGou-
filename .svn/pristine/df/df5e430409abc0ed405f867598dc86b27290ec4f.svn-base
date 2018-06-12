using System;
using Yme.Code;

namespace Yme.Mcp.Entity.SystemManage
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.11.02 14:27
    /// 描 述：系统字典
    /// </summary>
    public class DictCodeEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 字典编码
        /// </summary>	
        public string DictCode { get; set; }
        /// <summary>
        /// 字典值
        /// </summary>		
        public string Value { get; set; }
        /// <summary>
        /// 文本标签
        /// </summary>		
        public string Text { get; set; }
        /// <summary>
        /// 上级编码
        /// </summary>		
        public string ParentCode { get; set; }


        /// <summary>
        /// 是否默认:0-否,1-是
        /// </summary>		
        public int? IsDefault { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>		
        public int? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Description { get; set; }

        /// <summary>
        /// 字典类型名称
        /// </summary>		
        public string CodeTypeName { get; set; }
        /// <summary>
        /// 字典类型
        /// </summary>		
        public string CodeType { get; set; }
        


        /// <summary>
        /// 创建日期
        /// </summary>		
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>		
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>		
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>		
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        public string ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create(string keyValue)
        {
            this.DictCode = keyValue;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.DeleteMark = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.DictCode = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
