using System;
using System.Linq;
using System.Collections.Generic;
using Yme.Util;
using Yme.Util.Security;
using Yme.Mcp.Model.Definitions;
using Yme.Mcp.Model.Waimai.Meituan;
using Yme.Mcp.Model.QueryModels;
using Yme.Util.Extension;
using Yme.Mcp.Model.Enums;
using System.Text;
using Yme.Util.Log;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Service.BaseManage;

namespace Yme.Mcp.Service.Common
{
    /// <summary>
    /// 美团集成服务
    /// </summary>
    public class MeituanService
    {
        private IShopPrinterService spServ = new ShopPrinterService();

        #region 心跳服务

        /// <summary>
        /// 发送普通心跳
        /// </summary>
        public void SendCommonHeartbeat()
        {
            try
            {
                var deviceList = spServ.GetList(PlatformType.Meituan);
                var devices = (from d in deviceList
                               select new
                               {
                                   ePoiId = d.ShopId.ToString(),
                                   posId = d.PrinterCode
                               }).ToList();

                var parms = new SortedDictionary<string, object>();
                parms.Add("developerId", ConfigUtil.MeiDeveloperId);
                parms.Add("time", TimeUtil.SecondTicks_1970);
                parms.Add("list", devices);

                var datastr = JsonUtil.ToJson(parms);
                var data = new SortedDictionary<string, object>();
                data.Add("data", datastr);

                var sign = SignUtil.GetMeituanSign(data, ConfigUtil.MeiSignKey);
                var url = MeituanConsts.COMMON_HEARBEAT_API;
                var pars = string.Format("data={0}&sign={1}", datastr, sign);
                var result = HttpRequestUtil.HttpPost(url, pars);

                LogUtil.Info(string.Format("发送普通心跳,返回：{0}", result));
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("发送普通心跳失败,参考信息：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 发送补充数据【待完善】
        /// </summary>
        /// <returns></returns>
        public void SendUploadData()
        {
            try
            {
                var parms = new SortedDictionary<string, object>();
                var datastr = JsonUtil.ToJson(parms);
                var data = new SortedDictionary<string, object>();
                data.Add("data", datastr);

                var sign = SignUtil.GetMeituanSign(data, ConfigUtil.MeiSignKey);
                var url = MeituanConsts.COMMON_HEARBEAT_API;
                var pars = string.Format("data={0}&sign={1}", datastr, sign);
                var result = HttpRequestUtil.HttpPost(url, pars);

                LogUtil.Info(string.Format("发送补充数据,返回：{0}", result));
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("发送补充数据失败,参考信息：{0}", ex.Message));
            }
        }

        #endregion

        #region 解析订单

        public MeituanOrderModel AnalysisOrder(string responseString)
        {
            responseString = responseString.Replace("??", string.Empty);
            MeituanOrderModel orderModel = null;
            if (!string.IsNullOrWhiteSpace(responseString))
            {
                orderModel = JsonUtil.ToObject<MeituanOrderModel>(responseString);
            }
            return orderModel;
        }

        #endregion

        #region 解析订单明细

        public List<OrderDetailQueryModel> AnalysisOrderDetail(string detailJson)
        {
            detailJson = detailJson.Replace("??", string.Empty);
            var details = new List<OrderDetailQueryModel>();
            var mobject = JsonUtil.ToObject<List<MeituanOrderDetailModel>>(detailJson);
            if (mobject != null && mobject.Count > 0)
            {
                mobject.ForEach(d =>
                    {
                        details.Add(new OrderDetailQueryModel
                        {
                            Name = d.food_name,
                            Property = GetProductPropertyStr(d.spec, d.food_property),
                            Qty = string.Format("X{0}", d.quantity),
                            Price = d.price.ToDecimal(2),
                            GroupId = d.cart_id + 1,
                            InnerId = d.sku_id
                        });
                    });
            }
            return details;
        }

        /// <summary>
        /// 获取产品规格、属性字符串
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        private string GetProductPropertyStr(string attr, string features)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(attr))
            {
                sb.AppendFormat("({0}", attr);
            }
            if (!string.IsNullOrEmpty(features))
            {
                if (!string.IsNullOrEmpty(attr))
                {
                    sb.AppendFormat(",{0}", features);
                }
                else
                {
                    sb.AppendFormat("({0}", features);
                }
            }
            if (!string.IsNullOrWhiteSpace(sb.ToString()))
            {
                sb.Append(")");
            }
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="appAuthToken"></param>
        /// <returns></returns>
        public string ConfirmOrder(long orderId, string appAuthToken)
        {
            var errMsg = string.Empty;
            var parms = new SortedDictionary<string, object>();
            parms.Add("orderId", orderId);
            parms.Add("appAuthToken", appAuthToken);
            parms.Add("charset", "utf-8");
            parms.Add("timestamp", TimeUtil.SecondTicks_1970);
            var sign = SignUtil.GetMeituanSign(parms, ConfigUtil.MeiSignKey);

            var parmStr = new StringBuilder();
            var url = MeituanConsts.CONFIRM_ORDER_API;
            foreach (var parm in parms)
            {
                parmStr.AppendFormat("{0}={1}&", parm.Key, parm.Value);
            }
            var pars = string.Format("{0}sign={1}", parmStr.ToString(), sign);

            try
            {
                var result = HttpRequestUtil.HttpPost(url, pars);
                LogUtil.Info(string.Format("调用确认订单接口,返回：{0}", result));
                if (result.ToUpper().IndexOf("\"" + MeituanConsts.API_RETURN_OK + "\"") <= -1)
                {
                    errMsg = result;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.InnerException.ToString();
            }
            return errMsg;
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="appAuthToken"></param>
        /// <returns></returns>
        public MeituanOrderModel QueryOrder(string orderId, string appAuthToken)
        {
            MeituanOrderModel orderModel = null;
            try
            {
                var parms = new SortedDictionary<string, object>();
                parms.Add("orderId", orderId);
                parms.Add("appAuthToken", appAuthToken);
                parms.Add("charset", "utf-8");
                parms.Add("timestamp", TimeUtil.SecondTicks_1970);
                var sign = SignUtil.GetMeituanSign(parms, ConfigUtil.MeiSignKey);

                var parmStr = new StringBuilder();
                var url = MeituanConsts.QUERY_ORDER_BYID_API;
                foreach (var parm in parms)
                {
                    parmStr.AppendFormat("{0}={1}&", parm.Key, parm.Value);
                }
                var pars = string.Format("{0}sign={1}", parmStr.ToString(), sign);
                var result = HttpRequestUtil.HttpPost(url, pars);
                LogUtil.Info(string.Format("调用查询订单接口,返回：{0}", result));
                var retValue = JsonUtil.ToObject<ApiDataQueryModel>(result);
                orderModel = JsonUtil.ToObject<MeituanOrderModel>(retValue.data.ToString());
            }
            catch (Exception ex)
            {
                var msg = string.Format("查询美团订单失败，订单Id:{0},访问令牌：{1},参考信息:{2}", orderId, appAuthToken, ex.InnerException);
                LogUtil.Error(msg);
            }
            return orderModel;
        }

        /// <summary>
        /// 查询美团门店信息
        /// </summary>
        /// <returns></returns>
        public MeituanShopModel GetMeituanShop(string appAuthToken, string ePoiIds)
        {
            MeituanShopModel model = null;
            var parms = new SortedDictionary<string, object>();
            parms.Add("appAuthToken", appAuthToken);
            parms.Add("charset", "utf-8");
            parms.Add("timestamp", TimeUtil.SecondTicks_1970);
            parms.Add("ePoiIds", ePoiIds);
            var sign = SignUtil.GetMeituanSign(parms, ConfigUtil.MeiSignKey);

            var parmStr = new StringBuilder();
            var url = MeituanConsts.QUERY_SHOP_INFO_API;
            foreach (var parm in parms)
            {
                parmStr.AppendFormat("{0}={1}&", parm.Key, parm.Value);
            }
            var pars = string.Format("{0}sign={1}", parmStr.ToString(), sign);

            try
            {
                var result = HttpRequestUtil.HttpGet(url, pars);
                LogUtil.Info(string.Format("调用查询美团门店接口,返回：{0}", result));
                var list = JsonUtil.ToObject<List<MeituanShopModel>>(result.Replace("{\"data\":", string.Empty).TrimEnd('}'));
                model = (list != null && list.Count > 0) ? list.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("调用查询美团门店接口失败,参考信息：{0}", ex.InnerException.ToString()));
            }
            return model;
        }
    }
}
