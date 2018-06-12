using System.Collections.Generic;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.IService.BaseManage
{
    public interface IShopPrinterService
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        ShopPrinterEntity GetEntity(string keyValue);

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        List<ShopPrinterEntity> GetList(long shopId);

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        List<ShopPrinterEntity> GetList(PlatformType platformType);

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <param name="printerCode"></param>
        /// <returns></returns>
        List<ShopPrinterEntity> GetList(string printerCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="printerCode"></param>
        /// <param name="printerName"></param>
        /// <returns></returns>
        bool CheckEntity(long shopId, string printerCode, string printerName);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int InsertEntity(ShopPrinterEntity entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(ShopPrinterEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        int DeleteEntity(string keyValue);
    }
}