using System;

namespace Yme.Mcp.Entity.WeChatManage
{
    /// <summary>
    /// 版 本
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.11.03 10:58
    /// 描 述：微信用户
    /// </summary>
    public class WxUserEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 记录ID
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// 微信类型:0-门店端,1-商家端
        /// </summary>		
        public int WxType { get; set; }
        /// <summary>
        /// 微信用户OpenId
        /// </summary>		
        public string OpenId { get; set; }
        /// <summary>
        /// 门店Id
        /// </summary>		
        public long ShopId { get; set; }
        /// <summary>
        /// 终端设备特征码，如：mac地址
        /// </summary>		
        public string TerminalCode { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>		
        public int? EnabledFlag { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Description { get; set; }
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
            this.EnabledFlag = 1;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
        }
        #endregion
    }
}