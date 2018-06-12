using Yme.Util;

namespace Yme.Mcp.Model.Definitions
{
    /// <summary>
    /// 饿了么平台常量
    /// </summary>
    public static class ElemeConsts
    {
        #region 返回值

        /// <summary>
        /// 返回成功
        /// </summary>
        public const string RETURN_SUCCESS = "ok";

        /// <summary>
        /// 返回失败
        /// </summary>
        public const string RETURN_FAIL = "";

        /// <summary>
        /// API接口调用成功
        /// </summary>
        public const string API_RETURN_OK = "OK";

        #endregion

        #region 请求接口

        /// <summary>
        /// API接口版本
        /// </summary>
        public static string API_VER = "1.0.0";

        #region 门店映射/解绑

        /// <summary>
        /// 获取基础API
        /// </summary>
        public static string GET_BASE_API = string.Format("https://open-api{0}.shop.ele.me", ConfigUtil.IsTestModel ? "-sandbox" : string.Empty);

        /// <summary>
        /// 获取授权码（code）
        /// </summary>
        public static string GET_AUTH_CODE_API = string.Format("{0}/authorize", GET_BASE_API);

        /// <summary>
        /// 获取访问令牌（access token）
        /// </summary>
        public static string GET_ACCESS_TOKEN_API = string.Format("{0}/token", GET_BASE_API);

        /// <summary>
        /// API接口请求地址
        /// </summary>
        public static string GET_API_URL = string.Format("{0}/api/v1/", GET_BASE_API);

        /// <summary>
        /// 门店授权/取消授权
        /// </summary>
        public static string SHOP_AUTH_API = GET_AUTH_CODE_API + "?response_type=code&client_id={0}&redirect_uri={1}&scope=all&state={2}";

        #endregion

        #region 外卖订单接口

        /// <summary>
        /// 查询订单接口【订单】
        /// </summary>
        public const string QUERY_ORDER_API = "eleme.order.getOrder";

        /// <summary>
        /// 批量查询订单接口【订单】
        /// </summary>
        public const string BATCH_QUERY_ORDERS_API = "eleme.order.mgetOrders";

        /// <summary>
        /// 查询订单配送信息接口【订单】
        /// </summary>
        public const string QUERY_ORDER_DELIVER_API = "eleme.order.getDeliveryStateRecord";

        /// <summary>
        /// 批量查询订单配送接口【订单】
        /// </summary>
        public const string BATCH_QUERY_ORDERS_DELIVER_API = "eleme.order.batchGetDeliveryStates";

        /// <summary>
        /// 确认接单接口【订单】
        /// </summary>
        public const string CONFIRM_ORDER_API = "eleme.order.confirmOrderLite";

        #endregion

        #region 门店

        /// <summary>
        /// 查询商户信息接口
        /// </summary>
        public const string QUERY_MERCHATN_INFO_API = "eleme.user.getUser";

        /// <summary>
        /// 查询门店信息接口
        /// </summary>
        public const string QUERY_SHOP_INFO_API = "eleme.shop.getShop";

        #endregion

        #endregion
    }
}
