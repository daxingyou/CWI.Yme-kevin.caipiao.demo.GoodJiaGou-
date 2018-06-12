using Yme.Util;

namespace Yme.Mcp.Model.WeChat
{
    /// <summary>
    /// 消息模版基类
    /// </summary>
    public class TemplateMsgModel
    {
        public string TemplateId { get; set; }

        public TemplateDataItem first { get; set; }

        public TemplateDataItem remark { get; set; }
    }

    #region 订单消息

    /// <summary>
    /// 新订单
    /// </summary>
    public class NewOrderTempModel : TemplateMsgModel
    {
        public TemplateDataItem keyword1 { get; set; }

        public TemplateDataItem keyword2 { get; set; }

        public TemplateDataItem keyword3 { get; set; }

        public NewOrderTempModel()
            : base()
        {
            TemplateId = ConfigUtil.IsTestModel ? WeChatConsts.T_NewOrderWxTempId : WeChatConsts.NewOrderWxTempId;
        }
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    public class CancelOrderTempModel : TemplateMsgModel
    {
        public TemplateDataItem keyword1 { get; set; }

        public TemplateDataItem keyword2 { get; set; }

        public TemplateDataItem keyword3 { get; set; }

        public TemplateDataItem keyword4 { get; set; }

        public CancelOrderTempModel()
            : base()
        {
            TemplateId = ConfigUtil.IsTestModel ? WeChatConsts.T_CancelOrderWxTempId : WeChatConsts.CancelOrderWxTempId;
        }
    }

    /// <summary>
    /// 预订单提醒
    /// </summary>
    public class RemindPreOrderTempModel : TemplateMsgModel
    {
        public TemplateDataItem keyword1 { get; set; }

        public TemplateDataItem keyword2 { get; set; }

        public RemindPreOrderTempModel()
            : base()
        {
            TemplateId = ConfigUtil.IsTestModel ? WeChatConsts.T_RemindPreOrderWxTempId : WeChatConsts.RemindPreOrderWxTempId;
        }
    }

    /// <summary>
    /// 打印失败
    /// </summary>
    public class OrderPrintFailTempModel: TemplateMsgModel
    {
         public TemplateDataItem keyword1 { get; set; }

        public TemplateDataItem keyword2 { get; set; }

        public TemplateDataItem keyword3 { get; set; }

        public OrderPrintFailTempModel()
            : base()
        {
            TemplateId = ConfigUtil.IsTestModel ? WeChatConsts.T_OrderPrintFailWxTempId : WeChatConsts.OrderPrintFailWxTempId;
        }
    }

    #endregion

    #region 打印机异常消息

    /// <summary>
    /// 打印机故障
    /// </summary>
    public class PrinterFaultTempModel : TemplateMsgModel
    {
        public TemplateDataItem keyword1 { get; set; }

        public TemplateDataItem keyword2 { get; set; }

        public TemplateDataItem keyword3 { get; set; }

        public PrinterFaultTempModel()
            : base()
        {
            TemplateId = ConfigUtil.IsTestModel ? WeChatConsts.T_PrinterFaultWxTempId : WeChatConsts.PrinterFaultWxTempId;
        }
    }

    #endregion
}
