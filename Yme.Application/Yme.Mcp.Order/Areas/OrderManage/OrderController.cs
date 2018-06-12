using System;
using System.Web.Http;
using System.Collections.Generic;

using Yme.Util;
using Yme.Util.Log;
using Yme.Util.WebControl;
using Yme.Util.Extension;
using Yme.Mcp.Model;
using Yme.Mcp.BLL.OrderManage;
using Yme.Mcp.Order.Handel;
using Yme.Mcp.Model.QueryModels;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.Model.Enums;

namespace Yme.Mcp.Order.Controllers.Api
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    public class OrderController : ApiBaseController
    {
        /// <summary>
        /// 获取今日订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetList([FromUri]OrderLsitQueryModel model)
        {
            LogUtil.Info("执行获取今日订单列表请求...");

            var page = new Pagination();
            page.rows = model.PageSize;
            page.page = model.PageIndex;
            var result = SingleInstance<OrderBLL>.Instance.GetDayOrderList(base.CurrId, model.Status, page);
            return OK(result);
        }

        /// <summary>
        /// 获取预订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetPreList([FromUri]PageViewModel model)
        {
            LogUtil.Info("执行获取预订单列表请求...");

            var page = new Pagination();
            page.rows = model.PageSize;
            page.page = model.PageIndex;
            var result = SingleInstance<OrderBLL>.Instance.GetPreOrderList(base.CurrId, page);
            return OK(result);
        }

        /// <summary>
        /// 获取补打订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetPrintList([FromUri]PageViewModel model)
        {
            LogUtil.Info("执行获取补打订单列表请求...");

            var page = new Pagination();
            page.rows = model.PageSize;
            page.page = model.PageIndex;
            var result = SingleInstance<OrderBLL>.Instance.GetPrintOrderList(base.CurrId, page);
            return OK(result);
        }

        /// <summary>
        /// 综合搜索订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object DoSearch([FromUri]OrderSearchQueryModel model)
        {
            LogUtil.Info("执行综合搜索订单列表请求...");

            var page = new Pagination();
            page.rows = model.PageSize;
            page.page = model.PageIndex;
            var result = SingleInstance<OrderBLL>.Instance.GetSearchOrderList(base.CurrId, model.SearchType, model.OrderStatus, model.PrintStatus, model.PlatformId, model.KeyWords, page);
            return OK(result);
        }

        /// <summary>
        /// 获取打印设置
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpGet]
        public object GetPrintConfigs()
        {
            var isContainCancel = false;
            var requestForms = base.GetRequestParams(true, "执行获取补打/重打场景设置");
            var morderId = requestForms["MorderId"] != null ? requestForms["MorderId"].ToString().Trim() : string.Empty;
            if (string.IsNullOrEmpty(morderId))
            {
                return Failed("订单ID不可为空！");
            }
            else
            {
                var order = SingleInstance<OrderBLL>.Instance.GetOrder(morderId);
                if (order == null)
                {
                    return Failed("订单不存在！");
                }
                else
                {
                    isContainCancel = (order.OrderStatus == OrderStatus.Canceled.GetHashCode());
                    var result = SingleInstance<PrinterBLL>.Instance.GetShopPrintConfigList(order.ShopId, BusinessType.Waimai, isContainCancel);
                    return OK(result);
                }
            }
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpGet]
        public object GetDetail([FromUri]OrderViewModel model)
        {
            LogUtil.Info("执行获取订单详情请求...");

            var result = SingleInstance<OrderBLL>.Instance.GetOrderDetail(base.CurrId, model.k);
            return OK(result);
        }

        /// <summary>
        /// 补打/重打
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        public object DoRePrint()
        {
            var requestForms = base.GetRequestParams(true, "执行补打/重打请求");
            var morderId = requestForms["MorderId"] != null ? requestForms["MorderId"].ToString().Trim() : string.Empty;
            var billIds = requestForms["BillIds"] != null ? requestForms["BillIds"].ToString().Trim() : string.Empty;
            if (string.IsNullOrEmpty(billIds))
            {
                return Failed("请选择打印场景！");
            }
            if (string.IsNullOrEmpty(morderId))
            {
                return Failed("订单ID不可为空！");
            }
            else
            {
                var order = SingleInstance<OrderBLL>.Instance.GetOrder(morderId);
                if (order == null)
                {
                    return Failed("订单不存在！");
                }
                else
                {
                    var billIdList = new List<string>(billIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                    SingleInstance<OrderBLL>.Instance.PrintSmallBill(order, false, false, billIdList);
                    return OK();
                }
            }
        }

        /// <summary>
        /// 手动接单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public object DoConfirmOrder()
        {
            var requestForms = base.GetRequestParams(true, "执行手动接单请求");
            var morderId = requestForms["MorderId"] != null ? requestForms["MorderId"].ToString().Trim() : string.Empty;
            if (string.IsNullOrEmpty(morderId))
            {
                return Failed("订单ID不能为空！");
            }
            else
            {
                var errMsg = string.Empty;
                var isConfirm = SingleInstance<OrderBLL>.Instance.ConfirmOrder(morderId, base.CurrId, out errMsg);
                return isConfirm ? OK() : Failed(errMsg);
            }
        }
    }
}
