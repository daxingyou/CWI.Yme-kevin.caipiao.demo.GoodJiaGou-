using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Data;
using Yme.Data.Repository;
using Yme.Mcp.Entity.ReportManage;
using Yme.Util;
using Yme.Util.Exceptions;
using Yme.Util.Extension;
using Yme.Util.Log;

namespace Yme.Mcp.Service.ReportManage
{
    public class OrderDaystatisService : RepositoryFactory<OrderDaystatisEntity>, IOrderDaystatisService
    {
        /// <summary>
        /// 查询订单日报（业绩日报）
        /// </summary>
        public OrderDaystatisEntity GetEntity(long shopId, string queryDate)
        {
            OrderDaystatisEntity entity = null;
            try
            {
                if (!queryDate.IsDate())
                {
                    throw new MessageException("查询参数格式不正确！");
                }

                var sql = "SELECT * FROM rpt_order_daystatis WHERE shopId=@shopId AND rptDate=@rptDate";
                var parms = new DbParameter[]
                {
                   DbParameters.CreateDbParameter("shopId", shopId),
                   DbParameters.CreateDbParameter("rptDate", queryDate)
                };

                entity = this.BaseRepository().FindList(sql, parms).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("查询日报异常，参考信息：{0}", ex.Message));
            }
            return entity;
        }

        public List<OrderDaystatisEntity> FindList(string queryDate)
        {
            List<OrderDaystatisEntity> list = null;
            try
            {
                if (!queryDate.IsDate())
                {
                    throw new MessageException("查询参数格式不正确！");
                }

                var sql = "SELECT * FROM rpt_order_daystatis WHERE rptDate=@rptDate";
                var parms = new DbParameter[]
                {
                   DbParameters.CreateDbParameter("rptDate", queryDate)
                };

                list = this.BaseRepository().FindList(sql, parms).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("查询日报异常，参考信息：{0}", ex.Message));
            }
            return list;
        }

        /// <summary>
        /// 生成订单日报（业绩日报）
        /// </summary>
        public int InsertEntity(OrderDaystatisEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        public int InsertEntity(List<OrderDaystatisEntity> entities)
        {
            return this.BaseRepository().Insert(entities);
        }
    }
}
