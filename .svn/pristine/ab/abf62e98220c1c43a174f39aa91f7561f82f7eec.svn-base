using System;
using Evt.Framework.Common;

using Yme.Mcp.Model.Enums;
using Yme.Mcp.Entity.DemoManage;
using Yme.Mcp.Service.DemoManage;
using Yme.Util;

namespace Yme.Mcp.BLL.DemoManage
{
    public class DeviceBLL
    {
        #region 私有变量

        private IDeviceService deviceServ = new DeviceService();

        #endregion

        #region 公有方法

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public DeviceEntity GetDevice(string deviceId)
        {
            return deviceServ.GetEntity(deviceId);
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InserDevice(DeviceEntity entity)
        {
            var cnt = 0;
            if (entity != null)
            {
                if(entity.DeviceStatus == 0)
                {
                    entity.DeviceStatus = DeviceStatus.Enable.GetHashCode();
                    entity.CreateDate = TimeUtil.Now;
                }
                cnt = deviceServ.InsertEntity(entity);
            }
            return cnt;
        }

        /// <summary>
        /// 更新实体信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateDevice(DeviceEntity entity)
        {
            return deviceServ.UpdateEntity(entity);
        }

        /// <summary>
        /// 更新实体状态
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateStatus(string deviceId, int status)
        {
            var cnt = -1;
            try
            {
                DeviceEntity device = null;
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    throw new MessageException("设备Id不能为空");
                }
                else
                {
                    device = deviceServ.GetEntity(deviceId);
                    if (device == null)
                    {
                        throw new MessageException("设备不存在");
                    }
                }

                // 状态取值未校验
                if (device.DeviceStatus != status)
                {
                    device.DeviceStatus = status;
                    cnt = deviceServ.UpdateEntity(device);
                }
            }
            catch(Exception ex)
            {
                throw new MessageException(ex.Message);
            }
            return cnt;
        }

        #endregion
    }
}
