using System;
using System.Collections.Generic;
using Yme.Mcp.Entity.SystemManage;
using Yme.Mcp.IService.SystemManage;
using Yme.Mcp.Service.SystemManage;
using Yme.Util.WebControl;
using Yme.Util;

namespace Yme.Mcp.BLL.SystemManage
{
    public class ParameterBLL
    {
        #region 私有变量

        private static IParameterService service = new ParameterService();

        #endregion

        #region 公有方法

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public static object GetPageList(Pagination pagination, string queryJson)
        {
            IEnumerable<ParameterEntity> list = service.GetPageList(pagination, queryJson);
            var result = new
            {
                total = pagination.records,
                rows = list
            };
            return result;
            //return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取对象实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public static ParameterEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int InsertEntity(ParameterEntity entity)
        {
            return service.InsertEntity(entity);
        }

        /// <summary>
        /// 编辑实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static int UpdateEntity(ParameterEntity entity)
        {
            return service.UpdateEntity(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static int Remove(string keyValue)
        {
            return service.Remove(keyValue);
        }

        #endregion
    }
}