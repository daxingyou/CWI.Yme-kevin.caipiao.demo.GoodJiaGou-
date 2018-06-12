using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.IService.BaseManage
{
    public interface IShopService
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        ShopEntity GetEntity(long keyValue);

        /// <summary>
        /// 根据手机号获取门店实体
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        ShopEntity GetEntityByMobile(string mobile);

        /// <summary>
        /// 根据访问令牌获取用户实体
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        ShopEntity GetEntityByAccessToken(string accessToken);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Id"></param>
        int InsertEntity(ShopEntity entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(ShopEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        int Remove(long keyValue);
    }
}