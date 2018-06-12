using System.Collections.Generic;
using Yme.Mcp.Entity.DemoManage;
using Yme.Mcp.Model.QueryModels;

namespace Yme.Mcp.Service.DemoManage
{
    public interface IDeviceTaskService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        DeviceTaskEntity GetEntity(string entityId);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        DeviceTaskEntity GetEntity(string taskId, string deviceId);

        /// <summary>
        /// 获取设备下一实体
        /// </summary>
        /// <returns></returns>
        DeviceTaskQueryModel GetNextEntity(string deviceId);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Id"></param>
        int InsertEntity(DeviceTaskEntity entity);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities"></param>
        int InsertEntities(List<DeviceTaskEntity> entities);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(DeviceTaskEntity entity);
    }
}