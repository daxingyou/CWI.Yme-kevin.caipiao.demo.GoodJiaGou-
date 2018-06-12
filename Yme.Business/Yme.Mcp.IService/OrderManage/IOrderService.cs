using System;
using System.Collections.Generic;
using Yme.Mcp.Entity.OrderManage;
using Yme.Mcp.Model;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Model.ViewModels.Waimai.Baidu;
using Yme.Mcp.Model.ViewModels.Waimai.Eleme;
using Yme.Mcp.Model.Waimai.Meituan;
using Yme.Util.WebControl;

namespace Yme.Mcp.Service.OrderManage
{
    public interface IOrderService
    {
        OrderEntity GetEntity(string entityId);

        OrderEntity GetEntity(long id);

        OrderEntity GetEntity(PlatformType platformType, string orderId);

        List<OrderEntity> FindList(DateTime orderDate, long shopId = 0);

        bool CheckShopIsExistsOrder(long shopId);

        List<OrderEntity> FindPageList(string queryJson, Pagination pagination, OrderQueryType queryType);

        List<OrderEntity> FindWaitCompleteOrderList();

        List<OrderEntity> FindWaitRemindPreOrderList(int mins);

        OrderEntity SaveOrder(MeituanOrderModel orderModel, string responseString);

        OrderEntity SaveOrder(ElemeOrderModel orderModel, string responseString);

        OrderEntity SaveOrder(BaiduOrderModel orderModel, string responseString);

        int UpdateOrderRemindFlag(string mOrderId, int cnt);

        int UpdateOrderStatus(string mOrderId, OrderStatus status);

        int UpdateOrderPrintStatus(string mOrderId, PrintStatus status, string printResultCode = "");

        int UpdateOrderStatus(PlatformType platformType, string orderId, OrderStatus status, string remark = "");

        /// <summary>
        /// 更新订单状态【完成或取消】
        /// </summary>
        /// <param name="orderDics"></param>
        int UpdateOrderStatus(Dictionary<int, string> orderDics);

        /// <summary>
        /// 强制完成订单
        /// </summary>
        int ForceUpdateOrderStatus(int mins);
    }
}
