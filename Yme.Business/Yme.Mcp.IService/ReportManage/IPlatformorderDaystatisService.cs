using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Mcp.Entity.ReportManage;

namespace Yme.Mcp.Service.ReportManage
{
    public interface IPlatformorderDaystatisService
    {
        /// <summary>
        /// 查询平台订单日报
        /// </summary>
        /// <param name="queryDate"></param>
        /// <returns></returns>
        List<PlatformorderDaystatisEntity> FindList(long shopId, int platformId, string beginDate, string endDate);

        /// <summary>
        /// 生成平台订单日报
        /// </summary>
        int InsertEntity(PlatformorderDaystatisEntity entity);

        /// <summary>
        /// 生成平台订单日报
        /// </summary>
        int InsertEntity(List<PlatformorderDaystatisEntity> entities);
    }
}
