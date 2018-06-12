using Yme.Util;
using Yme.Util.Log;
using Yme.Util.Extension;
using Yme.Mcp.Order.Controllers;
using Yme.Mcp.BLL.ReportManage;
using Yme.Mcp.BLL.SystemManage;
using Yme.Mcp.Order.Handel;

using System.Web.Http;
using Yme.Mcp.Model;

namespace Yme.Mcp.Order.Controllers.Api
{
    /// <summary>
    /// 报表控制器
    /// </summary>
    public class ReportController : ApiBaseController
    {
        /// <summary>
        /// 获取业绩日报
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetOrderDayRpt([FromUri]DateRptViewModel model)
        {
            LogUtil.Info("执行获取业绩日报请求...");

            var result = SingleInstance<ReportBLL>.Instance.GetOrderDayReport(base.CurrId, model.QueryDate);
            return OK(result);
        }

        /// <summary>
        /// 获取平台统计日报
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetPlatformOrderDayRpt([FromUri]PlatformRptViewModel model)
        {
            LogUtil.Info("执行获取平台统计日报请求...");

            var sType = model != null ? model.StatisticsType.ToInt() : 0;
            var dateZone = SingleInstance<SystemBLL>.Instance.GetDateZone(sType);
            var result = SingleInstance<ReportBLL>.Instance.GetPlatformOrderDayReport(base.CurrId, model.PlatformId, dateZone.Item1.ToShorDateFormatString(), dateZone.Item2.ToShorDateFormatString());
            return OK(result);
        }

        /// <summary>
        /// 获取菜品报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetDishDayRpt([FromUri]DishRptViewModel model)
        {
            LogUtil.Info("执行获取菜品日报请求...");

            var result = SingleInstance<ReportBLL>.Instance.GetDishDayReport(base.CurrId, model.BeginDate, model.EndDate, model.orderBy);
            return OK(result);
        }

        /// <summary>
        /// 获取客户日报
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetCustomerDayRpt([FromUri]DateRptViewModel model)
        {
            LogUtil.Info("执行获取客户日报请求...");

            var result = SingleInstance<ReportBLL>.Instance.GetCustomerDayReport(base.CurrId, model.QueryDate);
            return OK(result);
        }
    }
}
