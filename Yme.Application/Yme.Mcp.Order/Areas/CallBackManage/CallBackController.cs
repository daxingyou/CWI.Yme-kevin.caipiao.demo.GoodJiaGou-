﻿using System;
using System.Text;
using Yme.Util;
using Yme.Util.Log;
using Yme.Util.Extension;
using Yme.Mcp.BLL.WeChatManage;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.Order.Controllers;
using Yme.Mcp.Model.Definitions;
using Yme.Mcp.BLL.OrderManage;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Order.Handel;
using System.Web.Http;
using Yme.Mcp.Entity.BaseManage;
using System.Collections.Generic;
using Yme.Mcp.Model.Waimai.Meituan;
using Yme.Util.Security;
using Yme.Mcp.Model.WeChat;
using Yme.Mcp.Model.ViewModels.Waimai.Eleme;
using Yme.Cache.Factory;
using System.Reflection;
using Yme.Mcp.Model.ViewModels.Waimai.Baidu;
using Yme.Mcp.BLL.SystemManage;
using System.Threading;
using Yme.Cache;

namespace Yme.Mcp.Order.Controllers.Api
{
    /// <summary>
    /// 回调控制器
    /// </summary>
    public class CallBackController : ApiBaseController
    {
        #region 私有常量/全局变量

        /// <summary>
        /// 尝试延长执行间隔，单位：毫秒
        /// </summary>
        private const int delayTryInterval = 3000;

        /// <summary>
        /// 失败后尝试次数
        /// </summary>
        private const int tryMax = 5;

        /// <summary>
        /// Redis缓存
        /// </summary>
        private static ICache cache = CacheFactory.Cache();

        /// <summary>
        /// 锁对象
        /// </summary>
        private static readonly object _syncObject = new object();

        private static readonly object _syncPrintObject = new object();

        #endregion

        #region 美团外卖

        #region 门店

        /// <summary>
        /// 【美团外卖】店铺映射回调
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object MeiShopMapCallBack()
        {
            var actionDesc = "美团店铺映射回调";
            var requestForm = base.GetRequestParams(true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count > 0)
            {
                var shopId = requestForm["ePoiId"] ?? string.Empty;
                var authType = (AuthBussinessType)(requestForm["businessId"] ?? string.Empty).ToInt();
                var appAuthToken = requestForm["appAuthToken"] ?? string.Empty;
                var expiresIn = (requestForm["timestamp"] ?? string.Empty).ToInt();

                //执行门店解除授权数据同步【重复执行】
                var handleResult = false;
                for (var t = 1; t <= tryMax; t++)
                {
                    try
                    {
                        handleResult = SyncShopPlatformData(shopId, appAuthToken, expiresIn, string.Empty, PlatformType.Meituan, authType, t);
                    }
                    catch (Exception ex)
                    {
                        handleResult = false;
                        LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                    }

                    if (handleResult)
                    {
                        LogUtil.Info(string.Format("{0}成功！", actionDesc));
                        break;
                    }
                    else
                    {
                        Thread.Sleep(delayTryInterval);
                        continue;
                    }
                }
                return MeiMsg(handleResult ? MeituanConsts.RETURN_SUCCESS : MeituanConsts.RETURN_FAIL);
            }
            else
            {
                return MeiMsg(MeituanConsts.RETURN_FAIL);
            }
        }

        /// <summary>
        /// 【美团外卖】店铺解除映射回调
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object MeiShopUnmapCallBack()
        {
            var actionDesc = "美团店铺解除映射回调";
            var requestForm = base.GetRequestParams(true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count > 0)
            {
                var shopId = 0;
                var ePoiId = requestForm["ePoiId"] ?? string.Empty;
                var authType = (AuthBussinessType)(requestForm["businessId"] ?? string.Empty).ToInt();
                if (ePoiId.IndexOf(",") > 0)
                {
                    var shopIds = ePoiId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    shopId = shopIds[0].ToInt();
                }
                else
                {
                    shopId = ePoiId.ToInt();
                }

                //执行门店解除授权数据同步【重复执行】
                var handleResult = false;
                for (var t = 1; t <= tryMax; t++)
                {
                    try
                    {
                        handleResult = SyncUnMapShopPlatformData(shopId, PlatformType.Meituan, authType, t);
                    }
                    catch (Exception ex)
                    {
                        handleResult = false;
                        LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                    }

                    if (handleResult)
                    {
                        LogUtil.Info(string.Format("{0}成功！", actionDesc));
                        break;
                    }
                    else
                    {
                        Thread.Sleep(delayTryInterval);
                        continue;
                    }
                }
                return MeiMsg(handleResult ? MeituanConsts.RETURN_SUCCESS : MeituanConsts.RETURN_FAIL);
            }
            else
            {
                return MeiMsg(MeituanConsts.RETURN_FAIL);
            }
        }

        #endregion

        #region 订单

        /// <summary>
        /// 【美团外卖】接收订单回调
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object MeiReceiveOrderCallBack()
        {
            var actionDesc = "美团新订单回调";
            var keyName = "order";
            var result = HandleMeituanPushMsgSyncData(MeiPushMsgType.NewOrder, actionDesc, keyName);
            LogUtil.Info(string.Format("{0},返回:{1}", actionDesc, result.ToJson()));
            return result;
        }

        /// <summary>
        /// 【美团外卖】取消订单回调
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object MeiCancelOrderCallBack()
        {
            var actionDesc = "美团订单取消回调";
            var keyName = "orderCancel";
            var result = HandleMeituanPushMsgSyncData(MeiPushMsgType.CancelOrder, actionDesc, keyName);
            LogUtil.Info(string.Format("{0},返回:{1}", actionDesc, result.ToJson()));
            return result;
        }

