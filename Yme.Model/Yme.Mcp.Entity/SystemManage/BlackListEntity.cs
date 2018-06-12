using System;
using Yme.Code;

namespace Yme.Mcp.Entity.SystemManage
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2016.1.8 9:56
    /// 描 述：黑名单信息
    /// </summary>
    public class BlackListEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 设置ID
        /// </summary>		
        public long BlackId { get; set; }
        /// <summary>
        /// APP应用标识
        /// </summary>		
        public string AppSign { get; set; }
        /// <summary>
        /// 设备标识
        /// </summary>		
        public string TerminalCode { get; set; }
        /// <summary>
        /// 计数
        /// </summary>		
        public int Counter { get; set; }
        /// <summary>
        /// 锁定日期
        /// </summary>		
        public DateTime? LockExpireDate { get; set; }
        /// <summary>
        /// 最后核查时间
        /// </summary>		
        public DateTime? LastCheckTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Description { get; set; }
        #endregion
    }
}
