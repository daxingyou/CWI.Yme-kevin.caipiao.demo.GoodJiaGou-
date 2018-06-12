
namespace Yme.Mcp.Model.QueryModels
{
    /// <summary>
    /// 设备任务查询实体
    /// </summary>
    public class DeviceTaskQueryModel
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 任务数据Id
        /// </summary>
        public int TaskDataType { get; set; }

        /// <summary>
        /// 任务数据
        /// </summary>
        public string TaskData { get; set; }

        /// <summary>
        /// 任务份数
        /// </summary>
        public int Copies { get; set; }
    }
}
