using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

using Yme.Data;
using Yme.Data.Repository;
using Yme.Mcp.Entity.DemoManage;
using Yme.Mcp.Model.QueryModels;
using Yme.Util.Extension;

namespace Yme.Mcp.Service.DemoManage
{
    public class DeviceTaskService : RepositoryFactory<DeviceTaskEntity>, IDeviceTaskService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public DeviceTaskEntity GetEntity(string entityId)
        {
            return this.BaseRepository().FindEntity(entityId);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public DeviceTaskEntity GetEntity(string taskId, string deviceId)
        {
            var expression = Extensions.True<DeviceTaskEntity>();
            expression = expression.And(t => t.TaskId == taskId);
            expression = expression.And(t => t.DeviceId == deviceId);
            return this.BaseRepository().IQueryable(expression).ToList().FirstOrDefault();
        }

        /// <summary>
        /// 获取设备下一实体
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <returns></returns>
        public DeviceTaskQueryModel GetNextEntity(string deviceId)
        {
            var sql = @"SELECT 
                              dt.DeviceId, dt.TaskId, dt.Copies, t.TaskDataType, t.TaskData 
                        FROM
                              device_task AS dt INNER JOIN task_info AS t ON dt.TaskId = t.TaskId 
                        WHERE dt.DeviceId = @deviceId AND dt.TaskStatus <> 1 AND TIMESTAMPDIFF(MINUTE, dt.CreateDate, NOW()) <= 5 ORDER BY dt.DeviceTaskId LIMIT 1 ";

            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("deviceId", deviceId));
            return this.BaseRepository().FindTable(sql, parms.ToArray()).ToList<DeviceTaskQueryModel>().FirstOrDefault();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int InsertEntity(DeviceTaskEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities"></param>
        public int InsertEntities(List<DeviceTaskEntity> entities)
        {
            return this.BaseRepository().Insert(entities);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int UpdateEntity(DeviceTaskEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }
    }
}
