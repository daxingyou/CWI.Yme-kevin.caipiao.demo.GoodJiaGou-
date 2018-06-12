using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Mcp.Entity.ReportManage;

namespace Yme.Mcp.Service.ReportManage
{
    public interface IOrderDaystatisService
    {
        /// <summary>
        /// 查询订单日报（业绩日报）
        /// </summary>
        OrderDaystatisEntity GetEntity(long shopId, string queryDate);

        List<OrderDaystatisEntity> FindList(string queryDate);

        /// <summary>
        /// 生成订单日报（业绩日报）
        /// </summary>
        int InsertEntity(OrderDaystatisEntity entity);

        /// <summary>
        /// 生成订单日报（业绩日报）
        /// </summary>
        int InsertEntity(List<OrderDaystatisEntity> entities);
    }
}
