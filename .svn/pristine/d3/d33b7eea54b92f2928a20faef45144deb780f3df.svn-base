using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Yme.Data;
using Yme.Data.Repository;
using Yme.Mcp.Entity.OrderManage;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Model.ViewModels.Waimai.Baidu;
using Yme.Mcp.Model.ViewModels.Waimai.Eleme;
using Yme.Mcp.Model.Waimai.Meituan;
using Yme.Mcp.Service.Common;
using Yme.Util;
using Yme.Util.Exceptions;
using Yme.Util.Extension;
using Yme.Util.Log;
using Yme.Util.WebControl;

namespace Yme.Mcp.Service.OrderManage
{
    public class OrderService : RepositoryFactory<OrderEntity>, IOrderService
    {
        private MeituanService meiServ = SingleInstance<MeituanService>.Instance;
        private ElemeService eleServ = SingleInstance<ElemeService>.Instance;
        private BaiduwmService baiServ = SingleInstance<BaiduwmService>.Instance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public OrderEntity GetEntity(string entityId)
        {
            return this.BaseRepository().FindEntity(entityId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderEntity GetEntity(long id)
        {
            var sql = @"SELECT * FROM bll_order WHERE Id = @Id ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("Id", id));

            return this.BaseRepository().FindList(sql, parms.ToArray()).FirstOrDefault();
        }

        public OrderEntity GetEntity(PlatformType platformType, string orderId)
        {
            var sql = @"SELECT * FROM bll_order WHERE PlatformId = @platformType AND OrderId = @orderId ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("platformType", platformType.GetHashCode()));
            parms.Add(DbParameters.CreateDbParameter("orderId", orderId));

            return this.BaseRepository().FindList(sql, parms.ToArray()).FirstOrDefault();
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="orderDate">查询日期</param>
        /// <param name="shopId">门店Id</param>
        /// <returns></returns>
        public List<OrderEntity> FindList(DateTime orderDate, long shopId = 0)
        {
            List<OrderEntity> list = null;
            try
            {
                var fliterStr = string.Empty;
                var parms = new List<DbParameter>();
                parms.Add(DbParameters.CreateDbParameter("beginDate", orderDate.GetDayBegin()));
                parms.Add(DbParameters.CreateDbParameter("endDate", orderDate.GetDayEnd()));
                if (shopId > 0)
                {
                    fliterStr = "AND ShopId = @shopId ";
                    parms.Add(DbParameters.CreateDbParameter("shopId", shopId));
                }

                var sql = string.Format(@"SELECT * FROM bll_order WHERE orderTime >= @beginDate AND orderTime <= @endDate {0} ORDER BY shopId, orderTime", fliterStr);
                list = this.BaseRepository().FindList(sql, parms.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("查询门店日订单异常，参考信息：{0}", ex.Message));
            }
            return list;
        }

        /// <summary>
        /// 校验当前门店是否存在订单
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public bool CheckShopIsExistsOrder(long shopId)
        {
            var expression = Extensions.True<OrderEntity>();
            expression = expression.And(t => t.ShopId == shopId);
            return this.BaseRepository().IQueryable(expression).Count() > 0;
        }

        /// <summary>
        /// 获取订单列表分页
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<OrderEntity> FindPageList(string queryJson, Pagination pagination, OrderQueryType queryType)
        {
            List<OrderEntity> list = null;
            var sbFliter = new StringBuilder();
            var parms = new List<DbParameter>();
            var orderByColName = "orderTime";
            var OrderSort = "DESC";
            var qType = queryType.GetHashCode();

            try
            {
                var queryParam = queryJson.ToJObject();
                parms.Add(DbParameters.CreateDbParameter("shopId", Extensions.ToString(queryParam["ShopId"])));
                if (!queryParam["BeginDate"].IsEmpty())
                {
                    sbFliter.Append(" AND OrderTime >= @beginDate ");
                    sbFliter.Append(" AND OrderTime <= @endDate ");

                    parms.Add(DbParameters.CreateDbParameter("beginDate", queryParam["BeginDate"]));
                    parms.Add(DbParameters.CreateDbParameter("endDate", queryParam["EndDate"]));
                }

                switch (qType)
                {
                    case (int)OrderQueryType.DayOrders:
                        {
                            //今日订单
                            var statusCode = queryParam["OrderStatus"] != null ? queryParam["OrderStatus"].ToString() : string.Empty;
                            var status = !string.IsNullOrWhiteSpace(statusCode) ? Extensions.ToInt(statusCode) : OrderStatus.Deliverying.GetHashCode();
                            if (status == OrderStatus.Completed.GetHashCode() || status == OrderStatus.Canceled.GetHashCode())
                            {
                                //已完成或已取消状态
                                orderByColName = (status == OrderStatus.Completed.GetHashCode()) ? "CompleteTime" : "CancelTime";
                                sbFliter.Append(" AND OrderStatus = @status ");
                                parms.Add(DbParameters.CreateDbParameter("status", status));
                            }
                            else if (status == OrderStatus.Deliverying.GetHashCode())
                            {
                                //进行中状态包括：待接单、已接单、配送中
                                sbFliter.Append(" AND OrderStatus >= @beginStatus ");
                                sbFliter.Append(" AND OrderStatus <= @endStatus ");
                                parms.Add(DbParameters.CreateDbParameter("beginStatus", OrderStatus.WaitConfirm.GetHashCode()));
                                parms.Add(DbParameters.CreateDbParameter("endStatus", OrderStatus.Deliverying.GetHashCode()));
                            }
                            else
                            {
                                //非法状态返回空
                                sbFliter.Append(" AND OrderStatus = @status ");
                                parms.Add(DbParameters.CreateDbParameter("status", -1));
                            }
                            break;
                        }
                    case (int)OrderQueryType.PreOrders:
                        {
                            //预订单
                            OrderSort = "ASC";
                            orderByColName = "PreDeliveryTime";
                            sbFliter.Append(" AND OrderType = @orderType ");
                            sbFliter.Append(" AND OrderStatus >= @beginStatus ");
                            sbFliter.Append(" AND OrderStatus <= @endStatus ");
                            parms.Add(DbParameters.CreateDbParameter("orderType", OrderType.PreOrder.GetHashCode()));
                            parms.Add(DbParameters.CreateDbParameter("beginStatus", OrderStatus.WaitConfirm.GetHashCode()));
                            parms.Add(DbParameters.CreateDbParameter("endStatus", OrderStatus.Deliverying.GetHashCode()));
                            break;
                        }
                    case (int)OrderQueryType.PrintOrders:
                        {
                            //补打订单仅查询：打印失败或超时未打状态
                            orderByColName = "PrintTime";
                            sbFliter.Append(" AND PrintStatus >= @beginStatus ");
                            sbFliter.Append(" AND PrintStatus <= @endStatus ");
                            parms.Add(DbParameters.CreateDbParameter("beginStatus", PrintStatus.PrintFail.GetHashCode()));
                            parms.Add(DbParameters.CreateDbParameter("endStatus", PrintStatus.TimeOutUnPrint.GetHashCode()));
                            break;
                        }
                    case (int)OrderQueryType.SearchOrders:
                        {
                            //搜索订单
                            var platformId = Extensions.ToInt(queryParam["PlatformId"], 0);
                            var orderStatus = Extensions.ToInt(queryParam["OrderStatus"], 0);
                            var printStatus = Extensions.ToInt(queryParam["PrintStatus"], 0);
                            var searchType = Extensions.ToInt(queryParam["SearchType"], OrderQueryType.SearchOrders.GetHashCode());
                            var keywords = Extensions.ToString(queryParam["KeyWords"]);
                            if(searchType == OrderQueryType.PreOrders.GetHashCode())
                            {
                                //0.预订单搜索
                                sbFliter.Append(" AND OrderType = @orderType ");
                                parms.Add(DbParameters.CreateDbParameter("orderType", OrderType.PreOrder.GetHashCode()));
                            }
                            if (platformId > 0)
                            {
                                //1.平台过滤
                                sbFliter.Append(" AND PlatformId = @platformId ");
                                parms.Add(DbParameters.CreateDbParameter("platformId", platformId));
                            }
                            if (orderStatus > 0)
                            {
                                //2.订单状态
                                if (orderStatus == OrderStatus.Completed.GetHashCode() || orderStatus == OrderStatus.Canceled.GetHashCode())
                                {
                                    //已完成或已取消状态
                                    sbFliter.Append(" AND OrderStatus = @status ");
                                    parms.Add(DbParameters.CreateDbParameter("status", orderStatus));
                                }
                                else if (orderStatus == OrderStatus.Deliverying.GetHashCode())
                                {
                                    //进行中状态包括：待接单、已接单、配送中
                                    sbFliter.Append(" AND (OrderStatus >= @boStatus AND OrderStatus <= @eoStatus)");
                                    parms.Add(DbParameters.CreateDbParameter("boStatus", OrderStatus.WaitConfirm.GetHashCode()));
                                    parms.Add(DbParameters.CreateDbParameter("eoStatus", OrderStatus.Deliverying.GetHashCode()));
                                }
                                else
                                {
                                    //非法状态返回空
                                    sbFliter.Append(" AND OrderStatus = @status ");
                                    parms.Add(DbParameters.CreateDbParameter("status", -1));
                                }
                            }
                            if (printStatus > 0)
                            {
                                //3.打印状态
                                if (printStatus == PrintStatus.PrintFail.GetHashCode() || printStatus == PrintStatus.TimeOutUnPrint.GetHashCode())
                                {
                                    sbFliter.Append(" AND (PrintStatus = @bpStatus OR PrintStatus = @epStatus)");
                                    parms.Add(DbParameters.CreateDbParameter("bpStatus", PrintStatus.PrintFail.GetHashCode()));
                                    parms.Add(DbParameters.CreateDbParameter("epStatus", PrintStatus.TimeOutUnPrint.GetHashCode()));
                                }
                                else
                                {
                                    sbFliter.Append(" AND PrintStatus = @printStatus ");
                                    parms.Add(DbParameters.CreateDbParameter("printStatus", printStatus));
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(keywords))
                            {
                                //4.关键字左模糊匹配：订单号、平台日序号、手机号
                                sbFliter.Append(" AND (OrderViewId LIKE @keywords OR RecipientPhone LIKE @keywords OR DaySeq LIKE @keywords) ");
                                parms.Add(DbParameters.CreateDbParameter("keywords", keywords + "%"));
                            }

                            //搜索排序规则
                            orderByColName = (searchType == OrderQueryType.PreOrders.GetHashCode()) ? "PreDeliveryTime" : "orderTime";
                            break;
                        }
                    default:
                        break;
                }

                //获取记录总数
                var sqlcount = string.Format(@"SELECT COUNT(*) AS cnt FROM bll_order WHERE ShopId = @shopId {0} ", sbFliter.ToString());
                var objCnt = this.BaseRepository().FindObject(sqlcount, parms.ToArray());
                pagination.records = Extensions.ToInt(objCnt, 0);

                //获取分页数据
                if (pagination != null)
                {
                    parms.Add(DbParameters.CreateDbParameter("startIndex", (pagination.page - 1) * pagination.rows));
                    parms.Add(DbParameters.CreateDbParameter("endIndex", pagination.rows));
                }

                var sql = string.Format(@"SELECT * FROM bll_order WHERE ShopId = @shopId {0} ORDER BY {1} {2} LIMIT @startIndex, @endIndex ", sbFliter.ToString(), orderByColName, OrderSort);
                list = this.BaseRepository().FindList(sql, parms.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("订单查询异常，参考信息：{0}", ex.Message));
            }
            return list;
        }

        /// <summary>
        /// 保存订单【美团外卖】
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public OrderEntity SaveOrder(MeituanOrderModel orderModel, string responseString)
        {
            OrderEntity order = null;
            if (orderModel != null && !string.IsNullOrWhiteSpace(orderModel.orderId.ToString()))
            {
                //已保存订单直接返回
                order = GetEntity(PlatformType.Meituan, orderModel.orderId.ToString());
            }
            if (order == null || string.IsNullOrWhiteSpace(order.MorderId))
            {
                //首次回调保存订单
                OrderDetailEntity detail = null;
                OrderInvoiceEntity invoice = null;
                var db = new RepositoryFactory().BaseRepository().BeginTrans();
                try
                {
                    if (orderModel != null)
                    {
                        #region 1.订单主体

                        var address = orderModel.recipientAddress.Split(new string[] { "@#" }, StringSplitOptions.RemoveEmptyEntries);
                        var orderTime = TimeUtil.GetDateTime(orderModel.ctime);
                        var preDeliveryTime = orderModel.deliveryTime == 0 ? orderTime.AddSeconds(orderModel.avgSendTime) : TimeUtil.GetDateTime(orderModel.deliveryTime);
                        order = new OrderEntity()
                        {
                            MorderId = StringUtil.UniqueStr(),
                            OrderViewId = orderModel.orderIdView.ToString(),
                            OrderId = orderModel.orderId.ToString(),
                            OrderType = orderModel.isPre == 1 ? OrderType.PreOrder.GetHashCode() : (orderTime.AddMinutes(ConfigUtil.PreOrderInterval) < preDeliveryTime ? 2 : 1),
                            PlatformId = PlatformType.Meituan.GetHashCode(),
                            DaySeq = orderModel.daySeq,
                            BussinessType = BusinessType.Waimai.GetHashCode(),
                            ShopId = orderModel.ePoiId.ToInt(),
                            ShopName = orderModel.poiName,
                            RecipientName = orderModel.recipientName,
                            RecipientPhone = orderModel.recipientPhone,
                            RecipientAddress = address[0],
                            OrderTime = orderTime,
                            DeliveryTime = GetDeliveryTime(orderModel.deliveryTime, orderModel.ctime, orderModel.avgSendTime),
                            DinnersNum = orderModel.dinnersNumber > 0 ? orderModel.dinnersNumber : 1,
                            Caution = orderModel.caution.Replace("??", string.Empty),
                            DeliveryService = ((MeilogisticsCode)orderModel.logisticsCode).GetRemark(),
                            DeliveryFee = orderModel.shippingFee.ToDecimal(),
                            OrderAmount = orderModel.originalPrice.ToDecimal(),
                            PayAmount = orderModel.total.ToDecimal(),
                            PayType = orderModel.payType,
                            OrderStatus = OrderStatus.WaitConfirm.GetHashCode(),
                            PrintStatus = PrintStatus.WaitPrint.GetHashCode(),
                            OrderData = responseString.Replace("??", string.Empty),
                            IsPreOrder = orderTime.AddMinutes(ConfigUtil.PreOrderInterval) < preDeliveryTime ? 1 : 0,
                            PreDeliveryTime = preDeliveryTime,
                            IsRemind = 0,
                            CreateDate = TimeUtil.Now
                        };

                        #endregion

                        #region 2.订单详细

                        detail = new OrderDetailEntity()
                        {
                            MorderId = order.MorderId,
                            OrderId = order.OrderId,
                            PlatformId = order.PlatformId,
                            BussinessType = order.BussinessType,
                            ShopId = order.ShopId,
                            Detail = orderModel.detail.Replace("??", string.Empty),
                            PoiReceiveDetail = orderModel.poiReceiveDetail.Replace("??", string.Empty),
                            Extras1 = orderModel.extras,
                            ModifyDate = order.CreateDate
                        };

                        #endregion

                        #region 3.订单菜品

                        var dishTotal = 0.00m;
                        var maxGroupId = 1;
                        var orderDishList = new List<OrderDishEntity>();
                        var orderDishs = meiServ.AnalysisOrderDetail(orderModel.detail.Replace("??", string.Empty));
                        if (orderDishs != null && orderDishs.Count > 0)
                        {
                            orderDishs.ForEach(d =>
                            {
                                var dishNum = (d.Qty.Replace("X", string.Empty)).ToDecimal();
                                dishTotal += dishNum;
                                orderDishList.Add(new OrderDishEntity
                                {
                                    MorderId = order.MorderId,
                                    PlatformId = order.PlatformId,
                                    DishName = d.Name.Replace("??", string.Empty),
                                    DishProperty = d.Property != null ? d.Property.Replace("??", string.Empty) : string.Empty,
                                    DishNum = dishNum,
                                    DishPrice = d.Price.ToDecimal(),
                                    DishAmount = dishNum * d.Price.ToDecimal(),
                                    DishGroupId = d.GroupId,
                                    DishInnerId = d.InnerId,
                                    OrderTime = order.OrderTime,
                                    ShopId = order.ShopId
                                });

                                if (d.GroupId.HasValue && d.GroupId > maxGroupId)
                                {
                                    maxGroupId = d.GroupId.Value;
                                }
                            });
                        }

                        if (order != null)
                        {
                            if (order.DinnersNum <= 0)
                            {
                                order.DinnersNum = maxGroupId;
                            }
                            order.OrderItemTotal = dishTotal;
                        }

                        #endregion

                        #region 4.订单发票

                        if (orderModel.hasInvoiced == BoolType.Yes.GetHashCode())
                        {
                            invoice = new OrderInvoiceEntity()
                            {
                                MorderId = order.MorderId,
                                OrderId = order.OrderId,
                                InvoiceType = string.IsNullOrWhiteSpace(orderModel.taxpayerId) ? InvoiceType.Personal.GetHashCode() : InvoiceType.Company.GetHashCode(),
                                InvoiceHeader = orderModel.invoiceTitle ?? string.Empty,
                                TaxPayerId = orderModel.taxpayerId ?? string.Empty,
                                IsOpen = BoolType.No.GetHashCode(),
                                CreateDate = TimeUtil.Now
                            };
                        }

                        #endregion

                        db.Insert(order);
                        db.Insert(detail);
                        db.Insert(orderDishList);
                        if (invoice != null)
                        {
                            db.Insert(invoice);
                        }
                    }

                    db.Commit();
                }
                catch (Exception ex)
                {
                    db.Rollback();
                    LogUtil.Error(ex.StackTrace);
                    throw new MessageException("系统错误，保存美团订单失败！");
                }
            }

            return order;
        }

        /// <summary>
        /// 保存订单【饿了么】
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public OrderEntity SaveOrder(ElemeOrderModel orderModel, string responseString)
        {
             OrderEntity order = null;
             if (orderModel != null && !string.IsNullOrWhiteSpace(orderModel.id.ToString()))
             {
                 //已保存订单直接返回
                 order = GetEntity(PlatformType.Eleme, orderModel.id.ToString());
             }
             if (order == null || string.IsNullOrWhiteSpace(order.MorderId))
             {
                 //首次回调保存订单
                 OrderDetailEntity detail = null;
                 OrderInvoiceEntity invoice = null;
                 var db = new RepositoryFactory().BaseRepository().BeginTrans();
                 try
                 {
                     if (orderModel != null)
                     {
                         #region 1.订单主体

                         //40分钟平均送达时间
                         var avgSendTime = 2400;
                         var orderTime = orderModel.createdAt ?? TimeUtil.Now;
                         var preDeliveryTime = orderModel.deliverTime ?? orderTime.AddSeconds(avgSendTime);
                         order = new OrderEntity()
                         {
                             MorderId = StringUtil.UniqueStr(),
                             OrderViewId = orderModel.orderId.ToString(),
                             OrderId = orderModel.id.ToString(),
                             OrderType = orderModel.book ? OrderType.PreOrder.GetHashCode() : OrderType.ImmediatelyOrder.GetHashCode(),
                             PlatformId = PlatformType.Eleme.GetHashCode(),
                             DaySeq = orderModel.daySn,
                             BussinessType = BusinessType.Waimai.GetHashCode(),
                             ShopId = orderModel.mShopId,
                             ShopName = orderModel.shopName,
                             RecipientName = orderModel.consignee,
                             RecipientPhone = orderModel.phoneList.FirstOrDefault(),
                             RecipientAddress = orderModel.deliveryPoiAddress,
                             OrderTime = orderTime,
                             DeliveryTime = GetDeliveryTime(orderModel.deliverTime, orderModel.createdAt, avgSendTime),
                             DinnersNum = 0,
                             Caution = orderModel.description.Replace("??", string.Empty),
                             DeliveryService = "视商家选择",
                             DeliveryFee = orderModel.deliverFee.ToDecimal(),
                             OrderAmount = orderModel.originalPrice.ToDecimal(),
                             PayAmount = orderModel.totalPrice.ToDecimal(),
                             PayType = orderModel.onlinePaid ? 2 : 1,
                             OrderStatus = OrderStatus.WaitConfirm.GetHashCode(),
                             PrintStatus = PrintStatus.WaitPrint.GetHashCode(),
                             OrderData = responseString.Replace("??", string.Empty),
                             IsPreOrder = orderTime.AddMinutes(ConfigUtil.PreOrderInterval) < preDeliveryTime ? 1 : 0,
                             PreDeliveryTime = preDeliveryTime,
                             IsRemind = 0,
                             CreateDate = TimeUtil.Now
                         };

                         #endregion

                         #region 2.订单详细

                         detail = new OrderDetailEntity()
                         {
                             MorderId = order.MorderId,
                             OrderId = order.OrderId,
                             PlatformId = order.PlatformId,
                             BussinessType = order.BussinessType,
                             ShopId = order.ShopId,
                             Detail = orderModel.groups.ToString().Replace("??", string.Empty),
                             PoiReceiveDetail = string.Empty,
                             Extras1 = orderModel.orderActivities.ToString().Replace("??", string.Empty),
                             Extras2 = orderModel.packageFee.ToDecimal(2).ToString(),
                             Extras3 = orderModel.hongbao.ToDecimal(2).ToString(),
                             ModifyDate = order.CreateDate
                         };

                         #endregion

                         #region 3.订单菜品

                         var maxGroupId = 1;
                         var dishTotal = 0.00m;
                         var orderDishList = new List<OrderDishEntity>();
                         var orderDishs = eleServ.AnalysisOrderDetail(detail.Detail);
                         if (orderDishs != null && orderDishs.Count > 0)
                         {
                             orderDishs.ForEach(d =>
                             {
                                 var dishNum = (d.Qty.Replace("X", string.Empty)).ToDecimal();
                                 dishTotal += dishNum;
                                 orderDishList.Add(new OrderDishEntity
                                 {
                                     MorderId = order.MorderId,
                                     PlatformId = order.PlatformId,
                                     DishName = d.Name.ToString().Replace("??", string.Empty),
                                     DishProperty = d.Property.ToString().Replace("??", string.Empty),
                                     DishNum = dishNum,
                                     DishPrice = d.Price.ToDecimal(),
                                     DishAmount = dishNum * d.Price.ToDecimal(),
                                     DishGroupId = d.GroupId,
                                     DishInnerId = d.InnerId,
                                     OrderTime = order.OrderTime,
                                     ShopId = order.ShopId
                                 });

                                 if (d.GroupId.HasValue && d.GroupId > maxGroupId)
                                 {
                                     maxGroupId = d.GroupId.Value;
                                 }
                             });
                         }

                         if (order != null)
                         {
                             if (order.DinnersNum <= 0)
                             {
                                 order.DinnersNum = maxGroupId;
                             }
                             order.OrderItemTotal = dishTotal;
                         }

                         #endregion

                         #region 4.订单发票

                         if (orderModel.invoiced)
                         {
                             var invoiceType = InvoiceType.Personal.GetHashCode();
                             if (orderModel.invoiceType != null)
                             {
                                 invoiceType = (orderModel.invoiceType.ToLower() == InvoiceType.Company.ToString().ToLower()) ? InvoiceType.Company.GetHashCode() : InvoiceType.Personal.GetHashCode();
                             }
                             else
                             {
                                 invoiceType = !string.IsNullOrWhiteSpace(orderModel.taxpayerId) ? InvoiceType.Company.GetHashCode() : InvoiceType.Personal.GetHashCode();
                             }
                             invoice = new OrderInvoiceEntity()
                             {
                                 MorderId = order.MorderId,
                                 OrderId = order.OrderId,
                                 InvoiceType = invoiceType,
                                 InvoiceHeader = orderModel.invoice ?? string.Empty,
                                 TaxPayerId = orderModel.taxpayerId ?? string.Empty,
                                 IsOpen = BoolType.No.GetHashCode(),
                                 CreateDate = TimeUtil.Now
                             };
                         }

                         #endregion

                         db.Insert(order);
                         db.Insert(detail);
                         db.Insert(orderDishList);
                         if (invoice != null)
                         {
                             db.Insert(invoice);
                         }
                     }

                     db.Commit();
                 }
                 catch (Exception ex)
                 {
                     db.Rollback();
                     LogUtil.Error(ex.StackTrace);
                     throw new MessageException("系统错误，保存饿了么订单失败！");
                 }
             }

            return order;
        }

        /// <summary>
        /// 保存订单【百度外卖】
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public OrderEntity SaveOrder(BaiduOrderModel orderModel, string responseString)
        {
             OrderEntity order = null;
             if (orderModel != null && orderModel.order != null && !string.IsNullOrWhiteSpace(orderModel.order.order_id))
             {
                 //已保存订单直接返回
                 order = GetEntity(PlatformType.Baidu, orderModel.order.order_id);
             }
             if (order == null || string.IsNullOrWhiteSpace(order.MorderId))
             {
                 //首次回调保存订单
                 OrderDetailEntity detail = null;
                 OrderInvoiceEntity invoice = null;
                 var db = new RepositoryFactory().BaseRepository().BeginTrans();
                 try
                 {
                     if (orderModel != null && orderModel.order != null && orderModel.shop != null)
                     {
                         #region 1.订单主体

                         //40分钟平均送达时间
                         var avgSendTime = 2400;
                         var orderTime = TimeUtil.GetDateTime(orderModel.order.create_time.ToInt());
                         var preDeliveryTime = orderModel.order.send_immediately == 1 ? orderTime.AddSeconds(avgSendTime) : TimeUtil.GetDateTime(orderModel.order.send_time.ToInt());
                         order = new OrderEntity()
                         {
                             MorderId = StringUtil.UniqueStr(),
                             OrderViewId = orderModel.order.order_id,
                             OrderId = orderModel.order.order_id,
                             OrderType = orderModel.order.send_immediately,
                             PlatformId = PlatformType.Baidu.GetHashCode(),
                             DaySeq = orderModel.order.order_index.ToInt(),
                             BussinessType = BusinessType.Waimai.GetHashCode(),
                             ShopId = orderModel.shop.id.ToInt(),
                             ShopName = orderModel.shop.name,
                             RecipientName = orderModel.user != null ? orderModel.user.name : string.Empty,
                             RecipientPhone = orderModel.user != null ? orderModel.user.phone : string.Empty,
                             RecipientAddress = orderModel.user != null ? string.Format("{0}{1}{2}", orderModel.user.city, orderModel.user.district, orderModel.user.address) : string.Empty,
                             OrderTime = orderTime,
                             DeliveryTime = GetDeliveryTime(orderModel.order.send_immediately, orderModel.order.send_time.ToInt(), orderModel.order.create_time.ToInt(), avgSendTime),
                             DinnersNum = orderModel.order.meal_num.ToInt(),
                             Caution = orderModel.order.remark.ToString().Replace("??", string.Empty),
                             DeliveryService = ((BaidulogisticsCode)orderModel.order.delivery_party).GetRemark(),
                             DeliveryFee = (orderModel.order.send_fee.ToDecimal(2) / 100m).ToDecimal(),
                             OrderAmount = (orderModel.order.total_fee.ToDecimal(2) / 100m).ToDecimal(),
                             PayAmount = (orderModel.order.user_fee.ToDecimal(2) / 100m).ToDecimal(),
                             PayType = orderModel.order.pay_type.ToInt(),
                             OrderStatus = OrderStatus.WaitConfirm.GetHashCode(),
                             PrintStatus = PrintStatus.WaitPrint.GetHashCode(),
                             OrderData = responseString.ToString().Replace("??", string.Empty),
                             IsPreOrder = orderTime.AddMinutes(ConfigUtil.PreOrderInterval) < preDeliveryTime ? 1 : 0,
                             PreDeliveryTime = preDeliveryTime,
                             IsRemind = 0,
                             CreateDate = TimeUtil.Now
                         };

                         #endregion

                         #region 2.订单详细

                         detail = new OrderDetailEntity()
                         {
                             MorderId = order.MorderId,
                             OrderId = order.OrderId,
                             PlatformId = order.PlatformId,
                             BussinessType = order.BussinessType,
                             ShopId = order.ShopId,
                             Detail = orderModel.products.ToString().Replace("??", string.Empty),
                             PoiReceiveDetail = string.Empty,
                             Extras1 = orderModel.discount.ToString().Replace("??", string.Empty),
                             Extras2 = (orderModel.order.package_fee.ToDecimal(2) / 100m).ToDecimal(2).ToString(),
                             Extras3 = string.Empty,
                             ModifyDate = order.CreateDate
                         };

                         #endregion

                         #region 3.订单菜品

                         var maxGroupId = 1;
                         var dishTotal = 0.00m;
                         var orderDishList = new List<OrderDishEntity>();
                         var orderDishs = baiServ.AnalysisOrderDetail(orderModel.products.ToString());
                         if (orderDishs != null && orderDishs.Count > 0)
                         {
                             orderDishs.ForEach(d =>
                             {
                                 var dishNum = (d.Qty.Replace("X", string.Empty)).ToDecimal();
                                 dishTotal += dishNum;
                                 orderDishList.Add(new OrderDishEntity
                                 {
                                     MorderId = order.MorderId,
                                     PlatformId = order.PlatformId,
                                     DishName = d.Name.ToString().Replace("??", string.Empty),
                                     DishProperty = d.Property.ToString().Replace("??", string.Empty),
                                     DishNum = dishNum,
                                     DishPrice = d.Price.ToDecimal(2),
                                     DishAmount = dishNum * d.Price.ToDecimal(2),
                                     DishGroupId = d.GroupId,
                                     DishInnerId = d.InnerId,
                                     OrderTime = order.OrderTime,
                                     ShopId = order.ShopId
                                 });

                                 if (d.GroupId.HasValue && d.GroupId > maxGroupId)
                                 {
                                     maxGroupId = d.GroupId.Value;
                                 }
                             });
                         }

                         if (order != null)
                         {
                             if (order.DinnersNum <= 0)
                             {
                                 order.DinnersNum = maxGroupId;
                             }
                             order.OrderItemTotal = dishTotal;
                         }

                         #endregion

                         #region 4.订单发票
                         if (orderModel.order.need_invoice == BoolType.Yes.GetHashCode())
                         {
                             invoice = new OrderInvoiceEntity()
                             {
                                 MorderId = order.MorderId,
                                 OrderId = order.OrderId,
                                 InvoiceType = string.IsNullOrWhiteSpace(orderModel.order.taxer_id) ? InvoiceType.Personal.GetHashCode() : InvoiceType.Company.GetHashCode(),
                                 InvoiceHeader = orderModel.order.invoice_title ?? string.Empty,
                                 TaxPayerId = orderModel.order.taxer_id ?? string.Empty,
                                 IsOpen = BoolType.No.GetHashCode(),
                                 CreateDate = TimeUtil.Now
                             };
                         }

                         #endregion

                         db.Insert(order);
                         db.Insert(detail);
                         db.Insert(orderDishList);
                         if (invoice != null)
                         {
                             db.Insert(invoice);
                         }
                     }

                     db.Commit();
                 }
                 catch (Exception ex)
                 {
                     db.Rollback();
                     LogUtil.Error(ex.StackTrace);
                     throw new MessageException("系统错误，保存百度外卖订单失败！");
                 }
             }

            return order;
        }

        /// <summary>
        /// 更新订单提醒标识
        /// </summary>
        /// <param name="mOrderId">订单ID</param>
        /// <param name="cnt">提醒标识</param>
        /// <returns></returns>
        public int UpdateOrderRemindFlag(string mOrderId, int cnt)
        {
            var sql = @"UPDATE bll_order SET IsRemind = @cnt, ModifyDate = NOW() WHERE MorderId = @mOrderId ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("mOrderId", mOrderId));
            parms.Add(DbParameters.CreateDbParameter("cnt", cnt));

            return this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
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
            var cnt = 0;
            var sql = string.Empty;
            var opTime = string.Empty;
            var parms = new List<DbParameter>();
            switch (status)
            {
                case OrderStatus.Completed:
                    {
                        sql = @"UPDATE bll_order SET OrderStatus = @status, ModifyDate = NOW(), CompleteTime = NOW() WHERE MorderId = @mOrderId ";
                        break;
                    }
                case OrderStatus.Canceled:
                    {
                        sql = @"UPDATE bll_order SET OrderStatus = @status, ModifyDate = NOW(), CancelTime = NOW() WHERE MorderId = @mOrderId ";
                        break;
                    }
                case OrderStatus.Confirmed:
                    {
                        parms.Add(DbParameters.CreateDbParameter("printstatus", (int)PrintStatus.WaitPrint));
                        sql = @"UPDATE bll_order SET OrderStatus = @status, PrintStatus = @printstatus, ModifyDate = NOW() WHERE MorderId = @mOrderId ";
                        break;
                    }
                case OrderStatus.WaitConfirm:
                case OrderStatus.Deliverying:
                    {
                        sql = @"UPDATE bll_order SET OrderStatus = @status, ModifyDate = NOW() WHERE MorderId = @mOrderId ";
                        break;
                    }
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(sql))
            {
                parms.Add(DbParameters.CreateDbParameter("mOrderId", mOrderId));
                parms.Add(DbParameters.CreateDbParameter("status", (int)status));
                cnt = this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
            }
            if (cnt <= 0)
            {
                LogUtil.Warn(string.Format("订单状态更新异常，sql:{0},mOrderId:{1},status:{2}", sql, mOrderId, (int)status));
            }
            return cnt;
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <param name="printResultCode"></param>
        /// <returns></returns>
        public int UpdateOrderStatus(PlatformType platformType, string orderId, OrderStatus status, string remark = "")
        {
            var cnt = 0;
            var sql = string.Empty;
            var parms = new List<DbParameter>();
            switch (status)
            {
                case OrderStatus.Completed:
                    {
                        parms.Add(DbParameters.CreateDbParameter("remark", remark));
                        sql = @"UPDATE bll_order SET OrderStatus = @status, Description = @remark, ModifyDate = NOW(), CompleteTime = NOW() WHERE PlatformId = @platformType AND OrderId = @orderId ";
                        break;
                    }
                case OrderStatus.Canceled:
                    {
                        parms.Add(DbParameters.CreateDbParameter("remark", remark));
                        sql = @"UPDATE bll_order SET OrderStatus = @status, CancelReason = @remark, ModifyDate = NOW(), CancelTime = NOW() WHERE PlatformId = @platformType AND OrderId = @orderId ";
                        break;
                    }
                case OrderStatus.Confirmed:
                    {
                        parms.Add(DbParameters.CreateDbParameter("printstatus", (int)PrintStatus.WaitPrint));
                        sql = @"UPDATE bll_order SET OrderStatus = @status, PrintStatus = @printstatus, ModifyDate = NOW() WHERE PlatformId = @platformType AND OrderId = @orderId ";
                        break;
                    }
                case OrderStatus.WaitConfirm:
                case OrderStatus.Deliverying:
                    {
                        sql = @"UPDATE bll_order SET OrderStatus = @status, ModifyDate = NOW() WHERE PlatformId = @platformType AND OrderId = @orderId ";
                        break;
                    }
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(sql))
            {
                parms.Add(DbParameters.CreateDbParameter("platformType", (int)platformType));
                parms.Add(DbParameters.CreateDbParameter("orderId", orderId));
                parms.Add(DbParameters.CreateDbParameter("status", (int)status));
                cnt = this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
            }
            if (cnt <= 0)
            {
                LogUtil.Warn(string.Format("订单状态更新异常，sql:{0},params:{1}", sql, parms.ToJson()));
            }
            return cnt;
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
            var sql = @"UPDATE bll_order SET PrintStatus = @status, PrintResultCode = @resultCode, PrintTotal = PrintTotal+1, PrintTime = NOW(), ModifyDate = NOW() WHERE MorderId = @mOrderId ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("mOrderId", mOrderId));
            parms.Add(DbParameters.CreateDbParameter("status", (int)status));
            parms.Add(DbParameters.CreateDbParameter("resultCode", printResultCode));
            var cnt = this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
            if (cnt <= 0)
            {
                LogUtil.Warn(string.Format("打印状态更新异常，sql:{0},params:{1}", sql, parms.ToJson()));
            }
            return cnt;
        }

        /// <summary>
        /// 查询待完成订单列表
        /// </summary>
        /// <returns></returns>
        public List<OrderEntity> FindWaitCompleteOrderList()
        {
            var sql = string.Format(@"SELECT * FROM bll_order WHERE (OrderStatus = 2 OR OrderStatus = 3) ORDER BY orderTime");
            return this.BaseRepository().FindList(sql).ToList();
        }

        /// <summary>
        /// 查询待提醒订单列表
        /// </summary>
        /// <returns></returns>
        public List<OrderEntity> FindWaitRemindPreOrderList(int mins)
        {
            var sql = string.Format(@"SELECT * FROM bll_order WHERE (OrderStatus = 2 OR OrderStatus = 3) AND IsPreOrder = 1  AND IsRemind = 0 AND TIMESTAMPDIFF(MINUTE, NOW(), PreDeliveryTime) <= @mins ORDER BY orderTime");
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("mins", mins));
            return this.BaseRepository().FindList(sql, parms.ToArray()).ToList();
        }

        /// <summary>
        /// 更新订单状态【完成或取消】
        /// </summary>
        /// <param name="orderDics"></param>
        public int UpdateOrderStatus(Dictionary<int, string> orderDics)
        {
            if (orderDics != null && orderDics.Count > 0)
            {
                var sql = new StringBuilder();
                var sqltemp = @"UPDATE bll_order SET OrderStatus = {0},ModifyDate = NOW() WHERE (OrderStatus = 2 OR OrderStatus = 3) AND OrderId IN({1}); ";
                foreach (var order in orderDics)
                {
                    sql.AppendFormat(sqltemp, order.Key, order.Value);
                }
                return this.BaseRepository().ExecuteBySql(sql.ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 强制完成订单
        /// </summary>
        public int ForceUpdateOrderStatus(int mins)
        {
            var sql = @"UPDATE bll_order SET OrderStatus = 4, CompleteTime = NOW(), ModifyDate = NOW() WHERE (OrderStatus = 2 OR OrderStatus = 3) AND TIMESTAMPDIFF(MINUTE,PreDeliveryTime,NOW()) >= @mins ";
            var parms = new List<DbParameter>();
            parms.Add(DbParameters.CreateDbParameter("mins", mins));

            return this.BaseRepository().ExecuteBySql(sql, parms.ToArray());
        }

        /// <summary>
        /// 【美团外卖】送达时间处理
        /// </summary>
        /// <returns></returns>
        private string GetDeliveryTime(long deliveryTime, long orderTime, long avgSendTime)
        {
            if (deliveryTime == 0)
            {
                var dt = TimeUtil.GetDateTime(orderTime).AddSeconds(avgSendTime);
                return string.Format("立即│大约{0}送达", dt.ToString("HH:mm"));
            }
            else
            {
                return string.Format("指定│{0}", TimeUtil.GetWeekDateTime(deliveryTime));
            }
        }

        /// <summary>
        /// 【饿了么】送达时间处理
        /// </summary>
        /// <param name="deliveryTime"></param>
        /// <param name="orderTime"></param>
        /// <param name="avgSendTime"></param>
        /// <returns></returns>
        private string GetDeliveryTime(DateTime? deliveryTime, DateTime? orderTime, long avgSendTime)
        {
            if(deliveryTime != null)
            {
                //预订单指定送达时间
                return string.Format("指定│{0}", TimeUtil.GetWeekDateTime(deliveryTime.Value));
            }
            else
            {
                //立即送达
                var dt = orderTime.Value.AddSeconds(avgSendTime);
                return string.Format("立即│大约{0}送达", dt.ToString("HH:mm"));
            }
        }

        /// <summary>
        /// 【百度外卖】送达时间处理
        /// </summary>
        /// <param name="sendImmediately">是否立即送餐:1-是,2-否</param>
        /// <param name="sendTime">期望送达时间</param>
        /// <param name="orderTime">下单时间</param>
        /// <param name="avgSendTime">平均送达时长</param>
        /// <returns></returns>
        private string GetDeliveryTime(int sendImmediately, long sendTime, long orderTime, long avgSendTime)
        {
            if (sendImmediately == 1)
            {
                //立即送达
                var dt = TimeUtil.GetDateTime(orderTime).AddSeconds(avgSendTime);
                return string.Format("立即│大约{0}送达", dt.ToString("HH:mm"));
            }
            else
            {
                //预订单指定送达时间
                var dt = TimeUtil.GetDateTime(sendTime);
                return string.Format("指定│{0}", TimeUtil.GetWeekDateTime(dt));
            }
        } 
    }
}
