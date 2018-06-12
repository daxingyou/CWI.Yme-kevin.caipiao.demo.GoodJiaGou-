using System;
using System.Linq;
using System.Collections.Generic;

using Yme.Util;
using Yme.Mcp.Model;
using Yme.Util.Extension;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.BLL.OrderManage;
using Yme.Mcp.Service.ReportManage;
using Yme.Mcp.Entity.ReportManage;
using Yme.Mcp.Model.QueryModels;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Service.BaseManage;
using Yme.Mcp.BLL.BaseManage;

namespace Yme.Mcp.BLL.ReportManage
{
    /// <summary>
    /// 报表业务
    /// </summary>
    public class ReportBLL
    {
        #region 私有变量

        private IOrderDaystatisService orderDayServ = new OrderDaystatisService();
        private IPlatformorderDaystatisService porderDayServ = new PlatformorderDaystatisService();
        private IPlatformdishDaystatisService dishDayServ = new PlatformdishDaystatisService();
        private ICustomerDaystatisService customerDayServ = new CustomerDaystatisService();

        private decimal defaultRatio = 999999.00m;
        private const string noExists = "--";
        #endregion

        #region 查询报表

        /// <summary>
        /// 查询订单日报（业绩日报）
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="queryDate">查询日期</param>
        /// <returns>业绩日报数据</returns>
        public object GetOrderDayReport(long shopId, string queryDate)
        {
            var entity = orderDayServ.GetEntity(shopId, queryDate);
            var rptData = new
            {
                CompleteTotal = entity != null ? entity.CompleteTotal : 0,
                CompleteAmount = entity != null ? string.Format("{0:N2}", entity.CompleteAmount) : "0.00",
                CompleteTotalRatio = entity != null ? (entity.CompleteTotalRatio != defaultRatio ? (entity.CompleteTotalRatio * 100).ToString() : noExists) : noExists,
                CompleteAmountRatio = entity != null ? (entity.CompleteAmountRatio != defaultRatio ? (entity.CompleteAmountRatio * 100).ToString() : noExists) : noExists,
                CancelTotal = entity != null ? entity.CancelTotal : 0,
                CancelAmount = entity != null ? string.Format("{0:N2}", entity.CancelAmount) : "0.00",
                CancelTotalRatio = entity != null ? (entity.CancelTotalRatio != defaultRatio ? (entity.CancelTotalRatio * 100).ToString() : noExists) : noExists,
                CancelAmountRatio = entity != null ? (entity.CancelAmountRatio != defaultRatio ? (entity.CancelAmountRatio * 100).ToString() : noExists) : noExists
            };

            return rptData;
        }

        /// <summary>
        /// 查询平台订单日报（平台统计日报）
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="platformId">平台Id</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>平台统计日报数据</returns>
        public object GetPlatformOrderDayReport(long shopId, int platformId, string beginDate, string endDate)
        {
            var maxVal = 0;
            var dates = TimeUtil.GetDateList(beginDate.ToDate(), endDate.ToDate(), 3);
            var rptDic = new Dictionary<string, object>();
            var rptDataDic = new Dictionary<string, int[]>();
            var pList = SingleInstance<PlatformBLL>.Instance.GetShopPlatformList(shopId, BusinessType.Waimai);
            var list = porderDayServ.FindList(shopId, platformId, beginDate, endDate);
            if (list != null && list.Count > 0)
            {
                maxVal = Math.Ceiling(list.Max(d => d.OrderTotal).ToDecimal()).ToInt();
                if (pList != null && pList.Count > 0)
                {
                    pList.ForEach(p =>
                    {
                        var datas = new List<int>();
                        dates.ForEach(t =>
                        {
                            var rpt = list.Find(d => d.PlatformId == p.PlatformId && d.RptDate.ToMonthPointDayString() == t);
                            datas.Add(rpt != null ? rpt.OrderTotal : 0);
                        });

                        rptDataDic.Add(p.PlatformId.ToString(), datas.ToArray());
                    });
                }
            }

