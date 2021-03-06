﻿using System.Collections.Generic;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.IService.BaseManage
{
    public interface IPlatformService
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        PlatformEntity GetEntity(long keyValue);

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        List<PlatformEntity> GetList();

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        List<PlatformEntity> GetList(BusinessType businessType);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Id"></param>
        int InsertEntity(PlatformEntity entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(PlatformEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        int Remove(long keyValue);
    }
}