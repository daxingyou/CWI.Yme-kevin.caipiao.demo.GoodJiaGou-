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
    public class WxUserService : RepositoryFactory<WxUserEntity>, IWxUserService
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <returns></returns>
        public List<WxUserEntity> GetList()
        {
            var expression = Extensions.True<WxUserEntity>();
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.CreateDate).ToList();
        }

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public WxUserEntity GetEntity(string keyValue) 
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="wxType"></param>
        /// <returns></returns>
        public WxUserEntity GetEntity(string openId, int wxType)
        {
            var expression = Extensions.True<WxUserEntity>();
            expression = expression.And(t => t.OpenId == openId);
            expression = expression.And(t => t.WxType == wxType);
            return this.BaseRepository().IQueryable(expression).FirstOrDefault();
        }

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="wxType"></param>
        /// <returns></returns>
        public WxUserEntity GetEntity(long shopId, int wxType)
        {
            var expression = Extensions.True<WxUserEntity>();
            expression = expression.And(t => t.ShopId == shopId);
            expression = expression.And(t => t.WxType == wxType);
            return this.BaseRepository().IQueryable(expression).FirstOrDefault();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertEntity(WxUserEntity entity)
        {
            entity.Create();
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateEntity(WxUserEntity entity)
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

        public int DeleteEntity(long shopId, int wxType)
        {
            var sql = @"DELETE FROM wx_user WHERE ShopId = @shopId AND WxType = @wxType";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("wxType", wxType));
            return this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
        }

        public int DeleteEntity(string openId, int wxType)
        {
            var sql = @"DELETE FROM wx_user WHERE OpenId = @openId AND WxType = @wxType";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("OpenId", openId));
            parms.Add(DbParameters.CreateDbParameter("wxType", wxType));
            return this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
        }
    }
}
