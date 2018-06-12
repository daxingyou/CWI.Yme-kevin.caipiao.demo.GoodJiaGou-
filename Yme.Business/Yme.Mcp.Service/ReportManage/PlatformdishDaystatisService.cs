using System;
using System.Linq;
using Yme.Util.Extension;
using Yme.Data.Repository;
using Yme.Mcp.Entity.ReportManage;
using Yme.Util.Exceptions;
using System.Data.Common;
using Yme.Data;
using Yme.Util;
using System.Collections.Generic;
using Yme.Util.Log;

namespace Yme.Mcp.Service.ReportManage
{
    public class PlatformdishDaystatisService : RepositoryFactory<PlatformdishDaystatisEntity>, IPlatformdishDaystatisService
    {
        public List<PlatformdishDaystatisEntity> FindList(long shopId, string beginDate, string endDate, int orderBy)
        {
            var list = new List<PlatformdishDaystatisEntity>();
            try
            {
                if (!beginDate.IsDate() || !endDate.IsDate())
                {
                    throw new MessageException("查询日期格式不正确！");
                }

                var fliterStr = string.Empty;
                var parms = new List<DbParameter>();
                parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
                parms.Add(DbParameters.CreateDbParameter("beginDate", beginDate));
                parms.Add(DbParameters.CreateDbParameter("endDate", endDate));

                var sql = string.Format(@"SELECT DishName,PlatformId,SUM(DishTotal) AS DishTotal FROM rpt_platformdish_daystatis WHERE ShopId = @shopId AND RptDate >= @beginDate AND RptDate <= @endDate GROUP BY DishName,PlatformId ORDER BY DishTotal {0} ",
                    (orderBy == 0 ? "DESC" : "ASC"));
                list = this.BaseRepository().FindList(sql, parms.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("查询菜品报表异常，参考信息：{0}", ex.Message));
            }
            return list;
        }

        /// <summary>
        /// 生成门店平台菜品日报
        /// </summary>
        public int InsertEntity(PlatformdishDaystatisEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 批量生成门店平台菜品日报
        /// </summary>
        public int InsertEntity(List<PlatformdishDaystatisEntity> entities)
        {
            return this.BaseRepository().Insert(entities);
        }
    }
}
