using System;
using System.Collections.Generic;
using Yme.Util.Extension;
using Yme.Util.Enums;
using Yme.Util.Log;
using System.Text;

namespace Yme.Util
{
    public class SmsUtil
    {
        #region 公用方法

        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="validateCode">验证码</param>
        public static void SendValidCode(string mobile, string validateCode, decimal validateCodeExpire, SmsFuncType funcType)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", validateCode);
            dic.Add("code_expire", validateCodeExpire);
            string[] paramArray = new string[] { validateCode, validateCodeExpire.ToString() };

            //发送渠道
            var sendWay = ConfigUtil.SmsWay;

            switch (funcType)
            {
                case SmsFuncType.Login:
                case SmsFuncType.BindMobile:
                    switch (sendWay)
                    {
                        case 0:
                            SendCodeSms(mobile, validateCode, validateCodeExpire);
                            break;
                        case 1:
                            SendSmsByRly(mobile, paramArray, SmsTemplateType.ORD_RLY_ValidCodeLogin.GetHashCode());
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 0- 【移动梦网】私有

        /// <summary>
        /// 【移动梦网】短信平台用户ID
        /// </summary>
        private static readonly string mwSmsUserId = "J91637";

        /// <summary>
        /// 【移动梦网】短信平台用户密码
        /// </summary>
        private static readonly string mwSmsPwd = "562223";

        /// <summary>
        /// 短信平台请求Api
        /// </summary>
        private static readonly string mwUrlApi = "http://61.145.229.26:8086/MWGate/wmgw.asmx";

        /// <summary>
        /// 发送短信通知
        /// </summary>
        /// <param name="mobile">手机号码（串）</param>
        /// <param name="content">短信内容</param>
        private static void SendSms(string mobile, string content)
        {
            string url = string.Format("{0}/MongateCsSpSendSmsNew", mwUrlApi);
            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("userId", mwSmsUserId);
            paras.Add("password", mwSmsPwd);
            paras.Add("pszMobis", mobile);
            paras.Add("iMobiCount", mobile.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length.ToString());
            paras.Add("pszMsg", content);
            HttpRequestUtil.SendGetRequest(url, paras);
        }

        /// <summary>
        /// 发送验证码短信
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="verifyCode">验证码</param>
        /// <param name="validateCodeExpire">有效期</param>
        private static void SendCodeSms(string mobile, string verifyCode, decimal validateCodeExpire)
        {
            byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(string.Format(MsgTemplateUtil.MsgDict["SmsValidCode"], verifyCode, validateCodeExpire));
            string str = Convert.ToBase64String(bytes);
            str = string.Format("&multixmt=*|{0}|{1}|*|*|*|*|*|100000001|*|*|*|1", mobile, str);
            string url = string.Format("{0}/MongateCsSpMultixMtSend?userid={1}&password={2}", mwUrlApi, mwSmsUserId, mwSmsPwd);
            Dictionary<string, string> para = new Dictionary<string, string>();
            HttpRequestUtil.HttpGet(url + str, "");
        }

        #endregion

        #region 1-【荣联云】私有

        /// <summary>
        /// 【荣联云】短信平台域名
        /// </summary>
        private static readonly string RlySmsIpDomain = "sandboxapp.cloopen.com";

        /// <summary>
        /// 【荣联云】短信平台端口
        /// </summary>
        private static readonly string RlySmsPort = "8883";

        /// <summary>
        /// 【荣联云】发送短信
        /// </summary>
        private static void SendSmsByRly(string mobile, string[] templateParam, int tempateId)
        {
            CCPRestSDK api = new CCPRestSDK();
            bool isInit = api.init(RlySmsIpDomain, RlySmsPort);
            api.setAccount(ConfigUtil.SmsAccountSid_rly, ConfigUtil.SmsAuthToken_rly);
            api.setAppId(ConfigUtil.SmsAppId_rly);

            try
            {
                if (isInit)
                {
                    Dictionary<string, object> retData = api.SendTemplateSMS(mobile, tempateId.ToString(), templateParam);
                    string ret = GetRlySmsDictData(retData);
                    LogUtil.Info(ret);
                }
                else
                {
                    LogUtil.Error("短信模版初始化失败！");
                }
            }
            catch (Exception exc)
            {
                LogUtil.Error(exc.ToString());
            }
        }

        /// <summary>
        /// 获取短信数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetRlySmsDictData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += GetRlySmsDictData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }

        #endregion

        #region 2-【电信】私有

        /// <summary>
        /// 获取访问令牌API接口
        /// </summary>
        private static readonly string GetDxSmsTokenApi = "https://oauth.api.189.cn/emp/oauth2/v3/access_token";

        /// <summary>
        /// 发送短信API接口
        /// </summary>
        private static readonly string SendDxSmsApi = "http://api.189.cn/v2/emp/templateSms/sendSms";

        /// <summary>
        /// Token字典
        /// </summary>
        private static Dictionary<string, Tuple<string, DateTime>> _dXSmsToken = new Dictionary<string, Tuple<string, DateTime>>();

        /// <summary>
        /// 获取登录Token
        /// </summary>
        /// <param name="tryNum"></param>
        /// <returns></returns>
        private static string GetDxSmsAccessToken(int tryNum)
        {
            string key = "smstoken";
            if (_dXSmsToken.ContainsKey(key))
            {
                var kv = _dXSmsToken[key];
                if (kv.Item2 > DateTime.Now)
                {
                    return kv.Item1;
                }
            }
            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("grant_type", "client_credentials");
            paras.Add("app_id", ConfigUtil.SmsAppId_dx);
            paras.Add("app_secret", ConfigUtil.SmsAppSecret_dx);
            try
            {
                string res = HttpRequestUtil.SendPostRequest(GetDxSmsTokenApi, paras);
                Dictionary<string, string> resDic = JsonUtil.ToObject<Dictionary<string, string>>(res);
                if (resDic.ContainsKey("access_token"))
                {
                    _dXSmsToken.Clear();
                    _dXSmsToken.Add(key, new Tuple<string, DateTime>(resDic["access_token"], DateTime.Now.AddSeconds(resDic["expires_in"].ToInt(0))));
                    return resDic["access_token"];
                }
                else
                {
                    LogUtil.Debug(resDic["res_message"]);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.ToString());
                System.Threading.Thread.Sleep(200);
                if (tryNum < 3)
                {
                    return GetDxSmsAccessToken(++tryNum);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 【电信】发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="templateParam"></param>
        /// <param name="tempateId"></param>
        private static void SendSmsByDx(string mobile, Dictionary<string, object> templateParam, int tempateId)
        {
            string token = GetDxSmsAccessToken(0);
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            Dictionary<string, string> paras = new Dictionary<string, string>();

            paras.Add("acceptor_tel", mobile);
            paras.Add("access_token", token);
            paras.Add("app_id", ConfigUtil.SmsAppId_dx);
            paras.Add("template_id", tempateId.ToString());
            paras.Add("template_param", JsonUtil.ToJson(templateParam));
            paras.Add("timestamp", DateTime.Now.ToString(Const.DATETIME_FORMAT));

            try
            {
                string res = HttpRequestUtil.SendPostRequest(SendDxSmsApi, paras);
                LogUtil.Debug(string.Format("发送手机短信：{0},{1}", mobile, JsonUtil.ToJson(templateParam)));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.ToString());
            }
        }

        #endregion
    }
}
