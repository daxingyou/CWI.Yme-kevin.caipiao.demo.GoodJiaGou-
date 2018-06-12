using System;
using Yme.Code;
using Yme.Util;

namespace Yme.Mcp.Entity.BaseManage
{
    /// <summary>
    /// 版 本
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.11.03 10:58
    /// 描 述：门店信息
    /// </summary>
    public class ShopEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 商户ID
        /// </summary>		
        public string MerchantId { get; set; }
        /// <summary>
        /// 门店ID
        /// </summary>		
        public long ShopId { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 店铺账号:手机号
        /// </summary>		
        public string ShopAccount { get; set; }
        /// <summary>
        /// 店主姓名
        /// </summary>		
        public string ShopmanName { get; set; }
        /// <summary>
        /// 所属城区
        /// </summary>		
        public string CityArea { get; set; }
        /// <summary>
        /// 所属商圈
        /// </summary>		
        public string BusinessDistrict { get; set; }
        /// <summary>
        /// 店铺详细地址
        /// </summary>		
        public string ShopAddress { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>		
        public string BusinessScope { get; set; }
        /// <summary>
        /// 用户访问令牌
        /// </summary>		
        public string AccessToken { get; set; }
        /// <summary>
        /// 截止有效期
        /// </summary>		
        public DateTime? ExpiresTime { get; set; }
        /// <summary>
        /// 最后一次登录IP
        /// </summary>		
        public string LastLoginIp { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>		
        public DateTime? LastLoginTime { get; set; }
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
            this.MerchantId = StringUtil.UniqueStr();
            this.CreateDate = DateTime.Now;
            this.EnabledFlag = 1;
            this.DeleteFlag = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(long keyValue)
        {
            this.ShopId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }
}