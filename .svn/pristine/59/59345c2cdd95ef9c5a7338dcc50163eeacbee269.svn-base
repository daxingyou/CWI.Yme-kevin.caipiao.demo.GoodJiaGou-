using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Mcp.Entity.ReportManage;
using Yme.Util.WebControl;

namespace Yme.Mcp.Service.ReportManage
{
    public interface IPlatformdishDaystatisService
    {
        List<PlatformdishDaystatisEntity> FindList(long shopId, string beginDate, string endDate, int orderBy);

        /// <summary>
        /// 生成门店平台菜品日报
        /// </summary>
        int InsertEntity(PlatformdishDaystatisEntity entity);

        /// <summary>
        /// 批量生成门店平台菜品日报
        /// </summary>
        int InsertEntity(List<PlatformdishDaystatisEntity> entities);
    }
}
