using Yme.Mcp.Entity.DemoManage;

namespace Yme.Mcp.Service.DemoManage
{
    public interface ITaskService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        TaskEntity GetEntity(string entityId);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Id"></param>
        int InsertEntity(TaskEntity entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(TaskEntity entity);
    }
}
