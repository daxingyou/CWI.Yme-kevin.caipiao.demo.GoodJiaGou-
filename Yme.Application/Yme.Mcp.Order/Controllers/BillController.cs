using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yme.Util.Log;
using Yme.Util.Extension;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Model.QueryModels;
using Yme.Util;
using Yme.Mcp.BLL.OrderManage;
using Yme.Mcp.Order.Handel;
using Yme.Mcp.Entity.OrderManage;

namespace Yme.Mcp.Order.Controllers
{
    /// <summary>
    /// 小票控制器
    /// </summary>
    public class BillController : Controller
    {
        /// <summary>
        /// 订单业务服务
        /// </summary>
        private OrderBLL obServ = SingleInstance<OrderBLL>.Instance;

        /// <summary>
        /// 获取小票信息
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        public ActionResult GetSamllBill()
        {
            var id = Request["id"] as string;
            var templeid = Request["templeid"] as string;
            var tempId = templeid.ToInt();
            List<OrderGroupQueryModel> details = null;
            OrderFeeQueryModel otherFee = null;
            OrderInvoiceEntity invoice = null;
            var qrcodeUrl = string.Empty;
            var viewName = "Delivery";
            var order = obServ.GetOrder(id);

            if (order != null)
            {
                var detail = obServ.GetDetail(id);
                details = obServ.GetOrderDetails(order.PlatformId, (detail != null ? detail.Detail : string.Empty));
                otherFee = obServ.GetOrderFeeDetail(order, detail);
                invoice = obServ.GetOrderInvoice(id);
                switch (tempId)
                {
                    case (int)SmallBillType.Kitchen:
                        {
                            //厨房小票
                            viewName = "Kitchen";
                            break;
                        }
                    case (int)SmallBillType.Merchant:
                        {
                            //商家小票
                            viewName = "Merchant";
                            break;
                        }
                    case (int)SmallBillType.Delivery:
                        {
                            //配送小票
                            viewName = "Delivery";
                            qrcodeUrl = obServ.GetOrderDetailUrl(order.Id);
                            break;
                        }
                    case (int)SmallBillType.Customer:
                        {
                            //客户小票
                            viewName = "Customer";
                            qrcodeUrl = obServ.GetOrderDetailUrl(order.Id);
                            break;
                        }
                    case (int)SmallBillType.Cancel:
                        {
                            //取消小票
                            viewName = "Cancel";
                            break;
                        }
                    default:
                        break;
                }
            }

            ViewBag.Order = order;
            ViewBag.Details = details;
            ViewBag.OtherFee = otherFee;
            ViewBag.Invoice = invoice != null ? (string.Format("{0}{1}", invoice.InvoiceHeader, (!string.IsNullOrWhiteSpace(invoice.TaxPayerId) ? string.Format("【{0}】", invoice.TaxPayerId) : string.Empty))) : string.Empty;
            ViewBag.QrcodeUrl = qrcodeUrl;
            return View(viewName);
        }
    }
}
