namespace Yme.Util
{
    /// <summary>
    /// 微信常量
    /// </summary>
    public static class WeChatConsts
    {
        #region 接口地址

        /// <summary>
        ///  获取基础接口AccessToken（包括JS-SDK）【7200s有效】
        /// </summary>
        public const string WECHAT_TOKEN = @"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        ///  获取网页授权AccessToken【AccessToken 及 OPENID，每次获取】
        /// </summary>
        public const string WECHAT_GET_OPENID = @"https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        /// <summary>
        ///  校验获取基础接口AccessToken
        /// </summary>
        public const string WECHAT_TOKEN_ISEXPIRED = @"https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";

        /// <summary>
        ///  校验网页授权AccessToken
        /// </summary>
        public const string AUTH_WECHAT_TOKEN_ISEXPIRED = @"https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}";

        /// <summary>
        ///  回调地址
        /// </summary>
        public const string WECHAT_AUTHORIZE = @"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";

        /// <summary>
        /// 获取JS-SDK时所需的票据
        /// </summary>
        public const string WECHAT_JSSDK_TICKET = @"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

        /// <summary>
        /// 创建菜单
        /// </summary>
        public const string WECHAT_MENU_ADD = @"https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

        /// <summary>
        /// 发送模版消息
        /// </summary>
        public const string WECHAT_SEND_MSG = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

        #endregion

        #region 消息模版ID

        #region 【正式】

        /// <summary>
        /// 新订单微信模版
        /// </summary>
        public const string NewOrderWxTempId = "fGOBaxfDYuRHO_8jvpPmZ5qFz8zGaH8bEE-8rrCgczQ";

        /// <summary>
        /// 取消订单微信模版
        /// </summary>
        public const string CancelOrderWxTempId = "hlgJwsREkaT463siyljefxfcYFrx-XkLmESfGFa5Q2E";

        /// <summary>
        /// 预订单提醒微信模版
        /// </summary>
        public const string RemindPreOrderWxTempId = "DKkDSf-a5LkRTUwMP5ZiSGjo0L7U3q--8Ft3Qtdhn8k";

        /// <summary>
        /// 订单打印失败微信模版
        /// </summary>
        public const string OrderPrintFailWxTempId = "rldKOaKbfjMuUKhu13U4GVUVY4n-V4x4bWyeVNKmJo8";

        /// <summary>
        /// 打印机异常微信模版
        /// </summary>
        public const string PrinterFaultWxTempId = "GySOPKfOinsfPv_5zbDN-rKHPWOLOVcPB1xLnDwF6C0";

        #endregion

        #region 【测试】

        /// <summary>
        /// 新订单微信模版
        /// </summary>
        public const string T_NewOrderWxTempId = "fBCHUq9GurAasNk2XeGsI_yS-GsXewdavNI9w0QgzvU";

        /// <summary>
        /// 取消订单微信模版
        /// </summary>
        public const string T_CancelOrderWxTempId = "yy1PoAIm1bzGJSxeSWdYtcQ2BeDwGRM6oUZkts4vao4";

        /// <summary>
        /// 预订单提醒微信模版
        /// </summary>
        public const string T_RemindPreOrderWxTempId = "fHISDGB9qKIBeQdiC7-NrAIdXv-3HWS2Dw4kawjvkn8";

        /// <summary>
        /// 订单打印失败微信模版
        /// </summary>
        public const string T_OrderPrintFailWxTempId = "UF3Qhbc8W7X593GlG2oOStKoFxqcPjpaC0gZ18NuJgI";

        /// <summary>
        /// 打印机异常微信模版
        /// </summary>
        public const string T_PrinterFaultWxTempId = "ICJdJBEkDBZPY5pJS2rKViebdfGS5nWoTR-vh7pQdc4";

        #endregion

        #endregion
    }
}
