using System.Collections.Generic;
using System.Data;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Model.QueryModels;

namespace Yme.Mcp.IService.BaseManage
{
    public interface IPrinterConfigService
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        PrinterConfigEntity GetEntity(long keyValue);

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        List<PrinterConfigEntity> GetList(long shopId, string printerCode);

        /// <summary>
        /// 查询门店配置信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="printerCode"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        DataTable GetShopPrinterConfigList(long shopId, string printerCode, BusinessType businessType);

        List<BillPrintQueryModel> GetPriterConfigList(long shopId, BusinessType businessType, bool isContainCancel = true, int sortBy = 0);

        DataTable GetShopPrintConfigList(long shopId, BusinessType businessType, bool isContainCancel = true);

        /// <summary>
        /// 查询门店配置信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="printerCode"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        string GetShopPrinterConfigStr(long shopId, string printerCode, BusinessType businessType);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Id"></param>
        int InsertEntity(PrinterConfigEntity entity);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        int InserEntities(List<PrinterConfigEntity> entityList);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int UpdateEntity(PrinterConfigEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        int DeleteEntity(long keyValue);

        /// <summary>
        /// 删除打印机所有配置
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="printerCode"></param>
        /// <returns></returns>
        int Delete(long shopId, string printerCode);
    }
}