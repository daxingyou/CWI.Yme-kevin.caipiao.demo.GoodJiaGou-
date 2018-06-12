using System.Collections.Generic;
using Yme.Data.Repository;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Model.Enums;
using Yme.Util.Extension;
using System.Linq;
using System.Data.Common;
using Yme.Data;

namespace Yme.Mcp.Service.BaseManage
{
    /// <summary>
    /// 门店打印机服务
    /// </summary>
    public class ShopPrinterService : RepositoryFactory<ShopPrinterEntity>, IShopPrinterService
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ShopPrinterEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取指定平台下打印机列表
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public List<ShopPrinterEntity> GetList(PlatformType platformType)
        {
            var sql = @"SELECT 
                              spr.*
                        FROM bll_shop_platform AS spl 
                             INNER JOIN bll_shop_printer AS spr ON spl.ShopId = spr.ShopId 
                        WHERE spl.PlatformId = @platformId AND spr.EnabledFlag = 1 AND spr.DeleteFlag = 0";

            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("platformId", platformType.GetHashCode()));

            return this.BaseRepository().FindList(sql, parms.ToArray()).ToList<ShopPrinterEntity>();
        }

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        public List<ShopPrinterEntity> GetList(string printerCode)
        {
            var expression = Extensions.True<ShopPrinterEntity>();
            expression = expression.And(t => t.PrinterCode.Equals(printerCode, System.StringComparison.CurrentCultureIgnoreCase));
            expression = expression.And(t => t.DeleteFlag == (int)DeleteFlagType.Valid);
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        public List<ShopPrinterEntity> GetList(long shopId)
        {
            var expression = Extensions.True<ShopPrinterEntity>();
            expression = expression.And(t => t.ShopId == shopId);
            expression = expression.And(t => t.DeleteFlag == (int)DeleteFlagType.Valid);
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        public bool CheckEntity(long shopId, string printerCode, string printerName)
        {
            var expression = Extensions.True<ShopPrinterEntity>();
            expression = expression.And(t => t.ShopId == shopId);
            expression = expression.And(t => !t.PrinterCode.Equals(printerCode, System.StringComparison.CurrentCultureIgnoreCase));
            expression = expression.And(t => t.PrinterName.Equals(printerName, System.StringComparison.CurrentCultureIgnoreCase));
            expression = expression.And(t => t.DeleteFlag == (int)DeleteFlagType.Valid);
            var obj = this.BaseRepository().IQueryable(expression).FirstOrDefault();
            return obj != null;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int InsertEntity(ShopPrinterEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int UpdateEntity(ShopPrinterEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public int DeleteEntity(string keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }
    }
}