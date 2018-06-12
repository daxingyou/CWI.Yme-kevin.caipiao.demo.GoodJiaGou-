using System.Collections.Generic;
using Yme.Data.Repository;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Model.Enums;
using Yme.Util.Extension;
using System.Linq;
using System.Data;
using System.Data.Common;
using Yme.Data;
using Yme.Mcp.Model.QueryModels;

namespace Yme.Mcp.Service.BaseManage
{
    /// <summary>
    /// 打印机配置服务
    /// </summary>
    public class PrinterConfigService : RepositoryFactory<PrinterConfigEntity>, IPrinterConfigService
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public PrinterConfigEntity GetEntity(long keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        public List<PrinterConfigEntity> GetList(long shopId, string printerCode)
        {
            var expression = Extensions.True<PrinterConfigEntity>();
            expression = expression.And(t => t.ShopId == shopId);
            expression = expression.And(t => t.PrinterCode == printerCode);
            expression = expression.And(t => t.EnabledFlag == (int)EnabledFlagType.Valid);
            expression = expression.And(t => t.DeleteFlag == (int)DeleteFlagType.Valid);
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="businessType"></param>
        /// <param name="sortBy"></param>
        /// <param name="isContainCancel"></param>
        /// <returns></returns>
        public List<BillPrintQueryModel> GetPriterConfigList(long shopId, BusinessType businessType, bool isContainCancel = true, int sortBy = 0)
        {
            var orderBy = (sortBy == 0 ? "bt.PrintSort" : "bt.DisplaySort");
            var sql = string.Format(@"SELECT 
                                           pc.BillId,pc.Copies,GROUP_CONCAT(pc.PrinterCode) AS PrinterCodes
                                      FROM bll_printer_config AS pc 
                                           INNER JOIN base_billtemplate AS bt ON pc.BillId = bt.BillId
                                           INNER JOIN bll_shop_printer AS sp ON pc.PrinterCode = sp.PrinterCode
                                      WHERE pc.BusinessType = @businessType AND pc.ShopId = @shopId 
                                            AND sp.EnabledFlag = 1 AND sp.DeleteFlag = 0
                                            AND pc.EnabledFlag = 1 AND pc.DeleteFlag = 0 {0}
                                      GROUP BY pc.BillId,pc.Copies
                                      ORDER BY {1}, pc.PrinterCode", (!isContainCancel ? " AND bt.BillId != " + SmallBillType.Cancel.GetHashCode() : string.Empty), orderBy);

            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("businessType", businessType.GetHashCode()));
            return this.BaseRepository().FindTable(sql, parms.ToArray()).ToList<BillPrintQueryModel>();
        }

        /// <summary>
        /// 查询门店配置信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="printerCode"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public DataTable GetShopPrinterConfigList(long shopId, string printerCode, BusinessType businessType)
        {
            var sql = @"SELECT 
                              bt.BillId, bt.BillName, CASE WHEN pc.BillId IS NULL THEN 0 ELSE 1 END AS IsConfig,
                              IFNULL(pc.Copies, 1) AS Copies, bt.BillTemplateUrl 
                        FROM  base_billtemplate AS bt 
                              LEFT JOIN (SELECT * FROM bll_printer_config WHERE ShopId = @shopId AND PrinterCode = @printerCode AND EnabledFlag = 1 AND DeleteFlag = 0) AS pc
                              ON pc.BillId = bt.BillId
                        WHERE bt.IsDisplay = 1 AND bt.BusinessType = @businessType";

            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("printerCode", printerCode));
            parms.Add(DbParameters.CreateDbParameter("businessType", businessType.GetHashCode()));

            return this.BaseRepository().FindTable(sql, parms.ToArray());
        }

        /// <summary>
        /// 获取门店配置信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="businessType"></param>
        /// <param name="isContainCancel"></param>
        /// <returns></returns>
        public DataTable GetShopPrintConfigList(long shopId, BusinessType businessType, bool isContainCancel = true)
        {
            var sql = string.Format(@"SELECT 
                              DISTINCT bt.BillId, bt.BillName
                        FROM  base_billtemplate AS bt
                              INNER JOIN bll_printer_config AS pc 
                              ON pc.BillId = bt.BillId AND pc.ShopId = @shopId AND pc.EnabledFlag = 1 AND pc.DeleteFlag = 0
                        WHERE bt.IsDisplay = 1 AND bt.BusinessType = @businessType {0}", !isContainCancel ? " AND bt.BillId != " + SmallBillType.Cancel.GetHashCode() : string.Empty);

            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("businessType", businessType.GetHashCode()));
            return this.BaseRepository().FindTable(sql, parms.ToArray());
        }

        /// <summary>
        /// 查询门店配置信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="printerCode"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public string GetShopPrinterConfigStr(long shopId, string printerCode, BusinessType businessType)
        {
            var sql = @"SELECT 
                              GROUP_CONCAT(bt.BillShortName) AS Configs 
                        FROM  bll_printer_config AS pc 
                              INNER JOIN base_billtemplate AS bt ON pc.BillId = bt.BillId
                        WHERE pc.ShopId = @shopId AND pc.PrinterCode = @printerCode AND pc.EnabledFlag = 1 AND pc.DeleteFlag = 0
                        GROUP BY pc.PrinterCode";

            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("printerCode", printerCode));
            parms.Add(DbParameters.CreateDbParameter("businessType", businessType.GetHashCode()));

            return Extensions.ToString(this.BaseRepository().FindObject(sql, parms.ToArray())).Replace(",", "、");
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int InsertEntity(PrinterConfigEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public int InserEntities(List<PrinterConfigEntity> entityList)
        {
            return this.BaseRepository().Insert(entityList);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int UpdateEntity(PrinterConfigEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public int DeleteEntity(long keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 删除打印机所有配置
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="printerCode"></param>
        /// <returns></returns>
        public int Delete(long shopId, string printerCode)
        {
            var sql = @"DELETE FROM bll_printer_config WHERE ShopId = @shopId AND PrinterCode = @printerCode ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
            parms.Add(DbParameters.CreateDbParameter("printerCode", printerCode));

            return this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
        }
    }
}