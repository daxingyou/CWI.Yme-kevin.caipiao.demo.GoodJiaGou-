using Yme.Util;
using Yme.Util.Log;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.Order.Controllers;
using Yme.Mcp.Order.Handel;

using System.Web.Http;
using Yme.Mcp.Model;
using Yme.Mcp.BLL.WeChatManage;

namespace Yme.Mcp.Order.Controllers.Api
{
    /// <summary>
    /// 打印机控制器
    /// </summary>
    public class PrinterController : ApiBaseController
    {
        /// <summary>
        /// 获取打印机列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetList()
        {
            LogUtil.Info("执行获取打印机列表请求...");

            var result = SingleInstance<PrinterBLL>.Instance.GetShopPrinterList(base.CurrId);
            return OK(result);
        }

        /// <summary>
        /// 获取打印机信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetInfo([FromUri]PrinterViewModel model)
        {
            LogUtil.Info("执行获取打印机信息请求...");

            var result = SingleInstance<PrinterBLL>.Instance.GetPrinterInfo(base.CurrId, model.PrinterCode.ToUpper());
            return OK(result);
        }

        /// <summary>
        /// 获取打印场景设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetConfigs([FromUri]PrinterViewModel model)
        {
            LogUtil.Info("执行获取打印场景设置请求...");

            var result = SingleInstance<PrinterBLL>.Instance.GetShopPrinterConfigList(base.CurrId, model.PrinterCode.ToUpper(), BusinessType.Waimai);
            return OK(result);
        }

        /// <summary>
        /// 修改打印机名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UpdateName([FromBody]PrinterUpdateViewModel model)
        {
            LogUtil.Info("执行修改打印机名称请求...");

            SingleInstance<PrinterBLL>.Instance.UpadatePrinterName(base.CurrId, model.PrinterCode.ToUpper(), model.PrinterName.ToUpper());
            return OK();
        }

        /// <summary>
        /// 设置打印机场景
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object DoConfig([FromBody]PrinterConfigViewModel model)
        {
            LogUtil.Info("执行设置打印机场景请求...");

            var result = SingleInstance<PrinterBLL>.Instance.DoConfigShopPrinter(base.CurrId, model.PrinterCode.ToUpper(), model.Configs);
            return OK(result);
        }

        /// <summary>
        /// 更新打印机状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UpdateStatus([FromBody]PrinterStatusViewModel model)
        {
            LogUtil.Info("执行更新打印机状态请求...");

            var result = SingleInstance<PrinterBLL>.Instance.UpadatePrinterStatus(base.CurrId, model.PrinterCode.ToUpper(), model.Status);
            return OK(result);
        }

        /// <summary>
        /// 绑定门店打印机
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object DoBind([FromBody]PrinterBindViewModel model)
        {
            LogUtil.Info("执行绑定门店打印机请求...");

            var result = SingleInstance<PrinterBLL>.Instance.DoBindShopPrinter(base.CurrId, model.PrinterCode, model.CheckCode);
            return OK(result);
        }

        /// <summary>
        /// 扫码绑定门店打印机
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public object DoBindByScan([FromBody]PrinterViewModel model)
        {
            LogUtil.Info("执行扫码绑定门店打印机请求...");

            var spChar = '-';
            var printer = model.PrinterCode.Trim();
            if (printer.IndexOf(spChar) > 0)
            {
                var printers = model.PrinterCode.Trim().Split(new char[] { spChar }, System.StringSplitOptions.RemoveEmptyEntries);

                var result = SingleInstance<PrinterBLL>.Instance.DoBindShopPrinter(base.CurrId, printers[0], printers[1]);
                return OK(result);
            }
            else
            {
                return Failed("二维码内容格式不正确！");
            }
        }

        /// <summary>
        /// 解除打印机绑定
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object DoUnBind([FromBody]PrinterViewModel model)
        {
            LogUtil.Info("执行解除打印机绑定请求...");

            var result = SingleInstance<PrinterBLL>.Instance.DoUnBindShopPrinter(base.CurrId, model.PrinterCode.ToUpper());
            return OK(result);
        }

        /// <summary>
        /// 获取JSAPI请求参数
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpGet]
        public object GetJsApiParamsModel()
        {
            LogUtil.Info("执行JsApi配置参数请求...");

            var wxType = EnumWeChatType.Client.GetHashCode();
            var url = string.Format("http://{0}/html/printer_add.html", ConfigUtil.McpOrderServerIp);
            var configModel = SingleInstance<WeChatBLL>.Instance.GetJsApiParamsModel(wxType, url);
            return OK(configModel);
        }
    }
}