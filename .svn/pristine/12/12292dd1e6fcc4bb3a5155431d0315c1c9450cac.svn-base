using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web.Security;
using System.Collections.Generic;
using System.Collections.Specialized;

using Yme.Util;
using Yme.Util.Extension;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Model.WeChat;
using Yme.Util.Security;
using Yme.Cache.Factory;
using Yme.Mcp.Entity.WeChatManage;
using Yme.Mcp.Service.WeChatManage;
using Yme.Mcp.BLL.BaseManage;
using Yme.Util.Log;
using Yme.Mcp.Model.McpApi;
using System.Transactions;
using Yme.Mcp.Model.Definitions;

namespace Yme.Mcp.BLL.WeChatManage
{
    /// <summary>
    /// 微信公众号业务
    /// </summary>
    public class WeChatBLL
    {
        #region 私有变量

        /// <summary>
        /// 微信关注用户
        /// </summary>
        private IWxUserService wxUserServ = new WxUserService();

        /// <summary>
        /// 微信Token
        /// </summary>
        private IWxTokenService wxTokenServ = new WxTokenService();

        /// <summary>
        /// 微信JsApiTicket
        /// </summary>
        private IWxJsapiticketService wxTicketServ = new WxJsapiticketService();

        #endregion

        #region 微信菜单

        /// <summary>
        /// 创建微信菜单
        /// </summary>
        /// <param name="menuList">微信菜单</param>
        /// <returns></returns>
        public string CreateMenu(List<WeChatMenuButton> menuList)
        {
            string json = Evt.Framework.Common.JsonUtil.Serialize("button", menuList);
            json = json.Replace("\\u0026", "&");
            string weiXinToken = GetWeiXinToken(EnumWeChatType.Client.GetHashCode());
            string tokenUrl = String.Format(WeChatConsts.WECHAT_MENU_ADD, weiXinToken);
            LogUtil.Debug(json);
            string result = HttpRequestUtil.WechatSendPostRequest(tokenUrl, json);
            return result;
        }

        #endregion

        #region 微信回调消息

