using System;
using System.Collections.Generic;
using Yme.Mcp.Entity.SystemManage;
using Yme.Util.WebControl;

namespace Yme.Mcp.IService.SystemManage
{
    public interface IParameterService
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ParameterEntity> GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        ParameterEntity GetEntity(string keyValue);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Id"></param>
        int InsertEntity(ParameterEntity entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(ParameterEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        int Remove(string Id);

    }
}
