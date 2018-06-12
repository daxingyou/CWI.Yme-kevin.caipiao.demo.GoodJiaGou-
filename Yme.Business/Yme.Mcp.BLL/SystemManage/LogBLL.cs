using Yme.Mcp.Entity.SystemManage;
using Yme.Mcp.IService.SystemManage;
using Yme.Mcp.Service.SystemManage;
using Yme.Util.WebControl;
using System;
using System.Collections.Generic;
using Yme.Util;
using Yme.Util.Log;

namespace Yme.Mcp.BLL.SystemManage
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2014-2017 深圳映美卡莫网络有限公司
    /// 创建人：kevin
    /// 日 期：2016.1.8 9:56
    /// 描 述：系统日志
    /// </summary>
    public static class LogBLL
    {
        #region 私有变量

        private static ILogService service = new LogService();

        #endregion

        #region 获取数据
        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public static IEnumerable<LogEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 日志实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public static LogEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 清空日志
        /// </summary>
        /// <param name="categoryId">日志分类Id</param>
        /// <param name="keepTime">保留时间段内</param>
        public static void RemoveLog(int categoryId, string keepTime)
        {
            try
            {
                service.RemoveLog(categoryId, keepTime);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logEntity">对象</param>
        public static void WriteLog(this LogEntity logEntity)
        {
            try
            {
                service.WriteLog(logEntity);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.InnerException.Message);
                throw;
            }
        }
        #endregion
    }
}
