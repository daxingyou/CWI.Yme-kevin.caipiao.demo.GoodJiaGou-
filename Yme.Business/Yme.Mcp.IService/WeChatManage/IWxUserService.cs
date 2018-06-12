using System.Collections.Generic;
using Yme.Mcp.Entity.WeChatManage;

namespace Yme.Mcp.Service.WeChatManage
{
    public interface IWxUserService
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <returns></returns>
        List<WxUserEntity> GetList();

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        WxUserEntity GetEntity(string keyValue);

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="wxType"></param>
        /// <returns></returns>
        WxUserEntity GetEntity(string openId, int wxType);

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="wxType"></param>
        /// <returns></returns>
        WxUserEntity GetEntity(long shopId, int wxType);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Id"></param>
        int InsertEntity(WxUserEntity entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(WxUserEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        int DeleteEntity(int keyValue);

        int DeleteEntity(long shopId, int wxType);

        int DeleteEntity(string openId, int wxType);
    }
}
