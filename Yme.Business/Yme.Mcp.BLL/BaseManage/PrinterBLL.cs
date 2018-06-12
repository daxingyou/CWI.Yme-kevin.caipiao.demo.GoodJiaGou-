using Yme.Util;
using Yme.Cache.Factory;
using Yme.Util.Extension;
using Yme.Util.Exceptions;
using Yme.Data.Repository;
using Yme.Mcp.Model.McpApi;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Service.BaseManage;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Model.QueryModels;

using System;
using System.Linq;
using System.Collections.Generic;
using Yme.Util.Log;
using Yme.Mcp.Model.Definitions;

namespace Yme.Mcp.BLL.BaseManage
{
    /// <summary>
    /// 打印机业务
    /// </summary>
    public class PrinterBLL
    {
        #region 私有变量

        private string apiDomain = ConfigUtil.McpApiDomain;
        private IShopPrinterService spServ = new ShopPrinterService();
        private IPrinterConfigService pcServ = new PrinterConfigService();

        #endregion

        #region 公有方法

        /// <summary>
        /// 获取打印机列表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <returns></returns>
        public object GetShopPrinterList(long shopId)
        {
            var sPrinterList = spServ.GetList(shopId);
            var printers = (from p in sPrinterList
                            select new
                            {
                                PrinterCode = p.PrinterCode,
                                PrinterName = p.PrinterName,
                                IsConfig = (p.EnabledFlag == EnabledFlagType.Valid.GetHashCode()) ? p.IsConfig : 2
                            });
            var printerDic = new Dictionary<string, object>();
            printerDic.Add("Printers", printers);
            return printerDic;
        }

