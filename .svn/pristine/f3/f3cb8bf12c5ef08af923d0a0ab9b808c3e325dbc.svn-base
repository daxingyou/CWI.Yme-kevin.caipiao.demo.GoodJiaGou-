using System.Collections.Generic;
using Yme.Mcp.Entity.WeChatManage;

namespace Yme.Mcp.Service.WeChatManage
{
    public interface IWxTokenService
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <returns></returns>
        List<WxTokenEntity> GetList();

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        WxTokenEntity GetEntity(int keyValue);

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        WxTokenEntity GetEntity(string appId);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int InsertEntity(WxTokenEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateEntity(WxTokenEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        int DeleteEntity(int keyValue);

        int DeleteEntity(string appId);
    }
}
