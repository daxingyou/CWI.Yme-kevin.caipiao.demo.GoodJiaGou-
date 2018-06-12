namespace Yme.Mcp.Model.QueryModels
{
    public class BillConfigsQueryModel
    {
        /// <summary>
        /// 小票ID
        /// </summary>
        public int BillId { get; set; }

        /// <summary>
        /// 小票名称
        /// </summary>
        public string BillName { get; set; }
    }

    /// <summary>
    /// 打印机配置信息
    /// </summary>
    public class PrinterConfigsQueryModel : BillConfigsQueryModel
    {
        /// <summary>
        /// 是否配置：0-未配置，1-已配置
        /// </summary>
        public int IsConfig { get; set; }

         /// <summary>
        /// 打印份数
        /// </summary>
        public int Copies { get; set; }

        /// <summary>
        /// 小票模版地址
        /// </summary>
        public string BillTemplateUrl { get; set; }
    }
}

