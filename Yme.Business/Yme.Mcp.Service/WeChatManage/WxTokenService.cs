using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Data;
using Yme.Data.Repository;
using Yme.Mcp.Entity.WeChatManage;
using Yme.Util.Extension;

namespace Yme.Mcp.Service.WeChatManage
{
    public class WxTokenService : RepositoryFactory<WxTokenEntity>, IWxTokenService
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <returns></returns>
        public List<WxTokenEntity> GetList()
        {
            var expression = Extensions.True<WxTokenEntity>();
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.ModifyDate).ToList();
        }

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public WxTokenEntity GetEntity(int keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public WxTokenEntity GetEntity(string appId)
        {
            var expression = Extensions.True<WxTokenEntity>();
            expression = expression.And(t => t.AppId == appId);
            return this.BaseRepository().IQueryable(expression).FirstOrDefault();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertEntity(WxTokenEntity entity)
        {
            entity.Create();
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateEntity(WxTokenEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public int DeleteEntity(int keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }

        public int DeleteEntity(string appId)
        {
            var sql = @"DELETE FROM wx_token WHERE AppId = @appId";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("appId", appId));
            return this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
        }
    }
}
