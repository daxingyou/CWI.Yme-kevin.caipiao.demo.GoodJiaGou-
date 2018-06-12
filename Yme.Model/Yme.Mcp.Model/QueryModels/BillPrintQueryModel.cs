
namespace Yme.Mcp.Model.QueryModels
{
    public class BillPrintQueryModel
    {
        /// <summary>
        /// 小票类型Id
        /// </summary>
        public int BillId { get; set; }

        /// <summary>
        /// 小票内容
        /// </summary>
        public string BillUrl { get; set; }

        /// <summary>
        /// 打印份数
        /// </summary>
        public int Copies { get; set; }

        /// <summary>
        /// 打印机设备号
        /// </summary>
        public string PrinterCodes { get; set; }
    }
}
