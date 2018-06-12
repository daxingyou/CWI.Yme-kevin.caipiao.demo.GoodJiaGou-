using System;
using Yme.Code;

namespace Yme.Mcp.Entity.ReportManage
{
    /// <summary>
    /// 版 本
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2015.11.03 10:58
    /// 描 述：客户日汇总报表
    /// </summary>
    public class CustomerDaystatisEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 报表记录ID
        /// </summary>		
        public long RptId { get; set; }
        /// <summary>
        /// 报表日期
        /// </summary>		
        public DateTime? RptDate { get; set; }
        /// <summary>
        /// 门店ID
        /// </summary>		
        public long ShopId { get; set; }
        /// <summary>
        /// 客户总数
        /// </summary>		
        public int CustomerTotal { get; set; }
        /// <summary>
        /// 订单量环比
        /// </summary>		
        public decimal TotalRelativeRatio { get; set; }
        /// <summary>
        /// 统计时间
        /// </summary>		
        public DateTime? CreateDate { get; set; }
        #endregion
    }
}