            //补充空数据
            if (rptDataDic.Count == 0)
            {
                var datas = new List<int>();
                for (var i = 0; i <= dates.Count; i++)
                {
                    datas.Add(0);
                }
                rptDataDic.Add(platformId.ToString(), datas.ToArray());
            }

            rptDic.Add("MaxVal", maxVal);
            rptDic.Add("Dates", dates.ToArray());
            rptDic.Add("PlatformData", rptDataDic);
            return rptDic;
        }

        /// <summary>
        /// 查询菜品报表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>菜品日报数据</returns>
        public object GetDishDayReport(long shopId, string beginDate, string endDate, int orderBy)
        {
            var dishTotal = 0;
            var dishes = new List<string>();
            var rptDic = new Dictionary<string, object>();
            var rptDataDic = new Dictionary<string, int[]>();
            var rptDishDic = new Dictionary<string, int>();
            var pList = SingleInstance<PlatformBLL>.Instance.GetShopPlatformList(shopId, BusinessType.Waimai);
            var list = dishDayServ.FindList(shopId, beginDate, endDate, orderBy);

            if (list != null && list.Count > 0)
            {
                dishes = (from d in list
                          group d by d.DishName.Replace("??", string.Empty) into dg
                          select dg.Key).ToList();

                if (pList != null && pList.Count > 0)
                {
                    pList.ForEach(p =>
                    {
                        var datas = new List<int>();
                        dishes.ForEach(t =>
                        {
                            var rpt = list.Find(d => d.PlatformId == p.PlatformId && d.DishName == t);
                            dishTotal = rpt != null ? rpt.DishTotal : 0;
                            datas.Add(dishTotal);

                            if (rptDishDic.ContainsKey(t))
                            {
                                rptDishDic[t] += dishTotal;
                            }
                            else
                            {
                                rptDishDic.Add(t, dishTotal);
                            }
                        });

                        rptDataDic.Add(p.PlatformId.ToString(), datas.ToArray());
                    });
                }
            }
            rptDic.Add("MaxVal", rptDishDic.Values.Count > 0 ? rptDishDic.Values.Max() : 0);
            rptDic.Add("SalesTotal", list.Sum(d => d.DishTotal));
            rptDic.Add("Dishes", dishes.ToArray());
            rptDic.Add("PlatformData", rptDataDic);
            return rptDic;
        }

        /// <summary>
        /// 查询客户报表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="queryDate">查询日期</param>
        /// <returns>客户日报数据</returns>
        public object GetCustomerDayReport(long shopId, string queryDate)
        {
            var entity = customerDayServ.GetEntity(shopId, queryDate);
            var rptData = new
            {
                CustomerTotal = entity != null ? entity.CustomerTotal : 0,
                TotalRelativeRatio = entity != null ? (entity.TotalRelativeRatio != defaultRatio ? (entity.TotalRelativeRatio * 100).ToString() : noExists) : noExists
            };

            return rptData;
        }

        #endregion

        #region 生成报表

