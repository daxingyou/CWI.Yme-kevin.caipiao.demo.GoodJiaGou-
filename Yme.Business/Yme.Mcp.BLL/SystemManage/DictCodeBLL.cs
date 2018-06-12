using System;
using System.Collections.Generic;
using Yme.Mcp.Entity.SystemManage;
using Yme.Mcp.IService.SystemManage;
using Yme.Mcp.Service.SystemManage;
using Yme.Util.WebControl;
using Yme.Util;

namespace Yme.Mcp.BLL.SystemManage
{
    public class DictCodeBLL
    {
        #region 私有变量

        private static IDictCodeService service = new DictCodeService();

        #endregion

        #region 公有方法

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public static IEnumerable<DictCodeEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取对象实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public static DictCodeEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int InsertEntity(DictCodeEntity entity)
        {
            return service.InsertEntity(entity);

        }

        #endregion
    }
}
