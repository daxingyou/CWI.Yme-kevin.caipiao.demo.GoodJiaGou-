
namespace Yme.Mcp.Model.ViewModels.Waimai.Baidu
{
    public class BaiduApiRetModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int errno { get; set; }

        /// <summary>
        /// 状态说明
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get; set; }
    }
}