        /// <summary>
        /// 生成订单日报（业绩日报）
        /// </summary>
        /// <param name="rptDate">报表日期</param>
        public void CreateOrderDayReport(DateTime rptDate)
        {
            var orderList = SingleInstance<OrderBLL>.Instance.GetOrderList(rptDate);
            var cpstatus = OrderStatus.Completed.GetHashCode();
            var clstatus = OrderStatus.Canceled.GetHashCode();
            var rptCurData = (from o in orderList
                              group o by o.ShopId into g
                              select new OrderDaystatisEntity
                              {
                                  ShopId = g.Key,
                                  RptDate = g.First().OrderTime.Date,
                                  OrderTotal = g.Count(),
                                  OrderAmount = g.Sum(d => d.PayAmount),
                                  CompleteTotal = g.Count(d => d.OrderStatus == cpstatus),
                                  CompleteAmount = g.Where(d => d.OrderStatus == cpstatus).Sum(d => d.PayAmount),
                                  CancelTotal = g.Count(d => d.OrderStatus == clstatus),
                                  CancelAmount = g.Where(d => d.OrderStatus == clstatus).Sum(d => d.PayAmount)
                              }).ToList();

            var preDate = rptDate.AddDays(-1).ToDateString();
            var rptPreData = orderDayServ.FindList(preDate);
            rptCurData.ForEach(d =>
                {
                    var pre = rptPreData.Where(t => t.ShopId == d.ShopId).FirstOrDefault();
                    if (pre != null)
                    {
                        d.CompleteTotalRatio = pre.CompleteTotal > 0 ? (d.CompleteTotal.ToDecimal() / pre.CompleteTotal.ToDecimal()) - 1 : defaultRatio;
                        d.CompleteAmountRatio = pre.CompleteAmount > 0 ? (d.CompleteAmount / pre.CompleteAmount) - 1 : defaultRatio;
                        d.CancelTotalRatio = pre.CancelTotal > 0 ? (d.CancelTotal.ToDecimal() / pre.CancelTotal.ToDecimal()) - 1 : defaultRatio;
                        d.CancelAmountRatio = pre.CancelAmount > 0 ? (d.CancelAmount / pre.CancelAmount) - 1 : defaultRatio;
                    }
                    else
                    {
                        d.CompleteTotalRatio = defaultRatio;
                        d.CompleteAmountRatio = defaultRatio;
                        d.CancelTotalRatio = defaultRatio;
                        d.CancelAmountRatio = defaultRatio;
                    }
                });

            orderDayServ.InsertEntity(rptCurData);
        }

        /// <summary>
        /// 生成平台订单日报（平台统计日报）
        /// </summary>
        /// <param name="rptDate">报表日期</param>
        public void CreatePlatformOrderDayReport(DateTime rptDate)
        {
            var orderList = SingleInstance<OrderBLL>.Instance.GetOrderList(rptDate);
            var rptData = (from d in orderList
                           where d.OrderStatus == OrderStatus.Completed.GetHashCode()
                           group d by new { ShopId = d.ShopId, PlatformId = d.PlatformId } into g
                           select new PlatformorderDaystatisEntity
                              {
                                  ShopId = g.Key.ShopId,
                                  PlatformId = g.Key.PlatformId,
                                  RptDate = g.First().OrderTime.Date,
                                  OrderTotal = g.Count()
                              }).ToList();

            porderDayServ.InsertEntity(rptData);
        }

        /// <summary>
        /// 生成菜品日报
        /// </summary>
        /// <param name="rptDate">报表日期</param>
        public void CreateDishDayReport(DateTime rptDate)
        {
            var orderDishList = SingleInstance<OrderBLL>.Instance.GetOrderDishList(rptDate);
            var rptData = (from d in orderDishList
                           group d by new { ShopId = d.ShopId, PlatformId = d.PlatformId, DishName = d.DishName.Replace("??", string.Empty) } into g
                           select new PlatformdishDaystatisEntity
                           {
                               ShopId = g.Key.ShopId,
                               PlatformId = g.Key.PlatformId,
                               RptDate = g.First().OrderTime.Date,
                               DishName = g.Key.DishName,
                               DishTotal = g.Sum(d => d.DishNum).ToInt(),
                           }).ToList();

            dishDayServ.InsertEntity(rptData);
        }

        /// <summary>
        /// 生成客户日报
        /// </summary>
        /// <param name="rptDate">报表日期</param>
        public void CreateCustomerDayReport(DateTime rptDate)
        {
            var preDate = rptDate.AddDays(-1).Date;
            var rptCurData = customerDayServ.GetShopDayCustomerTotal(rptDate);
            var rptPreData = customerDayServ.FindList(preDate);
            rptCurData.ForEach(d =>
            {
                d.RptDate = rptDate;
                var pre = rptPreData.Where(t => t.ShopId == d.ShopId).FirstOrDefault();
                if (pre != null)
                {
                    d.TotalRelativeRatio = pre.CustomerTotal > 0 ? (d.CustomerTotal.ToDecimal() / pre.CustomerTotal.ToDecimal()) - 1 : defaultRatio;
                }
                else
                {
                    d.TotalRelativeRatio = defaultRatio;
                }
            });

            customerDayServ.InsertEntity(rptCurData);
        }

        #endregion
    }
}
