using System;
using System.Collections.Generic;
using System.Linq;
using Yme.Mcp.Model.Definitions;
using Yme.Mcp.Model.QueryModels;
using Yme.Mcp.Model.ViewModels.Waimai.Baidu;
using Yme.Util;
using Yme.Util.Log;
using Yme.Util.Security;
using Yme.Util.Extension;
using System.Text;

namespace Yme.Mcp.Service.Common
{
    /// <summary>
    /// 百度外卖服务
    /// </summary>
    public class BaiduwmService
    {
        #region 业务接口

        /// <summary>
        /// 获取门店信息【百度外卖】
        /// </summary>
        /// <param name="bShopId">饿了么门店Id</param>
        /// <param name="source">合作方账号</param>
        /// <param name="secret">密钥</param>]
        /// <param name="encrypt">加密方式</param>
        /// <returns>请求结果</returns>
        public BaiduApiRetModel GetShopInfo(string bShopId, string source = "", string secret = "", string encrypt = "")
        {
            var parms = new Dictionary<string, object>();
            parms.Add("baidu_shop_id", bShopId);

            return CallApi(source, secret, BaiduwmConsts.QUERY_SHOP_API, parms, encrypt);
        }

        /// <summary>
        /// 获取订单信息【百度外卖】
        /// </summary>
        /// <param name="bOrderId">百度外卖订单Id</param>
        /// <param name="source">合作方账号</param>
        /// <param name="secret">密钥</param>]
        /// <param name="encrypt">加密方式</param>
        /// <returns>请求结果</returns>
        public BaiduApiRetModel GetOrderInfo(string bOrderId, string source = "", string secret = "", string encrypt = "")
        {
            var parms = new Dictionary<string, object>();
            parms.Add("order_id", bOrderId);

            return CallApi(source, secret, BaiduwmConsts.QUERY_ORDER_API, parms, encrypt);
        }

        /// <summary>
        /// 确认订单信息【百度外卖】
        /// </summary>
        /// <param name="bOrderId">百度外卖订单Id</param>
        /// <param name="source">合作方账号</param>
        /// <param name="secret">密钥</param>]
        /// <param name="encrypt">加密方式</param>
        /// <returns>请求结果</returns>
        public BaiduApiRetModel ConfirmOrder(string bOrderId, string source, string secret, string encrypt = "")
        {
            var parms = new Dictionary<string, object>();
            parms.Add("order_id", bOrderId);

            return CallApi(source, secret, BaiduwmConsts.CONFIRM_ORDER_API, parms, encrypt);
        }

        #endregion

        #region 接口基础

        /// <summary>
        /// 请求API接口
        /// </summary>
        /// <param name="source">合作方账号</param>
        /// <param name="secret">密钥</param>
        /// <param name="cmd">接口名称</param>
        /// <param name="body">接口参数</param>
        /// <param name="encrypt">加密方式</param>
        /// <returns></returns>
        public BaiduApiRetModel CallApi(string source, string secret, string cmd, Dictionary<string, object> body, string encrypt ="")
        {
            BaiduApiRetModel apiRetVal = null;
            body = body ?? new Dictionary<string, object>();
            source = string.IsNullOrWhiteSpace(source) ? ConfigUtil.BaiduSourceId : source;
            secret = string.IsNullOrWhiteSpace(secret) ? ConfigUtil.BaiduSourceSecret : secret;

            try
            {
                //构建接口参数
                var parmDic = new SortedDictionary<string, object>();
                parmDic.Add("version", BaiduwmConsts.API_VER);
                parmDic.Add("timestamp", TimeUtil.SecondTicks_1970);
                parmDic.Add("ticket", StringUtil.GuidStr().ToUpper());
                //加密方式，允许为空
                parmDic.Add("encrypt", encrypt);
                parmDic.Add("secret", secret);

                //合作方账号
                parmDic.Add("source", source);
                //接口名称
                parmDic.Add("cmd", cmd);
                //接口参数json串
                parmDic.Add("body", body.ToJson());

                //构建接口签名参数并签名
                var sign = SignUtil.GetBaiduSign(parmDic, secret);
                if (!parmDic.Keys.Contains("sign"))
                {
                    parmDic.Add("sign", sign);
                }
                var parms = new StringBuilder();
                foreach(var p in parmDic)
                {
                    parms.AppendFormat("{0}={1}&", p.Key, p.Value);
                }
                var parmstr = parms.ToString().TrimEnd('&');

                //执行接口请求并处理返回信息
                var responseText = HttpRequestUtil.HttpPost(BaiduwmConsts.GET_BASE_API, parmstr);
                if (!string.IsNullOrWhiteSpace(responseText))
                {
                    var retDic = JsonUtil.ToObject<Dictionary<string, object>>(responseText);
                    if (retDic != null && retDic["body"] != null)
                    {
                        apiRetVal = JsonUtil.ToObject<BaiduApiRetModel>(retDic["body"].ToString());
                        if (apiRetVal == null || apiRetVal.errno > 0)
                        {
                            LogUtil.Error(string.Format("返回错误信息：{0},请求参数：{1}", responseText, parmstr));
                        }
                        else
                        {
                            LogUtil.Info(string.Format("调用接口：{0},成功返回信息：{1}", cmd, responseText));
                        }
                    }
                    else
                    {
                        LogUtil.Error(string.Format("返回错误信息：{0},请求参数：{1}", responseText, retDic["body"].ToString()));
                    }
                }
                else
                {
                    LogUtil.Error(string.Format("返回信息为空,请求参数：{0}", parmstr));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("请求接口失败,参考信息：{0}", ex.Message));
            }

            return apiRetVal;
        }

