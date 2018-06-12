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
    public class ParameterService : RepositoryFactory<ParameterEntity>, IParameterService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns></returns>
        public IEnumerable<ParameterEntity> GetPageList(Pagination pagination, string queryJson)
        {
            //var expression = Extensions.True<ParameterEntity>();
            //var queryParam = queryJson.ToJObject();
            //if (!queryParam["CodeType"].IsEmpty())
            //{
            //string DictCodeType = queryParam["CodeType"].ToString();
            //expression = expression.And(t => t.CodeType == queryJson);
            //}
            return this.BaseRepository().FindList(pagination);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ParameterEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int InsertEntity(ParameterEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int UpdateEntity(ParameterEntity entity)
        {
            return this.BaseRepository().Update(entity);
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
