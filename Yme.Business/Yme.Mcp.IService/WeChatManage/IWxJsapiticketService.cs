using System.Collections.Generic;
using Yme.Mcp.Entity.WeChatManage;

namespace Yme.Mcp.Service.WeChatManage
{
    public interface IWxJsapiticketService
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <returns></returns>
        List<WxJsapiticketEntity> GetList();

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        WxJsapiticketEntity GetEntity(int keyValue);

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        WxJsapiticketEntity GetEntity(string appId);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int InsertEntity(WxJsapiticketEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateEntity(WxJsapiticketEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        int DeleteEntity(int keyValue);

        int DeleteEntity(string appId);
    }
}
