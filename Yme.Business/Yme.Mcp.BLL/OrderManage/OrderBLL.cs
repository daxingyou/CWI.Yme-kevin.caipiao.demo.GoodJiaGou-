using System;
using System.Collections.Generic;

using Yme.Util;
using Yme.Util.Extension;
using Yme.Util.WebControl;
using Yme.Mcp.Service.Common;
using Yme.Mcp.Model.QueryModels;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Entity.OrderManage;
using Yme.Mcp.Service.OrderManage;
using Evt.Framework.Common;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.BLL.WeChatManage;
using Yme.Mcp.Model.McpApi;
using Yme.Mcp.Model.Waimai.Meituan;
using System.Linq;
using Yme.Util.Security;
using Yme.Util.Log;
using Yme.Mcp.Model.WeChat;
using System.Threading;
using Yme.Mcp.Model.ViewModels.Waimai.Eleme;
using Yme.Cache;
using Yme.Cache.Factory;
using Yme.Mcp.Model.ViewModels.Waimai.Baidu;
using Yme.Mcp.BLL.SystemManage;
using Yme.Mcp.Entity.BaseManage;

namespace Yme.Mcp.BLL.OrderManage
{
    /// <summary>
    /// 订单业务
    /// </summary>
    public class OrderBLL
    {
        #region 私有变量

        private static ICache cache = CacheFactory.Cache();
        private static readonly object _syncObject = new object();
        private IOrderService orderServ = new OrderService();
        private IOrderDetailService orderDetailServ = new OrderDetailService();
        private IOrderDishService orderDishServ = new OrderDishService();
        private IOrderInvoiceService orderInvServ = new OrderInvoiceService();
        private MeituanService meiServ = SingleInstance<MeituanService>.Instance;
        private ElemeService eleServ = SingleInstance<ElemeService>.Instance;
        private BaiduwmService baiServ = SingleInstance<BaiduwmService>.Instance;

        /// <summary>
        /// 尝试延长执行间隔，单位：毫秒
        /// </summary>
        private const int delayTryInterval = 3000;

        /// <summary>
        /// 失败后尝试次数
        /// </summary>
        private const int tryMax = 5;

        #endregion

        #region 公有方法

        /// <summary>
        /// 获取今日订单列表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public object GetDayOrderList(long shopId, int orderStatus, Pagination pagination)
        {
            return GetPageList(OrderQueryType.DayOrders, pagination, shopId, orderStatus);
        }

        /// <summary>
        /// 获取预订单列表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public object GetPreOrderList(long shopId, Pagination pagination)
        {
            return GetPageList(OrderQueryType.PreOrders, pagination, shopId);
        }

        /// <summary>
        /// 获取补打订单列表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public object GetPrintOrderList(long shopId, Pagination pagination)
        {
            return GetPageList(OrderQueryType.PrintOrders, pagination, shopId);
        }

        /// <summary>
        /// 获取搜索订单列表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="searchType">搜索类型</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="printStatus">打印状态</param>
        /// <param name="platformId">平台Id</param>
        /// <param name="keyWords">关键字</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public object GetSearchOrderList(long shopId, int searchType, int orderStatus, int printStatus, int platformId, string keyWords, Pagination pagination)
        {
            return GetPageList(OrderQueryType.SearchOrders, pagination, shopId, orderStatus, printStatus, searchType, platformId, keyWords);
        }

        /// <summary>
        /// 通用获取订单列表
        /// </summary>
        /// <param name="queryType">查询类型</param>
        /// <param name="pagination">分页参数</param>
        /// <param name="shopId">门店Id</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="printStatus">打印状态</param>
        /// <param name="searchType">搜索类型</param>
        /// <param name="platformId">平台Id</param>
        /// <param name="keyWords">关键字</param>
        /// <returns></returns>
        private object GetPageList(OrderQueryType queryType, Pagination pagination, long shopId, int orderStatus = 0, int printStatus = 0, int searchType = 0, int platformId = 0, string keyWords = "")
        {
            var curDate = DateTime.Now.Date;
            searchType = (searchType == 0 ? queryType.GetHashCode() : searchType);
            var statusStatis = (searchType == OrderQueryType.DayOrders.GetHashCode()) ? GetOrderStatsStatis(shopId, curDate) : null;
            var parms = new Dictionary<string, object>();
            parms.Add("ShopId", shopId);
            parms.Add("BeginDate", (searchType == OrderQueryType.DayOrders.GetHashCode()) ? curDate.GetDayBegin() : curDate.AddDays(-6).GetDayBegin());
            parms.Add("EndDate", curDate.GetDayEnd());
            parms.Add("SearchType", searchType);

            switch((int)queryType)
            {
                case (int)OrderQueryType.DayOrders:
                    {
                        //今日订单
                        parms.Add("OrderStatus", orderStatus);
                        break;
                    }
                case (int)OrderQueryType.SearchOrders:
                    {
                        //查询订单
                        parms.Add("OrderStatus", orderStatus);
                        parms.Add("PlatformId", platformId);
                        parms.Add("PrintStatus", printStatus);
                        parms.Add("KeyWords", keyWords);
                        break;
                    }
                case (int)OrderQueryType.PreOrders:
                case (int)OrderQueryType.PrintOrders:
                default:
                    {
                        //预订单、补打订单及默认
                        break;
                    }
            }

            var orderList = orderServ.FindPageList(Yme.Util.JsonUtil.ToJson(parms), pagination, queryType);
            var orders = new List<object>();
            if (orderList != null && orderList.Count > 0)
            {
                var shopAuth = SingleInstance<PlatformBLL>.Instance.GetShopAuthPlatformList(shopId, BusinessType.Waimai);
                orderList.ForEach(d =>
                {
                    var phone = d.RecipientPhone;
                    var spAuth = shopAuth != null ? shopAuth.Find(o => o.PlatformId == d.PlatformId) : null;
                    if (d.RecipientPhone.StartsWith("95") && d.RecipientPhone.Length == 13)
                    {
                        phone = "匿名用户";
                    }
                    orders.Add(new
                    {
                        MorderId = d.MorderId,
                        OrderId = d.OrderViewId,
                        OrderKey = DESEncryptUtil.Encrypt(d.Id.ToString()),
                        PlatformId = d.PlatformId,
                        DaySeq = d.DaySeq,
                        OrderType = d.OrderType,
                        CustomerName = d.RecipientName,
                        Phone = phone,
                        OrderStatus = (d.OrderStatus <= OrderStatus.Deliverying.GetHashCode() && d.OrderStatus >= OrderStatus.Confirmed.GetHashCode()) ? OrderStatus.Deliverying.GetHashCode() : d.OrderStatus,
                        PrintStatus = (d.PrintStatus == PrintStatus.TimeOutUnPrint.GetHashCode()) ? PrintStatus.PrintFail.GetHashCode() : d.PrintStatus,
                        PrintDesc = (d.PrintStatus == PrintStatus.TimeOutUnPrint.GetHashCode()) ? PrintStatus.PrintFail.GetRemark() : ((PrintStatus)d.PrintStatus).GetRemark(),
                        PrintTotal = d.PrintTotal,
                        OrderTime = d.OrderTime.ToMonthDayTimeString(),
                        IsShowConfirm = (d.OrderStatus == OrderStatus.WaitConfirm.GetHashCode() && (spAuth != null ? spAuth.IsAutoConfirm == 0 : false)) ? 1 : 0
                    });
                });
            }

            var orderDic = new Dictionary<string, object>();
            if (statusStatis != null)
            {
                //今日订单
                orderDic.Add("DoingTotal", statusStatis != null ? statusStatis.DoingTotal : 0);
                orderDic.Add("CompletedTotal", statusStatis != null ? statusStatis.CompletedTotal : 0);
                orderDic.Add("CanceledTotal", statusStatis != null ? statusStatis.CanceledTotal : 0);
            }
            orderDic.Add("PageCount", pagination != null ? pagination.total : 0);
            orderDic.Add("TotalRecords", pagination != null ? pagination.records : 0);
            orderDic.Add("Orders", orders);
            return orderDic;
        }

        /// <summary>
        /// 获取订单详细
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetOrderDetail(long shopId, string key)
        {
            //订单主体
            var mOrderId = DESEncryptUtil.Decrypt(key).ToInt();
            var order = orderServ.GetEntity(mOrderId);
            if (order == null)
            {
                throw new MessageException("订单不存在！");
            }

            //订单详细
            var detail = orderDetailServ.GetEntity(order.MorderId);
            var invoice = orderInvServ.GetEntity(order.MorderId);
            var invoiceInfo = invoice != null ? (string.Format("{0}{1}", invoice.InvoiceHeader, !string.IsNullOrWhiteSpace(invoice.TaxPayerId) ? "【" + invoice.TaxPayerId + "】" : string.Empty)) : string.Empty;
            var groupItems = GetOrderDetails(order.PlatformId, detail != null ? detail.Detail : string.Empty);
            var spAuth = SingleInstance<PlatformBLL>.Instance.GetShopAuthPlatformList(order.ShopId, BusinessType.Waimai, order.PlatformId).FirstOrDefault();
            var orderDic = new Dictionary<string, object>();
            orderDic.Add("IsLogin", shopId == 0 ? 0 : 1);
            orderDic.Add("OrderPlatform", ((PlatformType)order.PlatformId).GetRemark());
            orderDic.Add("DaySeq", order.DaySeq);
            orderDic.Add("OrderId", order.OrderViewId);
            orderDic.Add("OrderTime", order.OrderTime.ToDateTimeString());
            orderDic.Add("OrderDetail", groupItems);
            orderDic.Add("OtherFee", GetOrderFeeDetail(order, detail));
            orderDic.Add("Delivery", GetOrderDelivery(order));
            orderDic.Add("Remark", GetOrderRemark(order, invoiceInfo));
            orderDic.Add("MorderId", order.MorderId);
            orderDic.Add("OrderType", order.OrderType);
            orderDic.Add("OrderStatus", (order.OrderStatus <= OrderStatus.Deliverying.GetHashCode() && order.OrderStatus >= OrderStatus.Confirmed.GetHashCode()) ? OrderStatus.Deliverying.GetHashCode() : order.OrderStatus);
            orderDic.Add("PrintStatus", (order.PrintStatus == PrintStatus.TimeOutUnPrint.GetHashCode()) ? PrintStatus.PrintFail.GetHashCode() : order.PrintStatus);
            orderDic.Add("OrderDesc", ((order.OrderStatus <= OrderStatus.Deliverying.GetHashCode() && order.OrderStatus >= OrderStatus.Confirmed.GetHashCode()) ? OrderStatus.Deliverying.GetRemark() : ((OrderStatus)order.OrderStatus).GetRemark()));
            orderDic.Add("PrintDesc", ((order.PrintStatus == PrintStatus.TimeOutUnPrint.GetHashCode()) ? PrintStatus.PrintFail.GetRemark() : ((PrintStatus)order.PrintStatus).GetRemark()));
            orderDic.Add("PrintTotal", order.PrintTotal);
            orderDic.Add("PrintTime", order.PrintTime != null ? order.PrintTime.ToDateTimeString() : string.Empty);
            orderDic.Add("CompleteTime", order.CompleteTime != null ? order.CompleteTime.ToDateTimeString() : string.Empty);
            orderDic.Add("CancelTime", order.CancelTime != null ? order.CancelTime.ToDateTimeString() : string.Empty);
            orderDic.Add("CancelReason", order.CancelReason != null ? order.CancelReason : string.Empty);
            orderDic.Add("IsShowConfirm", (order.OrderStatus == OrderStatus.WaitConfirm.GetHashCode() && (spAuth != null ? spAuth.IsAutoConfirm == 0 : false)) ? 1 : 0);

            return orderDic;
        }

