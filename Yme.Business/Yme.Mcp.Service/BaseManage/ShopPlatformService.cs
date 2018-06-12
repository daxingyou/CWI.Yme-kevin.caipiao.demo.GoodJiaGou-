using System.Collections.Generic;
using System.Data.Common;
using Yme.Data;
using Yme.Data.Repository;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Model.QueryModels;
using Yme.Util.Extension;
using System.Linq;
using Yme.Util;
using Yme.Util.Log;

namespace Yme.Mcp.Service.BaseManage
{
    /// <summary>
    /// 门店平台服务
    /// </summary>
    public class ShopPlatformService : RepositoryFactory<ShopPlatformEntity>, IShopPlatformService
    {
        public List<ShopPlatformEntity> FindShopPlatformList(BusinessType businessType)
        {
            var expression = Extensions.True<ShopPlatformEntity>();
            expression = expression.And(t => t.BusinessType == (int)businessType);
            expression = expression.And(t => t.EnabledFlag == (int)EnabledFlagType.Valid);
            expression = expression.And(t => t.DeleteFlag == (int)DeleteFlagType.Valid);
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// 获取访问令牌待更新列表
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public List<ShopPlatformEntity> FindTokenWaitUpdateList()
        {
            var sql = @"SELECT * FROM bll_shop_platform WHERE PlatformId = @platformId AND AuthBussiness = @authType AND  RefreshExpireTime <= NOW() ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("platformId", PlatformType.Eleme.GetHashCode()));
            parms.Add(DbParameters.CreateDbParameter("authType", AuthBussinessType.Waimai.GetHashCode()));

            var list = this.BaseRepository().FindList(sql, parms.ToArray()).ToList<ShopPlatformEntity>();
            LogUtil.Info(string.Format("待更新饿了么访问令牌门店个数：{0}", list.Count));
            return list;
        }

        /// <summary>
        /// 获取门店平台授权
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="businessType">业务类型</param>
        /// <returns></returns>
        public List<ShopPlatformQueryModel> FindShopPlatformList(long shopId, BusinessType businessType)
        {
            var sql = @"SELECT
                           p.PlatformId, 
                           p.PlatformName,
                           IFNULL(sp.IsKaMerchant,0) AS IsKaMerchant,
                           IFNULL(sp.KaMerchantAuthUrl,'') AS KaMerchantAuthUrl,
                           IFNULL(sp.AuthId,0) AS AuthId,
                           CASE WHEN sp.AuthId IS NOT NULL AND PlatformShopName!='' THEN 1 ELSE 0 END AS AuthStasus
                     FROM
                           base_platform AS p 
                           LEFT JOIN 
                           (SELECT * FROM bll_shop_platform WHERE ShopId = @shopId AND BusinessType = @businessType AND EnabledFlag = 1 AND DeleteFlag = 0) AS sp ON p.PlatformId = sp.PlatformId 
                     WHERE p.EnabledFlag = 1 AND p.DeleteFlag = 0 ";

            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("businessType", businessType.GetHashCode()));

            return this.BaseRepository().FindTable(sql, parms.ToArray()).ToList<ShopPlatformQueryModel>();
        }

        /// <summary>
        /// 获取门店已授权平台列表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="businessType">业务类型</param>
        /// <returns></returns>
        public List<ShopPlatformEntity> FindShopAuthPlatformList(long shopId, BusinessType businessType, int platformId = 0)
        {
            var fliter = string.Empty;
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("businessType", businessType.GetHashCode()));
            if (platformId > 0)
            {
                parms.Add(DbParameters.CreateDbParameter("platformId", platformId));
                fliter = "AND PlatformId = @platformId";
            }
            var sql = string.Format(@"SELECT * FROM bll_shop_platform WHERE shopid = @shopId AND BusinessType = @businessType AND platformshopname != '' AND platformshopname IS NOT NULL AND EnabledFlag = 1 AND DeleteFlag = 0 {0} ORDER BY PlatformId", fliter);

