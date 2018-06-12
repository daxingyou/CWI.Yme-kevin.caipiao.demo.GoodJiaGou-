using Yme.Data.Repository;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.IService.BaseManage;

namespace Yme.Mcp.Service.BaseManage
{
    /// <summary>
    /// 商家服务
    /// </summary>
    public class MerchantService : RepositoryFactory<MerchantEntity>, IMerchantService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public MerchantEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int InsertEntity(MerchantEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int UpdateEntity(MerchantEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int Remove(string keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }
    }
}