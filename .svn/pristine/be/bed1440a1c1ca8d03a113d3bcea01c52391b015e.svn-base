using System.Collections.Generic;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Model.QueryModels;

namespace Yme.Mcp.IService.BaseManage
{
    public interface IShopPlatformService
    {
        /// <summary>
        /// 获取门店平台关联信息
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        List<ShopPlatformEntity> FindShopPlatformList(BusinessType businessType);

        /// <summary>
        /// 获取待访问令牌待更新列表
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        List<ShopPlatformEntity> FindTokenWaitUpdateList();

        /// <summary>
        /// 获取门店平台授权列表
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        List<ShopPlatformQueryModel> FindShopPlatformList(long shopId, BusinessType businessType);

        /// <summary>
        /// 获取门店已授权列表平台
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        List<ShopPlatformEntity> FindShopAuthPlatformList(long shopId, BusinessType businessType, int platformId = 0);

        /// <summary>
        /// 获取门店授权平台信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="platformId"></param>
        /// <param name="businessType"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        ShopPlatformEntity GetEntity(long shopId, int platformId, int businessType, AuthBussinessType authType);

        /// <summary>
        /// 获取门店授权平台信息
        /// </summary>
        /// <param name="platformId">平台Id</param>
        /// <param name="pShopId">平台门店Id</param>
        /// <returns></returns>
        ShopPlatformEntity GetEntity(int platformId, string pShopId);

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        ShopPlatformEntity GetEntity(long entityId);

        /// <summary>
        /// 获取百度授权实体信息
        /// </summary>
        /// <param name="sourceId">百度收取应用Id</param>
        /// <returns></returns>
        ShopPlatformEntity GetEntityBySourceId(string sourceId);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int InsertEntity(ShopPlatformEntity entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(ShopPlatformEntity entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="platformId"></param>
        /// <param name="businessType"></param>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        int UpdateEntity(long shopId, int platformId, int businessType, string accessToken, string refreshToken, long expiresIn);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopPlatforms"></param>
        /// <returns></returns>
        int UpdateEntities(List<ShopPlatformEntity> shopPlatforms);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        int Delete(long keyValue);

        int Delete(long shopId, int platformId, int businessType);

        bool CheckShopIsAuhtPlatform(long shopId);
    }
}