        /// <summary>
        /// 校验当前门店是否关联打印机
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public bool CheckShopIsBindPrinter(long shopId)
        {
            var sPrinterList = spServ.GetList(shopId);
            return sPrinterList != null ? sPrinterList.Count() > 0 : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ShopPrinterEntity> GetShopPrinterList(string printerCode)
        {
            return spServ.GetList(printerCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public List<ShopPrinterEntity> GetPlatformPrinters(PlatformType platformType)
        {
            return spServ.GetList(platformType);
        }

        /// <summary>
        /// 获取打印机信息
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="printerCode">打印机设备号</param>
        /// <returns></returns>
        public object GetPrinterInfo(long shopId, string printerCode)
        {
            var entity = spServ.GetEntity(printerCode);
            var pconfigs = entity != null ? pcServ.GetShopPrinterConfigStr(shopId, entity.PrinterCode, BusinessType.Waimai) : string.Empty;

            var info = new
            {
                PrinterCode = entity != null ? entity.PrinterCode : string.Empty,
                PrinterName = entity != null ? entity.PrinterName : string.Empty,
                PrinterType = entity != null ? entity.PrinterType : string.Empty,
                PrintConfig = pconfigs,
                Status = entity != null ? entity.EnabledFlag : EnabledFlagType.Disabled.GetHashCode()
            };

            return info;
        }

        /// <summary>
        /// 获取打印场景设置列表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="printerCode">打印机设备号</param>
        /// <param name="businessType">业务类型</param>
        /// <returns></returns>
        public object GetShopPrinterConfigList(long shopId, string printerCode, BusinessType businessType)
        {
            var configs = pcServ.GetShopPrinterConfigList(shopId, printerCode, businessType);
            var configDic = new Dictionary<string, object>();
            configDic.Add("MaxCopies", ConfigUtil.MaxCopies);
            configDic.Add("Configs", configs.ToList<PrinterConfigsQueryModel>());
            return configDic;
        }

        /// <summary>
        /// 获取打印设置列表
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="businessType">业务类型</param>
        /// <returns></returns>
        public object GetShopPrintConfigList(long shopId, BusinessType businessType, bool isContainCancel = true)
        {
            var configs = pcServ.GetShopPrintConfigList(shopId, businessType, isContainCancel);
            var configDic = new Dictionary<string, object>();
            configDic.Add("Configs", configs.ToList<BillConfigsQueryModel>());
            return configDic;
        }

        /// <summary>
        /// 获取打印场景设置列表
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public List<BillPrintQueryModel> GetPriterConfigList(long shopId, BusinessType businessType, bool isContainCancel = true, int sortBy = 0)
        {
            return pcServ.GetPriterConfigList(shopId, businessType, isContainCancel, sortBy);
        }

        /// <summary>
        /// 设置打印机场景
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="printerCode">打印机设备号</param>
        /// <param name="configs">设置信息</param>
        /// <returns></returns>
        public object DoConfigShopPrinter(long shopId, string printerCode, string configs)
        {
            var configStr = string.Empty;
            var btList = SingleInstance<BillTemplateBLL>.Instance.GetBillTempList();
            var db = new RepositoryFactory().BaseRepository().BeginTrans();

            try
            {
                var printer = GetValidPrinter(shopId, printerCode);
                if (printer != null)
                {
                    #region 保存配置

                    var shopConfigs = pcServ.GetList(shopId, printerCode);
                    if (shopConfigs != null && shopConfigs.Count > 0)
                    {
                        //已配置先删除
                        pcServ.Delete(shopId, printerCode);
                    }

                    //添加新配置项目
                    var list = new List<PrinterConfigEntity>();
                    if (!string.IsNullOrWhiteSpace(configs))
                    {
                        var flag = 0;
                        var businessType = 0;
                        var tConfigList = configs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                        foreach (var config in tConfigList)
                        {
                            var configList = config.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            if (configList.Length == 2)
                            {
                                var billId = Extensions.ToInt(configList[0]);
                                if (flag == 0)
                                {
                                    var bt = btList.Find(d => d.BillId == billId);
                                    businessType = bt != null ? bt.BusinessType : 0;
                                }
                                list.Add(new PrinterConfigEntity()
                                {
                                    ShopId = shopId,
                                    PrinterCode = printerCode,
                                    BusinessType = businessType,
                                    BillId = billId,
                                    Copies = Extensions.ToInt(configList[1], 1),
                                    EnabledFlag = EnabledFlagType.Valid.GetHashCode(),
                                    DeleteFlag = DeleteFlagType.Valid.GetHashCode(),
                                    CreateUserId = shopId.ToString(),
                                    CreateDate = DateTime.Now
                                });

                                flag++;
                            }
                        }
                        pcServ.InserEntities(list);
                    }

                    //更新打印机配置标识
                    printer.IsConfig = list.Count > 0 ? 1 : 0;
                    spServ.UpdateEntity(printer);

                    #endregion

                    db.Commit();

                    configs = pcServ.GetShopPrinterConfigStr(shopId, printerCode, BusinessType.Waimai);
                }
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogUtil.Error(ex.StackTrace);
                throw new BusinessException(ex.Message);
            }

            return configStr;
        }

        /// <summary>
        /// 修改打印机名称
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="printerCode">打印机设备号</param>
        /// <param name="printerName">打印机名称</param>
        public void UpadatePrinterName(long shopId, string printerCode, string printerName)
        {
            try
            {
                var printer = GetValidPrinter(shopId, printerCode);

                if (printer != null)
                {
                    var isExists = spServ.CheckEntity(shopId, printerCode, printerName);
                    if (!isExists)
                    {
                        printer.PrinterName = printerName.ToUpper();
                        spServ.UpdateEntity(printer);
                    }
                    else
                    {
                        throw new BusinessException(string.Format("名称：{0}已存在，请重新命名。", printerName));
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.StackTrace);
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// 修改打印机状态
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="printerCode">打印机设备号</param>
        /// <param name="status">打印机状态</param>
        public object UpadatePrinterStatus(long shopId, string printerCode, int status)
        {
            var dbStatus = -1;
            try
            {
                var printer = GetValidPrinter(shopId, printerCode);
                if (printer != null)
                {
                    if (printer.EnabledFlag != status)
                    {
                        printer.EnabledFlag = status;
                        spServ.UpdateEntity(printer);
                    }
                    dbStatus = printer.EnabledFlag;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.StackTrace);
                throw new BusinessException(ex.Message);
            }
            return dbStatus;
        }

        /// <summary>
        /// 绑定门店打印机
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="printerCode">打印机设备号</param>
        /// <param name="checkCode">打印机校验码</param>
        /// <returns></returns>
        public object DoBindShopPrinter(long shopId, string printerCode, string checkCode)
        {
            try
            {
                //获取访问令牌
                var accessToken = GetAccessToken();

                //执行绑定打印机
                var errMsg = string.Empty;
                var bindModel = new PrintViewModel();
                bindModel.access_token = accessToken;
                bindModel.merchant_code = shopId.ToString();
                bindModel.printer_codes = string.Format(@"{0}#{1}", printerCode, checkCode);
                var isBind = DoBindPrinter(bindModel, out errMsg);
                if (isBind)
                {
                    //平台记录绑定关系
                    printerCode = printerCode.ToUpper();
                    var entity = new ShopPrinterEntity();
                    entity.ShopId = shopId;
                    entity.PrinterCode = printerCode;
                    entity.PrinterType = string.Empty;
                    entity.PrinterName = string.Format("MCP_{0}", printerCode.Substring(printerCode.Length - 6, 6));
                    entity.IsConfig = 0;
                    entity.EnabledFlag = EnabledFlagType.Valid.GetHashCode();
                    entity.CreateUserId = shopId.ToString();
                    entity.CreateDate = DateTime.Now;
                    spServ.InsertEntity(entity);

                    //返回打印机信息
                    var printer = new
                    {
                        PrinterCode = entity.PrinterCode,
                        PrinterName = entity.PrinterName,
                        PrinterType = entity.PrinterType,
                        PrintConfig = string.Empty,
                        Status = entity.EnabledFlag
                    };
                    return printer;
                }
                else
                {
                    throw new BusinessException(errMsg);
                }
            }
            catch(Exception ex)
            {
                LogUtil.Error(string.Format("绑定打印机失败，参考消息：{0}", ex.Message));
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// 解除打印机绑定
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="printerCode">打印机设备号</param>
        /// <returns></returns>
        public object DoUnBindShopPrinter(long shopId, string printerCode)
        {
            object list = null;
            var db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                var printer = GetValidPrinter(shopId, printerCode);
                if (printer != null)
                {
                    //获取访问令牌
                    var accessToken = GetAccessToken();

                    //执行解绑打印机
                    var errMsg = string.Empty;
                    var unBindModel = new PrinterBaseViewModel();
                    unBindModel.access_token = accessToken;
                    unBindModel.merchant_code = shopId.ToString();
                    unBindModel.printer_codes = printerCode;
                    var isUnBind = DoUnBindPrinter(unBindModel, out errMsg);
                    if (isUnBind)
                    {
                        //平台删除绑定关系
                        spServ.DeleteEntity(printer.PrinterCode);

                        //删除打印机配置信息
                        pcServ.Delete(shopId, printer.PrinterCode);
                    }
                    else
                    {
                        throw new BusinessException(errMsg);
                    }
                }

                db.Commit();

                //返回门店打印机列表
                list = GetShopPrinterList(shopId);
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogUtil.Error(string.Format("解绑打印机失败，参考消息：{0}", ex.Message));
                throw new BusinessException(ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 获取打印机信息
        /// </summary>
        /// <param name="printerCode"></param>
        /// <returns></returns>
        public ShopPrinterEntity GetPrinterInfo(string printerCode)
        {
            return spServ.GetEntity(printerCode);
        }

        #endregion

        #region 私有方法

        private ShopPrinterEntity GetValidPrinter(long shopId, string printerCode)
        {
            var printer = spServ.GetEntity(printerCode);
            if (printer != null)
            {
                if (shopId != printer.ShopId)
                {
                    printer = null;
                    throw new BusinessException("您无权限操作此打印机！");
                }
            }
            else
            {
                throw new BusinessException("打印机未关联！");
            }
            return printer;
        }

        #endregion

        #region 微云打API接口

        /// <summary>
        /// 获取微云打API接口访问令牌
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            var appModel = new AppViewModel();
            var expiresIn = 0;
            var accessToken = CacheFactory.Cache().GetCache<string>(appModel.app_id.Trim()) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                var url = string.Format("{0}/mcp/sys/getAccessToken", apiDomain);
                var parms = string.Format("app_id={0}&app_key={1}", appModel.app_id.Trim(), appModel.app_key.Trim());
                var result = HttpRequestUtil.HttpGet(url, parms);
                var apiData = JsonUtil.ToObject<ApiDataQueryModel>(result);
                if (apiData != null && apiData.status == 1)
                {
                    var token = JsonUtil.ToObject<Dictionary<string, object>>(apiData.data.ToString());
                    if (token != null && token.ContainsKey("access_token"))
                    {
                        //缓存中访问令牌,提前10分钟过期
                        accessToken = token["access_token"].ToString();
                        expiresIn = Extensions.ToInt(token["expires_in"], 0) - CacheKeyConsts.CachePreExpiresSeconds;
                        expiresIn = expiresIn > 0 ? expiresIn : 0;
                        CacheFactory.Cache().WriteCache(expiresIn > 0 ? accessToken : string.Empty, appModel.app_id.Trim(), TimeUtil.Now.AddSeconds(expiresIn));
                    }
                }
                return accessToken;
            }
            else
            {
                return accessToken ?? string.Empty;
            }
        }

        /// <summary>
        /// 关联打印机
        /// </summary>
        /// <param name="bindModel"></param>
        /// <returns></returns>
        public bool DoBindPrinter(PrintViewModel bindModel, out string errMsg)
        {
            var isOk = false;
            var url = string.Format("{0}/mcp/sys/bindPrinters", apiDomain);
            var parms = string.Format("app_id={0}&access_token={1}&merchant_code={2}&printer_codes={3}",
                bindModel.app_id.Trim(), bindModel.access_token.Trim(), bindModel.merchant_code.Trim(), bindModel.printer_codes.Trim());
            var result = HttpRequestUtil.HttpPost(url, parms);
            var apiData = JsonUtil.ToObject<ApiDataQueryModel>(result);
            if (apiData != null)
            {
                isOk = apiData.status == 1;
                errMsg = isOk ? string.Empty : apiData.data.ToString();
            }
            else
            {
                errMsg = "关联打印机失败";
            }
            return isOk;
        }

        /// <summary>
        /// 解绑打印机
        /// </summary>
        /// <param name="unBindModel"></param>
        /// <returns></returns>
        public bool DoUnBindPrinter(PrinterBaseViewModel unBindModel, out string errMsg)
        {
            var isOk = false;
            var url = string.Format("{0}/mcp/sys/unBindPrinters", apiDomain);
            var parms = string.Format("access_token={0}&merchant_code={1}&printer_codes={2}",
                unBindModel.access_token.Trim(), unBindModel.merchant_code.Trim(), unBindModel.printer_codes.Trim().ToUpper());
            var result = HttpRequestUtil.HttpPost(url, parms);
            var apiData = JsonUtil.ToObject<ApiDataQueryModel>(result);
            if (apiData != null)
            {
                isOk = apiData.status == 1;
                errMsg = isOk ? string.Empty : apiData.data.ToString();
            }
            else
            {
                errMsg = "解除打印机失败";
            }
            return isOk;
        }

        /// <summary>
        /// 打印票据
        /// </summary>
        /// <param name="billModel"></param>
        /// <returns></returns>
        public bool DoPrint(BillViewModel billModel, out string errMsg)
        {
            var isOk = false;
            try
            {
                LogUtil.Info("开始执行小票打印...");

                var url = string.Format("{0}/mcp/sys/print", apiDomain);
                var parms = string.Format("app_id={0}&access_token={1}&merchant_code={2}&printer_codes={3}&copies={4}&bill_no={5}&bill_type={6}&template_id={7}&bill_content={8}",
                    billModel.app_id.Trim(), billModel.access_token.Trim(), billModel.merchant_code.Trim(), billModel.printer_codes.Trim().ToUpper(),
                    billModel.copies.Trim(), billModel.bill_no.Trim(), billModel.bill_type.Trim(),
                    billModel.template_id.Trim(), billModel.bill_content.Trim());
                var result = HttpRequestUtil.HttpPost(url, parms);
                var apiData = JsonUtil.ToObject<ApiDataQueryModel>(result);
                if (apiData != null)
                {
                    isOk = apiData.status == 1;
                    errMsg = isOk ? string.Empty : apiData.data.ToString();
                }
                else
                {
                    errMsg = "打印失败";
                }
            }
            catch (Exception ex)
            {
                errMsg = "打印失败";
                LogUtil.Error(string.Format("执行打印失败，参考消息：{0}", ex.Message));
            }
            return isOk;
        }

        /// <summary>
        /// 检验打印机是否可关联
        /// </summary>
        /// <param name="chkModel"></param>
        /// <returns></returns>
        public bool DoChkPrintersEnableBind(PrinterCheckViewModel chkModel, out string errMsg)
        {
            var isOk = false;
            var url = string.Format("{0}/mcp/sys/chkPrintersEnableBind", apiDomain);
            var parms = string.Format("access_token={0}&printer_codes={1}",
                chkModel.access_token.Trim(), chkModel.printer_codes.Trim().ToUpper());
            var result = HttpRequestUtil.HttpPost(url, parms);
            var apiData = JsonUtil.ToObject<ApiDataQueryModel>(result);
            if (apiData != null)
            {
                isOk = apiData.status == 1;
                errMsg = isOk ? string.Empty : apiData.data.ToString();
            }
            else
            {
                errMsg = "检验失败";
            }
            return isOk;
        }

        /// <summary>
        /// 获取打印机状态
        /// </summary>
        /// <param name="printerCode"></param>
        /// <returns></returns>
        public int GetPrinterStatus(string printerCode)
        {
            var status = -1;
            var url = string.Format("{0}/mcp/sys/getPrinterStatus", apiDomain);
            var parms = string.Format("did={0}", printerCode);
            var result = HttpRequestUtil.HttpGet(url, parms);
            var apiData = JsonUtil.ToObject<ApiDataQueryModel>(result);
            if (apiData != null)
            {
                if (apiData.status == 1)
                {
                    status = apiData.data.ToInt(0);
                }
            }
            return status;
        }

        #endregion
    }
}