        /// <summary>
        /// 【美团外卖】完成订单回调
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object MeiCompleteOrderCallBack()
        {
            var actionDesc = "美团订单完成回调";
            var keyName = "order";
            var result = HandleMeituanPushMsgSyncData(MeiPushMsgType.CompleteOrder, actionDesc, keyName);
            LogUtil.Info(string.Format("{0},返回:{1}", actionDesc, result.ToJson()));
            return result;
        }

        /// <summary>
        /// 【美团外卖】订单配送状态回调
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object MeiDeliveryStatusCallBack()
        {
            var actionDesc = "美团订单配送回调";
            var keyName = "shippingStatus";
            var result = HandleMeituanPushMsgSyncData(MeiPushMsgType.Delivering, actionDesc, keyName);
            LogUtil.Info(string.Format("{0},返回:{1}", actionDesc, result.ToJson()));
            return result;
        }

        #endregion

        #region 心跳

        /// <summary>
        /// 【美团外卖】云端心跳回调
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object MeiHeartbeatCallBack()
        {
            var queryStrings = WebUtil.UrlDecode(Request.Content.ReadAsStringAsync().Result, Encoding.UTF8);
            LogUtil.Info(string.Format("执行云端心跳回调,请求参数：{0}", queryStrings));

            return MeiMsg(MeituanConsts.RETURN_SUCCESS);
        }

        #endregion

        #endregion

        #region 饿了么

        /// <summary>
        /// 【饿了么】订单回调【新/取消/完成/配送订单】
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object EleOrderCallBack()
        {
            //1.校验请求参数
            var actionDesc = "饿了么消息推送回调";
            var msg = string.Empty;
            object result = null;
            var queryStrings = Request.Content.ReadAsStringAsync().Result;
            LogUtil.Info(string.Format("{0},请求参数:{1}", actionDesc, queryStrings));

            var sortDics = new SortedDictionary<string, object>();
            var signModel = Yme.Util.JsonUtil.ToObject<ElemeApiPushMsgModel>(queryStrings);
            if (signModel == null)
            {
                result = EleMsg(ElemeConsts.API_RETURN_OK);
                LogUtil.Info(string.Format("{0},返回:{1}", actionDesc, result.ToJson()));
                return result;
            }

            //2.校验请求合法性，验证签名
            var signKey = "signature";
            var t = signModel.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                var fieldName = pi.Name;
                if (!fieldName.ToLower().Equals(signKey))
                {
                    var fieldValue = pi.GetValue(signModel, null);
                    sortDics.Add(fieldName, fieldValue);
                }
            }
            var sign = signModel.signature ?? string.Empty;
            var mySign = SignUtil.GetEleSign(sortDics, ConfigUtil.EleAppSecret);
            if (!sign.Equals(mySign, System.StringComparison.CurrentCultureIgnoreCase))
            {
                msg = string.Format("{0}签名不合法,请求参数:{1},微云打订单平台签名:{2}", actionDesc, queryStrings, mySign);
                LogUtil.Error(msg);
                result = EleMsg(ElemeConsts.RETURN_FAIL);
                LogUtil.Info(string.Format("{0},返回:{1}", actionDesc, result.ToJson()));
                return result;
            }

            //3.校验消息是否已处理
            var requestId = signModel.requestId.ToString();
            if (!string.IsNullOrWhiteSpace(requestId))
            {
                var cacheMsgId = cache.GetCache<string>(requestId);
                if (string.IsNullOrWhiteSpace(cacheMsgId))
                {
                    //3.1. 未处理，接收消息推送回调执行数据同步【重复执行】
                    var msgType = signModel.type;
                    var eShopId = signModel.shopId.ToString();
                    var pushMsgCoreString = signModel.message.ToString();
                    result = HandleElemePushMsgSyncData(msgType, pushMsgCoreString, requestId, eShopId);
                    LogUtil.Info(string.Format("{0}:{1}返回:{2}", actionDesc, requestId, result.ToJson()));
                    return result;
                }
                else
                {
                    //3.2. 已处理，直接返回成功
                    msg = string.Format("成功过滤{0}请求,requestId:{0}", actionDesc, requestId);
                    LogUtil.Warn(msg);
                    result = EleMsg(ElemeConsts.RETURN_SUCCESS);
                    LogUtil.Info(string.Format("{0}:{1}返回:{2}", actionDesc, requestId, result.ToJson()));
                    return result;
                }
            }
            else
            {
                //3.3. 消息ID为空直接返回错误
                msg = string.Format("{0},requestId为空", actionDesc);
                LogUtil.Warn(msg);
                result = EleMsg(ElemeConsts.RETURN_FAIL);
                LogUtil.Info(string.Format("{0}:{1}返回:{2}", actionDesc, requestId, result.ToJson()));
                return result;
            }
        }

        #endregion

        #region 百度外卖