        /// <summary>
        /// 获取订单详细页面Url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetOrderDetailUrl(long id)
        {
            var url = "http://{0}/html/oi.html?k={1}";
            return string.Format(url, ConfigUtil.McpOrderServerIp, DESEncryptUtil.Encrypt(id.ToString()));
        }

        /// <summary>
        /// 获取订单详细页面Url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetOrderDetailUrl(PlatformType platformType, string orderId)
        {
            var order = orderServ.GetEntity(platformType, orderId);
            if (order != null)
            {
                return GetOrderDetailUrl(order.Id);
            }
            else
            {
                return string.Empty;
            }
        }

        #region 基础

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="mOrderId"></param>
        /// <returns></returns>
        public OrderEntity GetOrder(string mOrderId)
        {
            return orderServ.GetEntity(mOrderId);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="platformId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderEntity GetOrder(int platformId, string orderId)
        {
            return orderServ.GetEntity((PlatformType)platformId, orderId);
        }

        /// <summary>
        /// 获取订单详细信息
        /// </summary>
        /// <param name="mOrderId"></param>
        /// <returns></returns>
        public OrderDetailEntity GetDetail(string mOrderId)
        {
            return orderDetailServ.GetEntity(mOrderId);
        }

        /// <summary>
        /// 获取订单发票信息
        /// </summary>
        /// <param name="mOrderId"></param>
        /// <returns></returns>
        public OrderInvoiceEntity GetOrderInvoice(string mOrderId)
        {
            return orderInvServ.GetEntity(mOrderId);
        }

        /// <summary>
        /// 获取查询日期订单列表
        /// </summary>
        /// <param name="orderDate"></param>
        /// <returns></returns>
        public List<OrderEntity> GetOrderList(DateTime orderDate)
        {
            return orderServ.FindList(orderDate);
        }

        /// <summary>
        /// 获取订单菜品列表
        /// </summary>
        /// <param name="orderDate"></param>
        /// <returns></returns>
        public List<OrderDishEntity> GetOrderDishList(DateTime orderDate)
        {
            return orderDishServ.FindList(orderDate);
        }

        /// <summary>
        /// 校验门店中是否存在订单
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public bool CheckShopIsExistsOrder(long shopId)
        {
            return orderServ.CheckShopIsExistsOrder(shopId);
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="mOrderId"></param>
        /// <param name="status"></param>
        /// <param name="printResultCode"></param>
        /// <returns></returns>
        public int UpdateOrderStatus(string mOrderId, OrderStatus status)
        {
            return orderServ.UpdateOrderStatus(mOrderId, status);
        }

        /// <summary>
        /// 更新订单打印状态
        /// </summary>
        /// <param name="mOrderId"></param>
        /// <param name="status"></param>
        /// <param name="printResultCode"></param>
        /// <returns></returns>
        public int UpdateOrderPrintStatus(string mOrderId, PrintStatus status, string printResultCode = "")
        {
            return orderServ.UpdateOrderPrintStatus(mOrderId, status, printResultCode);
        }

        /// <summary>
        /// 定时更新订单状态
        /// </summary>
        public void UpdateOrderStatusTask(int mins)
        {
            //强制更新订单完成状态
            orderServ.ForceUpdateOrderStatus(mins);

            //待完成订单列表
            var orders = orderServ.FindWaitCompleteOrderList();

            //待查询平台订单状态列表
            var dicts = HandleQueryPlatformOrder(orders);

            //更新订单状态【完成或取消】
            orderServ.UpdateOrderStatus(dicts);
        }

        /// <summary>
        /// 预定单定时提醒
        /// </summary>
        /// <param name="maxMins"></param>
        /// <param name="interval"></param>
        public void ExecPreOrderRemindTask(int maxMins, int interval)
        {
            //待提醒订单列表
            var orders = orderServ.FindWaitRemindPreOrderList(maxMins + interval);

            //发送预订单提醒通知
            var id = 0;
            var url = string.Empty;
            var res = string.Empty;
            var openId = string.Empty;
            var msgModel = new RemindPreOrderTempModel();
            var wechatToken = SingleInstance<WeChatBLL>.Instance.GetWeiXinToken(EnumWeChatType.Client.GetHashCode());
            foreach (var order in orders)
            {
                openId = SingleInstance<WeChatBLL>.Instance.GetWechatOpenId(EnumWeChatType.Client.GetHashCode(), order.ShopId.ToString());
                if (!string.IsNullOrWhiteSpace(openId))
                {
                    id = order.Id == 0 ? GetOrder(order.MorderId).Id : order.Id;
                    url = GetOrderDetailUrl(id);
                    msgModel = SingleInstance<WeChatBLL>.Instance.GetRemindOrderMsgTemplateModel(((PlatformType)order.PlatformId).GetRemark(), order.OrderViewId, order.OrderTime, order.DaySeq, maxMins);
                    res = SingleInstance<WeChatBLL>.Instance.SendTemplateMsg<RemindPreOrderTempModel>(wechatToken, openId, "#FF0000", url, msgModel);
                    LogUtil.Info(string.Format("发送预定单：{0},提醒通知：{1}。", order.OrderViewId, res));

                    //更新提醒状态为已提醒
                    orderServ.UpdateOrderRemindFlag(order.MorderId, 1);
                    LogUtil.Info(string.Format("更新订单提醒标识,订单号：{0}。", order.OrderViewId));
                }
            }
        }

        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取门点查询日期状态统计信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="orderDate"></param>
        /// <returns></returns>
        private OrderStatisQueryModel GetOrderStatsStatis(long shopId, DateTime orderDate)
        {
            List<OrderEntity> dList = null;
            List<OrderEntity> cList = null;
            List<OrderEntity> qList = null;
            var orderList = orderServ.FindList(orderDate, shopId);
            if (orderList != null && orderList.Count > 0)
            {
                //进行中订单
                dList = orderList.FindAll(d => d.OrderStatus == OrderStatus.Confirmed.GetHashCode() || d.OrderStatus == OrderStatus.Deliverying.GetHashCode());
                //已完成订单
                cList = orderList.FindAll(d => d.OrderStatus == OrderStatus.Completed.GetHashCode());
                //已取消订单
                qList = orderList.FindAll(d => d.OrderStatus == OrderStatus.Canceled.GetHashCode());
            }

            var statisModel = new OrderStatisQueryModel()
            {
                DoingTotal = dList != null ? dList.Count : 0,
                CompletedTotal = cList != null ? cList.Count : 0,
                CanceledTotal = qList != null ? qList.Count : 0
            };

            return statisModel;
        }

        /// <summary>
        /// 获取指定门店查询日期订单列表
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="orderDate"></param>
        /// <returns></returns>
        private List<OrderEntity> GetOrderList(long shopId, DateTime orderDate)
        {
            return orderServ.FindList(orderDate, shopId);
        }

        /// <summary>
        /// 获取平台订单明细列表
        /// </summary>
        /// <param name="platformId"></param>
        /// <param name="detailJson"></param>
        /// <returns></returns>
        public List<OrderGroupQueryModel> GetOrderDetails(int platformId, string detailJson)
        {
            var groupItems = new List<OrderGroupQueryModel>();
            var details = new List<OrderDetailQueryModel>();
            switch (platformId)
            {
                case (int)PlatformType.Meituan:
                    {
                        details = meiServ.AnalysisOrderDetail(detailJson);
                        break;
                    }
                case (int)PlatformType.Eleme:
                    {
                        details = eleServ.AnalysisOrderDetail(detailJson);
                        break;
                    }
                case (int)PlatformType.Baidu:
                    {
                        details = baiServ.AnalysisOrderDetail(detailJson);
                        break;
                    }
                default:
                    break;
            }

            if (details != null)
            {
                groupItems = (from o in details 
                              group o by o.GroupId into g
                              select new OrderGroupQueryModel
                              {
                                  GroupId = g.Key.ToString(),
                                  Items = (from i in details
                                           where i.GroupId == g.Key
                                           select new OrderGroupItemQueryModel
                                           {
                                               Name = string.Format("{0}{1}", i.Name, i.Name.IndexOf((i.Property.Replace("(",string.Empty).Replace(")",string.Empty))) > 1 ? string.Empty : (i.Property ?? string.Empty)),
                                               Qty = i.Qty,
                                               Price = i.Price.ToString("#0.00")
                                           }).ToList()
                              }).ToList();
            }
            return groupItems;
        }

        /// <summary>
        /// 获取平台订单费用明细
        /// </summary>
        /// <param name="order"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        public OrderFeeQueryModel GetOrderFeeDetail(OrderEntity order, OrderDetailEntity detail)
        {
            var model = new OrderFeeQueryModel();
            var feeDetail = new List<OrderDetailQueryModel>();
            model.PayDesc = order.PayType == 2 ? "已在线支付" : "货到付款";
            model.OrderAmount = order.OrderAmount.ToString("#0.00");
            model.PayAmount = order.PayAmount.ToString("#0.00");
            model.FeeDetail = HandlePlaformOrderFeeDetail(order.PlatformId, detail, order.DeliveryFee);
            return model;
        }

        /// <summary>
        /// 获取订单备注信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetOrderRemark(OrderEntity order, string invoiceInfo)
        {
            var remarkDic = new Dictionary<string, object>();
            remarkDic.Add("DeliveryTime", order.DeliveryTime);
            remarkDic.Add("DinnersNum", order.DinnersNum);
            remarkDic.Add("Caution", order.Caution ?? string.Empty);
            remarkDic.Add("InvoiceInfo", invoiceInfo);
            return remarkDic;
        }

        /// <summary>
        /// 获取订单配送信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetOrderDelivery(OrderEntity order)
        {
            var phone = order.RecipientPhone;
            if (order.RecipientPhone.StartsWith("95") && order.RecipientPhone.Length == 13)
            {
                phone = "匿名用户";
            }
            else
            {
                phone = order.RecipientPhone.SplitMoblie();
            }
            var deliveryDic = new Dictionary<string, object>();
            deliveryDic.Add("DeliveryService", order.DeliveryService);
            deliveryDic.Add("RecipientAddress", order.RecipientAddress);
            deliveryDic.Add("RecipientName", order.RecipientName);
            deliveryDic.Add("RecipientPhone", phone);
            return deliveryDic;
        }

        /// <summary>
        /// 获取处理后的打印订单Id
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="mOrderId"></param>
        /// <param name="copies"></param>
        /// <returns></returns>
        private string GetPrintTaskOrderId(int billId, string mOrderId, int copies)
        {
            //格式：小票类型ID#微云打订单ID_份数，如：3#64f8d392535c42aab010307594f39855_2
            return string.Format("{0}#{1}_{2}", billId, mOrderId, copies);
        }

        #endregion

        #region 集成平台

        #region 平台订单处理

        /// <summary>
        /// 处理接收平台订单
        /// </summary>
        /// <param name="platformId">平台Id</param>
        /// <param name="callBackString">回调字符串</param>
        /// <returns>处理结果</returns>
        public bool HandleReceivePlatformOrder(long platformId, string callBackString, string requestId = "")
        {
            var isHandle = false;
            switch (platformId)
            {
                case (int)PlatformType.Meituan:
                    {
                        isHandle = HandleReceiveMeituanOrder(callBackString);
                        break;
                    }
                case (int)PlatformType.Eleme:
                    {
                        isHandle = HandleReceiveElemeOrder(callBackString, requestId);
                        break;
                    }
                case (int)PlatformType.Baidu:
                    {
                        isHandle = HandleReceiveBaiduOrder(callBackString, requestId);
                        break;
                    }
                default:
                    break;
            }
            return isHandle;
        }

        /// <summary>
        /// 处理取消平台订单
        /// </summary>
        /// <param name="platformId">平台Id</param>
        /// <param name="callBackString">回调字符串</param>
        /// <returns>处理结果</returns>
        public bool HandleCancelPlatformOrder(long platformId, string callBackString, string requestId = "")
        {
            var isHandle = false;
            switch (platformId)
            {
                case (int)PlatformType.Meituan:
                    {
                        isHandle = HandleCancelMeituanOrder(callBackString);
                        break;
                    }
                case (int)PlatformType.Eleme:
                    {
                        isHandle = HandleCancelElemeOrder(callBackString, requestId);
                        break;
                    }
                case (int)PlatformType.Baidu:
                    {
                        isHandle = HandleCancelBaiduOrder(callBackString, requestId);
                        break;
                    }
                default:
                    break;
            }
            return isHandle;
        }

        /// <summary>
        /// 处理完成平台订单
        /// </summary>
        /// <param name="platformId">平台Id</param>
        /// <param name="callBackString">回调字符串</param>
        /// <returns>处理结果</returns>
        public bool HandleCompletePlatformOrder(long platformId, string callBackString, string requestId = "")
        {
            var isHandle = false;
            switch (platformId)
            {
                case (int)PlatformType.Meituan:
                    {
                        isHandle = HandleCompleteMeituanOrder(callBackString);
                        break;
                    }
                case (int)PlatformType.Eleme:
                    {
                        isHandle = HandleCompleteElemeOrder(callBackString, requestId);
                        break;
                    }
                case (int)PlatformType.Baidu:
                    {
                        isHandle = HandleCompleteBaiduOrder(callBackString, requestId);
                        break;
                    }
                default:
                    break;
            }
            return isHandle;
        }

        /// <summary>
        /// 处理配送平台订单
        /// </summary>
        /// <param name="platformId">平台Id</param>
        /// <param name="callBackString">回调字符串</param>
        /// <returns>处理结果</returns>
        public bool HandleDeliveryPlatformOrder(long platformId, string callBackString, string requestId = "")
        {
            var isHandle = false;
            switch (platformId)
            {
                case (int)PlatformType.Meituan:
                    {
                        isHandle = HandleDeliveryMeituanOrder(callBackString);
                        break;
                    }
                case (int)PlatformType.Eleme:
                    {
                        isHandle = HandleDeliveryElemeOrder(callBackString, requestId);
                        break;
                    }
                case (int)PlatformType.Baidu:
                    {
                        isHandle = HandleDeliveryBaiduOrder(callBackString, requestId);
                        break;
                    }
                default:
                    break;
            }
            return isHandle;
        }

        /// <summary>
        /// 处理平台订单费用明细
        /// </summary>
        /// <param name="platformId">平台Id</param>
        /// <param name="detail">订单明细</param>
        /// <param name="deliveryFee">配送费</param>
        /// <returns></returns>
        public List<OrderGroupItemQueryModel> HandlePlaformOrderFeeDetail(long platformId, OrderDetailEntity detail, decimal deliveryFee)
        {
            var detailJson = string.Empty;
            var activeJson = string.Empty;
            var feeDetails = new List<OrderGroupItemQueryModel>();
            switch (platformId)
            {
                case (int)PlatformType.Meituan:
                    {
                        //1.餐盒费用
                        detailJson = detail != null ? detail.Detail.Replace("??", string.Empty) : string.Empty;
                        feeDetails = GetMeituanOrderFeeDetails(detailJson);

                        //2.优惠信息
                        activeJson = detail != null ? detail.Extras1.Replace("??", string.Empty) : string.Empty;
                        feeDetails.AddRange(GetMeituanOrderActivesDetails(activeJson));
                        break;
                    }
                case (int)PlatformType.Eleme:
                    {
                        //1.餐盒费用
                        detailJson = detail != null ? detail.Detail.Replace("??", string.Empty) : string.Empty;
                        feeDetails = GetElemeOrderFeeDetails(detailJson);

                        //2.优惠信息
                        activeJson = detail != null ? detail.Extras1.Replace("??", string.Empty) : string.Empty;
                        var hongbao = detail != null ? detail.Extras3 : string.Empty;
                        feeDetails.AddRange(GetElemeOrderActivesDetails(activeJson, hongbao));
                        break;
                    }
                case (int)PlatformType.Baidu:
                    {
                        //1.餐盒费用
                        detailJson = detail != null ? detail.Detail.Replace("??", string.Empty) : string.Empty;
                        feeDetails = GetBaiduOrderFeeDetails(detailJson);

                        //2.优惠信息
                        activeJson = detail != null ? detail.Extras1.Replace("??", string.Empty) : string.Empty;
                        feeDetails.AddRange(GetBaiduOrderActivesDetails(activeJson));

                        break;
                    }
                default:
                    break;
            }

            //3.配送费
            feeDetails.Add(new OrderGroupItemQueryModel()
            {
                Name = "配送费",
                Qty = string.Empty,
                Price = deliveryFee.ToString("#0.00")
            });
            return feeDetails;
        }

        /// <summary>
        /// 处理平台待查询订单
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public Dictionary<int, string> HandleQueryPlatformOrder(List<OrderEntity> orders)
        {
            var dics = new Dictionary<int, string>();
            if (orders == null && orders.Count <= 0)
            {
                return dics;
            }

            //外卖平台列表
            var status = OrderStatus.Completed.GetHashCode();
            var platForms = PlatformType.Meituan.List();
            var spList = SingleInstance<PlatformBLL>.Instance.FindShopPlatformList(BusinessType.Waimai);
            foreach (var platForm in platForms)
            {
                switch (platForm)
                {
                    case PlatformType.Meituan:
                        {
                            var meiOrders = orders.FindAll(d => d.PlatformId == platForm.GetHashCode());
                            if (meiOrders != null && meiOrders.Count > 0)
                            {
                                foreach (var m in meiOrders)
                                {
                                    var sp = spList.Find(d => d.ShopId == m.ShopId && d.PlatformId == m.PlatformId);
                                    if (sp != null && !string.IsNullOrWhiteSpace(sp.AuthToken))
                                    {
                                        var morder = meiServ.QueryOrder(m.OrderId, sp.AuthToken);
                                        if (morder != null && (morder.status == MeiStatusCode.Completed.GetHashCode() || morder.status == MeiStatusCode.Canceled.GetHashCode()))
                                        {
                                            if (morder.status == MeiStatusCode.Completed.GetHashCode())
                                            {
                                                status = OrderStatus.Completed.GetHashCode();
                                            }
                                            else if (morder.status == MeiStatusCode.Canceled.GetHashCode())
                                            {
                                                status = OrderStatus.Canceled.GetHashCode();
                                            }

                                            if (dics.Keys.Contains(status))
                                            {
                                                dics[status] += string.Format(",'{0}'", morder.orderId);
                                            }
                                            else
                                            {
                                                dics.Add(status, string.Format("'{0}'", morder.orderId));
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case PlatformType.Eleme:
                        {
                            break;
                        }
                    case PlatformType.Baidu:
                        {
                            break;
                        }
                    default:
                        break;
                }
            }
            return dics;
        }

        /// <summary>
        /// 生成并打印小票
        /// </summary>
        /// <param name="order"></param>
        /// <param name="billCategory"></param>
        public void PrintSmallBill(OrderEntity order, bool isCancel = false, bool isAutoPrint = true, List<string> billIds = null)
        {
            lock (_syncObject)
            {
                //1.生成订单小票
                var billPrintTasks = SingleInstance<BillTemplateBLL>.Instance.GetSmallBillUrlList(order, billIds);
                if (billPrintTasks.Count <= 0)
                {
                    var msg = "未配置相关打印场景";
                    LogUtil.Warn(string.Format("门店ID：{0},{1},订单ID：{2}未打印", order.ShopId, msg, order.OrderViewId));

                    //更新订单打印状态为：打印失败
                    SyncOrderPrintStatusData(order.MorderId, PrintStatus.PrintFail, PrintResultType.UnConfig.GetHashCode().ToString());

                    //发送未配置打印场景通知
                    SingleInstance<MsgBLL>.Instance.SendPrintFailPushMsg(order, "未配置相关打印场景");
                }
                else
                {
                    //2.发送打印任务
                    DoPrintOrder(order, billPrintTasks, isAutoPrint);
                }
            }
        }

        /// <summary>
        /// 打印取消小票
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="isAutoPrint">是否自动打印</param>
        public void PrintCancelSmallBill(OrderEntity order, bool isAutoPrint = true)
        {
            var billIds = new List<string>();
            billIds.Add(SmallBillType.Cancel.GetHashCode().ToString());

            PrintSmallBill(order, true, isAutoPrint, billIds);
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="morderId">微云打平台订单Id</param>
        /// <param name="shopId">门店Id</param>
        /// <param name="errMsg">错误消息</param>
        /// <returns></returns>
        public bool ConfirmOrder(string morderId, long shopId, out string errMsg)
        {
            //1.1.校验订单信息及权限
            errMsg = string.Empty;
            var isConfirm = false;
            var order = GetOrder(morderId);
            if (order == null)
            {
                errMsg = "此订单不存在！";
                return isConfirm;
            }
            else
            {
                if (order.ShopId != shopId)
                {
                    errMsg = "您无权操作此订单！";
                    return isConfirm;
                }
            }

            //1.2.授权信息校验
            var platformName = ((PlatformType)order.PlatformId).GetRemark();
            var shopAuth = SingleInstance<PlatformBLL>.Instance.GetShopPlatformInfo(order.ShopId, order.PlatformId, BusinessType.Waimai.GetHashCode(), AuthBussinessType.Waimai);
            if (shopAuth == null || string.IsNullOrWhiteSpace(shopAuth.AuthToken))
            {
                errMsg = string.Format("未授权【{0}】平台,无法接单！", platformName);
                return isConfirm;
            }
            else
            {
                //2.执行接单
                switch (order.PlatformId)
                {
                    case (int)PlatformType.Meituan:
                        {
                            //美团外卖订单
                            var confirmMsg = meiServ.ConfirmOrder(order.OrderId.ToLong(), shopAuth.AuthToken);
                            isConfirm = string.IsNullOrWhiteSpace(confirmMsg);
                            errMsg = !string.IsNullOrWhiteSpace(confirmMsg) ? confirmMsg : string.Empty;
                            LogUtil.Error(string.Format("手动确认【{0}】订单失败,订单ID：{1}【{2}】,参考信息：{3}", platformName, order.OrderId, order.OrderViewId, errMsg));

                            if (!string.IsNullOrWhiteSpace(errMsg))
                            {
                                errMsg = "接单失败！";
                                return isConfirm;
                            }
                            break;
                        }
                    case (int)PlatformType.Eleme:
                        {
                            //饿了么订单
                            var token = SingleInstance<PlatformBLL>.Instance.GetEleShopAccessToken(shopAuth);
                            var confirmMsg = eleServ.ConfirmOrder(order.OrderId, token);
                            if (confirmMsg != null && confirmMsg.error == null)
                            {
                                isConfirm = true;
                            }
                            else
                            {
                                isConfirm = false;
                                if (confirmMsg.error != null)
                                {
                                    var error = Yme.Util.JsonUtil.ToObject<ElemeApiRetErrorModel>(confirmMsg.error.ToString());
                                    errMsg = error != null ? error.message.ToString() : string.Empty;
                                }

                                LogUtil.Error(string.Format("手动确认【{0}】订单失败,订单ID：{1},参考信息：{2}", platformName, order.OrderViewId, errMsg));

                                errMsg = "接单失败！";
                                return isConfirm;
                            }
                            break;
                        }
                    case (int)PlatformType.Baidu:
                        {
                            //百度外卖订单
                            if (string.IsNullOrWhiteSpace(shopAuth.RefreshToken) || string.IsNullOrWhiteSpace(shopAuth.PlatformShopId))
                            {
                                errMsg = string.Format("未授权【{0}】平台,无法接单", platformName);
                                return isConfirm;
                            }
                            else
                            {
                                var confirmMsg = baiServ.ConfirmOrder(order.OrderId, shopAuth.RefreshToken, shopAuth.AuthToken);
                                if (confirmMsg != null && confirmMsg.errno == 0)
                                {
                                    isConfirm = true;
                                }
                                else
                                {
                                    LogUtil.Error(string.Format("手动确认【{0}】订单失败,订单ID：{1},参考信息：{2}", platformName, order.OrderViewId, errMsg));

                                    isConfirm = false;
                                    errMsg = "接单失败！";
                                    return isConfirm;
                                }
                            }
                            break;
                        }
                    default:
                        {
                            errMsg = "未知平台订单！";
                            isConfirm = false;
                            break;
                        }
                }
            }

            //3.已确认接单：更新订单状态、生成并打印小票
            if (isConfirm)
            {
                Action ac = new Action(() =>
                {
                    //4.更新平台订单状态
                    UpdateOrderStatus(order, OrderStatus.Confirmed);

                    //5.生成并打印小票
                    PrintSmallBill(order);

                });
                ac.BeginInvoke(null, null);
            }
            return isConfirm;
        }

        /// <summary>
        /// 执行订单打印状态数据同步
        /// </summary>
        /// <param name="mOrderId"></param>
        /// <param name="status"></param>
        /// <param name="printResultCode"></param>
        public bool SyncOrderPrintStatusData(string mOrderId, PrintStatus status, string printResultCode)
        {
            var handleResult = false;
            var errMsg = string.Empty;
            for (var t = 1; t <= tryMax; t++)
            {
                try
                {
                    handleResult = SyncOrderPrintStatusData(mOrderId, status, printResultCode, t);
                }
                catch (Exception ex)
                {
                    handleResult = false;
                    errMsg = string.Format("更新订单打印状态失败,微云打订单ID:{0},参考信息：{1}", mOrderId, ex.Message);
                    LogUtil.Error(errMsg);
                }

                if (handleResult)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(delayTryInterval);
                    continue;
                }
            }
            return handleResult;
        }
        
        #region 私有方法

        /// <summary>
        /// （重/补）打印订单
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="billPrintTasks">打印任务</param>
        /// <param name="isAutoPrint">是否自动打印</param>
        private void DoPrintOrder(OrderEntity order, List<BillPrintQueryModel> billPrintTasks, bool isAutoPrint = true)
        {
            //发送打印任务
            var isPrintOk = false;
            var platformName = ((PlatformType)order.PlatformId).GetRemark();
            var printToken = SingleInstance<PrinterBLL>.Instance.GetAccessToken();
            LogUtil.Info(string.Format("发送【{0}】打印命令,用户显示订单号：{1}", platformName, order.OrderViewId));

            var printTypeDesc = isAutoPrint ? "自动打印" : "重/补打印";
            foreach (var task in billPrintTasks)
            {
                var billModel = new BillViewModel()
                {
                    access_token = printToken,
                    merchant_code = order.ShopId.ToString(),
                    printer_codes = task.PrinterCodes,
                    bill_type = BillType.SmallBill.GetHashCode().ToString(),
                    bill_no = GetPrintTaskOrderId(task.BillId, order.MorderId, task.Copies),
                    bill_content = WebUtil.UrlEncode(task.BillUrl),
                    copies = task.Copies.ToString(),
                    template_id = string.Empty
                };
                var printErrorMsg = string.Empty;
                isPrintOk = SingleInstance<PrinterBLL>.Instance.DoPrint(billModel, out printErrorMsg);
                if (isPrintOk)
                {
                    LogUtil.Info(string.Format("{0}【{1}】订单成功,订单ID：{2},小票单号：{3}", printTypeDesc, platformName, order.OrderViewId, billModel.bill_no));
                }
                else
                {
                    LogUtil.Error(string.Format("{0}【{1}】订单失败,订单ID：{2},小票单号：{3},参考信息：{4}", printTypeDesc, platformName, order.OrderViewId, billModel.bill_no, printErrorMsg));
                }
            }
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="status">订单状态</param>
        private bool UpdateOrderStatus(OrderEntity order, OrderStatus status)
        {
            var isOk = false;
            try
            {
                //更新平台订单状态
                var updateStatus = orderServ.UpdateOrderStatus(order.MorderId, OrderStatus.Confirmed);
                isOk = updateStatus > 0;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("更新【{0}】订单状态失败,平台订单ID：{0},参考信息：{1}", ((PlatformType)order.PlatformId).GetRemark(), order.OrderViewId, ex.Message));
            }
            return isOk;
        }

        /// <summary>
        /// 保存订单
        /// </summary>
        /// <param name="platformType">平台类型</param>
        /// <param name="callBackString">回调字符串</param>
        /// <param name="platformOrder">平台订单实体</param>
        /// <returns>微云打平台订单信息</returns>
        private OrderEntity SaveOrder(PlatformType platformType, string callBackString, object platformOrder)
        {
            //保存订单【微云打订单系统】
            OrderEntity order = null;
            var platformName = platformType.GetRemark();
            try
            {
                switch ((int)platformType)
                {
                    case (int)PlatformType.Meituan:
                        {
                            order = orderServ.SaveOrder(((MeituanOrderModel)platformOrder), callBackString);
                            break;
                        }
                    case (int)PlatformType.Eleme:
                        {
                            order = orderServ.SaveOrder(((ElemeOrderModel)platformOrder), callBackString);
                            break;
                        }
                    case (int)PlatformType.Baidu:
                        {
                            order = orderServ.SaveOrder(((BaiduOrderModel)platformOrder), callBackString);
                            break;
                        }
                    default:
                        break;

                }

                if (order != null)
                {
                    LogUtil.Info(string.Format("【{0}】订单保存成功！", platformName));
                }
                else
                {
                    LogUtil.Info(string.Format("【{0}】订单保存失败,接收回调信息：{1}", platformName, callBackString));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Info(string.Format("【{0}】订单保存失败,接收回调信息：{1},错误信息：{2}", platformName, callBackString, ex.Message));
            }
            return order;
        }

        /// <summary>
        /// 解析平台订单
        /// </summary>
        /// <param name="platformType">平台类型</param>
        /// <param name="callBackString">回调字符串</param>
        /// <returns>平台订单信息</returns>
        private object AnalysisOrder(PlatformType platformType, string callBackString)
        {
            //解析平台订单
            object platformOrder = null;
            var platformName = platformType.GetRemark();
            try
            {
                switch ((int)platformType)
                {
                    case (int)PlatformType.Meituan:
                        {
                            platformOrder = meiServ.AnalysisOrder(callBackString);
                            break;
                        }
                    case (int)PlatformType.Eleme:
                        {
                            platformOrder = eleServ.AnalysisOrder(callBackString);
                            break;
                        }
                    case (int)PlatformType.Baidu:
                        {
                            platformOrder = baiServ.AnalysisOrder(callBackString);
                            break;
                        }
                    default:
                        break;

                }
                if (platformOrder != null)
                {
                    LogUtil.Info(string.Format("【{0}】订单回调解析成功！", platformName));
                }
                else
                {
                    LogUtil.Info(string.Format("【{0}】订单回调解析失败,接收回调信息：{1}", platformName, callBackString));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Info(string.Format("【{0}】订单回调解析失败,接收回调信息：{1},错误信息：{2}", platformName, callBackString, ex.Message));
            }
            return platformOrder;
        }

        /// <summary>
        /// 执行订单打印状态数据同步
        /// </summary>
        /// <param name="mOrderId"></param>
        /// <param name="status"></param>
        /// <param name="printResultCode"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool SyncOrderPrintStatusData(string mOrderId, PrintStatus status, string printResultCode, int t)
        {
            LogUtil.Info(string.Format("第{0}次尝试订单打印状态数据同步。", t));
            return SingleInstance<OrderBLL>.Instance.UpdateOrderPrintStatus(mOrderId, status, printResultCode) > 0;
        }

        #endregion

        #endregion

        #region 美团外卖

        /// <summary>
        /// 【美团外卖】新订单回调
        /// </summary>
        /// <param name="callBackString">新订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleReceiveMeituanOrder(string callBackString)
        {
            #region 回调处理【外部】

            var isConfrim = false;
            var isAutoConfirm = false;
            var handResult = false;
            var platformType = PlatformType.Meituan;

            //1.解析订单
            var platformOrder = (MeituanOrderModel)AnalysisOrder(platformType, callBackString);
            if (platformOrder == null)
            {
                handResult = false;
                return handResult;
            }

            //2.确认订单
            isConfrim = ConfirmOrder(platformOrder,out isAutoConfirm);

            #endregion

            #region 回调处理【内部】

            //3.保存订单【微云打订单系统】
            var order = SaveOrder(platformType, callBackString, platformOrder);
            if (order != null)
            {
                Action ac = new Action(() =>
                {
                    //已确认接单
                    var cacheValue = cache.GetCache<string>(order.MorderId);
                    if (isConfrim)
                    {
                        //4.更新平台订单状态
                        handResult = true;
                        handResult = UpdateOrderStatus(order, OrderStatus.Confirmed);
                        order.OrderStatus = OrderStatus.Confirmed.GetHashCode();

                        //5.发送提醒通知
                        if (string.IsNullOrWhiteSpace(cacheValue))
                        {
                            SingleInstance<MsgBLL>.Instance.SendNewOrderPushMsg(order);
                            cache.WriteCache("New", order.MorderId, DateTime.Now.AddMinutes(10));
                        }

                        //6.生成并打印小票
                        PrintSmallBill(order);
                    }
                    else
                    {
                        handResult = false;
                        if (string.IsNullOrWhiteSpace(cacheValue))
                        {
                            //未提醒过
                            if (isAutoConfirm)
                            {
                                //消息通知接单失败
                                SingleInstance<MsgBLL>.Instance.SendPrintFailPushMsg(order, "自动接单失败");
                            }
                            else
                            {
                                //消息提醒手动接单
                                SingleInstance<MsgBLL>.Instance.SendNewOrderPushMsg(order);
                            }
                            cache.WriteCache("New", order.MorderId, DateTime.Now.AddMinutes(10));
                        }
                    }
                });
                ac.BeginInvoke(null, null);
            }

            return handResult;

            #endregion
        }

        /// <summary>
        /// 【美团外卖】取消订单回调
        /// </summary>
        /// <param name="callBackString">取消订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleCancelMeituanOrder(string callBackString)
        {
            var handResult = false;
            try
            {
                MeituanOrderCancelModel model = null;
                if (!string.IsNullOrWhiteSpace(callBackString))
                {
                    model = Yme.Util.JsonUtil.ToObject<MeituanOrderCancelModel>(callBackString);
                }
                if (model == null)
                {
                    LogUtil.Error(string.Format("美团取消订单解析失败,取消订单回调信息：{0}", callBackString));
                    return handResult;
                }

                //0.校验订单当前状态
                var order = orderServ.GetEntity(PlatformType.Meituan, model.orderId.ToString());
                if (order.OrderStatus.GetHashCode() == OrderStatus.Canceled.GetHashCode())
                {
                    handResult = true;
                }
                else
                {
                    //1.更新业务数据
                    var remark = model.reason;  //string.Format("{0}【{1}】", model.reason, model.reasonCode);
                    handResult = orderServ.UpdateOrderStatus(PlatformType.Meituan, model.orderId.ToString(), OrderStatus.Canceled, remark) > 0;
                    LogUtil.Info(string.Format("同步取消美团订单{0},订单ID：{1}", handResult ? "成功" : "失败", model.orderId.ToString()));

                    if (handResult)
                    {
                        Action ac = new Action(() =>
                        {
                            order.OrderStatus = OrderStatus.Canceled.GetHashCode();

                            //2.发送取消提醒通知
                            SingleInstance<MsgBLL>.Instance.SendCancelOrderPushMsg(order, remark);

                            //3.生成并打印取消小票
                            PrintCancelSmallBill(order);
                        });
                        ac.BeginInvoke(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("美团订单取消回调失败,回调信息：{0},参考信息：{1}", callBackString, ex.Message));
            }
            return handResult;
        }

        /// <summary>
        /// 【美团外卖】完成订单回调
        /// </summary>
        /// <param name="callBackString">完成订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleCompleteMeituanOrder(string callBackString)
        {
            var handResult = false;
            try
            {
                var meituanOrder = meiServ.AnalysisOrder(callBackString);
                if (meituanOrder == null)
                {
                    LogUtil.Error(string.Format("美团订单完成回调解析失败,回调信息：{0}", callBackString));
                    return handResult;
                }

                handResult = orderServ.UpdateOrderStatus(PlatformType.Meituan, meituanOrder.orderId.ToString(), OrderStatus.Completed) > 0;
                if (handResult)
                {
                    LogUtil.Info(string.Format("美团订单完成回调成功,订单ID：{0}", meituanOrder.orderId.ToString()));
                }
                else
                {
                    LogUtil.Error(string.Format("美团订单完成回调失败,订单ID：{0}", meituanOrder.orderId.ToString()));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("美团订单完成回调失败,回调信息：{0},参考信息：{1}", callBackString, ex.Message));
            }
            return handResult;
        }

        /// <summary>
        /// 【美团外卖】配送订单回调
        /// </summary>
        /// <param name="callBackString">配送订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleDeliveryMeituanOrder(string callBackString)
        {
            var handResult = false;
            try
            {
                MeituanDeliveryStatusModel model = null;
                if (!string.IsNullOrWhiteSpace(callBackString))
                {
                    model = Yme.Util.JsonUtil.ToObject<MeituanDeliveryStatusModel>(callBackString);
                }
                if (model == null)
                {
                    LogUtil.Error(string.Format("美团订单配送回调解析失败，回调信息：{0}", callBackString));
                    return handResult;
                }

                if (model.shippingStatus > 0)
                {
                    handResult = orderServ.UpdateOrderStatus(PlatformType.Meituan, model.orderId.ToString(), OrderStatus.Deliverying) > 0;
                    if (handResult)
                    {
                        LogUtil.Info(string.Format("美团订单配送回调成功,订单ID：{0}", model.orderId.ToString()));
                    }
                    else
                    {
                        LogUtil.Error(string.Format("美团订单配送回调失败,订单ID：{0}", model.orderId.ToString()));
                    }
                }
                else
                {
                    handResult = true;
                    LogUtil.Info(string.Format("美团订单配送回调成功,本次回调未更新,订单ID：{0},回调信息：{0}", model.orderId.ToString(), callBackString));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("美团订单配送回调失败,回调信息：{0},参考信息：{1}", callBackString, ex.Message));
            }
            return handResult;
        }

        /// <summary>
        /// 【美团外卖】确认订单
        /// </summary>
        /// <param name="platformOrder"></param>
        /// <returns></returns>
        private bool ConfirmOrder(MeituanOrderModel platformOrder, out bool isAutoConfirm)
        {
            //确认订单
            isAutoConfirm = true;
            if (platformOrder == null)
                return false;

            var isOk = false;
            var pushMsg = string.Empty;
            var platformType = PlatformType.Meituan;
            var shopId = platformOrder.ePoiId.ToInt();
            var orderId = platformOrder.orderId;
            var orderViewId = platformOrder.orderIdView;
            var daySeq = platformOrder.daySeq;
            var orderTime = TimeUtil.GetDateTime(platformOrder.ctime);
            var shopAuth = SingleInstance<PlatformBLL>.Instance.GetShopPlatformInfo(shopId, platformType.GetHashCode(), BusinessType.Waimai.GetHashCode(), AuthBussinessType.Waimai);

            //消息通知未授权
            if (shopAuth == null || string.IsNullOrWhiteSpace(shopAuth.AuthToken))
            {
                isAutoConfirm = false;
                LogUtil.Error(string.Format("【{0}】门店未授权或权限校验失败,门店ID：{1}", platformType.GetRemark(), shopId));
                return isOk;
            }

            if (shopAuth.IsAutoConfirm.ToBool())
            {
                //已开启自动确认订单
                isAutoConfirm = true;
                var confirmMsg = meiServ.ConfirmOrder(orderId, shopAuth.AuthToken);
                if (!string.IsNullOrWhiteSpace(confirmMsg))
                {
                    isOk = false;
                    LogUtil.Error(string.Format("自动确认【{0}】订单失败,订单ID：{1},参考信息：{2}", platformType.GetRemark(), orderViewId, confirmMsg));
                }
                else
                {
                    isOk = true;
                }
            }
            else
            {
                //未开启自动确认
                isOk = false;
                isAutoConfirm = false;
                LogUtil.Error(string.Format("自动确认【{0}】订单失败,订单ID：{1},参考信息：{2}", platformType.GetRemark(), orderViewId, "门店该平台设置为非自动接单"));
            }
            return isOk;
        }

        /// <summary>
        /// 【美团外卖】费用明细
        /// </summary>
        /// <param name="detailJson">订单明细</param>
        /// <returns>费用明细</returns>
        private List<OrderGroupItemQueryModel> GetMeituanOrderFeeDetails(string detailJson)
        {
            //餐盒费用
            var feeDetail = new List<OrderGroupItemQueryModel>();
            if (!string.IsNullOrWhiteSpace(detailJson))
            {
                var detailList = Yme.Util.JsonUtil.ToObject<List<MeituanOrderDetailModel>>(detailJson);
                if (detailList != null && detailList.Count > 0)
                {
                    //var boxNum = detailList.Sum(d => d.box_num);
                    //餐盒总价
                    var boxFee = detailList.Sum(d => d.box_price * d.box_num);
                    if (boxFee > 0)
                    {
                        feeDetail.Add(new OrderGroupItemQueryModel
                        {
                            Name = "餐盒",
                            Qty = string.Empty,//string.Format("X{0}", boxNum),
                            Price = boxFee.ToString("#0.00")
                        });
                    }
                }
            }
            return feeDetail;
        }

        /// <summary>
        /// 【美团外卖】优惠费用明细
        /// </summary>
        /// <param name="activeJson">订单明细</param>
        /// <returns>优惠费用明细</returns>
        private List<OrderGroupItemQueryModel> GetMeituanOrderActivesDetails(string activeJson)
        {
            //优惠信息明细
            var feeDetail = new List<OrderGroupItemQueryModel>();
            if (!string.IsNullOrWhiteSpace(activeJson))
            {
                var feeList = Yme.Util.JsonUtil.ToObject<List<MeituanOrderExtrasModel>>(activeJson);
                if (feeList != null && feeList.Count > 0)
                {
                    feeList.ForEach(d =>
                    {
                        if (!string.IsNullOrWhiteSpace(d.remark))
                        {
                            feeDetail.Add(new OrderGroupItemQueryModel
                            {
                                Name = d.remark,
                                Qty = string.Format("X{0}", 1),
                                Price = string.Format("{0}{1}", (d.reduce_fee > 0 ? "-" : string.Empty), d.reduce_fee.ToString("#0.00"))
                            });
                        }
                    });
                }
            }
            return feeDetail;
        }

        #endregion

        #region 饿了么

        /// <summary>
        /// 【饿了么】新订单回调
        /// </summary>
        /// <param name="callBackString">新订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleReceiveElemeOrder(string callBackString, string requestId)
        {
            #region 回调处理【外部】

            var isConfrim = false;
            var isAutoConfirm = false;
            var handResult = false;
            var platformType = PlatformType.Eleme;

            //1.解析订单
            var platformOrder = (ElemeOrderModel)AnalysisOrder(platformType, callBackString);
            if (platformOrder == null)
            {
                handResult = false;
                return handResult;
            }

            //2.确认订单
            long mShopId = 0;
            isConfrim = ConfirmOrder(platformOrder, requestId, out mShopId, out isAutoConfirm);
            platformOrder.mShopId = mShopId;

            #endregion

            #region 回调处理【内部】

            //3.保存订单【微云打订单系统】
            var order = SaveOrder(platformType, callBackString, platformOrder);
            if (order != null)
            {
                Action ac = new Action(() =>
                {
                    //已确认接单
                    var cacheValue = cache.GetCache<string>(order.MorderId);
                    if (isConfrim)
                    {
                        handResult = true;
                        //4.更新平台订单状态
                        handResult = UpdateOrderStatus(order, OrderStatus.Confirmed);
                        order.OrderStatus = OrderStatus.Confirmed.GetHashCode();

                        //5.发送提醒通知
                        if (string.IsNullOrWhiteSpace(cacheValue))
                        {
                            SingleInstance<MsgBLL>.Instance.SendNewOrderPushMsg(order);
                            cache.WriteCache("New", order.MorderId, DateTime.Now.AddMinutes(10));
                        }

                        //6.生成并打印小票
                        PrintSmallBill(order);
                    }
                    else
                    {
                        handResult = false;
                        if (string.IsNullOrWhiteSpace(cacheValue))
                        {
                            //未提醒过
                            if (isAutoConfirm)
                            {
                                //消息通知接单失败
                                SingleInstance<MsgBLL>.Instance.SendPrintFailPushMsg(order, "系统接单失败");
                            }
                            else
                            {
                                //消息提醒手动接单
                                SingleInstance<MsgBLL>.Instance.SendNewOrderPushMsg(order);
                            }
                            cache.WriteCache("New", order.MorderId, DateTime.Now.AddMinutes(10));
                        }
                    }
                });
                ac.BeginInvoke(null, null);
            }

            return handResult;

            #endregion
        }

        /// <summary>
        /// 【饿了么】取消订单回调
        /// </summary>
        /// <param name="callBackString">取消订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleCancelElemeOrder(string callBackString, string requestId)
        {
            var handResult = false;
            try
            {
                ElemeOrderStatusModel model = null;
                if (!string.IsNullOrWhiteSpace(callBackString))
                {
                    model = Yme.Util.JsonUtil.ToObject<ElemeOrderStatusModel>(callBackString);
                }
                if (model == null)
                {
                    LogUtil.Error(string.Format("饿了么取消订单解析失败,取消订单回调信息：{0}", callBackString));
                    return handResult;
                }

                //0.校验订单当前状态
                var order = orderServ.GetEntity(PlatformType.Eleme, model.orderId.ToString());
                if (order.OrderStatus.GetHashCode() == OrderStatus.Canceled.GetHashCode())
                {
                    handResult = true;
                    cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                    LogUtil.Info(string.Format("饿了么取消订单推送消息requestId：{0}订单状态未改变视为处理成功,写入缓存", requestId));
                }
                else
                {
                    //1.更新业务数据
                    var remark = string.Format("{0}取消", ((ElemeRoleType)model.role).GetRemark());
                    handResult = orderServ.UpdateOrderStatus(PlatformType.Eleme, model.orderId.ToString(), OrderStatus.Canceled, remark) > 0;
                    LogUtil.Info(string.Format("同步取消饿了么订单{0},订单ID：{1}", handResult ? "成功" : "失败", model.orderId.ToString()));

                    if (handResult)
                    {
                        Action ac = new Action(() =>
                        {
                            order.OrderStatus = OrderStatus.Canceled.GetHashCode();
                            cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                            LogUtil.Info(string.Format("饿了么取消订单推送消息requestId：{0}处理成功,写入缓存", requestId));

                            //2.发送取消提醒通知
                            SingleInstance<MsgBLL>.Instance.SendCancelOrderPushMsg(order, remark);

                            //3.生成并打印取消小票
                            PrintCancelSmallBill(order);
                        });
                        ac.BeginInvoke(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("饿了么订单取消回调失败,回调信息：{0},参考信息：{1}", callBackString, ex.Message));
            }
            return handResult;
        }

        /// <summary>
        /// 【饿了么】完成订单回调
        /// </summary>
        /// <param name="callBackString">完成订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleCompleteElemeOrder(string callBackString, string requestId)
        {
            var handResult = false;
            try
            {
                var elemeOrder = Yme.Util.JsonUtil.ToObject<ElemeOrderStatusModel>(callBackString);
                if (elemeOrder == null)
                {
                    LogUtil.Error(string.Format("饿了么订单完成回调解析失败,回调信息：{0}", callBackString));
                    return handResult;
                }

                handResult = orderServ.UpdateOrderStatus(PlatformType.Eleme, elemeOrder.orderId.ToString(), OrderStatus.Completed) > 0;
                if (handResult)
                {
                    cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                    LogUtil.Info(string.Format("饿了么订单完成回调成功,订单ID：{0},requestId：{1}处理成功,写入缓存", elemeOrder.orderId.ToString(), requestId));
                }
                else
                {
                    LogUtil.Error(string.Format("饿了么订单完成回调失败,订单ID：{0}", elemeOrder.orderId.ToString()));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("饿了么订单完成回调失败,回调信息：{0},参考信息：{1}", callBackString, ex.Message));
            }
            return handResult;
        }

        /// <summary>
        /// 【饿了么】配送订单回调
        /// </summary>
        /// <param name="callBackString">配送订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleDeliveryElemeOrder(string callBackString, string requestId)
        {
            var handResult = false;
            try
            {
                ElemeDeliveryStatusModel model = null;
                if (!string.IsNullOrWhiteSpace(callBackString))
                {
                    model = Yme.Util.JsonUtil.ToObject<ElemeDeliveryStatusModel>(callBackString);
                }
                if (model == null)
                {
                    LogUtil.Error(string.Format("饿了么订单配送回调解析失败，回调信息：{0}", callBackString));
                    return handResult;
                }

                handResult = orderServ.UpdateOrderStatus(PlatformType.Eleme, model.orderId.ToString(), OrderStatus.Deliverying) > 0;
                if (handResult)
                {
                    cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                    LogUtil.Info(string.Format("饿了么订单配送回调成功,订单ID：{0}", model.orderId.ToString()));
                    LogUtil.Info(string.Format("饿了么取消配送推送消息requestId：{0}处理成功,写入缓存", requestId));
                }
                else
                {
                    LogUtil.Error(string.Format("饿了么订单配送回调失败,订单ID：{0}", model.orderId.ToString()));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("饿了么订单配送回调失败,回调信息：{0},参考信息：{1}", callBackString, ex.Message));
            }
            return handResult;
        }

        /// <summary>
        /// 【饿了么】确认订单
        /// </summary>
        /// <param name="platformOrder"></param>
        /// <returns></returns>
        private bool ConfirmOrder(ElemeOrderModel platformOrder, string requestId, out long mShopId, out bool isAutoConfirm)
        {
            //确认订单
            mShopId = 0;
            isAutoConfirm = true;
            if (platformOrder == null)
                return false;

            var isOk = false;
            var pushMsg = string.Empty;
            var platformType = PlatformType.Eleme;
            var shopId = platformOrder.shopId;
            var orderId = platformOrder.orderId;
            var orderViewId = platformOrder.orderId;
            var daySeq = platformOrder.daySn;
            var orderTime = platformOrder.createdAt ?? TimeUtil.Now;
            var shopAuth = SingleInstance<PlatformBLL>.Instance.GetShopPlatformInfo(platformType.GetHashCode(), shopId);
            if (shopAuth == null || string.IsNullOrWhiteSpace(shopAuth.AuthToken))
            {
                isAutoConfirm = false;
                LogUtil.Error(string.Format("【{0}】门店未授权或权限校验失败,门店ID：{1}", platformType.GetRemark(), shopId));
                return isOk;
            }
            else
            {
                isOk = true;
                mShopId = shopAuth.ShopId;
            }

            if (shopAuth.IsAutoConfirm.ToBool())
            {
                //已开启自动确认订单
                isAutoConfirm = true;
                var token = shopAuth.AuthToken;
                LogUtil.Info(string.Format("token有效期：{0},当前时间：{1}", shopAuth.ModifyDate.Value.AddSeconds(shopAuth.ExpiresIn), TimeUtil.Now));
                if (shopAuth.ModifyDate.Value.AddSeconds(shopAuth.ExpiresIn) < TimeUtil.Now)
                {
                    var rToken = eleServ.GetAccessTokenByRefreshToken(shopAuth.RefreshToken);
                    token = rToken != null ? rToken.access_token : string.Empty;
                    var refreshToken = rToken != null ? rToken.refresh_token : string.Empty;
                    LogUtil.Info(string.Format("{0}门店：{1}, accessToken：{2}已过期,通过refreshToken获取新accessToken：{3},RefreshToken：{4}", platformType.GetRemark(), shopAuth.ShopId, shopAuth.AuthToken, token, refreshToken));

                    //更新token到DB
                    shopAuth.AuthToken = token;
                    shopAuth.ExpiresIn = rToken.expires_in;
                    shopAuth.RefreshToken = refreshToken;
                    shopAuth.RefreshExpireTime = TimeUtil.Now.AddDays(ConfigUtil.EleReTokenExpiresDays);
                    shopAuth.ModifyDate = TimeUtil.Now;
                    SingleInstance<PlatformBLL>.Instance.UpdateEntity(shopAuth);
                }
                var confirmMsg = eleServ.ConfirmOrder(platformOrder.orderId, token);
                if (confirmMsg != null && confirmMsg.error == null)
                {
                    isOk = true;
                    cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                    LogUtil.Info(string.Format("【{0}】新订单推送消息requestId：{1}处理成功,写入缓存", platformType.GetRemark(), requestId));
                }
                else
                {
                    isOk = false;
                    var errMsg = string.Empty;
                    if (confirmMsg.error != null)
                    {
                        var error = Yme.Util.JsonUtil.ToObject<ElemeApiRetErrorModel>(confirmMsg.error.ToString());
                        errMsg = error != null ? error.message.ToString() : string.Empty;
                    }

                    LogUtil.Error(string.Format("自动确认【{0}】订单失败,订单ID：{1},参考信息：{2}", platformType.GetRemark(), orderViewId, errMsg));
                }
            }
            else
            {
                //未开启自动确认
                isOk = false;
                isAutoConfirm = false;
                LogUtil.Error(string.Format("自动确认【{0}】订单失败,订单ID：{1},参考信息：{2}", platformType.GetRemark(), orderViewId, "门店该平台设置为非自动接单"));
            }
            return isOk;
        }

        /// <summary>
        /// 【饿了么】费用明细
        /// </summary>
        /// <param name="detail">订单明细</param>
        /// <returns>费用明细</returns>
        private List<OrderGroupItemQueryModel> GetElemeOrderFeeDetails(string detailJson)
        {
            //餐盒费用
            var feeDetail = new List<OrderGroupItemQueryModel>();
            var mobject = Yme.Util.JsonUtil.ToObject<List<EleOrderGroupsModel>>(detailJson);
            if (mobject != null && mobject.Count > 0)
            {
                mobject.ForEach(d =>
                {
                    if (d.items != null && d.items.Count > 0)
                    {
                        foreach (var item in d.items)
                        {
                            if (d.type.ToLower() == "extra")
                            {
                                //总费用
                                var amount = (item.quantity ?? 1) * item.price;
                                feeDetail.Add(new OrderGroupItemQueryModel
                                {
                                    Name = item.name,
                                    Qty = string.Empty,//string.Format("X{0}", item.quantity),
                                    Price = amount.ToString("#0.00")
                                });
                            }
                        }
                    }
                });
            }
            return feeDetail;
        }

        /// <summary>
        /// 【饿了么】优惠费用明细
        /// </summary>
        /// <param name="detail">订单明细</param>
        /// <returns>优惠费用明细</returns>
        private List<OrderGroupItemQueryModel> GetElemeOrderActivesDetails(string activeJson, string hongbao)
        {
            //优惠信息明细
            var feeDetail = new List<OrderGroupItemQueryModel>();
            if (!string.IsNullOrWhiteSpace(activeJson))
            {
                var feeList = Yme.Util.JsonUtil.ToObject<List<EleOrderActiveModel>>(activeJson);
                if (feeList != null && feeList.Count > 0)
                {
                    feeList.ForEach(d =>
                    {
                        if (!string.IsNullOrWhiteSpace(d.name))
                        {
                            var amount = d.amount.HasValue ? d.amount.Value : 0.00d;
                            feeDetail.Add(new OrderGroupItemQueryModel
                            {
                                Name = d.name,
                                Qty = string.Format("X{0}", 1),
                                Price = string.Format("{0}{1}", (amount > 0 ? "-" : string.Empty), amount.ToString("#0.00"))
                            });
                        }
                    });
                }
            }
            if (hongbao.ToDecimal() != 0.0m)
            {
                feeDetail.Add(new OrderGroupItemQueryModel
                {
                    Name = "红包优惠",
                    Qty = string.Empty,
                    Price = string.Format("-{0}", hongbao.ToDecimal().ToString("#0.00"))
                });
            }
            return feeDetail;
        }

        #endregion

        #region 百度外卖

        /// <summary>
        /// 【百度外卖】新订单回调
        /// </summary>
        /// <param name="callBackString">新订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleReceiveBaiduOrder(string callBackString, string requestId)
        {
            #region 回调处理【外部】

            var isConfrim = false;
            var isAutoConfirm = false;
            var handResult = false;
            var platformType = PlatformType.Baidu;

            //1.解析订单
            var platformOrder = (BaiduOrderModel)AnalysisOrder(platformType, callBackString);
            if (platformOrder == null)
            {
                handResult = false;
                return handResult;
            }

            //2.确认订单
            long mShopId = 0;
            isConfrim = ConfirmOrder(platformOrder, requestId, out mShopId, out isAutoConfirm);
            platformOrder.shop.id = mShopId.ToString();

            #endregion

            #region 回调处理【内部】

            //3.保存订单【微云打订单系统】
            var order = SaveOrder(platformType, callBackString, platformOrder);
            if (order != null)
            {
                Action ac = new Action(() =>
                {
                    //已确认接单
                    var cacheValue = cache.GetCache<string>(order.MorderId);
                    if (isConfrim)
                    {
                        handResult = true;
                        //4.更新平台订单状态
                        handResult = UpdateOrderStatus(order, OrderStatus.Confirmed);
                        order.OrderStatus = OrderStatus.Confirmed.GetHashCode();

                        //5.发送提醒通知
                        if (string.IsNullOrWhiteSpace(cacheValue))
                        {
                            SingleInstance<MsgBLL>.Instance.SendNewOrderPushMsg(order);
                            cache.WriteCache("New", order.MorderId, DateTime.Now.AddMinutes(10));
                        }

                        //6.生成并打印小票
                        PrintSmallBill(order);
                    }
                    else
                    {
                        handResult = false;
                        if (string.IsNullOrWhiteSpace(cacheValue))
                        {
                            //未提醒过
                            if (isAutoConfirm)
                            {
                                //消息通知接单失败
                                SingleInstance<MsgBLL>.Instance.SendPrintFailPushMsg(order, "系统接单失败");
                            }
                            else
                            {
                                //消息提醒手动接单
                                SingleInstance<MsgBLL>.Instance.SendNewOrderPushMsg(order);
                            }
                            cache.WriteCache("New", order.MorderId, DateTime.Now.AddMinutes(10));
                        }
                    }
                });
                ac.BeginInvoke(null, null);
            }

            return handResult;
        }

        /// <summary>
        /// 【百度外卖】取消订单回调
        /// </summary>
        /// <param name="callBackString">取消订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleCancelBaiduOrder(string callBackString, string requestId)
        {
            var handResult = false;
            try
            {
                BaiduOrderStatusModel model = null;
                if (!string.IsNullOrWhiteSpace(callBackString))
                {
                    model = Yme.Util.JsonUtil.ToObject<BaiduOrderStatusModel>(callBackString);
                }
                if (model == null)
                {
                    LogUtil.Error(string.Format("百度外卖取消订单解析失败,回调信息：{0}", callBackString));
                    return handResult;
                }

                //0.校验订单当前状态
                var order = orderServ.GetEntity(PlatformType.Baidu, model.order_id.ToString());
                if (order.OrderStatus.GetHashCode() == OrderStatus.Canceled.GetHashCode())
                {
                    handResult = true;
                    cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                    LogUtil.Info(string.Format("百度外卖取消订单推送消息requestId：{0}订单状态未改变视为处理成功,写入缓存", requestId));
                }
                else
                {
                    //1.更新业务数据
                    var remark = model.reason;
                    handResult = orderServ.UpdateOrderStatus(PlatformType.Baidu, model.order_id.ToString(), OrderStatus.Canceled, remark) > 0;
                    LogUtil.Info(string.Format("同步取消百度外卖订单{0},订单ID：{1}", handResult ? "成功" : "失败", model.order_id.ToString()));

                    if (handResult)
                    {
                        Action ac = new Action(() =>
                        {
                            order.OrderStatus = OrderStatus.Canceled.GetHashCode();
                            cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                            LogUtil.Info(string.Format("百度外卖取消订单推送消息requestId：{0}处理成功,写入缓存", requestId));

                            //2.发送取消提醒通知
                            SingleInstance<MsgBLL>.Instance.SendCancelOrderPushMsg(order, remark);

                            //3.生成并打印取消小票
                            PrintCancelSmallBill(order);
                        });
                        ac.BeginInvoke(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("百度外卖订单取消回调失败,回调信息：{0},参考信息：{1}", callBackString, ex.Message));
            }
            return handResult;
        }

        /// <summary>
        /// 【百度外卖】完成订单回调
        /// </summary>
        /// <param name="callBackString">完成订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleCompleteBaiduOrder(string callBackString, string requestId)
        {
            var handResult = false;
            try
            {
                BaiduOrderStatusModel model = null;
                if (!string.IsNullOrWhiteSpace(callBackString))
                {
                    model = Yme.Util.JsonUtil.ToObject<BaiduOrderStatusModel>(callBackString);
                }
                if (model == null)
                {
                    LogUtil.Error(string.Format("百度外卖订单完成回调解析失败,回调信息：{0}", callBackString));
                    return handResult;
                }

                handResult = orderServ.UpdateOrderStatus(PlatformType.Baidu, model.order_id.ToString(), OrderStatus.Completed) > 0;
                if (handResult)
                {
                    cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                    LogUtil.Info(string.Format("百度外卖订单完成回调成功,订单ID：{0},requestId：{1}处理成功,写入缓存", model.order_id.ToString(), requestId));
                }
                else
                {
                    LogUtil.Error(string.Format("百度外卖订单完成回调失败,订单ID：{0}", model.order_id.ToString()));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("百度外卖订单完成回调失败,回调信息：{0},参考信息：{1}", callBackString, ex.Message));
            }
            return handResult;
        }

        /// <summary>
        /// 【百度外卖】配送订单回调
        /// </summary>
        /// <param name="callBackString">配送订单回调信息</param>
        /// <returns>处理结果</returns>
        private bool HandleDeliveryBaiduOrder(string callBackString, string requestId)
        {
            var handResult = false;
            try
            {
                BaiduOrderStatusModel model = null;
                if (!string.IsNullOrWhiteSpace(callBackString))
                {
                    model = Yme.Util.JsonUtil.ToObject<BaiduOrderStatusModel>(callBackString);
                }
                if (model == null)
                {
                    LogUtil.Error(string.Format("百度外卖订单配送回调解析失败,回调信息：{0}", callBackString));
                    return handResult;
                }

                handResult = orderServ.UpdateOrderStatus(PlatformType.Baidu, model.order_id.ToString(), OrderStatus.Deliverying) > 0;
                if (handResult)
                {
                    cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                    LogUtil.Info(string.Format("百度外卖订单配送回调成功,订单ID：{0},requestId：{1}处理成功,写入缓存", model.order_id.ToString(), requestId));
                }
                else
                {
                    LogUtil.Error(string.Format("百度外卖订单配送回调失败,订单ID：{0}", model.order_id.ToString()));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("百度外卖订单配送回调失败,回调信息：{0},参考信息：{1}", callBackString, ex.Message));
            }
            return handResult;
        }

        /// <summary>
        /// 【百度外卖】确认订单
        /// </summary>
        /// <param name="platformOrder"></param>
        /// <returns></returns>
        private bool ConfirmOrder(BaiduOrderModel platformOrder, string requestId, out long mShopId, out bool isAutoConfirm)
        {
            //确认订单
            mShopId = 0;
            isAutoConfirm = true;
            if (platformOrder == null)
                return false;

            var isOk = false;
            var pushMsg = string.Empty;
            var platformType = PlatformType.Baidu;
            var shopId = platformOrder.shop.baidu_shop_id;
            var orderId = platformOrder.order.order_id;
            var orderViewId = orderId;
            var daySeq = platformOrder.order.order_index.ToInt();
            var orderTime = TimeUtil.GetDateTime(platformOrder.order.create_time.ToInt());
            var shopAuth = SingleInstance<PlatformBLL>.Instance.GetShopPlatformInfo(platformType.GetHashCode(), shopId);
            if (shopAuth == null || string.IsNullOrWhiteSpace(shopAuth.RefreshToken) || string.IsNullOrWhiteSpace(shopAuth.AuthToken) || string.IsNullOrWhiteSpace(shopAuth.PlatformShopId))
            {
                isAutoConfirm = false;
                LogUtil.Error(string.Format("【{0}】门店未授权或权限校验失败,门店ID：{1}", platformType.GetRemark(), shopId));
                return false;
            }
            else
            {
                isOk = true;
                if (shopAuth != null && shopAuth.ShopId > 0)
                {
                    mShopId = shopAuth.ShopId;
                }
            }

            if (shopAuth.IsAutoConfirm.ToBool())
            {
                //已开启自动确认订单
                isAutoConfirm = true;
                var confirmMsg = baiServ.ConfirmOrder(orderId, shopAuth.RefreshToken, shopAuth.AuthToken);
                if (confirmMsg != null && confirmMsg.errno == 0)
                {
                    isOk = true;
                    cache.WriteCache("S", requestId, DateTime.Now.AddMinutes(10));
                    LogUtil.Info(string.Format("【{0}】新订单推送消息requestId：{1}处理成功,写入缓存", platformType.GetRemark(), requestId));
                }
                else
                {
                    isOk = false;
                    LogUtil.Error(string.Format("自动确认【{0}】订单失败,订单ID：{1},参考信息：{2}", platformType.GetRemark(), orderViewId, confirmMsg.error));
                }
            }
            else
            {
                //未开启自动确认
                isOk = false;
                isAutoConfirm = false;
                LogUtil.Error(string.Format("自动确认【{0}】订单失败,订单ID：{1},参考信息：{2}", platformType.GetRemark(), orderViewId, "门店该平台设置为非自动接单"));
            }
            return isOk;
        }

        /// <summary>
        /// 【百度外卖】费用明细
        /// </summary>
        /// <param name="detail">订单明细</param>
        /// <returns>费用明细</returns>
        private List<OrderGroupItemQueryModel> GetBaiduOrderFeeDetails(string detailJson)
        {
            //餐盒费用
            var packageFee = 0;
            var feeDetail = new List<OrderGroupItemQueryModel>();
            var mobject = Yme.Util.JsonUtil.ToObject<List<List<BaiduOrderProductDishModel>>>(detailJson.Replace("\r\n  ", string.Empty));
            if (mobject != null && mobject.Count > 0)
            {
                mobject.ForEach(d =>
                {
                    foreach (var item in d)
                    {
                        //本单餐盒总价
                        packageFee += item.package_fee;
                    }
                });

                feeDetail.Add(new OrderGroupItemQueryModel
                {
                    Name = "餐盒",
                    Qty = string.Empty,//string.Format("X{0}", item.package_amount),
                    Price = (packageFee.ToDecimal(2) / 100m).ToDecimal(2).ToString("#0.00")
                });
            }

            return feeDetail;
        }

        /// <summary>
        /// 【百度外卖】优惠费用明细
        /// </summary>
        /// <param name="detail">订单明细</param>
        /// <returns>优惠费用明细</returns>
        private List<OrderGroupItemQueryModel> GetBaiduOrderActivesDetails(string activeJson)
        {
            //优惠信息明细
            var feeDetail = new List<OrderGroupItemQueryModel>();
            if (!string.IsNullOrWhiteSpace(activeJson))
            {
                var feeList = Yme.Util.JsonUtil.ToObject<List<BaiduOrderDiscountModel>>(activeJson);
                if (feeList != null && feeList.Count > 0)
                {
                    feeList.ForEach(d =>
                    {
                        if (!string.IsNullOrWhiteSpace(d.desc))
                        {
                            var amount = (d.fee.ToDecimal(2) / 100m).ToDecimal();
                            feeDetail.Add(new OrderGroupItemQueryModel
                            {
                                Name = d.desc,
                                Qty = string.Format("X{0}", 1),
                                Price = string.Format("{0}{1}", (amount > 0 ? "-" : string.Empty), amount.ToString("#0.00"))
                            });
                        }
                    });
                }
            }
            return feeDetail;
        }

        #endregion

        #endregion

        #endregion

        #region 测试

        public object QueryMeiOrder(string orderId, string authToken)
        {
            var morder = meiServ.QueryOrder(orderId, authToken);
            return morder;
        }

        #endregion
    }
}