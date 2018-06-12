using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Common;

using Yme.Util;
using Yme.Data;
using Yme.Util.Extension;
using Yme.Data.Repository;
using Yme.Mcp.Entity.OrderManage;
using Yme.Util.Log;

namespace Yme.Mcp.Service.OrderManage
{
    public class OrderDishService : RepositoryFactory<OrderDishEntity>, IOrderDishService
    {
        /// <summary>
        /// 查询订单菜品日列表
        /// </summary>
        /// <param name="orderDate"></param>
        /// <returns></returns>
        public List<OrderDishEntity> FindList(DateTime orderDate)
        {
            List<OrderDishEntity> list = null;
            try
            {
                var sql = "SELECT od.* FROM bll_order_dish AS od INNER JOIN bll_order AS o ON od.MorderId = o.MorderId WHERE od.orderTime >= @beginTime AND od.orderTime <= @endTime AND (o.OrderStatus >= 2 AND o.OrderStatus <= 4) ORDER BY od.shopId, od.orderTime";
                var parms = new DbParameter[]
                {
                   DbParameters.CreateDbParameter("beginTime", orderDate.GetDayBegin()),
                   DbParameters.CreateDbParameter("endTime", orderDate.GetDayEnd())
                };

                list = this.BaseRepository().FindList(sql, parms).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("查询门店菜品日订单异常，参考信息：{0}", ex.Message));
            }
            return list;
        }
    }
}
