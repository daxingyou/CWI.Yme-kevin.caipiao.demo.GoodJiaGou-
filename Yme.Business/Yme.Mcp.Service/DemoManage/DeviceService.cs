using System.Collections.Generic;
using Yme.Data.Repository;
using Yme.Mcp.Entity.DemoManage;

namespace Yme.Mcp.Service.DemoManage
{
    public class DeviceService : RepositoryFactory<DeviceEntity>, IDeviceService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public DeviceEntity GetEntity(string entityId)
        {
            return this.BaseRepository().FindEntity(entityId);
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int InsertEntity(DeviceEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int UpdateEntity(DeviceEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }
    }
}
