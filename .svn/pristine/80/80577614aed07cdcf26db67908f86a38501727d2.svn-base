using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Mcp.Entity.SystemManage;
using Yme.Mcp.IService.SystemManage;
using Yme.Mcp.Service.SystemManage;
using Yme.Util.WebControl;
using Yme.Util;
using Yme.Util.Log;

namespace Yme.Mcp.BLL.SystemManage
{
    /// <summary>
    /// 登录日志操作记录
    /// </summary>
    public static class LoginLogBLL
    {
        #region 私有变量

        private static ILoginLogService service = new LoginLogService();

        #endregion

        #region 获取数据
        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public static IEnumerable<LoginLogEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 日志实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public static LoginLogEntity GetEntity(string keyValue)
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
        public static void WriteLog(this LoginLogEntity logEntity)
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