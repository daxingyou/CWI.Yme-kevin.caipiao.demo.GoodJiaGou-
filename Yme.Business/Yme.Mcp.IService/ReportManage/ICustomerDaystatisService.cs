using System;
using System.Collections.Generic;
using Yme.Mcp.Entity.ReportManage;

namespace Yme.Mcp.Service.ReportManage
{
    public interface ICustomerDaystatisService
    {
        List<CustomerDaystatisEntity> FindList(DateTime queryDate);

        /// <summary>
        /// 查询订单日报（业绩日报）
        /// </summary>
        CustomerDaystatisEntity GetEntity(long shopId, string queryDate);

        /// <summary>
        /// 生成门店客户日报
        /// </summary>
        int InsertEntity(CustomerDaystatisEntity entity);

        /// <summary>
        /// 批量生成门店客户日报
        /// </summary>
        int InsertEntity(List<CustomerDaystatisEntity> entities);

        List<CustomerDaystatisEntity> GetShopDayCustomerTotal(DateTime queryDate);
    }
}
