using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yme.Util.Extension;
using System.Threading.Tasks;
using Yme.Mcp.Model.Enums;
using Yme.Util;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.BLL.WeChatManage;
using Yme.Mcp.Model.WeChat;
using Yme.Util.Log;
using Yme.Mcp.BLL.OrderManage;
using Yme.Mcp.Entity.OrderManage;

namespace Yme.Mcp.BLL.SystemManage
{
    public class MsgBLL
    {
        private WeChatBLL weChatBLL = SingleInstance<WeChatBLL>.Instance;
        private PrinterBLL printBLL = SingleInstance<PrinterBLL>.Instance;
        private OrderBLL orderBLL = SingleInstance<OrderBLL>.Instance;
        private int wxType = EnumWeChatType.Client.GetHashCode();

        /// <summary>
        /// 发送新订单消息通知
        /// </summary>
        /// <param name="order">订单信息</param>
        public string SendNewOrderPushMsg(OrderEntity order)
        {
            var errMsg = string.Empty;
            try
            {
                var openId = weChatBLL.GetWechatOpenId(wxType, order.ShopId.ToString());
                if (!string.IsNullOrWhiteSpace(openId) && order != null)
                {
                    var isConfirm = order.OrderStatus > OrderStatus.WaitConfirm.GetHashCode();
                    var id = order.Id == 0 ? orderBLL.GetOrder(order.MorderId).Id : order.Id;
                    var url = orderBLL.GetOrderDetailUrl(id);
                    var platformName = ((PlatformType)order.PlatformId).GetRemark();
                    var wechatToken = weChatBLL.GetWeiXinToken(wxType);
                    var orderScource = string.Format("来自{0}", platformName);
                    var msgModel = weChatBLL.GetNewOrderMsgTemplateModel(platformName, order.DaySeq, order.OrderViewId, order.OrderTime, isConfirm);
                    string resMsg = weChatBLL.SendTemplateMsg<NewOrderTempModel>(wechatToken, openId, "#FF0000", url, msgModel);
                    LogUtil.Info(string.Format("【{0}】新订单:{1}{2}确认,发送提醒通知：{3}。", platformName, order.OrderViewId, isConfirm ? "已自动" : "待手动", resMsg));
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogUtil.Error(string.Format("发送新订单提醒通知失败,错误信息：{0}。", errMsg));
            }
            return errMsg;
        }

        /// <summary>
        /// 发送取消订单消息
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="cancelReason">取消原因</param>
        /// <returns></returns>
        public string SendCancelOrderPushMsg(OrderEntity order, string cancelReason)
        {
            var errMsg = string.Empty;
            try
            {
                var openId = weChatBLL.GetWechatOpenId(wxType, order.ShopId.ToString());
                if (!string.IsNullOrWhiteSpace(openId) && order != null)
                {
                    var id = order.Id == 0 ? orderBLL.GetOrder(order.MorderId).Id : order.Id;
                    var url = orderBLL.GetOrderDetailUrl(id);
                    var platformName = ((PlatformType)order.PlatformId).GetRemark();
                    var wechatToken = weChatBLL.GetWeiXinToken(wxType);
                    var msgModel = weChatBLL.GetCancelOrderMsgTemplateModel(platformName, order.DaySeq, order.OrderViewId, order.OrderTime, cancelReason);
                    var resMsg = weChatBLL.SendTemplateMsg<CancelOrderTempModel>(wechatToken, openId, "#FF0000", url, msgModel);
                    LogUtil.Info(string.Format("【{0}】订单:{1}已取消,发送提醒通知：{2}。", platformName, order.OrderViewId, resMsg));
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogUtil.Error(string.Format("发送取消订单提醒通知失败,错误信息：{0}。", errMsg));
            }
            return errMsg;
        }

        /// <summary>
        /// 发送订单打印失败消息
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="pusMsg">推送消息</param>
        /// <returns></returns>
        public string SendPrintFailPushMsg(OrderEntity order, string pusMsg)
        {
            var errMsg = string.Empty;
            try
            {
                var openId = weChatBLL.GetWechatOpenId(wxType, order.ShopId.ToString());
                if (!string.IsNullOrWhiteSpace(openId) && order != null)
                {
                    var id = order.Id == 0 ? orderBLL.GetOrder(order.MorderId).Id : order.Id;
                    var url = orderBLL.GetOrderDetailUrl(id);
                    var platformName = ((PlatformType)order.PlatformId).GetRemark();
                    var wechatToken = weChatBLL.GetWeiXinToken(wxType);
                    var msgModel = weChatBLL.GetPrintFailMsgTemplateModel(order.PlatformId, order.OrderId, order.DaySeq, order.OrderTime, pusMsg);
                    var resMsg = weChatBLL.SendTemplateMsg<OrderPrintFailTempModel>(wechatToken, openId, "#FF0000", url, (OrderPrintFailTempModel)msgModel);
                    LogUtil.Info(string.Format("【{0}】新订单:{1}打印失败,发送提醒通知：{2}。", platformName, order.OrderViewId, resMsg));
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogUtil.Error(string.Format("发送打印失败提醒通知失败,错误信息：{0}。", errMsg));
            }
            return errMsg;
        }

        /// <summary>
        /// 发送订单打印失败消息
        /// </summary>
        /// <param name="pusMsg"></param>
        /// <param name="shopId"></param>
        /// <param name="platformId"></param>
        /// <param name="orderId"></param>
        /// <param name="daySeq"></param>
        /// <param name="orderTime"></param>
        /// <param name="orderViewId"></param>
        /// <returns></returns>
        public string SendPrintFailPushMsg(string pusMsg, string shopId, int platformId, string orderId, int daySeq, DateTime orderTime, string orderViewId = "")
        {
            var errMsg = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(shopId) || shopId == "0")
                {
                    var order = SingleInstance<OrderBLL>.Instance.GetOrder(platformId, orderId);
                    shopId = order != null ? order.ShopId.ToString() : string.Empty;
                }
                var platformType = (PlatformType)platformId;
                var openId = weChatBLL.GetWechatOpenId(wxType, shopId);
                if (!string.IsNullOrWhiteSpace(openId))
                {
                    var url = orderBLL.GetOrderDetailUrl(platformType, orderId);
                    var platformName = platformType.GetRemark();
                    var wechatToken = weChatBLL.GetWeiXinToken(wxType);
                    var msgModel = weChatBLL.GetPrintFailMsgTemplateModel(platformId, orderId, daySeq, orderTime, pusMsg);
                    var resMsg = weChatBLL.SendTemplateMsg<OrderPrintFailTempModel>(wechatToken, openId, "#FF0000", url, (OrderPrintFailTempModel)msgModel);
                    LogUtil.Info(string.Format("【{0}】新订单:{1}打印失败,发送提醒通知：{2}。", platformName, orderViewId, resMsg));
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogUtil.Error(string.Format("发送打印失败提醒通知失败,错误信息：{0}。", errMsg));
            }
            return errMsg;
        }

        /// <summary>
        /// 发送打印失败消息
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="orderId"></param>
        /// <param name="printerCode"></param>
        /// <param name="printTime"></param>
        /// <returns></returns>
        public string SendPrintFailPushMsg(string printerCode, string resultCode, DateTime opTime, string orderId, int platformId, int daySeq, string orderViewId)
        {
            var errMsg = string.Empty;
            try
            {
                var msgCode = resultCode.ToInt();
                if (!string.IsNullOrWhiteSpace(printerCode)
                     && ((msgCode > PrintResultType.Success.GetHashCode() && msgCode <= PrintResultType.UnBindClear.GetHashCode()) || msgCode == PrintResultType.UnKnow.GetHashCode()))
                {
                    SendMsg(PushMsgType.PrintFailMsg, printerCode, msgCode, opTime, orderId, platformId, daySeq, orderViewId);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogUtil.Error(string.Format("发送打印失败提醒通知失败,错误信息：{0}。", errMsg));
            }
            return errMsg;
        }

        /// <summary>
        /// 发送打印机异常消息
        /// </summary>
        /// <param name="printerCode"></param>
        /// <param name="statusCode"></param>
        /// <param name="opTime"></param>
        /// <returns></returns>
        public string SendPriterFaultPushMsg(string printerCode, string statusCode, DateTime opTime)
        {
            var errMsg = string.Empty;
            try
            {
                var msgCode = statusCode.ToInt();
                if (!string.IsNullOrWhiteSpace(printerCode)
                    && ((msgCode > PrinterFaultType.Enable.GetHashCode() && msgCode <= PrinterFaultType.NotFind.GetHashCode()) || msgCode == PrinterFaultType.UnKnow.GetHashCode()))
                {
                    SendMsg(PushMsgType.PrinterFaultMsg, printerCode, msgCode, opTime);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogUtil.Error(string.Format("发送设备异常提醒通知失败,错误信息：{0}。", errMsg));
            }
            return errMsg;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="printerCode">设备编号</param>
        /// <param name="msgCode">消息编码</param>
        /// <param name="opTime">操作时间</param>
        /// <param name="orderId">订单Id</param>
        /// <param name="platformId">平台Id</param>
        /// <param name="daySeq">平台日序号</param>
        private void SendMsg(PushMsgType msgType, string printerCode, int msgCode, DateTime opTime, string orderId = "", int platformId = 0, int daySeq = 0, string orderViewId = "")
        {
            var openId = string.Empty;
            var msg = string.Empty;
            var msgModel = new TemplateMsgModel();
            if (!string.IsNullOrWhiteSpace(printerCode))
            {
                var shopPrinters = printBLL.GetShopPrinterList(printerCode);
                if (shopPrinters != null && shopPrinters.Count > 0 && msgCode > 0)
                {
                    foreach (var sp in shopPrinters)
                    {
                        openId = weChatBLL.GetWechatOpenId(wxType, sp.ShopId.ToString());
                        if (!string.IsNullOrWhiteSpace(openId))
                        {
                            var wechatToken = weChatBLL.GetWeiXinToken(wxType);
                            if (msgType == PushMsgType.PrinterFaultMsg)
                            {
                                msgModel = weChatBLL.GetPrinterFaultMsgTemplateModel(sp.PrinterName, msgCode, opTime.ToDate(), sp.PrinterCode);
                                msg = weChatBLL.SendTemplateMsg<PrinterFaultTempModel>(wechatToken, openId, "#FF0000", string.Empty, (PrinterFaultTempModel)msgModel);
                            }
                            else
                            {
                                var platformType = (PlatformType)platformId;
                                var url = orderBLL.GetOrderDetailUrl(platformType, orderId);
                                msgModel = weChatBLL.GetPrintFailMsgTemplateModel(msgCode, opTime.ToDate(), orderViewId, platformId, daySeq, sp.PrinterName);
                                msg = weChatBLL.SendTemplateMsg<OrderPrintFailTempModel>(wechatToken, openId, "#FF0000", url, (OrderPrintFailTempModel)msgModel);
                            }

                            LogUtil.Info(string.Format("{0}通知：{1}。", msgType == PushMsgType.PrinterFaultMsg ? "打印机异常" : "订单打印失败", msg));
                        }
                    }
                }
                else
                {
                    //通过订单Id查找对应门店关注的OpenId进行消息推送
                    if (!string.IsNullOrWhiteSpace(orderId) && platformId > 0 && msgType == PushMsgType.PrintFailMsg)
                    {
                        var order = SingleInstance<OrderBLL>.Instance.GetOrder(platformId, orderId);
                        if (order != null)
                        {
                            openId = weChatBLL.GetWechatOpenId(wxType, order.ShopId.ToString());
                            if (!string.IsNullOrWhiteSpace(openId))
                            {
                                var wechatToken = weChatBLL.GetWeiXinToken(wxType);
                                var platformType = (PlatformType)platformId;
                                var url = orderBLL.GetOrderDetailUrl(platformType, orderId);
                                msgModel = weChatBLL.GetPrintFailMsgTemplateModel(msgCode, opTime.ToDate(), orderViewId, platformId, daySeq);
                                msg = weChatBLL.SendTemplateMsg<OrderPrintFailTempModel>(wechatToken, openId, "#FF0000", url, (OrderPrintFailTempModel)msgModel);
                                LogUtil.Info(string.Format("订单打印失败通知：{0}。", msg));
                            }
                        }
                    }
                }
            }
        }
    }
}