        /// <summary>
        /// 【百度外卖】订单回调
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object BaiduOrderCallBack()
        {
            //1.校验请求参数
            var actionDesc = "百度外卖订单推送回调";
            var queryStrings = string.Empty;
            object result = null;
            var sortDics = new SortedDictionary<string, object>();
            var requestForm = base.GetRequestParams(out sortDics, out queryStrings, true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count <= 0)
            {
                result = EleMsg(ElemeConsts.RETURN_FAIL);
                LogUtil.Info(string.Format("{0}返回：{1}", actionDesc, result.ToJson()));
                return result;
            }

            //2.校验请求合法性，验证签名
            var cmd = requestForm["cmd"] as string;
            var sign = requestForm["sign"] as string;
            var source = requestForm["source"] as string;
            var spInfo = SingleInstance<PlatformBLL>.Instance.GetShopBaiduPlatformInfo(source);
            var secret = spInfo != null ? spInfo.AuthToken : ConfigUtil.BaiduSourceSecret;
            var mySign = SignUtil.GetBaiduSign(sortDics, secret);
            if (!sign.Equals(mySign, System.StringComparison.CurrentCultureIgnoreCase))
            {
                LogUtil.Error(string.Format("{0}签名不合法,请求参数：{1},微云打订单平台签名：{2}", actionDesc, queryStrings, mySign));
                result = SingleInstance<PlatformBLL>.Instance.GetBaiApiRetResponse(cmd, null, source, secret, "", "签名验证不正确");
                LogUtil.Info(string.Format("{0},Cmd:{1},返回:{2}", actionDesc, cmd, result.ToJson()));
                return result;
            }

            //3.校验消息是否已处理
            var body = requestForm["body"] as string;
            var parms = body.ToObject<Dictionary<string, object>>();
            var bOrderId = parms.ContainsKey("order_id") ? parms["order_id"] as string : string.Empty;
            var bOrderStatus = parms.ContainsKey("status") ? parms["status"].ToInt() : 0;
            var parm = new Dictionary<string, object>();
            var data = new Dictionary<string, object>();
            data.Add("source_order_id", bOrderId);
            parm.Add("data", data);
            var requestId = string.Format("{0}_{1}_{2}", cmd, bOrderId, bOrderStatus);
            var cacheMsgId = cache.GetCache<string>(requestId);
            if (string.IsNullOrWhiteSpace(cacheMsgId))
            {
                //if (cmd.ToLower() == BaiduwmConsts.CREATE_ORDER_API)
                //{
                //    //3.0. 新订单先创建订单,在同步数据后返回
                //    result =  HandleBaiduPushMsgSyncData(cmd, source, secret, body, parms, requestId);
                //    //result = SingleInstance<PlatformBLL>.Instance.GetBaiApiRetResponse(cmd, parm, BaiduwmConsts.API_SUCCESS, source, secret);
                //    LogUtil.Info(string.Format("{0},requestId:{1},返回:{2}", actionDesc, requestId, result.ToJson()));
                //    return result;
                //}
                //else
                //{
                //    //3.1. 未处理,同步订单数据
                //    result = HandleBaiduPushMsgSyncData(cmd, source, secret, body, parms, requestId);
                //    LogUtil.Info(string.Format("{0},requestId:{1},返回:{2}", actionDesc, requestId, result.ToJson()));
                //    return result;
                //}
                result = HandleBaiduPushMsgSyncData(cmd, source, secret, body, parms, requestId);
                LogUtil.Info(string.Format("{0},requestId:{1},返回:{2}", actionDesc, requestId, result.ToJson()));
                return result;
            }
            else
            {
                //3.2. 已处理,直接返回成功
                LogUtil.Warn(string.Format("成功过滤{0}请求,requestId:{1}", actionDesc, requestId));
                result = SingleInstance<PlatformBLL>.Instance.GetBaiApiRetResponse(cmd, (cmd.ToLower() == BaiduwmConsts.CREATE_ORDER_API ? parm : null), BaiduwmConsts.API_SUCCESS, source, secret);
                LogUtil.Info(string.Format("{0},requestId:{1},返回:{2}", actionDesc, requestId, result.ToJson()));
                return result;
            }
        }

        #endregion

        #region 微云打

        /// <summary>
        /// 订单打印状态回调
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object UpdatePrintStatusCallBack()
        {
            //1.校验请求参数
            var actionDesc = "微云打订单打印状态回调";
            var errMsg = string.Empty;
            var billCode = string.Empty;
            var billNo = string.Empty;
            var queryStrings = string.Empty;
            var sortDics = new SortedDictionary<string, object>();
            var requestForm = base.GetRequestParams(out sortDics, out queryStrings, true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count <= 0)
            {
                errMsg = string.Format("{0}参数错误！", actionDesc);
                LogUtil.Error(errMsg);
                return Failed(errMsg);
            }
            else
            {
                //格式：小票类型ID#微云打订单ID_份数，如：3#64f8d392535c42aab010307594f39855_2
                billNo = requestForm["bill_no"] as string;
                if (string.IsNullOrWhiteSpace(billNo))
                {
                    return errMsg;
                }
                else
                {
                    var billnos = billNo.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    if (billnos.Length == 2)
                    {
                        var nos = billnos[1].ToString().Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        billCode = nos[0];
                    }
                    else
                    {
                        return errMsg;
                    }
                }
            }

            //2.校验请求合法性，验证签名
            var sign = requestForm["sign"] as string;
            var mySign = MD5Util.GetSign(sortDics, ConfigUtil.McpSignKey);
            if (sign == null || !sign.Equals(mySign, System.StringComparison.CurrentCultureIgnoreCase))
            {
                LogUtil.Error(string.Format("{0}签名不合法，请求参数：{1}，微云打订单平台签名：{2}", actionDesc, queryStrings, mySign));
                return Failed("签名验证失败！");
            }

            //3.执行订单打印状态回调数据同步【重复执行】
            var resultCode = requestForm["result_code"] as string;
            var faultTime = requestForm["fault_time"] as string;
            var printerCode = requestForm["printer_code"] as string;
            if (!string.IsNullOrWhiteSpace(resultCode))
            {
                //3.1 发送提醒通知【订单打印失败】
                if (resultCode != PrintResultType.Success.GetHashCode().ToString())
                {
                    var order = SingleInstance<OrderBLL>.Instance.GetOrder(billCode);
                    if (order != null)
                    {
                        lock (_syncPrintObject)
                        {
                            var cacheKey = string.Format("{0}#{1}#{2}#{3}", order.PlatformId, order.OrderId, printerCode, resultCode);
                            var cacheMsgVal = cache.GetCache<string>(cacheKey);
                            if (string.IsNullOrWhiteSpace(cacheMsgVal))
                            {
                                var opTime = !string.IsNullOrWhiteSpace(faultTime) ? faultTime.ToDate() : TimeUtil.Now;
                                SingleInstance<MsgBLL>.Instance.SendPrintFailPushMsg(printerCode, resultCode, opTime, order.OrderId, order.PlatformId, order.DaySeq, order.OrderViewId);
                                cache.WriteCache("S", cacheKey, TimeUtil.Now.AddMinutes(1));
                            }
                            else
                            {
                                LogUtil.Info(string.Format("成功过滤打印失败提醒订单：{0}", cacheKey));
                            }
                        }
                    }
                }

                //3.2 更新订单打印状态
                var handleResult = false;
                var status = PrintStatus.WaitPrint;
                if (resultCode == PrintResultType.Success.GetHashCode().ToString())
                {
                    //打印成功
                    status = PrintStatus.Printed;
                }
                else if (resultCode == PrintResultType.Fail.GetHashCode().ToString() || resultCode == PrintResultType.UnBindClear.GetHashCode().ToString())
                {
                    //打印失败
                    status = PrintStatus.PrintFail;
                }
                else if (resultCode == PrintResultType.TimeOutUnPrint.GetHashCode().ToString())
                {
                    //超时未打
                    status = PrintStatus.TimeOutUnPrint;
                }
                else
                {
                    //待打印
                    status = PrintStatus.WaitPrint;
                }
                handleResult = SingleInstance<OrderBLL>.Instance.SyncOrderPrintStatusData(billCode, status, resultCode);
                if (handleResult)
                {
                    LogUtil.Info(string.Format("{0}成功,微云打订单ID:{1}", actionDesc, billCode));
                    return OK();
                }
                else
                {
                    errMsg = string.Format("{0}失败,微云打订单ID:{1}", actionDesc, billCode);
                    LogUtil.Error(errMsg);
                    return Failed(errMsg);
                }
            }
            else
            {
                LogUtil.Error(string.Format("{0}核心字符串为空。", actionDesc));
                return Failed(errMsg);
            }
        }

