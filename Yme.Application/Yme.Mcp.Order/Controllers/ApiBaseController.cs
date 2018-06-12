using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Evt.Framework.Common;

using Yme.Util;
using Yme.Util.Log;
using Yme.Util.Exceptions;
using Yme.Mcp.Entity;
using Yme.Mcp.Model.Definitions;
using Yme.Mcp.Model.WebApi;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Order.Handel;
using System.Text;

namespace Yme.Mcp.Order.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [WebAPIFilter]
    [UnhandledExceptionFilter]
    [ModelValidationFilter]
    public class ApiBaseController : ApiController
    {
        /// <summary>
        /// 会话
        /// </summary>
        public Session Session { get; set; }

        /// <summary>
        /// 当前用户Id
        /// </summary>
        public long CurrId
        {
            get
            {
                long id = 0;
                if (ServiceContext.Current != null && ServiceContext.Current.currUser != null)
                {
                    id = ServiceContext.Current.currUser.UserId;
                }
                return id;
            }
        }

        /// <summary>
        /// 当前门店信息
        /// </summary>
        public ShopEntity CurrUser
        {
            get
            {
                return SingleInstance<ShopBLL>.Instance.GetShopById(CurrId);
            }
        }

        /// <summary>
        /// 当前应用标识
        /// </summary>
        public string AppSign
        {
            get
            {
                var appSign = string.Empty;
                try
                {

                    if (ServiceContext.Current != null && ServiceContext.Current.RequestTerminal != null)
                    {
                        appSign = ServiceContext.Current.RequestTerminal.AppSign;
                        if (string.IsNullOrWhiteSpace(appSign))
                        {
                            appSign = ConfigUtil.SystemAppSign;
                        }
                    }
                    else
                    {
                        appSign = ConfigUtil.SystemAppSign;
                    }
                }
                catch
                {
                    appSign = ConfigUtil.SystemAppSign;
                }
                return appSign;
            }
        }

        /// <summary>
        /// 用户访问令牌
        /// </summary>
        public string AccessToken
        {
            get
            {
                var accessToken = string.Empty;
                try
                {

                    if (ServiceContext.Current != null && ServiceContext.Current.RequestTerminal != null)
                    {
                        accessToken = ServiceContext.Current.RequestTerminal.ClientToken;
                    }
                }
                catch
                {
                    accessToken = string.Empty;
                }
                return accessToken;
            }
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        private JavaScriptSerializer _js = null;

        /// <summary>
        /// 获取序列化对象
        /// </summary>
        private JavaScriptSerializer JS
        {
            get
            {
                if (_js == null)
                {
                    _js = new JavaScriptSerializer();
                }
                return _js;
            }
        }

        /// <summary>
        /// 请求头是否合法
        /// </summary>
        protected bool RequestHeaderIsValid
        {
            get
            {
                bool isValid = false;
                var curr = ServiceContext.Current;
                if (curr != null)
                {
                    var terminal = curr.RequestTerminal;
                    if (terminal != null)
                    {
                        isValid = !string.IsNullOrWhiteSpace(terminal.AppSign) && !string.IsNullOrWhiteSpace(terminal.Code);
                    }
                }

                return isValid;
            }
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns>object</returns>
        [NonAuthorize]
        [AcceptVerbs("GET")]
        public object Index()
        {
            return OK();
        }

        /// <summary>
        /// 单点登录
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="mobile">当前用户手机号</param>
        public void SingleLoginOn(string accessToken, string mobile = "")
        {
            //删除已经登录的session,单点登录
            var dic = SessionManager.GetSessions();
            foreach (var session in dic.Values)
            {
                if (session.SessionID == Session.SessionID)
                {
                    continue;
                }

                if (session.ContainsKey(ConfigUtil.SystemUserSessionKey))
                {
                    var info = session[ConfigUtil.SystemUserSessionKey] as LoginInfo;
                    if (info != null && (info.Mobile == mobile || info.AccessToken == accessToken))
                    {
                        // 清除session
                        SessionManager.RemoveSession(session.SessionID);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 清空当前会话
        /// </summary>
        public bool ClearCurrSession()
        {
            var dic = SessionManager.GetSessions();
            foreach (var session in dic.Values)
            {
                if (session.SessionID == Session.SessionID)
                {
                    continue;
                }

                if (session.ContainsKey(ConfigUtil.SystemUserSessionKey))
                {
                    // 清除session
                    SessionManager.RemoveSession(session.SessionID);
                    break;
                }
            }

            return !Session.ContainsKey(ConfigUtil.SystemUserSessionKey);
        }

        #region 受保护的辅助方法

        /// <summary>
        /// 返回状态为成功的JsonResult。
        /// </summary>
        /// <returns>JsonResult</returns>
        protected object OK()
        {
            return OK(null);
        }

        /// <summary>
        /// 返回状态为成功的JsonResult。
        /// </summary>
        /// <param name="data">需返回给客户端的Json数据。</param>
        /// <returns>JsonResult</returns>
        protected object OK(object data)
        {
            object result = null;
            if (data is DataTable)
            {
                var dataTable = ConvertUtil.ConvertDataTableToList(data as DataTable);
                result = new { status = ActionResultCode.Success.GetHashCode(), data = dataTable };
            }
            else if (data is DataSet)
            {
                var dataSet = ConvertUtil.ConvertDataSetToDictionary(data as DataSet);
                result = new { status = ActionResultCode.Success.GetHashCode(), data = dataSet };
            }
            else if (data == null)
            {
                result = new { status = ActionResultCode.Success.GetHashCode(), data = "ok" };
            }
            else
            {
                result = new { status = ActionResultCode.Success.GetHashCode(), data = data };
            }

            return result;
        }

        /// <summary>
        /// 返回状态为错误的JsonResult。
        /// </summary>
        /// <param name="message">错误信息。</param>
        /// <returns>JsonResult</returns>
        protected object Failed(string message)
        {
            object result = null;
            result = new { status = ActionResultCode.Failed.GetHashCode(), data = message };
            return result;
        }

        /// <summary>
        /// 返回状态为重复提交的JsonResult。
        /// </summary>
        /// <param name="message">重复提交信息。</param>
        /// <returns>JsonResult</returns>
        protected object RepeatSubmit(string message)
        {
            object result = null;
            result = new { status = ActionResultCode.RepeatSubmit.GetHashCode(), data = message };
            return result;
        }

        /// <summary>
        /// 返回确认提示，需等待客户端确定下一步操作
        /// </summary>
        /// <param name="message">确认消息</param>
        /// <returns>响应</returns>
        protected object Confirm(string message)
        {
            return new { status = ActionResultCode.Confirm.GetHashCode(), data = message };
        }

        /// <summary>
        /// 请求头是否合法
        /// </summary>
        protected void CheckRequestHeaderIsValid()
        {
            if (!RequestHeaderIsValid)
            {
                throw new Evt.Framework.Common.MessageException("HTTP请求头无效。");
            }
        }

        /// <summary>
        /// 获取请求参数列表
        /// </summary>
        /// <param name="actionDesc">接口描述</param>
        /// <returns></returns>
        protected NameValueCollection GetRequestParams(bool isDecode = true, string actionDesc = "")
        {
            var queryStrings = GetQueryStrings();
            LogUtil.Info(string.Format("{0}{1}请求参数：{2}", actionDesc, (!string.IsNullOrWhiteSpace(actionDesc) ? "," : string.Empty), queryStrings));

            var requestForms = HttpRequestUtil.GetNameValueCollection(queryStrings, isDecode);
            if (requestForms.Keys.Count <= 0)
            {
                var msg = string.Format("{0}缺少参数！", actionDesc);
                LogUtil.Warn(msg);
            }
            return requestForms;
        }

        /// <summary>
        /// 获取请求参数列表
        /// </summary>
        /// <param name="sortDics">参数字典</param>
        /// <param name="queryStrings">请求参数串</param>
        /// <param name="actionDesc">接口描述</param>
        /// <returns></returns>
        protected NameValueCollection GetRequestParams(out SortedDictionary<string, object> sortDics, out string queryStrings, bool isDecode = true, string actionDesc = "")
        {
            queryStrings = GetQueryStrings();
            LogUtil.Info(string.Format("{0}{1}请求参数：{2}", actionDesc, (!string.IsNullOrWhiteSpace(actionDesc) ? "," : string.Empty), queryStrings));

            sortDics = new SortedDictionary<string, object>();
            var requestForms = HttpRequestUtil.GetNameValueCollection(queryStrings, out sortDics, isDecode);
            if (requestForms.Keys.Count <= 0)
            {
                var msg = string.Format("{0}缺少参数！", actionDesc);
                LogUtil.Warn(msg);
            }
            return requestForms;
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="isDecode">是否解码</param>
        /// <returns></returns>
        private string GetQueryStrings()
        {
            var queryStrings = Request.Content.ReadAsStringAsync().Result;
            if (string.IsNullOrEmpty(queryStrings))
            {
                queryStrings =  Request.RequestUri.Query.TrimStart('?');
            }
            return queryStrings;
        }

        #endregion

        #region 第三方返回

        /// <summary>
        /// 美团返回
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected object MeiMsg(object data)
        {
            object result = null;
            if (data is DataTable)
            {
                var dataTable = ConvertUtil.ConvertDataTableToList(data as DataTable);
                result = new { data = dataTable };
            }
            else if (data is DataSet)
            {
                var dataSet = ConvertUtil.ConvertDataSetToDictionary(data as DataSet);
                result = new { data = dataSet };
            }
            else if (data == null)
            {
                result = new { data = MeituanConsts.RETURN_SUCCESS };
            }
            else
            {
                result = new { data = data };
            }

            return result;
        }

        /// <summary>
        /// 饿了么返回
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected object EleMsg(object data)
        {
            object result = null;
            if (data is DataTable)
            {
                var dataTable = ConvertUtil.ConvertDataTableToList(data as DataTable);
                result = new { message = dataTable };
            }
            else if (data is DataSet)
            {
                var dataSet = ConvertUtil.ConvertDataSetToDictionary(data as DataSet);
                result = new { message = dataSet };
            }
            else if (data == null)
            {
                result = new { message = ElemeConsts.RETURN_SUCCESS };
            }
            else
            {
                result = new { message = data };
            }

            return result;
        }


        /// <summary>
        /// 返回状态为错误的JsonResult。
        /// </summary>
        /// <param name="message">错误信息。</param>
        /// <returns>JsonResult</returns>
        protected object Error(string message = "")
        {
            object result = null;
            result = new { data = message };
            return result;
        }

        #endregion
    }

    /// <summary>
    /// 过滤器属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class WebAPIFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        #region 公共方法

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="actionContext">当前异常</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var returnType = "application/json";
            var fileType = "multipart/form-data";
            HttpRequestMessage request = actionContext.Request;
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(returnType));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(fileType));
            request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("zh-cn", 0.5));
            request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("en-us", 0.5));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

            System.Collections.ObjectModel.Collection<CookieHeaderValue> cookies = actionContext.Request.Headers.GetCookies("SessionID");

            ApiBaseController bc = (ApiBaseController)actionContext.ControllerContext.Controller;

            //初始化Cookie和Session以及当前登录用户
            InitiSession(cookies, actionContext, bc);

            //检查当前Action是否需要身份验证和授权才能执行
            CheckLoginAndOperation(actionContext, bc);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="actionExecutedContext">异常类型</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            ApiBaseController bc = (ApiBaseController)actionExecutedContext.ActionContext.ControllerContext.Controller;
            string actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName ?? string.Empty;

            if (actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.AddCookies(new CookieHeaderValue[] { new CookieHeaderValue("SessionID", bc.Session.SessionID) { Path = "/" } });

                //如果请求不带版本号，则添加响应头以指示浏览器不缓存当前请求结果
                if (actionExecutedContext.ActionContext.Request.Properties.ContainsKey("v"))
                {
                    actionExecutedContext.ActionContext.Response.Headers.Add("Pragma", "no-cache");
                    actionExecutedContext.ActionContext.Response.Headers.Add("Expires", "0");
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取集合中的键值对,并序列号
        /// </summary>
        /// <param name="collection">集合</param>
        /// <returns>json对象</returns>
        private string GetJson(NameValueCollection collection)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string key in collection.AllKeys)
            {
                dic.Add(key, collection[key]);
            }

            return Evt.Framework.Common.JsonUtil.Serialize(dic);
        }

        /// <summary>
        /// 检查当前Action是否需要身份验证和授权才能执行
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        /// <param name="bc">BaseController</param>
        private void CheckLoginAndOperation(HttpActionContext actionContext, ApiBaseController bc)
        {
            var attributes = actionContext.ActionDescriptor.GetCustomAttributes<NonAuthorizeAttribute>();

            //验证是否需要登录
            if (attributes != null && attributes.Count > 0)
            {
                return;
            }

            //验证是否已经登录
            bool isLogin = true;
            if (bc.Session.ContainsKey(ConfigUtil.SystemUserSessionKey))
            {
                var currUser = bc.Session[ConfigUtil.SystemUserSessionKey] as LoginInfo;
                isLogin = currUser != null;
            }
            else
            {
                isLogin = false;
            }

            if (!isLogin && !actionContext.Request.RequestUri.LocalPath.ToLower().Equals("/shop/dologout"))
            {
                var noSessionMsg = "会话超时，请重新登录！";
                var current = ServiceContext.Current;
                if (current != null && current.RequestTerminal != null && !string.IsNullOrWhiteSpace(current.RequestTerminal.ClientToken) && current.RequestTerminal.ClientToken != "null")
                {
                    bc.SingleLoginOn(current.RequestTerminal.ClientToken);
                    var currUser = SingleInstance<ShopBLL>.Instance.DoLogin(null, current.RequestTerminal.ClientToken);
                    if (currUser != null)
                    {
                        var loginInfo = new LoginInfo() { UserId = currUser.ShopId, Mobile = currUser.ShopAccount, AccessToken = currUser.AccessToken };
                        bc.Session[ConfigUtil.SystemUserSessionKey] = loginInfo;
                    }
                    else
                    {
                        throw new Evt.Framework.Common.AuthenticationException(noSessionMsg);
                    }
                }
                else
                {
                    throw new Evt.Framework.Common.AuthenticationException(noSessionMsg);
                }
            }
        }

        /// <summary>
        /// 初始化Session和当前用户
        /// </summary>
        /// <param name="cookies">Collection</param>
        /// <param name="bc">BaseController</param>
        private void InitiSession(Collection<CookieHeaderValue> cookies, HttpActionContext actionContext, ApiBaseController bc)
        {
            if (cookies == null || cookies.Count == 0)
            {
                bc.Session = SessionManager.CreateSession();
            }
            else
            {
                string sessionID = string.Empty;
                foreach (CookieState cookieState in cookies[0].Cookies)
                {
                    if (cookieState.Name == "SessionID")
                    {
                        sessionID = cookieState.Value;
                        break;
                    }
                }
                Session session = SessionManager.GetSession(sessionID);

                if (session == null)
                {
                    bc.Session = SessionManager.CreateSession();
                }
                else
                {
                    bc.Session = session;
                }
            }

            //获取客户端信息
            var clientModel = new RequestClientInfoModel();
            if (bc.Session.ContainsKey(ConfigUtil.SystemTerminalSessionKey))
            {
                var clientSeesion = bc.Session[ConfigUtil.SystemTerminalSessionKey];
                if (clientSeesion != null)
                {
                    clientModel = bc.Session[ConfigUtil.SystemTerminalSessionKey] as RequestClientInfoModel;
                }
            }
            else
            {
                clientModel = GetClientInfo(actionContext.Request);
                clientModel.ClientIP = NetUtil.Ip;
                bc.Session[ConfigUtil.SystemTerminalSessionKey] = clientModel;
            }

            //设置Service中的ContextUser         
            ServiceContext.Current.ContextUser = new SysServericeContext(bc.Session.SessionID);

            //设置Service中的RequestTerminal
            ServiceContext.Current.RequestTerminal = new PrdRequestTerminal(bc.Session.SessionID);
        }

        /// <summary>
        /// 解析Yingmei-Header内容
        /// </summary>
        /// <param name="valueString">Yingmei-Header值</param>
        /// <returns>解析Yingmei-Header内容</returns>
        private RequestClientInfoModel ResolveClientInfo(string valueString)
        {
            var model = new RequestClientInfoModel();
            if (string.IsNullOrWhiteSpace(valueString))
            {
                return model;
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] values = valueString.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var v in values)
            {
                string[] item = v.Split(new char[] { '=' });
                if (item.Length != 2)
                {
                    LogUtil.Warn(string.Format("Yingmei-Header is invalid:{0}.", valueString));
                    continue;
                }

                if (!dic.ContainsKey(item[0]))
                {
                    dic.Add(item[0], item[1]);
                }
            }

            return Evt.Framework.Common.JsonUtil.Deserialize<RequestClientInfoModel>(Evt.Framework.Common.JsonUtil.Serialize(dic));
        }

        /// <summary>
        /// 从Request对象中获取Headers请求的Client-Info参数DeviceID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private RequestClientInfoModel GetClientInfo(HttpRequestMessage request)
        {
            var model = new RequestClientInfoModel();
            var clientInfo = string.Empty;
            if (request.Headers.Contains("Yingmei-Header"))
            {
                model = ResolveClientInfo(request.Headers.GetValues("Yingmei-Header").FirstOrDefault());
            }
            return model;
        }

        #endregion
    }

    /// <summary>
    /// 未处理异常
    /// </summary>
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        private JavaScriptSerializer _js = null;

        /// <summary>
        /// 获取序列化对象
        /// </summary>
        private JavaScriptSerializer JS
        {
            get
            {
                if (_js == null)
                {
                    _js = new JavaScriptSerializer();
                }
                return _js;
            }
        }

        /// <summary>
        /// 异常抓获事件
        /// </summary>
        /// <param name="context">当前异常</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            object result = null;
            Exception exception = context.Exception;

            //把错误号转化为实际的错误信息 
            string message = exception.Message;
            if (exception is Evt.Framework.Common.AuthenticationException)
            {
                //未登录
                Exception ex = exception.GetBaseException();
                result = new { status = ActionResultCode.Unauthorized, data = ex.Message };
                LogUtil.Info(GetExceptionMessage(context, ex));
            }
            else if (exception is InnerException)
            {
                //内部错误
                Exception ex = exception.GetBaseException();
                result = new { status = ActionResultCode.InnerError, data = message };
                LogUtil.Warn(GetExceptionMessage(context, ex));
            }
            else if (exception is Evt.Framework.Common.MessageException)
            {
                //数据异常
                Exception ex = exception.GetBaseException();
                result = new { status = ActionResultCode.Failed, data = message };
                LogUtil.Warn(GetExceptionMessage(context, ex));
            }
            else if (exception is BusinessException)
            {
                //业务异常
                BusinessException businessEx = exception as BusinessException;
                Exception ex = exception.GetBaseException();
                result = new { status = ActionResultCode.Failed, data = businessEx.Message };
                LogUtil.Warn(GetExceptionMessage(context, ex));
            }
            else if (exception is HttpRequestValidationException)
            {
                //验证异常
                Exception ex = exception.GetBaseException();
                result = new { status = ActionResultCode.Failed, data = "请输入有效字符！" };
                LogUtil.Info(GetExceptionMessage(context, ex));
            }
            else
            {
                //未知异常：全部写入Error级别的日志
                Exception ex = exception.GetBaseException();
                LogUtil.Error(GetExceptionMessage(context, ex));

                result = new { status = ActionResultCode.UnknownError, data = "出故障啦，请您稍后重新尝试！" };
            }
            context.Response = context.Request.CreateResponse();
            context.Response.Headers.Clear();
            context.Response.Content = new StringContent(JS.Serialize(result));
            context.Response.StatusCode = System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// 拼装异常信息与环境信息
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <param name="ex">异常</param>
        /// <returns>异常信息与环境信息文本</returns>
        private static string GetExceptionMessage(HttpActionExecutedContext context, Exception ex)
        {
            string paramData = Evt.Framework.Common.JsonUtil.Serialize(context.ActionContext.ActionArguments);
            ApiBaseController bc = (ApiBaseController)context.ActionContext.ControllerContext.Controller;
            string sessionID = bc.Session.SessionID;
            return ex.ToString() + "\r\n  ParamData:" + paramData + "  SessionID:" + sessionID + "\r\n  URL:" + context.Request.RequestUri + "\r\n\r\n";
        }
    }

    /// <summary>
    /// Model校验特性过滤器
    /// </summary>
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        private JavaScriptSerializer _js = null;

        /// <summary>
        /// 获取序列化对象
        /// </summary>
        private JavaScriptSerializer JS
        {
            get
            {
                if (_js == null)
                {
                    _js = new JavaScriptSerializer();
                }
                return _js;
            }
        }

        /// <summary>
        /// Action执行时触发
        /// </summary>
        /// <param name="actionContext">actionContext</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                string errors = string.Empty;
                foreach (KeyValuePair<string, ModelState> keyValue in actionContext.ModelState)
                {
                    if (keyValue.Value.Errors.Count > 0)
                    {
                        errors = keyValue.Value.Errors[0].ErrorMessage;
                        break;
                    }
                }

                object result = new { status = ActionResultCode.Failed, data = errors };
                actionContext.Response = actionContext.Request.CreateResponse();
                actionContext.Response.Headers.Clear();
                actionContext.Response.Content = new StringContent(JS.Serialize(result));
                actionContext.Response.StatusCode = System.Net.HttpStatusCode.OK;
            }
        }
    }
}
