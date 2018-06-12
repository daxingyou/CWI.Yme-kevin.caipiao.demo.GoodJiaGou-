using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Yme.Data;
using Yme.Data.Repository;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Model;
using Yme.Util;
using Yme.Util.Exceptions;

namespace Yme.Mcp.Service.BaseManage
{
    /// <summary>
    /// 门店服务
    /// </summary>
    public class ShopService : RepositoryFactory<ShopEntity>, IShopService
    {
        /// <summary>
        /// 根据手机号获取门店实体
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ShopEntity GetEntityByMobile(string mobile)
        {
            var sql = @"SELECT * FROM bll_shop WHERE ShopAccount= @mobile ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("mobile", mobile));

            var list = this.BaseRepository().FindList(sql, parms.ToArray());
            return list != null ? list.FirstOrDefault() : null;
        }

        /// <summary>
        /// 根据访问令牌获取用户实体
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public ShopEntity GetEntityByAccessToken(string accessToken)
        {
            var sql = @"SELECT * FROM bll_shop WHERE AccessToken= @accessToken ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("accessToken", accessToken));

            var list = this.BaseRepository().FindList(sql, parms.ToArray());
            return list != null ? list.FirstOrDefault() : null;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ShopEntity GetEntity(long keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int InsertEntity(ShopEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int UpdateEntity(ShopEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int Remove(long keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }
    }
}