        /// <summary>
        /// 打印机状态回调通知
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object PrinterStatusCallBack()
        {
            //1.校验请求参数
            var actionDesc = "打印机状态更改回调";
            var errMsg = string.Empty;
            var queryStrings = string.Empty;
            var sortDics = new SortedDictionary<string, object>();
            var requestForm = base.GetRequestParams(out sortDics, out queryStrings, true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count <= 0)
            {
                errMsg = string.Format("{0}参数错误！", actionDesc);
                LogUtil.Error(errMsg);
                return Failed(errMsg);
            }

            //2.校验请求合法性，验证签名
            var sign = requestForm["sign"] as string;
            var mySign = MD5Util.GetSign(sortDics, ConfigUtil.McpSignKey);
            if (sign == null || !sign.Equals(mySign, System.StringComparison.CurrentCultureIgnoreCase))
            {
                LogUtil.Error(string.Format("{0}签名不合法,请求参数：{1},微云打订单平台签名：{2}", actionDesc, queryStrings, mySign));
                return Failed("签名验证失败！");
            }

            //3.发送提醒通知
            var resultCode = requestForm["result_code"] as string;
            var faultTime = requestForm["fault_time"] as string;
            var printerCode = requestForm["printer_code"] as string;
            var opTime = !string.IsNullOrWhiteSpace(faultTime) ? faultTime.ToDate() : TimeUtil.Now;
            lock (_syncPrintObject)
            {
                var cacheVal = cache.GetCache<string>(printerCode);
                if (string.IsNullOrWhiteSpace(cacheVal) || cacheVal != resultCode)
                {
                    cache.WriteCache(resultCode, printerCode, TimeUtil.Now.AddMinutes(60));
                    if (resultCode.ToInt() != PrinterFaultType.Enable.GetHashCode())
                    {
                        for (var t = 1; t <= tryMax; t++)
                        {
                            try
                            {
                                errMsg = SingleInstance<MsgBLL>.Instance.SendPriterFaultPushMsg(printerCode, resultCode, opTime);
                            }
                            catch (Exception ex)
                            {
                                errMsg = ex.Message;
                                LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                            }

                            if (string.IsNullOrEmpty(errMsg))
                            {
                                LogUtil.Info(string.Format("{0}成功！", actionDesc));
                                break;
                            }
                            else
                            {
                                Thread.Sleep(delayTryInterval);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        errMsg = string.Empty;
                    }
                    return string.IsNullOrWhiteSpace(errMsg) ? OK() : Failed(errMsg);
                }
                else
                {
                    LogUtil.Info(string.Format("成功过滤打印机状态更新提醒：{0}", printerCode));
                    return OK();
                }
            }
        }

        #endregion

        #region 私有方法

        #region 平台通用

        /// <summary>
        /// 执行门店解除授权数据同步
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="pType"></param>
        /// <param name="authType"></param>
        /// <param name="t"></param>
        private bool SyncUnMapShopPlatformData(long shopId, PlatformType pType, AuthBussinessType authType, int t)
        {
            LogUtil.Info(string.Format("第{0}次尝试执行门店解除授权数据同步。", t));
            return SingleInstance<PlatformBLL>.Instance.SyncUnMapShopPlatformData(shopId, pType, authType) > 0;
        }

        /// <summary>
        /// 执行门店授权数据同步
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="appAuthToken"></param>
        /// <param name="expiresIn"></param>
        /// <param name="refreshToken"></param>
        /// <param name="pType"></param>
        /// <param name="authType"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool SyncShopPlatformData(string shopId, string appAuthToken, long expiresIn, string refreshToken, PlatformType pType, AuthBussinessType authType, int t, string pShopId = "", string source = "", string secret = "", string remark = "")
        {
            LogUtil.Info(string.Format("第{0}次尝试执行门店授权数据同步。", t));
            return SingleInstance<PlatformBLL>.Instance.SyncShopPlatformData(shopId, appAuthToken, expiresIn, refreshToken, pType, authType, BusinessType.Waimai, pShopId, source, secret, remark) > 0;
        }

        /// <summary>
        /// 执行平台新订单数据同步
        /// </summary>
        /// <param name="platfromId"></param>
        /// <param name="orderCallBackString"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool SyncNewOrderData(long platfromId, string orderCallBackString, int t, string requestId = "")
        {
            LogUtil.Info(string.Format("第{0}次尝试同步【{1}】新订单数据,消息ID：{2}。", t, ((PlatformType)platfromId).GetRemark(), requestId));
            return SingleInstance<OrderBLL>.Instance.HandleReceivePlatformOrder(platfromId, orderCallBackString, requestId);
        }

        /// <summary>
        /// 执行平台订单取消数据同步
        /// </summary>
        /// <param name="platfromId"></param>
        /// <param name="orderCallBackString"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool SyncCancelOrderData(long platfromId, string orderCallBackString, int t, string requestId = "", string pShopId = "")
        {
            LogUtil.Info(string.Format("第{0}次尝试同步【{1}】订单取消数据,消息ID：{2}。", t, ((PlatformType)platfromId).GetRemark(), requestId));
            return SingleInstance<OrderBLL>.Instance.HandleCancelPlatformOrder(platfromId, orderCallBackString, pShopId);
        }

        /// <summary>
        /// 执行平台订单完成数据同步
        /// </summary>
        /// <param name="platfromId"></param>
        /// <param name="orderCallBackString"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool SyncCompleteOrderData(long platfromId, string orderCallBackString, int t, string requestId = "", string pShopId = "")
        {
            LogUtil.Info(string.Format("第{0}次尝试同步【{1}】订单完成数据,消息ID：{2}。", t, ((PlatformType)platfromId).GetRemark(), requestId));
            return SingleInstance<OrderBLL>.Instance.HandleCompletePlatformOrder(platfromId, orderCallBackString, pShopId);
        }

        /// <summary>
        /// 执行平台订单配送状态数据同步
        /// </summary>
        /// <param name="platfromId"></param>
        /// <param name="orderCallBackString"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool SyncDeliveryStatusOrderData(long platfromId, string orderCallBackString, int t, string requestId = "", string pShopId = "")
        {
            LogUtil.Info(string.Format("第{0}次尝试同步【{1}】订单配送数据,消息ID：{2}。", t, ((PlatformType)platfromId).GetRemark(), requestId));
            return SingleInstance<OrderBLL>.Instance.HandleDeliveryPlatformOrder(platfromId, orderCallBackString, pShopId);
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

        #region 美团外卖专用

        /// <summary>
        /// 【美团外卖】推送消息处理
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="actionDesc">操作描述</param>
        /// <param name="keyName">关键字段名称</param>
        /// <returns></returns>
        private object HandleMeituanPushMsgSyncData(MeiPushMsgType msgType, string actionDesc, string keyName)
        {
            //1.校验请求参数
            var queryStrings = string.Empty;
            var sortDics = new SortedDictionary<string, object>();
            var requestForm = base.GetRequestParams(out sortDics, out queryStrings, true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count <= 0)
            {
                return MeiMsg(MeituanConsts.RETURN_FAIL);
            }

            //2.校验请求合法性，验证签名
            var sign = requestForm["sign"] as string;
            var mySign = SignUtil.GetMeituanSign(sortDics, ConfigUtil.MeiSignKey);
            if (!sign.Equals(mySign, System.StringComparison.CurrentCultureIgnoreCase))
            {
                LogUtil.Error(string.Format("{0}签名不合法，请求参数：{0}，微云打订单平台签名：{1}", actionDesc, queryStrings, mySign));
                return MeiMsg(MeituanConsts.RETURN_FAIL);
            }
            else
            {
                //美团测试账号配置跳转至测试环境
                var shopId = requestForm["ePoiId"] as string;
                if (ConfigUtil.MeiIsToTest && ConfigUtil.MeiToTestShopIds.Contains(shopId))
                {
                    return MeiToTest(msgType,queryStrings, actionDesc);
                }
                else
                {
                    //3.执行订单配送状态回调数据同步【重复执行】
                    var coreCallbackStr = requestForm[keyName] as string;
                    var platformId = PlatformType.Meituan.GetHashCode();
                    if (!string.IsNullOrWhiteSpace(coreCallbackStr))
                    {
                        var t = 1;
                        var handleResult = false;
                        try
                        {
                            if (msgType == MeiPushMsgType.NewOrder)
                            {
                                //新订单回调
                                handleResult = SyncNewOrderData(platformId, coreCallbackStr, t);
                            }
                            else if (msgType == MeiPushMsgType.Delivering)
                            {
                                //配送回调
                                handleResult = SyncDeliveryStatusOrderData(platformId, coreCallbackStr, t);
                            }
                            else if (msgType == MeiPushMsgType.CompleteOrder)
                            {
                                //完成回调
                                handleResult = SyncCompleteOrderData(platformId, coreCallbackStr, t);
                            }
                            else if (msgType == MeiPushMsgType.CancelOrder)
                            {
                                //取消回调
                                handleResult = SyncCancelOrderData(platformId, coreCallbackStr, t);
                            }
                        }
                        catch (Exception ex)
                        {
                            handleResult = false;
                            LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                        }

                        LogUtil.Info(string.Format("{0}{1}！", actionDesc, (handleResult ? "成功" : "失败")));
                        return MeiMsg(handleResult ? MeituanConsts.API_RETURN_OK : MeituanConsts.RETURN_FAIL);
                    }
                    else
                    {
                        LogUtil.Error(string.Format("{0}核心字符串为空。", actionDesc));
                        return MeiMsg(MeituanConsts.RETURN_FAIL);
                    }
                }
            }
        }

        /// <summary>
        /// 跳转至测试环境
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="actionDesc"></param>
        /// <returns></returns>
        private object MeiToTest(MeiPushMsgType msgType, string queryStrings, string actionDesc)
        {
            var handleResult = false;
            var apiUrl = string.Empty;
            var apiDomain = "http://order.yinmei.me/api/callback/{0}";
            if (msgType == MeiPushMsgType.NewOrder)
            {
                //新订单回调
                apiUrl = string.Format(apiDomain, "MeiReceiveOrderCallBack");
            }
            else if (msgType == MeiPushMsgType.Delivering)
            {
                //配送回调
                apiUrl = string.Format(apiDomain, "MeiDeliveryStatusCallBack");
            }
            else if (msgType == MeiPushMsgType.CompleteOrder)
            {
                //完成回调
                apiUrl = string.Format(apiDomain, "MeiCompleteOrderCallBack");
            }
            else if (msgType == MeiPushMsgType.CancelOrder)
            {
                //取消回调
                apiUrl = string.Format(apiDomain, "MeiCancelOrderCallBack");
            }

            //调用测试环境相关接口
            if (!string.IsNullOrWhiteSpace(apiUrl) && !string.IsNullOrWhiteSpace(queryStrings))
            {
                var result = HttpRequestUtil.HttpPost(apiUrl, queryStrings);
                var apiData = JsonUtil.ToObject<Dictionary<string, string>>(result);
                handleResult = apiData != null ? (apiData["data"] == MeituanConsts.API_RETURN_OK) : false;
                LogUtil.Info(string.Format("{0}{1}！", actionDesc, (handleResult ? "转至测试环境成功" : "转至测试环境失败")));
                return result;
            }
            else
            {
                LogUtil.Info(string.Format("{0}{1}！", actionDesc, "转至测试环境失败，参数有误"));
                return MeiMsg(MeituanConsts.RETURN_FAIL);
            }
        }

        #endregion

        #region 饿了么专用

        /// <summary>
        /// 【饿了么】订单通用回调
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="pushMsgCoreString">推送核心消息内容</param>
        /// <param name="requestId">请求消息Id</param>
        /// <param name="eShopId">饿了么门店</param>
        /// <returns></returns>
        private object HandleElemePushMsgSyncData(int msgType, string pushMsgCoreString, string requestId, string eShopId = "")
        {
            var actionDesc = GetElemeActionDesc(msgType);
            LogUtil.Info(string.Format("{0}推送消息：{1}", actionDesc, pushMsgCoreString));
            if (!string.IsNullOrWhiteSpace(pushMsgCoreString))
            {
                var t = 1;
                var handleResult = false;
                var platformId = PlatformType.Eleme.GetHashCode();
                try
                {
                    if (msgType == ElemePushMsgType.NewOrder.GetHashCode())
                    {
                        handleResult = SyncNewOrderData(platformId, pushMsgCoreString, t, requestId);
                    }
                    else if (msgType == ElemePushMsgType.Delivering.GetHashCode())
                    {
                        handleResult = SyncDeliveryStatusOrderData(platformId, pushMsgCoreString, t, requestId, eShopId);
                    }
                    else if (msgType == ElemePushMsgType.CompleteOrder.GetHashCode())
                    {
                        handleResult = SyncCompleteOrderData(platformId, pushMsgCoreString, t, requestId, eShopId);
                    }
                    else if (msgType == ElemePushMsgType.CancelOrder.GetHashCode() || msgType == ElemePushMsgType.InvalidOrder.GetHashCode() || msgType == ElemePushMsgType.ForceInvalid.GetHashCode())
                    {
                        handleResult = SyncCancelOrderData(platformId, pushMsgCoreString, t, requestId, eShopId);
                    }
                }
                catch (Exception ex)
                {
                    handleResult = false;
                    LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                }

                LogUtil.Info(string.Format("消息ID：{0},{1}{2}！", requestId, actionDesc, (handleResult ? "成功" : "失败")));
                return EleMsg(handleResult ? ElemeConsts.API_RETURN_OK : ElemeConsts.RETURN_FAIL);
            }
            else
            {
                LogUtil.Error(string.Format("{0}核心字符串为空。", actionDesc));
                return EleMsg(ElemeConsts.RETURN_FAIL);
            }
        }

        /// <summary>
        /// 获取饿了么回调操作描述
        /// </summary>
        /// <param name="msgType">回调操作类型</param>
        /// <returns>回调操作描述</returns>
        private string GetElemeActionDesc(int msgType)
        {
            var actionDesc = string.Empty;
            switch (msgType)
            {
                case (int)ElemePushMsgType.NewOrder:
                    {
                        //新订单
                        actionDesc = "饿了么新订单回调";
                        break;
                    }
                case (int)ElemePushMsgType.Delivering:
                    {
                        //配送中
                        actionDesc = "饿了么订单配送回调";
                        break;
                    }
                case (int)ElemePushMsgType.CompleteOrder:
                    {
                        //已完成
                        actionDesc = "饿了么订单完成回调";
                        break;
                    }
                case (int)ElemePushMsgType.CancelOrder:
                case (int)ElemePushMsgType.InvalidOrder:
                case (int)ElemePushMsgType.ForceInvalid:
                    {
                        //已取消
                        actionDesc = "饿了么订单取消回调";
                        break;
                    }
                default:
                    break;
            }
            return actionDesc;
        }

        #endregion

        #region 百度外卖专用

        /// <summary>
        /// 【百度外卖】推送消息处理
        /// </summary>
        /// <param name="cmd">接口命令</param>
        /// <param name="source">百度商户账号</param>
        /// <param name="secret">百度密钥</param>
        /// <param name="pushMsgCoreString">消息内容字符串</param>
        /// <param name="parms">消息内容字典</param>
        /// <param name="requestId">消息请求标识</param>
        /// <returns>消息处理结果</returns>
        private object HandleBaiduPushMsgSyncData(string cmd, string source, string secret, string pushMsgCoreString, Dictionary<string, object> parms, string requestId)
        {
            int status = -1;
            var body = new Dictionary<string, object>();
            body.Add("data", BaiduwmConsts.API_SUCCESS_DEFAULT_DATA);

            if (cmd.ToLower() == BaiduwmConsts.CREATE_ORDER_API)
            {
                //新订单-待确认状态
                status = BaiduOrderStatusType.WaitConfirm.GetHashCode();
            }
            else if (cmd.ToLower() == BaiduwmConsts.PUSH_ORDER_STATUS_API)
            {
                //订单状态推送
                status = parms.ContainsKey("status") && parms["status"] != null ? parms["status"].ToInt() : 0;
            }

            if (status >= 0)
            {
                //订单状态处理
                var bOrderId = string.Empty;
                if (status == BaiduOrderStatusType.WaitConfirm.GetHashCode())
                {
                    //新订单
                    bOrderId = parms.ContainsKey("order_id") && parms["order_id"] != null ? parms["order_id"].ToString() : string.Empty;
                }

                return HandleBaiduPushMsgSyncData(status, cmd, pushMsgCoreString, source, secret, requestId, bOrderId);
            }
            else
            {
                var result = parms.ContainsKey("bind_result") && parms["bind_result"] != null ? parms["bind_result"].ToString().ToInt() : 0;
                if (result >= 1)
                {
                    //门店(取消)授权回调
                    return HandleBaiduShopAuthSyncData(cmd, parms, source, secret, requestId);
                }
                else
                {
                    var actionDesc = string.Empty;
                    if (cmd.ToLower() == BaiduwmConsts.SHOP_AUTH_API)
                    {
                        actionDesc = "百度外卖门店授权回调";
                    }
                    else if (cmd.ToLower() == BaiduwmConsts.SHOP_UNAUTH_API)
                    {
                        actionDesc = "百度外卖解除门店授权回调";
                    }

                    LogUtil.Error(string.Format("{0},参数：{1}-{2}", actionDesc, requestId, parms.ToJson()));
                    return SingleInstance<PlatformBLL>.Instance.GetBaiApiRetResponse(cmd, null, actionDesc, source, secret);
                }
            }
        }

        /// <summary>
        /// 【百度外卖】订单配送状态回调
        /// </summary>
        /// <param name="pushMsgCoreString">推送核心消息内容</param>
        /// <param name="requestId">推送消息标识</param>
        /// <returns>回调结果</returns>
        private object HandleBaiduPushMsgSyncData(int msgType, string cmd, string pushMsgCoreString, string source, string secret, string requestId, string bOrderId = "")
        {
            var actionDesc = GetBaiduActionDesc(msgType);
            if (!string.IsNullOrEmpty(bOrderId))
            {
                var bOrder = SingleInstance<PlatformBLL>.Instance.GetBaiOrderInfo(bOrderId, source, secret);
                pushMsgCoreString = (bOrder != null && bOrder.data != null) ? bOrder.data.ToString() : string.Empty;
            }
            else
            {
                LogUtil.Info(string.Format("{0}推送消息：{1}", actionDesc, pushMsgCoreString));
            }

            if (!string.IsNullOrWhiteSpace(pushMsgCoreString))
            {
                var t = 1;
                var handleResult = false;
                var platformId = PlatformType.Baidu.GetHashCode();
                try
                {
                    if (msgType == BaiduOrderStatusType.WaitConfirm.GetHashCode())
                    {
                        //新订单
                        handleResult = SyncNewOrderData(platformId, pushMsgCoreString, t, requestId);
                    }
                    else if (msgType == BaiduOrderStatusType.Delivering.GetHashCode())
                    {
                        //配送中
                        handleResult = SyncDeliveryStatusOrderData(platformId, pushMsgCoreString, t, requestId);
                    }
                    else if (msgType == BaiduOrderStatusType.Completed.GetHashCode())
                    {
                        //已完成
                        handleResult = SyncCompleteOrderData(platformId, pushMsgCoreString, t, requestId);
                    }
                    else if (msgType == BaiduOrderStatusType.Canceled.GetHashCode())
                    {
                        //已取消
                        handleResult = SyncCancelOrderData(platformId, pushMsgCoreString, t, requestId);
                    }
                }
                catch (Exception ex)
                {
                    handleResult = false;
                    LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                }

                LogUtil.Info(string.Format("消息ID：{0},{1}{2}！", requestId, actionDesc, (handleResult ? "成功" : "失败")));
                return SingleInstance<PlatformBLL>.Instance.GetBaiApiRetResponse(cmd, null, (handleResult ? BaiduwmConsts.API_SUCCESS : string.Format("{0}失败", actionDesc)), source, secret);
            }
            else
            {
                var error = string.Format("{0}核心字符串为空。", actionDesc);
                LogUtil.Error(error);
                return SingleInstance<PlatformBLL>.Instance.GetBaiApiRetResponse(cmd, null, error, source, secret);
            }
        }

        /// <summary>
        /// 【百度外卖】门店(取消)授权回调
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parms">推送核心消息内容</param>
        /// <param name="source"></param>
        /// <param name="secret"></param>
        /// <param name="requestId">推送消息标识</param>
        /// <returns></returns>
        private object HandleBaiduShopAuthSyncData(string cmd, Dictionary<string, object> parms, string source, string secret, string requestId)
        {
            var actionDesc = string.Empty;
            var platformType = PlatformType.Baidu;
            if (cmd.ToLower() == BaiduwmConsts.SHOP_AUTH_API)
            {
                actionDesc = "百度外卖门店授权回调";
            }
            else if (cmd.ToLower() == BaiduwmConsts.SHOP_UNAUTH_API)
            {
                actionDesc = "百度外卖解除门店授权回调";
            }

            if (parms.Count > 0)
            {
                long shopId = 0;
                var mapShopId = string.Empty;
                var supplierId = string.Empty;
                var body = new Dictionary<string, object>();
                body.Add("data", BaiduwmConsts.API_SUCCESS_DEFAULT_DATA);
                var bShopList = parms["shop_list"] != null ? JsonUtil.ToList<BaiduShopModel>(parms["shop_list"].ToString()) : new List<BaiduShopModel>();
                var bShopId = bShopList.Count > 0 ? bShopList[0].baidu_shop_id : string.Empty;
                if (cmd.ToLower() == BaiduwmConsts.SHOP_AUTH_API)
                {
                    //授权参数
                    supplierId = bShopList.Count > 0 ? bShopList[0].supplier_id : string.Empty;
                    var shop = SingleInstance<PlatformBLL>.Instance.GetBaiShopInfo(bShopId, source, secret);
                    var bShop = Yme.Util.JsonUtil.ToObject<Dictionary<string, object>>((shop != null && shop.data != null) ? shop.data.ToString() : string.Empty);
                    mapShopId = (bShop != null && bShop["shop_id"] != null) ? bShop["shop_id"].ToString() : string.Empty;
                }
                else if (cmd.ToLower() == BaiduwmConsts.SHOP_UNAUTH_API)
                {
                    //解除授权参数
                    var authPlatform = SingleInstance<PlatformBLL>.Instance.GetShopPlatformInfo(platformType.GetHashCode(), bShopId);
                    shopId = authPlatform != null ? authPlatform.ShopId : 0;
                }

                //执行同步操作
                var handleResult = false;
                for (var t = 1; t <= tryMax; t++)
                {
                    try
                    {
                        if (cmd.ToLower() == BaiduwmConsts.SHOP_AUTH_API)
                        {
                            handleResult = SyncShopPlatformData(mapShopId, secret, 0, source, platformType, AuthBussinessType.Waimai, t, bShopId, source, secret, supplierId);
                        }
                        else if (cmd.ToLower() == BaiduwmConsts.SHOP_UNAUTH_API)
                        {
                            handleResult = SyncUnMapShopPlatformData(shopId, platformType, AuthBussinessType.Waimai, t);
                        }
                    }
                    catch (Exception ex)
                    {
                        handleResult = false;
                        LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                    }

                    if (handleResult)
                    {
                        LogUtil.Info(string.Format("{0}成功！", actionDesc));
                        break;
                    }
                    else
                    {
                        Thread.Sleep(delayTryInterval);
                        continue;
                    }
                }
                return SingleInstance<PlatformBLL>.Instance.GetBaiApiRetResponse(cmd, (handleResult ? body : null), (handleResult ? BaiduwmConsts.API_SUCCESS : string.Format("{0}失败", actionDesc)), source, secret);
            }
            else
            {
                var error = string.Format("{0}核心字符串为空。", actionDesc);
                LogUtil.Error(error);
                return SingleInstance<PlatformBLL>.Instance.GetBaiApiRetResponse(cmd, null, error, source, secret);
            }
        }

        /// <summary>
        /// 获取百度外卖回调操作描述
        /// </summary>
        /// <param name="msgType">回调操作类型</param>
        /// <returns>回调操作描述</returns>
        private string GetBaiduActionDesc(int msgType)
        {
            var actionDesc = string.Empty;
            switch (msgType)
            {
                case (int)BaiduOrderStatusType.WaitConfirm:
                    {
                        //新订单
                        actionDesc = "百度外卖新订单回调";
                        break;
                    }
                case (int)BaiduOrderStatusType.Delivering:
                    {
                        //配送中
                        actionDesc = "百度外卖订单配送回调";
                        break;
                    }
                case (int)BaiduOrderStatusType.Completed:
                    {
                        //已完成
                        actionDesc = "百度外卖订单完成回调";
                        break;
                    }
                case (int)BaiduOrderStatusType.Canceled:
                    {
                        //已取消
                        actionDesc = "百度外卖订单取消回调";
                        break;
                    }
                default:
                    break;
            }
            return actionDesc;
        }

        #endregion

        #endregion

        #region 测试接口

        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object QueryMeiOrder()
        {
            var requestForms = base.GetRequestParams();
            var id = requestForms["id"] ?? string.Empty;
            var token = requestForms["token"] ?? string.Empty;

            var ret = SingleInstance<OrderBLL>.Instance.QueryMeiOrder(id, token);
            return OK(ret);
        }

        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object CallbackTest()
        {
            var handleResult = false;
            for (var t = 1; t <= tryMax; t++)
            {
                try
                {
                    handleResult = SyncData(t);
                }
                catch (Exception ex)
                {
                    handleResult = false;
                    LogUtil.Error(string.Format("失败，参考信息：{0}", ex.Message));
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
           
            if (handleResult)
            {
                LogUtil.Info("成功！");
                return OK();
            }
            else
            {
                LogUtil.Error("失败！");
                return Failed("失败！");
            }
        }

        private bool SyncData(int t)
        {
            LogUtil.Info(string.Format("第{0}次尝试数据同步。", t));
            if (t % 2 == 0)
            {
                throw new Exception("测试错误！");
            }
            else
            {
                if (t == 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}

