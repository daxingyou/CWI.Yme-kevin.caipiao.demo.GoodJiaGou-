using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Cache.Factory;
using Yme.Mcp.BLL.OrderManage;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Entity.OrderManage;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Model.QueryModels;
using Yme.Mcp.Model.Waimai.Meituan;
using Yme.Mcp.Service.BaseManage;
using Yme.Util;
using Yme.Util.Extension;
using Yme.Util.Log;

namespace Yme.Mcp.BLL.BaseManage
{
    /// <summary>
    /// 小票模版
    /// </summary>
    public class BillTemplateBLL
    {
        #region 私有变量

        private IBilltemplateService btServ = new BilltemplateService();

        #endregion

        #region 公有方法

        public List<BilltemplateEntity> GetBillTempList()
        {
            return btServ.GetList();
        }

        /// <summary>
        /// 获取小票打印任务地址
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="billIds">指定小票模版ID列表</param>
        /// <returns>小票打印任务地址</returns>
        public List<BillPrintQueryModel> GetSmallBillUrlList(OrderEntity order, List<string> billIds = null)
        {
            if (order == null)
            {
                return new List<BillPrintQueryModel>();
            }
            var isContainCancel = order.OrderStatus == OrderStatus.Canceled.GetHashCode();
            var shopBills = SingleInstance<PrinterBLL>.Instance.GetPriterConfigList(order.ShopId, BusinessType.Waimai, isContainCancel);
            if (billIds != null)
            {
                shopBills = shopBills.FindAll(d => billIds.Contains(d.BillId.ToString()));
            }

            return GetSmallBillUrlList(order, shopBills);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 生成打印小票列表
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="shopBills">门店小票配置列表</param>
        /// <returns></returns>
        private List<BillPrintQueryModel> GetSmallBillUrlList(OrderEntity order, List<BillPrintQueryModel> shopBills)
        {
            var billUrl = string.Empty;
            var billList = new List<BillPrintQueryModel>();
            var domain = ConfigUtil.McpOrderServerIp;
            foreach (var config in shopBills)
            {
                if (config.BillId >= SmallBillType.Kitchen.GetHashCode() && config.BillId <= SmallBillType.Cancel.GetHashCode())
                {
                    billUrl = @"http://{0}/bill/getSamllBill?id={1}&templeid={2}";
                    billList.Add(new BillPrintQueryModel
                    {
                        BillId = config.BillId,
                        BillUrl = string.Format(billUrl, domain, order.MorderId, config.BillId.ToString()),
                        Copies = config.Copies,
                        PrinterCodes = config.PrinterCodes
                    });
                }
            }
            return billList;
        }

        #endregion
    }
}
