using System;
using System.Collections.Generic;
using Yme.Mcp.Entity.SystemManage;
using Yme.Mcp.IService.SystemManage;
using Yme.Data.Repository;
using Yme.Util.WebControl;
using Yme.Util.Extension;
using Yme.Util;

namespace Yme.Mcp.Service.SystemManage
{
    /// <summary>
    /// 系统字典
    /// </summary>
    public class DictCodeService : RepositoryFactory<DictCodeEntity>, IDictCodeService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns></returns>
        public IEnumerable<DictCodeEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = Extensions.True<DictCodeEntity>();
            //var queryParam = queryJson.ToJObject();
            //if (!queryParam["CodeType"].IsEmpty())
            //{
                //string DictCodeType = queryParam["CodeType"].ToString();
                expression = expression.And(t => t.CodeType == queryJson);
            //}
            return this.BaseRepository().FindList(expression,pagination);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DictCodeEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int InsertEntity(DictCodeEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int UpdateEntity(string keyValue)
        {
            var expression = Extensions.True<DictCodeEntity>();
            expression = expression.And(t => t.DictCode == keyValue);
            return this.BaseRepository().Update(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int Remove(string keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }


    }
}
