using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Mcp.Model.Definitions;
using Yme.Mcp.Model.QueryModels;
using Yme.Mcp.Model.ViewModels.Waimai.Eleme;
using Yme.Util;
using Yme.Util.Log;
using Yme.Util.Security;
using Yme.Util.Extension;

namespace Yme.Mcp.Service.Common
{
    /// <summary>
    /// 饿了么服务
    /// </summary>
    public class ElemeService
    {
        #region 业务接口

        /// <summary>
        /// 获取商户账号信息【饿了么】
        /// </summary>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel GetMerchatAccountInfo(string token)
        {
            LogUtil.Info(string.Format("获取商户账号信息,传入参数token：{0}", token));
            return CallApi(token, ElemeConsts.QUERY_MERCHATN_INFO_API, null);
        }

        /// <summary>
        /// 获取门店信息【饿了么】
        /// </summary>
        /// <param name="eShopId">饿了么门店Id</param>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel GetShopInfo(long eShopId, string token)
        {
            LogUtil.Info(string.Format("获取门店信息,传入参数token：{0}, eShopId: {1}", token, eShopId));
            var parms = new Dictionary<string, object>();
            parms.Add("shopId", eShopId);

            return CallApi(token, ElemeConsts.QUERY_SHOP_INFO_API, parms);
        }

        /// <summary>
        /// 获取订单信息【饿了么】
        /// </summary>
        /// <param name="eOrderId">饿了么订单Id</param>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel GetOrderInfo(string eOrderId, string token)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("orderId", eOrderId);

            return CallApi(token, ElemeConsts.QUERY_ORDER_API, parms);
        }

        /// <summary>
        /// 批量获取订单列表【饿了么】
        /// </summary>
        /// <param name="eOrderIds">饿了么订单Id列表</param>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel GetBatchOrders(List<string> eOrderIds, string token)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("orderIds", eOrderIds.ToArray());

