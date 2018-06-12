using System;
using Evt.Framework.Common;

using Yme.Mcp.Model.Enums;
using Yme.Mcp.Entity.DemoManage;
using Yme.Mcp.Service.DemoManage;
using Yme.Util;
using Yme.Data.Repository;
using Yme.Util.Log;
using System.Collections.Generic;
using Yme.Mcp.Model.QueryModels;

namespace Yme.Mcp.BLL.DemoManage
{
    public class TaskBLL
    {
        #region 私有变量

        private ITaskService taskServ = new TaskService();
        private IDeviceTaskService deviceTaskServ = new DeviceTaskService();
       
        #endregion

        #region 公有方法

        /// <summary>
        /// 获取下一打印任务
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <returns></returns>
        public DeviceTaskQueryModel GetNextTask(string deviceId)
        {
            return deviceTaskServ.GetNextEntity(deviceId);
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public DeviceTaskEntity GetTask(string taskId, string deviceId)
        {
            return deviceTaskServ.GetEntity(taskId, deviceId);
        }

        /// <summary>
        /// 新增打印任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public int InsertTask(TaskEntity task, Dictionary<string, int> dicDevice)
        {
            var cnt = 0;
            var db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                if (task != null && dicDevice.Count > 0)
                {
                    //1.任务信息
                    task.TaskId = string.IsNullOrWhiteSpace(task.TaskId) ? StringUtil.UniqueStr() : task.TaskId;
                    task.TaskDataType = task.TaskDataType <= TaskDataType.TEMPLATE.GetHashCode() ? TaskDataType.TEMPLATE.GetHashCode() : task.TaskDataType;
                    task.CreateDate = TimeUtil.Now;


                    //2.设备任务信息
                    var deviceTasks = new List<DeviceTaskEntity>();
                    foreach (var device in dicDevice)
                    {
                        deviceTasks.Add(new DeviceTaskEntity()
                        {
                            TaskId = task.TaskId,
                            DeviceId = device.Key.Trim(),
                            Copies = device.Value <= 0 ? 1 : device.Value,
                            TaskStatus = TaskStatus.WaitPrint.GetHashCode(),
                            CreateDate = TimeUtil.Now
                        });
                    }

                    taskServ.InsertEntity(task);
                    cnt = deviceTaskServ.InsertEntities(deviceTasks);

                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogUtil.Error(string.Format("打印任务保存失败，参考消息：{0}", ex.StackTrace));
                throw new MessageException(ex.Message);
            }
            return cnt;
        }

        /// <summary>
        /// 更新打印任务
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateTask(DeviceTaskEntity task)
        {
            return deviceTaskServ.UpdateEntity(task);
        }

        /// <summary>
        /// 更新打印任务状态
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateTaskStatus(string taskId, string deviceId, int status)
        {
            var cnt = -1;
            try
            {
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    throw new MessageException("设备Id不能为空");
                }
                if (string.IsNullOrWhiteSpace(taskId))
                {
                    throw new MessageException("任务Id不能为空");
                }

                var task = GetTask(taskId, deviceId);
                if (task == null)
                {
                    throw new MessageException("此任务不存在");
                }

                // 状态取值未校验
                if (task.TaskStatus != status)
                {
                    task.TaskStatus = status;
                    cnt = deviceTaskServ.UpdateEntity(task);
                }
            }
            catch (Exception ex)
            {
                throw new MessageException(ex.Message);
            }
            return cnt;
        }

        #endregion
    }
}
