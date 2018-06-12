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
    public class PlatformorderDaystatisService : RepositoryFactory<PlatformorderDaystatisEntity>, IPlatformorderDaystatisService
    {
        public List<PlatformorderDaystatisEntity> FindList(long shopId, int platformId, string beginDate, string endDate)
        {
            var list = new List<PlatformorderDaystatisEntity>();
            try
            {
                if (!beginDate.IsDate() || !endDate.IsDate())
                {
                    throw new MessageException("查询日期参数格式不正确！");
                }

                var fliterStr = string.Empty;
                var parms = new List<DbParameter>();
                parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
                parms.Add(DbParameters.CreateDbParameter("beginDate", beginDate.ToDate().GetDayBegin()));
                parms.Add(DbParameters.CreateDbParameter("endDate", endDate.ToDate().GetDayEnd()));
                if (platformId > 0)
                {
                    fliterStr = " AND PlatformId = @platformId ";
                    parms.Add(DbParameters.CreateDbParameter("platformId", platformId));
                }
                var sql = string.Format(@"SELECT * FROM rpt_platformorder_daystatis WHERE ShopId = @shopId {0} AND RptDate >= @beginDate AND RptDate <= @endDate", fliterStr);
                list = this.BaseRepository().FindList(sql, parms.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("查询平台日报异常，参考信息：{0}", ex.Message));
            }
            return list;
        }

        /// <summary>
        /// 生成平台订单日报
        /// </summary>
        public int InsertEntity(PlatformorderDaystatisEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 生成平台订单日报
        /// </summary>
        public int InsertEntity(List<PlatformorderDaystatisEntity> entities)
        {
            return this.BaseRepository().Insert(entities);
        }
    }
}