            return CallApi(token, ElemeConsts.BATCH_QUERY_ORDERS_API, parms);
        }

        /// <summary>
        /// 确认订单信息【饿了么】
        /// </summary>
        /// <param name="eOrderId">饿了么订单Id</param>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel ConfirmOrder(string eOrderId, string token)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("orderId", eOrderId);

            LogUtil.Info(string.Format("调用饿了么确认接口,订单Id:{0}", eOrderId));

            return CallApi(token, ElemeConsts.CONFIRM_ORDER_API, parms);
        }

        /// <summary>
        /// 获取订单配送信息【饿了么】
        /// </summary>
        /// <param name="eOrderId">饿了么订单Id</param>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel GetOrderDeliverInfo(string eOrderId, string token)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("orderId", eOrderId);

            return CallApi(token, ElemeConsts.QUERY_ORDER_DELIVER_API, parms);
        }

        /// <summary>
        /// 批量获取订单配送列表【饿了么】
        /// </summary>
        /// <param name="eOrderIds">饿了么订单Id列表</param>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel GetBatchOrdersDeliver(List<string> eOrderIds, string token)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("orderIds", eOrderIds.ToArray());

            return CallApi(token, ElemeConsts.BATCH_QUERY_ORDERS_DELIVER_API, parms);
        }

        #endregion

        #region 接口基础

        /// <summary>
        /// 获取访问令牌【通过授权码】
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>访问令牌信息</returns>
        public ElemeRetTokenModel GetAccessTokenByCode(string code)
        {
            //构建请求参数
            ElemeRetTokenModel token = null;
            var parms = new StringBuilder();
            parms.AppendFormat("grant_type={0}&", "authorization_code");
            parms.AppendFormat("code={0}&", code);
            parms.AppendFormat("redirect_uri={0}&", WebUtil.UrlEncode(ConfigUtil.EleAuthCallBack));
            parms.AppendFormat("client_id={0}", ConfigUtil.EleAppKey);

            try
            {
                //请求接口地址
                var header = Base64Util.Base64Encode(string.Format("{0}:{1}", ConfigUtil.EleAppKey, ConfigUtil.EleAppSecret), Encoding.UTF8);
                var responseText = HttpRequestUtil.HttpPost(ElemeConsts.GET_ACCESS_TOKEN_API, parms.ToString(), false, header);
                if (!string.IsNullOrWhiteSpace(responseText))
                {
                    token = JsonUtil.ToObject<ElemeRetTokenModel>(responseText);
                    if (token == null)
                    {
                        LogUtil.Error(string.Format("通过Code获取AccessToken返回错误信息：{0},请求参数：{1}", responseText, parms.ToJson()));
                    }
                    else
                    {
                        //令牌提前半天过期（12h）
                        token.expires_in = token.expires_in - (12 * 60 * 60);
                    }
                }
                else
                {
                    LogUtil.Error(string.Format("通过Code获取AccessToken失败,返回信息为空,请求参数：{0}", parms.ToJson()));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("通过Code获取AccessToken失败,参考信息：{0}", ex.Message));
            }
            return token;
        }

        /// <summary>
        /// 获取访问令牌【通过更新令牌】
        /// </summary>
        /// <param name="refreshToken">更新令牌</param>
        /// <returns>访问令牌信息</returns>
        public ElemeRetTokenModel GetAccessTokenByRefreshToken(string refreshToken)
        {
            //构建请求参数
            ElemeRetTokenModel token = null;
            var parms = new StringBuilder();
            parms.AppendFormat("grant_type={0}&", "refresh_token");
            parms.AppendFormat("refresh_token={0}", refreshToken);

            int tryNum = 1;
            try
            {
                token = GetTokenByRefreshToken(parms, tryNum);
            }
            catch (Exception ex)
            {
                System.Threading.Thread.Sleep(100);
                if (tryNum <= 3)
                {
                    token = GetTokenByRefreshToken(parms, tryNum++);
                }
                else
                {
                    LogUtil.Error(string.Format("通过RefreshToken获取AccessToken失败，参考信息：{0}", ex.Message));
                }
            }

            return token;
        }

        private ElemeRetTokenModel GetTokenByRefreshToken(StringBuilder parms, int tryNum)
        {
            //请求接口地址
            ElemeRetTokenModel token = null;
            var header = Base64Util.Base64Encode(string.Format("{0}:{1}", ConfigUtil.EleAppKey, ConfigUtil.EleAppSecret), Encoding.UTF8);
            var responseText = HttpRequestUtil.HttpPost(ElemeConsts.GET_ACCESS_TOKEN_API, parms.ToString(), false, header);
            if (!string.IsNullOrWhiteSpace(responseText))
            {
                token = JsonUtil.ToObject<ElemeRetTokenModel>(responseText);
                if (token == null)
                {
                    LogUtil.Error(string.Format("通过RefreshToken获取AccessToken返回错误信息：{0},请求参数：{1}", responseText, parms.ToJson()));
                }
                else
                {
                    //令牌提前半天过期（12h）
                    token.expires_in = token.expires_in - (12 * 60 * 60);
                    LogUtil.Info(string.Format("尝试通过RefreshToken:{0}获取AccessToken:{1}", parms.ToString(), token.ToJson()));
                }
            }
            else
            {
                LogUtil.Error(string.Format("通过RefreshToken获取AccessToken失败,返回信息为空,请求参数：{0}", parms.ToJson()));
            }
            return token;
        }

        /// <summary>
        /// 请求API接口
        /// </summary>
        /// <param name="token">访问令牌</param>
        /// <param name="action">接口名称</param>
        /// <param name="parms">接口参数</param>
        /// <returns></returns>
        public ElemeApiRetModel CallApi(string token, string action, Dictionary<string, object> parms)
        {
            ElemeApiRetModel apiRetVal = null;
            parms = parms ?? new Dictionary<string, object>();

            try
            {
                //构建接口公用参数
                var m = new Dictionary<string, object>();
                m.Add("app_key", ConfigUtil.EleAppKey);
                m.Add("timestamp", TimeUtil.SecondTicks_1970);

                //构建接口参数
                var p = new SortedDictionary<string, object>();
                p.Add("nop", ElemeConsts.API_VER);
                p.Add("id", StringUtil.GuidStr());
                p.Add("action", action);
                p.Add("token", token);
                p.Add("metas", m);
                p.Add("params", parms);

                //构建接口签名参数并签名
                var sa = new SortedDictionary<string, object>();
                foreach (var cm in m)
                {
                    sa.Add(cm.Key, cm.Value);
                }
                if (parms != null && parms.Count > 0)
                {
                    foreach (var pm in parms)
                    {
                        sa.Add(pm.Key, pm.Value);
                    }
                }
                p.Add("signature", SignUtil.GetEleSign(sa, ConfigUtil.EleAppSecret, true, action, token));

                //执行接口请求并处理返回信息
                var responseText = HttpRequestUtil.HttpPost(ElemeConsts.GET_API_URL, p.ToJson(), true);
                if (!string.IsNullOrWhiteSpace(responseText))
                {
                    apiRetVal = JsonUtil.ToObject<ElemeApiRetModel>(responseText);
                    if (apiRetVal == null || apiRetVal.error != null)
                    {
                        LogUtil.Error(string.Format("返回错误信息：{0},请求参数：{1}", responseText, p.ToJson()));
                    }
                    else
                    {
                        LogUtil.Info(string.Format("调用接口：{0},成功返回信息：{1}", action, responseText));
                    }
                }
                else
                {
                    LogUtil.Error(string.Format("返回信息为空,请求参数：{0}", parms.ToJson()));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("请求接口失败,参考信息：{0}", ex.Message));
            }

            return apiRetVal;
        }

        #endregion

        #region 业务基础

        /// <summary>
        /// 解析订单【饿了么】
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns></returns>
        public ElemeOrderModel AnalysisOrder(string responseString)
        {
            ElemeOrderModel orderModel = null;
            if (!string.IsNullOrWhiteSpace(responseString))
            {
                orderModel = Yme.Util.JsonUtil.ToObject<ElemeOrderModel>(responseString);
            }
            return orderModel;
        }

        /// <summary>
        /// 解析订单明细【饿了么】
        /// </summary>
        /// <param name="detailJson"></param>
        /// <returns></returns>
        public List<OrderDetailQueryModel> AnalysisOrderDetail(string detailJson)
        {
            var details = new List<OrderDetailQueryModel>();
            var mobject = Yme.Util.JsonUtil.ToObject<List<EleOrderGroupsModel>>(detailJson);
            if (mobject != null && mobject.Count > 0)
            {
                mobject.ForEach(d =>
                {
                    if (d.items != null && d.items.Count > 0)
                    {
                        foreach (var item in d.items)
                        {
                            if (d.type.ToLower() == "normal")
                            {
                                details.Add(new OrderDetailQueryModel
                                {
                                    Name = item.name,
                                    Property = GetProductPropertyStr(item.newSpecs, item.attributes),
                                    Qty = string.Format("X{0}", item.quantity),
                                    Price = item.price.ToDecimal(2),
                                    GroupId = (d.name.Replace("号篮子", string.Empty).Replace("用户 发起人", string.Empty).Replace("... 的篮子", string.Empty).Replace("号", string.Empty)).ToInt(),
                                    InnerId = item.extendCode
                                });
                            }
                        }
                    }
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
        private string GetProductPropertyStr(object newSpecs, object attributes)
        {
            var sb = new StringBuilder();
            var attrList = new List<OrderProductAttrModel>();
            if (newSpecs != null)
            {
                attrList = JsonUtil.ToObject<List<OrderProductAttrModel>>(newSpecs.ToString());
            }
            if (attributes != null)
            {
                attrList.AddRange(JsonUtil.ToObject<List<OrderProductAttrModel>>(attributes.ToString()));
            }
            foreach (var at in attrList)
            {
                sb.AppendFormat("{0},", string.Join(",", at.value));
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