        /// <summary>
        /// 处理json回调返回参数
        /// </summary>
        /// <param name="retstr"></param>
        public void HandleJsonCallBackStr(Stream stream, string opType = "")
        {
            byte[] b = new byte[stream.Length];
            stream.Read(b, 0, (int)stream.Length);
            string postStr = Encoding.UTF8.GetString(b);
            LogUtil.Debug(string.Format("{0}接收信息：{1}", opType, postStr));
            if (!string.IsNullOrEmpty(postStr))
            {
                WeChatMsgViewModel receiveMessageModel = new WeChatMsgViewModel();
                try
                {
                    LogUtil.Debug(string.Format("postStr：{0}", postStr));
                    WeChatDeviceMsgModel wechatDeviceMsgModel = JsonUtil.ToObject<WeChatDeviceMsgModel>(postStr);
                    LogUtil.Debug(string.Format("device_id:{0},download_url:{1},name:{2},user:{3},msg_type:{4}", wechatDeviceMsgModel.device_id, wechatDeviceMsgModel.services.wxmsg_file.download_url, wechatDeviceMsgModel.services.wxmsg_file.name, wechatDeviceMsgModel.user, wechatDeviceMsgModel.msg_type));
                    if (wechatDeviceMsgModel != null)
                    {
                        if (wechatDeviceMsgModel.msg_type.ToLower() == "set")
                        {
                            //todo...
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("json反序列化失败，异常信息：{0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 处理xml回调返回参数
        /// </summary>
        /// <param name="retstr"></param>
        public string HandleXmlCallBackStr(Stream stream, string requestUrl, string opType = "")
        {
            var retStr = string.Empty;
            byte[] b = new byte[stream.Length];
            stream.Read(b, 0, (int)stream.Length);
            string postStr = Encoding.UTF8.GetString(b);
            LogUtil.Debug(string.Format("{0}接收信息：{1}", opType, postStr));
            if (!string.IsNullOrEmpty(postStr))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(postStr);
                XmlNodeList list = doc.GetElementsByTagName("xml");
                XmlNode xn = list[0];
                WeChatMsgViewModel receiveMessageModel = new WeChatMsgViewModel();
                receiveMessageModel.FromUserName = xn.SelectSingleNode("//FromUserName") != null ? xn.SelectSingleNode("//FromUserName").InnerText : String.Empty;
                receiveMessageModel.ToUserName = xn.SelectSingleNode("//ToUserName") != null ? xn.SelectSingleNode("//ToUserName").InnerText : String.Empty;
                receiveMessageModel.Content = xn.SelectSingleNode("//Content") != null ? xn.SelectSingleNode("//Content").InnerText : String.Empty;
                receiveMessageModel.MsgType = xn.SelectSingleNode("//MsgType") != null ? xn.SelectSingleNode("//MsgType").InnerText : String.Empty;
                receiveMessageModel.CreateTime = xn.SelectSingleNode("//CreateTime") != null ? xn.SelectSingleNode("//CreateTime").InnerText : String.Empty;
                receiveMessageModel.Description = xn.SelectSingleNode("//Description") != null ? xn.SelectSingleNode("//Description").InnerText : String.Empty;
                receiveMessageModel.Format = xn.SelectSingleNode("//Format") != null ? xn.SelectSingleNode("//Format").InnerText : String.Empty;
                receiveMessageModel.MediaId = xn.SelectSingleNode("//MediaId") != null ? xn.SelectSingleNode("//MediaId").InnerText : String.Empty;
                receiveMessageModel.PicUrl = xn.SelectSingleNode("//PicUrl") != null ? xn.SelectSingleNode("//PicUrl").InnerText : String.Empty;
                receiveMessageModel.ThumbMediaId = xn.SelectSingleNode("//ThumbMediaId") != null ? xn.SelectSingleNode("//ThumbMediaId").InnerText : String.Empty;
                receiveMessageModel.Title = xn.SelectSingleNode("//Title") != null ? xn.SelectSingleNode("//Title").InnerText : String.Empty;
                receiveMessageModel.MsgId = xn.SelectSingleNode("//MsgId") != null ? xn.SelectSingleNode("//MsgId").InnerText : String.Empty;
                receiveMessageModel.Event = xn.SelectSingleNode("//Event") != null ? xn.SelectSingleNode("//Event").InnerText : String.Empty;
                receiveMessageModel.EventKey = xn.SelectSingleNode("//EventKey") != null ? xn.SelectSingleNode("//EventKey").InnerText : String.Empty;
                receiveMessageModel.Latitude = xn.SelectSingleNode("//Latitude") != null ? Extensions.ToDouble(xn.SelectSingleNode("//Latitude").InnerText) : 0;
                receiveMessageModel.Longitude = xn.SelectSingleNode("//Longitude") != null ? Extensions.ToDouble(xn.SelectSingleNode("//Longitude").InnerText) : 0;
                receiveMessageModel.Precision = xn.SelectSingleNode("//Precision") != null ? Extensions.ToDouble(xn.SelectSingleNode("//Precision").InnerText) : 0;

                #region 硬件平台属性

                receiveMessageModel.DeviceType = xn.SelectSingleNode("//DeviceType") != null ? xn.SelectSingleNode("//DeviceType").InnerText : String.Empty;
                receiveMessageModel.DeviceID = xn.SelectSingleNode("//DeviceID") != null ? xn.SelectSingleNode("//DeviceID").InnerText : String.Empty;
                receiveMessageModel.SessionID = xn.SelectSingleNode("//SessionID") != null ? xn.SelectSingleNode("//SessionID").InnerText : String.Empty;
                receiveMessageModel.OpenID = xn.SelectSingleNode("//OpenID") != null ? xn.SelectSingleNode("//OpenID").InnerText : String.Empty;

                #endregion

                retStr = DisposeWechatXmlMsg(receiveMessageModel, requestUrl);
            }
            return retStr;
        }

        /// <summary>
        /// 处理xml推送消息后响应
        /// </summary>
        /// <param name="wechatMessageModel"></param>
        /// <param name="shopModel"></param>
        /// <returns></returns>
        public string DisposeWechatXmlMsg(WeChatMsgViewModel wechatMessageModel, string requestUrl)
        {
            string xmlMsg = string.Empty;
            // 因为 枚举中不能使用event关键字  做特殊处理
            wechatMessageModel.MsgType = wechatMessageModel.MsgType != "event" ? wechatMessageModel.MsgType : "events";
            LogUtil.Debug("openId:" + wechatMessageModel.FromUserName);

            // 目前只处理图片 文字 和事件推送
            var wechatType = (EnumWeChatMessageType)Enum.Parse(typeof(EnumWeChatMessageType), wechatMessageModel.MsgType);
            switch (wechatType)
            {
                case EnumWeChatMessageType.events:
                    {
                        if (wechatMessageModel.Event == EnumWeChatEventType.subscribe.ToString())
                        {
                            //关注 
                            LogUtil.Debug("关注");
                            //Subscribe(wechatMessageModel);

                            //发送关注消息
                            var hrefUrl = String.Format("<a href='http://{0}/{1}?openid={2}'>", requestUrl, "html/login.html", wechatMessageModel.FromUserName);
                            var aubscribeAutoMessage = String.Format(ConfigUtil.WechatSubscribeAutoMessage, hrefUrl, "</a>");
                            xmlMsg = GetWxXmlTextMsgTemplate(wechatMessageModel, aubscribeAutoMessage);
                        }
                        if (wechatMessageModel.Event == EnumWeChatEventType.unsubscribe.ToString())
                        {
                            //取消关注
                            LogUtil.Debug("取消关注");
                            UnSubscribe(wechatMessageModel);
                            xmlMsg = EnumWeChatEventType.unsubscribe.ToString();
                        }
                        break;
                    }
                case EnumWeChatMessageType.text:
                    {
                        //关键词回复
                        if (wechatMessageModel.Content.Trim().ToInt() == EnumWeChatAutoMsgKey.WiFi.GetHashCode()
                            || ConfigUtil.WechatAutoMsgKeyWord1.Contains(wechatMessageModel.Content.Trim().ToLower()))
                        {
                            //Wi-Fi配网
                            var msgList = new List<Dictionary<string, string>>();
                            var msgDic = new Dictionary<string, string>();
                            msgDic.Add("Title", EnumWeChatAutoMsgKey.WiFi.GetRemark());
                            msgDic.Add("Description", ConfigUtil.WechatAutoMsgKeyDesc1);
                            msgDic.Add("PicUrl", ConfigUtil.WechatAutoMsgKeyPicUrl1);
                            msgDic.Add("Url", ConfigUtil.WechatAutoMsgKeyUrl1);
                            msgList.Add(msgDic);
                            xmlMsg = GetWxXmlNewsMsgTemplate(wechatMessageModel, msgList);
                        }
                        else if (wechatMessageModel.Content.Trim().ToInt() == EnumWeChatAutoMsgKey.Guide.GetHashCode()
                            || ConfigUtil.WechatAutoMsgKeyWord2.Contains(wechatMessageModel.Content.Trim().ToLower()))
                        {
                            //平台使用指南
                            var msgList = new List<Dictionary<string, string>>();
                            var msgDic = new Dictionary<string, string>();
                            msgDic.Add("Title", EnumWeChatAutoMsgKey.Guide.GetRemark());
                            msgDic.Add("Description", ConfigUtil.WechatAutoMsgKeyDesc2);
                            msgDic.Add("PicUrl", ConfigUtil.WechatAutoMsgKeyPicUrl2);
                            msgDic.Add("Url", ConfigUtil.WechatAutoMsgKeyUrl2);
                            msgList.Add(msgDic);
                            xmlMsg = GetWxXmlNewsMsgTemplate(wechatMessageModel, msgList);
                        }
                        else
                        {
                            //关键词列表
                            var sbStr = new StringBuilder();
                            sbStr.AppendLine("你说啥？我没听清~");
                            sbStr.AppendLine("更多服务,请回复序号:");
                            sbStr.AppendLine("[1]查看打印机Wi-Fi配网指南");
                            sbStr.AppendLine("[2]查看平台使用指南");
                            xmlMsg = GetWxXmlTextMsgTemplate(wechatMessageModel, sbStr.ToString());
                        }
                        break;
                    }
                default:
                    break;

            }
            return xmlMsg;
        }

        /// <summary>
        /// 微信回调请求
        /// </summary>
        public string CallBack(NameValueCollection requestQuery)
        {
            var redirectUrl = string.Empty;
            var openId = string.Empty;
            try
            {
                redirectUrl = requestQuery["state"] as string;
                var code = requestQuery["code"] as string;
                LogUtil.Debug(string.Format("state:{0},code:{1}", redirectUrl, code));
                if (!string.IsNullOrWhiteSpace(code))
                {
                    openId = GetWechatOpenId(code);
                }

                LogUtil.Debug(string.Format("url:{0}", redirectUrl));
            }
            catch (Exception ex)
            {
                LogUtil.Debug(ex.Message);
            }
            redirectUrl = !string.IsNullOrWhiteSpace(openId) ? string.Format("{0}?openid={1}", redirectUrl, openId) : redirectUrl;
            return redirectUrl;
        }

        #endregion

        #region 微信安全校验

        /// <summary>
        ///  微信推送消息安全校验
        /// </summary>
        /// <param name="appToken">Token</param>
        /// <returns>是否通过验证</returns>
        /// <param name="requestQuery">回调参数</param>
        /// <returns></returns>
        public bool CheckSignature(string appToken, NameValueCollection requestQuery)
        {
            string signature = Extensions.ToString(requestQuery["signature"]);
            string timestamp = Extensions.ToString(requestQuery["timestamp"]);
            string nonce = Extensions.ToString(requestQuery["nonce"]);

            string[] ArrTmp = { appToken, timestamp, nonce };
            Array.Sort(ArrTmp);//字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            return signature == tmpStr.ToLower();
        }

        #endregion

        #region 微信消息模版

        /// <summary>
        /// 发送模版消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="topcolor"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SendTemplateMsg<T>(string accessToken, string openId, string topcolor, string url, T data) where T : TemplateMsgModel
        {
            string urlFormat = string.Format(WeChatConsts.WECHAT_SEND_MSG, accessToken);
            var msgData = new TempleteModel()
            {
                touser = openId,
                template_id = data.TemplateId,
                topcolor = topcolor,
                url = url,
                data = data
            };
            return HttpRequestUtil.WechatSendPostRequest(urlFormat, JsonUtil.ToJson(msgData));
        }

        #region 业务消息模版

        /// <summary>
        /// 新订单提醒消息
        /// </summary>
        /// <param name="platformName">平台名称</param>
        /// <param name="daySeq">平台日序号</param>
        /// <param name="orderId">订单编号</param>
        /// <param name="orderTime">下单时间</param>
        /// <returns></returns>
        public NewOrderTempModel GetNewOrderMsgTemplateModel(string platformName, int daySeq, string orderId, DateTime orderTime, bool isConfirm = true)
        {
            var sbStr = new StringBuilder();
            if (isConfirm)
            {
                sbStr.AppendLine("请在确认订单后尽快备好客户下单的菜品。");
                sbStr.AppendLine("若因门店原因无法达成客户需求，请与客户电话协商处理。");
            }
            else
            {
                sbStr.AppendLine("请您根据实际情况尽快确认是否接单。");
                sbStr.AppendLine("若确认接单请及时备好客户下单的菜品。");
            }

            var temp = new NewOrderTempModel();
            temp.first = new TemplateDataItem(string.Format("您有一个新的{0}订单！", isConfirm ? string.Empty : "【待确认】"));
            temp.keyword1 = new TemplateDataItem(orderTime.ToDateTimeString());
            temp.keyword2 = new TemplateDataItem(string.Format("单号{0}", orderId));
            temp.keyword3 = new TemplateDataItem(string.Format("{0}#{1}", platformName, daySeq));
            temp.remark = new TemplateDataItem(sbStr.ToString());
            return temp;
        }

        /// <summary>
        /// 取消订单提醒消息
        /// </summary>
        /// <param name="platformName">平台名称</param>
        /// <param name="daySeq">平台日序号</param>
        /// <param name="orderId">订单编号</param>
        /// <param name="orderTime">下单时间</param>
        /// <returns></returns>
        public CancelOrderTempModel GetCancelOrderMsgTemplateModel(string platformName, int daySeq, string orderId, DateTime orderTime, string cancelReason)
        {
            var temp = new CancelOrderTempModel();
            temp.first = new TemplateDataItem("您有一个订单已取消，请关注！");
            temp.keyword1 = new TemplateDataItem(string.Format("{0}#{1}", platformName, daySeq));
            temp.keyword2 = new TemplateDataItem(orderId);
            temp.keyword3 = new TemplateDataItem(orderTime.ToDateTimeString());
            temp.keyword4 = new TemplateDataItem(cancelReason);
            temp.remark = new TemplateDataItem("请及时处理。");
            return temp;
        }

        /// <summary>
        /// 预订单提醒消息
        /// </summary>
        /// <param name="orderSource">订单来源</param>
        /// <param name="orderId">订单编号</param>
        /// <param name="orderTime">下单时间</param>
        /// <param name="daySeq">平台当日编号</param>
        /// <param name="mins">提前分钟数</param>
        /// <returns></returns>
        public RemindPreOrderTempModel GetRemindOrderMsgTemplateModel(string orderSource, string orderId, DateTime orderTime, int daySeq, int mins)
        {
            var temp = new RemindPreOrderTempModel();
            temp.first = new TemplateDataItem(string.Format("您{0}有一个{1}#{2}预订单，需提前准备以准时送达！",
                orderTime.ToChineseMonthDayString(),
                orderSource,
                daySeq));
            temp.keyword1 = new TemplateDataItem(orderId);
            temp.keyword2 = new TemplateDataItem(string.Format("{0}分钟", mins));
            temp.remark = new TemplateDataItem("若因门店原因无法达成客户下单需求，请与客户电话协商处理。");
            return temp;
        }

        /// <summary>
        /// 订单打印失败提醒消息
        /// </summary>
        /// <param name="msgCode">消息编码</param>
        /// <param name="opTime">操作时间</param>
        /// <param name="orderId">订单Id</param>
        /// <param name="platformId">平台Id</param>
        /// <param name="daySeq">日序号</param>
        /// <param name="printerName">打印机名称</param>
        /// <param name="pushMsg">推送消息</param>
        /// <returns></returns>
        public OrderPrintFailTempModel GetPrintFailMsgTemplateModel(int msgCode, DateTime opTime, string orderId, int platformId, int daySeq, string printerName = "", string pushMsg = "")
        {
            var faultResaon = PrintResultType.UnKnow.GetRemark();
            if (!string.IsNullOrWhiteSpace(pushMsg))
            {
                faultResaon = pushMsg;
            }
            else
            {
                if (msgCode > PrintResultType.Success.GetHashCode() && msgCode <= PrintResultType.UnBindClear.GetHashCode())
                {
                    faultResaon = ((PrintResultType)msgCode).GetRemark();
                }
            }

            var platformName = ((PlatformType)platformId).GetRemark();
            var sbFirstStr = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(printerName))
            {
                sbFirstStr.AppendFormat(@"您门店内订单：{0}#{1}在打印机【{2}】上打印失败，请及时处理。", platformName, daySeq, printerName);
            }
            else
            {
                sbFirstStr.AppendFormat(@"您门店内订单：{0}#{1}打印失败，请及时处理。", platformName, daySeq);
            }
            var sbRemarkStr = new StringBuilder();
            sbRemarkStr.AppendLine("您可选择手动补打。");
            sbRemarkStr.AppendLine("客服电话：400-930-8899");
            var temp = new OrderPrintFailTempModel();
            temp.first = new TemplateDataItem(sbFirstStr.ToString());
            temp.keyword1 = new TemplateDataItem(orderId);
            temp.keyword2 = new TemplateDataItem(faultResaon);
            temp.keyword3 = new TemplateDataItem(opTime.ToDateTimeString());
            temp.remark = new TemplateDataItem(sbRemarkStr.ToString());
            return temp;
        }

        /// <summary>
        /// 订单打印失败提醒消息
        /// </summary>
        /// <param name="platformId"></param>
        /// <param name="orderId"></param>
        /// <param name="daySeq"></param>
        /// <param name="opTime"></param>
        /// <param name="pushMsg"></param>
        /// <returns></returns>
        public OrderPrintFailTempModel GetPrintFailMsgTemplateModel(int platformId, string orderId, int daySeq, DateTime opTime, string pushMsg)
        {
            return GetPrintFailMsgTemplateModel(-1, opTime, orderId, platformId, daySeq, "", pushMsg);
        }

        /// <summary>
        /// 打印机异常提醒消息
        /// </summary>
        /// <param name="printerName">打印机名称</param>
        /// <param name="msgCode">异常类型</param>
        /// <param name="opTime">异常发生时间</param>
        /// <param name="printerCode">打印机设备号</param>
        /// <returns>消息实体</returns>
        public PrinterFaultTempModel GetPrinterFaultMsgTemplateModel(string printerName, int msgCode, DateTime opTime, string printerCode = "")
        {
            var faultResaon = PrinterFaultType.UnKnow.GetRemark();
            if (msgCode > PrinterFaultType.Enable.GetHashCode() && msgCode <= PrinterFaultType.NotFind.GetHashCode())
            {
                faultResaon = ((PrinterFaultType)msgCode).GetRemark();
            }

            var sbFirstStr = new StringBuilder();
            sbFirstStr.Append("您门店内有一台打印机无法正常打印，请确认后及时进行处理。");
            var sbRemarkStr = new StringBuilder();
            sbRemarkStr.AppendLine("仍无法解决问题，请联系客服人员。");
            sbRemarkStr.AppendLine("客服电话：400-930-8899");

            var temp = new PrinterFaultTempModel();
            temp.first = new TemplateDataItem(sbFirstStr.ToString());
            temp.keyword1 = new TemplateDataItem(printerName);
            temp.keyword2 = new TemplateDataItem(faultResaon);
            temp.keyword3 = new TemplateDataItem(opTime.ToDateTimeString());
            temp.remark = new TemplateDataItem(sbRemarkStr.ToString());
            return temp;
        }

        #endregion

        #endregion

        #region 微信关注&取消关注

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="wechatMessageModel"></param>
        /// <returns></returns>
        private void Subscribe(WeChatMsgViewModel wechatMessageModel)
        {
            try
            {
                LogUtil.Debug("新关注用户OpenId:" + wechatMessageModel.FromUserName);
                var dbNow = TimeUtil.Now;
                var entity = wxUserServ.GetEntity(wechatMessageModel.FromUserName, EnumWeChatType.Client.GetHashCode());
                if (entity == null)
                {
                    //新关注用户
                    entity = new WxUserEntity();
                    entity.ShopId = 0;
                    entity.OpenId = wechatMessageModel.FromUserName;
                    entity.WxType = EnumWeChatType.Client.GetHashCode();
                    entity.EnabledFlag = EnabledFlagType.Valid.GetHashCode();
                    entity.CreateDate = dbNow;
                    wxUserServ.InsertEntity(entity);
                    LogUtil.Debug(string.Format("openId:{0},关注记录写入成功!", wechatMessageModel.FromUserName));
                }
                else
                {
                    //已取消关注用户重新关注
                    if (entity.EnabledFlag != EnabledFlagType.Valid.GetHashCode())
                    {
                        entity.EnabledFlag = EnabledFlagType.Valid.GetHashCode();
                        wxUserServ.UpdateEntity(entity);
                        LogUtil.Debug(string.Format("openId:{0},关注记录更新成功!", wechatMessageModel.FromUserName));
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("openId:{0},关注记录写入失败，参考信息：{1}", wechatMessageModel.FromUserName, ex.Message));
            }
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="wechatMessageModel"></param>
        /// <returns></returns>
        private void UnSubscribe(WeChatMsgViewModel wechatMessageModel)
        {
            LogUtil.Debug(string.Format("取消关注，微信用户OpenId：{0}:", wechatMessageModel.FromUserName));
            using (TransactionScope tran = new TransactionScope())
            {
                var entity = wxUserServ.GetEntity(wechatMessageModel.FromUserName, EnumWeChatType.Client.GetHashCode());
                if (entity != null)
                {
                    //清空用户AccessToken强制用户重新登录
                    wxUserServ.DeleteEntity(entity.Id);
                    var currUser = SingleInstance<ShopBLL>.Instance.GetShopById(entity.ShopId);
                    if (currUser != null)
                    {
                        SingleInstance<ShopBLL>.Instance.DoLogout(currUser);
                    }
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 关注后回复的消息内容
        /// </summary>
        /// <param name="wechatMessageModel"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        private string GetAubscribeAutoMessage(WeChatMsgViewModel wechatMessageModel, string xml, string requestUrl)
        {
            string hrefUrl = String.Format("<a href='http://{0}/{1}?openid={2}'>", requestUrl, "html/login.html", wechatMessageModel.FromUserName);
            string aubscribeAutoMessage = String.Format(ConfigUtil.WechatSubscribeAutoMessage, hrefUrl, "</a>");
            xml = String.Format(xml, wechatMessageModel.FromUserName, wechatMessageModel.ToUserName, aubscribeAutoMessage, wechatMessageModel.CreateTime);
            return xml;
        }

        /// <summary>
        /// 关键词自动回复消息内容
        /// </summary>
        /// <param name="wechatMessageModel"></param>
        /// <param name="xml"></param>
        /// <param name="responseUrl"></param>
        /// <returns></returns>
        private string GetKeyWordsAutoMessage(WeChatMsgViewModel wechatMessageModel, string xml, string responseUrl)
        {
            xml = String.Format(xml, wechatMessageModel.FromUserName, wechatMessageModel.ToUserName, responseUrl, wechatMessageModel.CreateTime);
            return xml;
        }

        #endregion

        #region 微信自动回复消息模版

        /// <summary>
        /// 获取微信图文消息实体
        /// </summary>
        /// <param name="wechatMessageModel"></param>
        /// <param name="msgTxt"></param>
        /// <returns></returns>
        private string GetWxXmlTextMsgTemplate(WeChatMsgViewModel wechatMessageModel,string msgTxt)
        {
            var xmlSb = new StringBuilder();
            xmlSb.AppendLine("<xml>");
            xmlSb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>",wechatMessageModel.FromUserName);
            xmlSb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>",wechatMessageModel.ToUserName);
            xmlSb.AppendFormat("<CreateTime>{0}</CreateTime>",wechatMessageModel.CreateTime);
            xmlSb.AppendLine("<MsgType><![CDATA[text]]></MsgType>");
            xmlSb.AppendFormat("<Content><![CDATA[{0}]]></Content>",msgTxt);
            xmlSb.Append("</xml>");
            return xmlSb.ToString();
        }

        /// <summary>
        /// 获取微信图文消息实体列表
        /// </summary>
        /// <param name="wechatMessageModel"></param>
        /// <param name="newsList"></param>
        /// <returns></returns>
        public string GetWxXmlNewsMsgTemplate(WeChatMsgViewModel wechatMessageModel, List<Dictionary<string, string>> newsList)
        {
            //图文项目明细
            var itemSb = new StringBuilder();
            foreach (var news in newsList)
            {
                itemSb.AppendLine("<item>");
                itemSb.AppendFormat("<Title><![CDATA[{0}]]></Title> ", news["Title"].ToString());
                itemSb.AppendFormat("<Description><![CDATA[{0}]]></Description>", news["Description"].ToString());
                itemSb.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl>", news["PicUrl"].ToString());
                itemSb.AppendFormat("<Url><![CDATA[{0}]]></Url>", news["Url"].ToString());
                itemSb.Append("</item>");
            }

            //图文消息实体
            var xmlSb = new StringBuilder();
            xmlSb.AppendLine("<xml>");
            xmlSb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", wechatMessageModel.FromUserName);
            xmlSb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", wechatMessageModel.ToUserName);
            xmlSb.AppendFormat("<CreateTime>{0}</CreateTime>", wechatMessageModel.CreateTime);
            xmlSb.AppendLine("<MsgType><![CDATA[news]]></MsgType>");
            xmlSb.AppendFormat("<ArticleCount>{0}</ArticleCount>", newsList.Count);
            xmlSb.AppendLine("<Articles>");
            xmlSb.AppendFormat("{0}", itemSb.ToString());
            xmlSb.AppendLine("</Articles>");
            xmlSb.AppendLine("</xml>");
            return xmlSb.ToString();
        }

        /// <summary>
        /// 获取音乐消息实体
        /// </summary>
        /// <param name="wechatMessageModel"></param>
        /// <param name="msgDic"></param>
        /// <returns></returns>
        public string GetWxXmMusicMsgTemplate(WeChatMsgViewModel wechatMessageModel, Dictionary<string, string> msgDic)
        {
            //音乐消息实体
            var xmlSb = new StringBuilder();
            xmlSb.AppendLine("<xml>");
            xmlSb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", wechatMessageModel.FromUserName);
            xmlSb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", wechatMessageModel.ToUserName);
            xmlSb.AppendFormat("<CreateTime>{0}</CreateTime>", wechatMessageModel.CreateTime);
            xmlSb.AppendLine("<MsgType><![CDATA[music]]></MsgType>");
            xmlSb.AppendLine("<Music>");
            xmlSb.AppendFormat("<Title><![CDATA[{0}]]></Title>", msgDic["Title"]);
            xmlSb.AppendFormat("<Description><![CDATA[{0}]]></Description>", msgDic["Description"]);
            xmlSb.AppendFormat("<MusicUrl><![CDATA[{0}]]></MusicUrl>", msgDic["MusicUrl"]);
            xmlSb.AppendFormat("<HQMusicUrl><![CDATA[{0}]]></HQMusicUrl>", msgDic["HQMusicUrl"]);
            xmlSb.AppendFormat("<ThumbMediaId><![CDATA[{0}]]></HQMusicUrl>", msgDic["ThumbMediaId"]);
            xmlSb.AppendLine("</Music>");
            xmlSb.AppendLine("</xml>");
            return xmlSb.ToString();
        }

        #endregion

        #region 微信助手

        /// <summary>
        /// 更新微信用户信息
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="openId">微信用户openId</param>
        /// <param name="wxType">微信类型</param>
        /// <returns></returns>
        public int UpdateWxUserInfo(long shopId, string openId, int wxType)
        {
            int cnt = 0;
            var dbNow = TimeUtil.Now;
            try
            {
                var wxUser = wxUserServ.GetEntity(shopId, wxType);
                if (wxUser != null)
                {
                    if (wxUser.OpenId != openId)
                    {
                        LogUtil.Info("更新OpenId");

                        //先删除新OpenId原来有关联的记录
                        wxUserServ.DeleteEntity(openId, wxType);

                        //关联门店和新OpenId
                        wxUser.OpenId = openId ?? string.Empty;
                        wxUser.ModifyDate = dbNow;
                        wxUser.EnabledFlag = (int)EnabledFlagType.Valid;
                        cnt = wxUserServ.UpdateEntity(wxUser);
                    }
                }
                else
                {
                    wxUser = wxUserServ.GetEntity(openId, wxType);
                    if (wxUser != null)
                    {
                        if (wxUser.ShopId != shopId)
                        {
                            LogUtil.Info("更新门店Id");

                            //先删除原来新门店Id原来有关联的记录
                            wxUserServ.DeleteEntity(shopId, wxType);

                            //更新OpenId对应的新门店Id
                            wxUser.ShopId = shopId;
                            wxUser.ModifyDate = dbNow;
                            wxUser.EnabledFlag = (int)EnabledFlagType.Valid;
                            cnt = wxUserServ.UpdateEntity(wxUser);
                        }
                    }
                    else
                    {
                        LogUtil.Info("新增");

                        wxUser = new WxUserEntity();
                        wxUser.WxType = wxType;
                        wxUser.OpenId = openId ?? string.Empty;
                        wxUser.ShopId = shopId;
                        wxUser.CreateDate = dbNow;
                        wxUser.EnabledFlag = (int)EnabledFlagType.Valid;
                        cnt = wxUserServ.InsertEntity(wxUser);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.InnerException.ToString());
            }
            return cnt;
        }

        /// <summary>
        ///  获取用户openID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetWechatOpenId(string code)
        {
            string openId = String.Empty;
            string wechatAppId = ConfigUtil.WechatAppId;
            string wechatAppSecret = ConfigUtil.WechatAppSecret;

            string url = String.Format(WeChatConsts.WECHAT_GET_OPENID, wechatAppId, wechatAppSecret, code);
            string resultText = HttpRequestUtil.WechatSendPostRequest(url, string.Empty);
            LogUtil.Debug(string.Format("GetWechatOpenId: code: {0}, res: {1}", code, resultText));

            var dicWechat = JsonUtil.ToObject<Dictionary<string, object>>(resultText);
            if (dicWechat.ContainsKey("openid"))
            {
                openId = Extensions.ToString(dicWechat["openid"]);
            }
            return openId;
        }

        /// <summary>
        /// 通过关键字获取微信OpenId
        /// </summary>
        /// <param name="wxType"></param>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public string GetWechatOpenId(int wxType, string keyId)
        {
            var openId = string.Empty;
            if (!string.IsNullOrWhiteSpace(keyId))
            {
                if (wxType == EnumWeChatType.Client.GetHashCode())
                {
                    var wechat = wxUserServ.GetEntity(keyId.ToInt(), wxType);
                    openId = wechat != null ? wechat.OpenId : string.Empty;
                }
            }
            return openId;
        }

        /// <summary>
        /// 获取微信Token
        /// </summary>
        /// <param name="wxType"></param>
        /// <returns></returns>
        public string GetWeiXinToken(int wxType)
        {
            var isExpires = false;
            var token = string.Empty;
            var wechatAppId = wxType == 0 ? ConfigUtil.WechatAppId :string.Empty;
            var wechatAppSecret = wxType == 0 ? ConfigUtil.WechatAppSecret : string.Empty;
            var wechatToken = wxTokenServ.GetEntity(wechatAppId);
            if (wechatToken != null && !string.IsNullOrWhiteSpace(wechatToken.AccessToken))
            {
                //DB中存在校验是否过期
                isExpires = wechatToken.ExpiresTime.Value <= TimeUtil.Now;
                if (!isExpires)
                {
                    isExpires = CheckTokenIsExpired(wechatToken.AccessToken);
                    if (!isExpires)
                    {
                        token = wechatToken.AccessToken;
                    }
                    else
                    {
                        LogUtil.Debug(string.Format("DB中存在微信AccessToken显示未过期但实际已过期,DB有效期：{0}，当前时间：{1}", wechatToken.ExpiresTime.Value, TimeUtil.Now));
                    }
                }
                else
                {
                    LogUtil.Debug(string.Format("DB中存在微信AccessToken但过期,DB有效期：{0}，当前时间：{1}", wechatToken.ExpiresTime.Value, TimeUtil.Now));
                }
            }
            else
            {
                isExpires = true;
                if (wechatToken == null)
                {
                    wechatToken = new WxTokenEntity();
                    wechatToken.AppId = wechatAppId;
                }

                LogUtil.Debug("DB中不存在微信AccessToken");
            }

            if (isExpires)
            {
                //已过期重新请求
                var tokenUrl = String.Format(WeChatConsts.WECHAT_TOKEN, wechatAppId, wechatAppSecret);
                var tokenResult = HttpRequestUtil.HttpGet(tokenUrl, string.Empty);
                var dicWechat = JsonUtil.ToObject<Dictionary<string, object>>(tokenResult);
                if (dicWechat != null && dicWechat.ContainsKey("access_token"))
                {
                    wechatToken.AccessToken = dicWechat["access_token"].ToString();
                    wechatToken.ExpiresTime = TimeUtil.Now.AddSeconds(Extensions.ToInt(dicWechat["expires_in"], 0) - CacheKeyConsts.CachePreExpiresSeconds);
                    wechatToken.ModifyDate = TimeUtil.Now;
                    if (wechatToken.Id > 0)
                    {
                        //更新
                        wxTokenServ.UpdateEntity(wechatToken);
                    }
                    else
                    {
                        //初始化
                        wxTokenServ.InsertEntity(wechatToken);
                    }
                    token = wechatToken.AccessToken;
                }
            }

            return token;
        }

        /// <summary>
        /// 获取微信JsAPI票据
        /// </summary>
        /// <returns></returns>
        public string GetWxJsApiTicket(int wxType)
        {
            var isExpires = false;
            var ticket = string.Empty;
            var wechatAppId = wxType == 0 ? ConfigUtil.WechatAppId : string.Empty;
            var jsApiTicket = wxTicketServ.GetEntity(wechatAppId);
            if (jsApiTicket != null && !string.IsNullOrWhiteSpace(jsApiTicket.JsapiTicket))
            {
                //DB中存在校验是否过期
                isExpires = jsApiTicket.ExpiresTime.Value <= TimeUtil.Now;
                ticket = jsApiTicket.JsapiTicket;
            }
            else
            {
                isExpires = true;
                if (jsApiTicket == null)
                {
                    jsApiTicket = new WxJsapiticketEntity();
                    jsApiTicket.AppId = wechatAppId;
                }
            }

            if (isExpires)
            {
                //已过期重新请求
                var ticketUrl = string.Format(WeChatConsts.WECHAT_JSSDK_TICKET, GetWeiXinToken(wxType));
                string ticketResult = HttpRequestUtil.WeChatSendGetRequest(ticketUrl);
                var dicWechat = JsonUtil.ToObject<Dictionary<string, object>>(ticketResult);
                if (dicWechat != null && dicWechat.ContainsKey("ticket"))
                {
                    jsApiTicket.JsapiTicket = dicWechat["ticket"].ToString();
                    jsApiTicket.ExpiresTime = TimeUtil.Now.AddSeconds(Extensions.ToInt(dicWechat["expires_in"], 0) - CacheKeyConsts.CachePreExpiresSeconds);
                    jsApiTicket.ModifyDate = TimeUtil.Now;
                    if (jsApiTicket.Id > 0)
                    {
                        //更新
                        wxTicketServ.UpdateEntity(jsApiTicket);
                    }
                    else
                    {
                        //初始化
                        wxTicketServ.InsertEntity(jsApiTicket);
                    }
                    ticket = jsApiTicket.JsapiTicket;
                }
            }

            return ticket;
        }

        /// <summary>
        /// 校验accessToken是否过期
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public bool CheckTokenIsExpired(string accessToken)
        {
            string jsonStr = HttpRequestUtil.HttpGet(string.Format(WeChatConsts.WECHAT_TOKEN_ISEXPIRED, accessToken));
            var dicWechat = JsonUtil.ToObject<Dictionary<string, object>>(jsonStr);
            if (dicWechat != null && dicWechat.ContainsKey("errcode"))
            {
                var errcode = dicWechat["errcode"].ToString();
                if (errcode.Length > 0)
                {
                    LogUtil.Warn(jsonStr);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// JS-SDK使用权限签名算法
        /// </summary>
        /// <param name="jsapiTicket">The jsapi_ticket.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public WeChatJsApiParamsViewModel GetSign(int wxType, string url, string jsapiTicket)
        {
            //构建配置参数
            var wxConfigModel = new WeChatJsApiParamsViewModel();
            wxConfigModel.appId = ConfigUtil.WechatAppId;
            wxConfigModel.timestamp = TimeUtil.SecondTicks_1970;
            wxConfigModel.nonceStr = StringUtil.UniqueStr();
            string str = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapiTicket, wxConfigModel.nonceStr, wxConfigModel.timestamp, url);
            wxConfigModel.signature = SHAEncryptUtil.SHA1Encrypt(str);//SHA1加密
            return wxConfigModel;
        }

        /// <summary>
        /// 获取配置接口参数
        /// </summary>
        /// <param name="apiList">接口列表</param>
        /// <returns></returns>
        public WeChatJsApiParamsViewModel GetJsApiParamsModel(int wxType, string url)
        {
            string jsapiTicket = GetWxJsApiTicket(wxType);
            return GetSign(wxType, url, jsapiTicket);
        }

        #endregion
    }
}