        /// <summary>
        /// API接口下行返回
        /// </summary>
        /// <param name="cmd">接口名称</param>
        /// <param name="body">接口参数</param>
        /// <param name="error">错误描述</param>
        /// <param name="source">合作方账号</param>
        /// <param name="secret">密钥</param>
        /// <param name="encrypt">加密方式</param>
        /// <returns></returns>
        public object RetApiResponse(string cmd, Dictionary<string, object> body, string error = "success", string source = "", string secret = "", string encrypt = "")
        {
            body = body ?? new Dictionary<string, object>();
            if (!body.Keys.Contains("errno"))
            {
                body.Add("errno", error.ToLower() == "success" ? 0 : 1);
            }
            if (!body.Keys.Contains("error"))
            {
                body.Add("error", error);
            }

            //构建接口参数
            var parmDic = new SortedDictionary<string, object>();
            parmDic.Add("version", BaiduwmConsts.API_VER);
            parmDic.Add("timestamp", TimeUtil.SecondTicks_1970);
            parmDic.Add("ticket", StringUtil.GuidStr().ToUpper());
            parmDic.Add("encrypt", encrypt);

            //合作方账号
            parmDic.Add("source", source);
            //接口名称
            parmDic.Add("cmd", string.Format("resp.{0}", cmd));
            //接口参数json串
            parmDic.Add("body", body.ToJson());
            //构建接口签名参数并签名
            var sign = SignUtil.GetBaiduSign(parmDic, secret);
            if (parmDic.Keys.Contains("secret"))
            {
                parmDic.Remove("secret");
            }
            if (!parmDic.Keys.Contains("sign"))
            {
                parmDic.Add("sign", sign);
            }
            parmDic["body"] = body;

            return parmDic;
        }

        #endregion

        #region 业务基础

        /// <summary>
        /// 解析订单【百度外卖】
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns></returns>
        public BaiduOrderModel AnalysisOrder(string responseString)
        {
            BaiduOrderModel orderModel = null;
            if (!string.IsNullOrWhiteSpace(responseString))
            {
                var orderDic = Yme.Util.JsonUtil.ToObject<Dictionary<string, object>>(responseString);
                if (orderDic != null)
                {
                    orderModel = new BaiduOrderModel();
                    orderModel.shop = orderDic["shop"] != null ? JsonUtil.ToObject<BaiduOrderShopModel>(orderDic["shop"].ToString()) : null;
                    orderModel.order = orderDic["order"] != null ? JsonUtil.ToObject<BaiduOrderDetailModel>(orderDic["order"].ToString()) : null;
                    orderModel.user = orderDic["user"] != null ? JsonUtil.ToObject<BaiduOrderUserModel>(orderDic["user"].ToString()) : null;
                    orderModel.products = orderDic["products"] != null ? orderDic["products"].ToString() : string.Empty;
                    orderModel.discount = orderDic["discount"] != null ? orderDic["discount"].ToString() : string.Empty;
                }
            }
            return orderModel;
        }

        /// <summary>
        /// 解析订单明细【百度外卖】
        /// </summary>
        /// <param name="detailJson"></param>
        /// <returns></returns>
        public List<OrderDetailQueryModel> AnalysisOrderDetail(string detailJson)
        {
            var details = new List<OrderDetailQueryModel>();
            var mobject = JsonUtil.ToObject<List<List<BaiduOrderProductDishModel>>>(detailJson.Replace("\r\n  ", string.Empty));
            if (mobject != null && mobject.Count > 0)
            {
                var index = 1;
                mobject.ForEach(d =>
                {
                    foreach (var item in d)
                    {
                        details.Add(new OrderDetailQueryModel
                        {
                            Name = item.product_name,
                            Property = GetProductPropertyStr(item.product_attr, item.product_features),
                            Qty = string.Format("X{0}", item.product_amount),
                            Price = (item.product_price.ToDecimal(2) / 100m).ToDecimal(2),
                            GroupId = index,
                            InnerId = item.other_dish_id
                        });
                    }
                    index++;
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
        private string GetProductPropertyStr(object attr, object features)
        {
            var sb = new StringBuilder();
            var attrList = new List<OrderProductFeaturesModel>();
            if (attr != null)
            {
                attrList = JsonUtil.ToObject<List<OrderProductFeaturesModel>>(attr.ToString());
            }
            if (features != null)
            {
                attrList.AddRange(JsonUtil.ToObject<List<OrderProductFeaturesModel>>(features.ToString()));
            }
            foreach (var at in attrList)
            {
                sb.AppendFormat("{0},", at.option);
            }

            var str = sb.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                return string.Format("({0})", str.TrimEnd(','));
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
