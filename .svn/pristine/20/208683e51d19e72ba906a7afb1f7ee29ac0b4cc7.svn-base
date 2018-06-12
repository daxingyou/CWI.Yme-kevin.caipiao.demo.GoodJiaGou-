using System;
using System.Data.Common;
using System.Linq;
using Yme.Util.Extension;
using Yme.Data.Repository;
using Yme.Mcp.Entity.ReportManage;
using Yme.Util.Exceptions;
using Yme.Data;
using System.Collections.Generic;
using Yme.Util.Log;

namespace Yme.Mcp.Service.ReportManage
{
    public class CustomerDaystatisService : RepositoryFactory<CustomerDaystatisEntity>, ICustomerDaystatisService
    {
        /// <summary>
        /// 查询客户日报
        /// </summary>
        public CustomerDaystatisEntity GetEntity(long shopId, string queryDate)
        {
            CustomerDaystatisEntity entity = null;
            try
            {
                if (!queryDate.IsDate())
                {
                    throw new MessageException("查询参数格式不正确！");
                }

                var sql = "SELECT * FROM rpt_customer_daystatis WHERE shopId=@shopId AND rptDate=@rptDate";
                var parms = new DbParameter[]
                {
                   DbParameters.CreateDbParameter("shopId", shopId),
                   DbParameters.CreateDbParameter("rptDate", queryDate)
                };

                entity = this.BaseRepository().FindList(sql, parms).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("查询客户日报异常，参考信息：{0}", ex.Message));
            }
            return entity;
        }

        public List<CustomerDaystatisEntity> FindList(DateTime queryDate)
        {
            List<CustomerDaystatisEntity> list = null;
            try
            {
                var sql = "SELECT * FROM rpt_customer_daystatis WHERE rptDate=@rptDate";
                var parms = new DbParameter[]
                {
                   DbParameters.CreateDbParameter("rptDate", queryDate.Date)
                };

                list = this.BaseRepository().FindList(sql, parms).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("查询客户日报异常，参考信息：{0}", ex.Message));
            }
            return list;
        }

        /// <summary>
        /// 查询门店日客户量
        /// </summary>
        /// <param name="queryDate"></param>
        /// <returns></returns>
        public List<CustomerDaystatisEntity> GetShopDayCustomerTotal(DateTime queryDate)
        {
            var parms = new List<DbParameter>();;
            parms.Add(DbParameters.CreateDbParameter("beginDate", queryDate.GetDayBegin()));
            parms.Add(DbParameters.CreateDbParameter("endDate", queryDate.GetDayEnd()));

            var sql = @"SELECT 
                              ShopId,COUNT(RecipientPhone) AS CustomerTotal
                        FROM 
                              (SELECT DISTINCT shopId,RecipientPhone FROM bll_order WHERE OrderTime >= @beginDate AND  OrderTime <= @endDate) AS t
                        GROUP BY ShopId ";

            return this.BaseRepository().FindTable(sql, parms.ToArray()).ToList<CustomerDaystatisEntity>();
        }

        /// <summary>
        /// 生成门店客户日报
        /// </summary>
        public int InsertEntity(CustomerDaystatisEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 批量生成门店客户日报
        /// </summary>
        public int InsertEntity(List<CustomerDaystatisEntity> entities)
        {
            return this.BaseRepository().Insert(entities);
        }
    }
}
