using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Mcp.Entity.SystemManage;
using Yme.Util.WebControl;

namespace Yme.Mcp.IService.SystemManage
{
    /// <summary>
    /// 登录日志操作接口
    /// </summary>
    public interface ILoginLogService
    {
        #region 获取数据
        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<LoginLogEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 日志实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LoginLogEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 清空日志
        /// </summary>
        /// <param name="categoryId">日志分类Id</param>
        /// <param name="keepTime">保留时间段内</param>
        void RemoveLog(int categoryId, string keepTime);
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logEntity">对象</param>
        void WriteLog(LoginLogEntity entity);
        #endregion
    }
}