            return this.BaseRepository().FindList(sql, parms.ToArray()).ToList<ShopPlatformEntity>();
        }

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="platformId"></param>
        /// <param name="businessType"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        public ShopPlatformEntity GetEntity(long shopId, int platformId, int businessType, AuthBussinessType authType)
        {
            var sql = @"SELECT * FROM bll_shop_platform WHERE ShopId = @shopId AND PlatformId = @platformId AND BusinessType = @businessType AND AuthBussiness = @authType ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("platformId", platformId));
            parms.Add(DbParameters.CreateDbParameter("businessType", businessType));
            parms.Add(DbParameters.CreateDbParameter("authType", authType.GetHashCode()));

            return this.BaseRepository().FindList(sql, parms.ToArray()).FirstOrDefault();
        }

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <param name="platformId">平台Id</param>
        /// <param name="pShopId">平台门店Id</param>
        /// <returns></returns>
        public ShopPlatformEntity GetEntity(int platformId, string pShopId)
        {
            var sql = @"SELECT * FROM bll_shop_platform WHERE PlatformShopId = @shopId AND PlatformId = @platformId ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("platformId", platformId));
            parms.Add(DbParameters.CreateDbParameter("shopId", pShopId));

            return this.BaseRepository().FindList(sql, parms.ToArray()).FirstOrDefault();
        }

        /// <summary>
        /// 获取百度授权实体信息
        /// </summary>
        /// <param name="sourceId">百度收取应用Id</param>
        /// <returns></returns>
        public ShopPlatformEntity GetEntityBySourceId(string sourceId)
        {
            var sql = @"SELECT * FROM bll_shop_platform WHERE PlatformId = @platformId AND RefreshToken = @sourceId ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("platformId", PlatformType.Baidu.GetHashCode()));
            parms.Add(DbParameters.CreateDbParameter("sourceId", sourceId));

            return this.BaseRepository().FindList(sql, parms.ToArray()).FirstOrDefault();
        }

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public ShopPlatformEntity GetEntity(long entityId)
        {
            return this.BaseRepository().FindEntity(entityId);
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int InsertEntity(ShopPlatformEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int UpdateEntity(ShopPlatformEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 更新平台访问令牌
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="platformId"></param>
        /// <param name="businessType"></param>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        public int UpdateEntity(long shopId, int platformId, int businessType, string accessToken, string refreshToken, long expiresIn)
        {
            var sql = @"UPDATE bll_shop_platform SET AuthToken=@accessToken, ExpiresIn=@expiresIn, RefreshToken=@refreshToken,RefreshExpireTime=@rexpiresIn,ModifyDate=NOW() WHERE ShopId = @shopId AND PlatformId = @platformId AND AuthBussiness = @businessType";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("accessToken", accessToken));
            parms.Add(DbParameters.CreateDbParameter("expiresIn", expiresIn));
            parms.Add(DbParameters.CreateDbParameter("refreshToken", refreshToken));
            parms.Add(DbParameters.CreateDbParameter("rexpiresIn", TimeUtil.Now.AddDays(ConfigUtil.EleReTokenExpiresDays)));

            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("platformId", platformId));
            parms.Add(DbParameters.CreateDbParameter("businessType", businessType));

            LogUtil.Info(string.Format("platformId:{0},businessType:{1},shopId:{2},accessToken:{3},refreshToken:{4},expiresIn:{5}", platformId, businessType, shopId, accessToken, refreshToken, expiresIn));
            return this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
        }

        /// <summary>
        /// 批量更新平台访问令牌
        /// </summary>
        /// <param name="shopPlatforms"></param>
        /// <returns></returns>
        public int UpdateEntities(List<ShopPlatformEntity> shopPlatforms)
        {
            return this.BaseRepository().Update(shopPlatforms);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public int Delete(long keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="platformId"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public int Delete(long shopId, int platformId, int businessType)
        {
            var sql = @"DELETE FROM bll_shop_platform WHERE ShopId = @shopId AND PlatformId = @platformId AND AuthBussiness = @businessType";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("platformId", platformId));
            parms.Add(DbParameters.CreateDbParameter("businessType", businessType));

            return this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
        }

        /// <summary>
        /// 校验门店是否关联第三方平台
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public bool CheckShopIsAuhtPlatform(long shopId)
        {
            var expression = Extensions.True<ShopPlatformEntity>();
            expression = expression.And(t => t.ShopId == shopId);
            return this.BaseRepository().IQueryable(expression).Count() > 0;
        }
    }
}