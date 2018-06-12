using System.Collections.Generic;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.IService.BaseManage
{
    public interface IMerchantService
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        MerchantEntity GetEntity(string keyValue);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int InsertEntity(MerchantEntity entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(MerchantEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        int Remove(string keyValue);
    }
}