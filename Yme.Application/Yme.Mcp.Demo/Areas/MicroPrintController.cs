using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Yme.Mcp.BLL.DemoManage;
using Yme.Mcp.Demo.Handel;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.Model.QueryModels;
using Yme.Util;
using Yme.Util.Log;
using Yme.Util.Extension;
using System.Reflection;
using System.Collections.Specialized;

namespace Yme.Mcp.Demo.Controllers.Api
{
    /// <summary>
    /// 私有云控制器
    /// </summary>
    public class MicroPrintController : ApiBaseController
    {
        #region 常量

        /// <summary>
        /// 尝试延长执行间隔，单位：毫秒
        /// </summary>
        private const int delayTryInterval = 3000;

        /// <summary>
        /// 失败后尝试次数
        /// </summary>
        private const int tryMax = 5;

        /// <summary>
        /// 密钥名称
        /// </summary>
        private const string keyName = "secrect_key";

        #endregion

        /// <summary>
        /// 查询打印任务
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        [HttpGet]
        public object GetPrintTask()
        {
            var actionDesc = "查询打印任务";
            var queryStrings = string.Empty;
            var retData = new Dictionary<string, object>();
            var sortDics = new SortedDictionary<string, object>();
            var requestForm = base.GetRequestParams(out sortDics, out queryStrings, true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count > 0)
            {
                //1.参数校验
                var retModel = new McpCommRetQueryModel();
                retModel.OperationType = McpOpType.GetTask.GetHashCode();
                var result = CheckParams(requestForm, sortDics, ref retModel);
                if (result != null)
                {
                    return result;
                }

                //2.执行查询
                var task = SingleInstance<TaskBLL>.Instance.GetNextTask(retModel.device_id);
                if (task != null)
                {
                    retData = RespData(ResponseType.NewPrintTask, task.TaskId, task.TaskDataType, task.TaskData.ToJObject());
                }
                else
                {
                    retData = RespData(ResponseType.NoPrintTask);
                }
                return RespMsg(ErrorCodeType.Success, "SUCCESS", retData);
            }
            else
            {
                retData = RespData(ResponseType.NoPrintTask);
                return RespMsg(ErrorCodeType.ParamError, "缺少参数", retData);
            }
        }

        /// <summary>
        /// 同步打印任务状态
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        public object SyncPrintTaskStatus()
        {
            var actionDesc = "同步打印任务状态";
            var queryStrings = string.Empty;
            var sortDics = new SortedDictionary<string, object>();
            var requestForm = base.GetRequestParams(out sortDics, out queryStrings, true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count > 0)
            {
                //1.参数校验
                var retModel = new McpCommRetQueryModel();
                retModel.OperationType = McpOpType.SyncTaskStatus.GetHashCode();
                var result = CheckParams(requestForm, sortDics, ref retModel);
                if (result != null)
                {
                    return result;
                }

                //2.执行同步【失败将重复执行】
                var msg = string.Empty;
                var handleResult = false;
                for (var t = 1; t <= tryMax; t++)
                {
                    try
                    {
                        var cnt = SingleInstance<TaskBLL>.Instance.UpdateTaskStatus(retModel.task_id, retModel.device_id, retModel.print_result);
                        handleResult = cnt > 0 || cnt == -1;
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        handleResult = false;
                        LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                    }
                    if (handleResult)
                    {
                        msg = "SUCCESS";
                        LogUtil.Info(string.Format("{0}成功！", actionDesc));
                        break;
                    }
                    else
                    {
                        Thread.Sleep(delayTryInterval);
                        continue;
                    }
                }
                return RespMsg(ErrorCodeType.Success, msg);
            }
            else
            {
                return RespMsg(ErrorCodeType.ParamError, "缺少参数");
            }
        }

        /// <summary>
        /// 同步打印机状态
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        public object SyncPrinterStatus()
        {
            var actionDesc = "同步打印机状态";
            var queryStrings = string.Empty;
            var retData = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(ConfigUtil.CallBackUrl))
            {
                retData.Add("new_url", ConfigUtil.CallBackUrl);
            }
            var sortDics = new SortedDictionary<string, object>();
            var requestForm = base.GetRequestParams(out sortDics, out queryStrings, true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count > 0)
            {
                //1.参数校验
                var retModel = new McpCommRetQueryModel();
                retModel.OperationType = McpOpType.SyncDeviceStatus.GetHashCode();
                var result = CheckParams(requestForm, sortDics, ref retModel);
                if (result != null)
                {
                    return result;
                }

                //2.执行同步【失败将重复执行】
                var msg = string.Empty;
                var handleResult = false;
                for (var t = 1; t <= tryMax; t++)
                {
                    try
                    {
                        var cnt = SingleInstance<DeviceBLL>.Instance.UpdateStatus(retModel.device_id, retModel.status);
                        handleResult = cnt > 0 || cnt == -1;
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        handleResult = false;
                        LogUtil.Error(string.Format("{0}失败, 参考信息：{1}", actionDesc, ex.Message));
                    }
                    if (handleResult)
                    {
                        msg = "SUCCESS";
                        LogUtil.Info(string.Format("{0}成功！", actionDesc));
                        break;
                    }
                    else
                    {
                        Thread.Sleep(delayTryInterval);
                        continue;
                    }
                }
                return RespMsg(ErrorCodeType.Success, msg, retData);
            }
            else
            {
                return RespMsg(ErrorCodeType.ParamError, "缺少参数", retData);
            }
        }

        /// <summary>
        /// 校验参数
        /// </summary>
        /// <param name="requestForm"></param>
        /// <param name="sortDics"></param>
        /// <param name="deviceId"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private object CheckParams(NameValueCollection requestForm, SortedDictionary<string, object> sortDics, ref McpCommRetQueryModel retModel)
        {
            object checkResult = null;
            sortDics = sortDics ?? new SortedDictionary<string, object>();
            retModel = retModel ?? new McpCommRetQueryModel();
            var data = requestForm["data"] ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(data))
            {
                //json请求格式
                var parms = data.ToString().ToObject<TaskResultQueryModel>();
                retModel.device_id = parms != null ? parms.device_id : string.Empty;
                retModel.sign = parms != null ? parms.sign : string.Empty;
                retModel.task_id = parms != null ? parms.task_id : string.Empty;
                retModel.print_result = parms != null ? parms.print_result : 0;
                retModel.status = parms != null ? parms.status : 0;


                sortDics.Clear();
                var signKey = "sign";
                var p = parms.GetType();
                var flag = true;
                foreach (PropertyInfo pi in p.GetProperties())
                {
                    var fieldName = pi.Name;
                    if (!fieldName.ToLower().Equals(signKey))
                    {
                        if(fieldName.ToLower().Equals("task_id") || fieldName.ToLower().Equals("print_result"))
                        {
                            flag = retModel.OperationType == 1;
                        }
                        else if(fieldName.ToLower().Equals("status"))
                        {
                            flag = retModel.OperationType == 2;
                        }
                        else
                        {
                            flag = true;
                        }

                        if (flag)
                        {
                            var fieldValue = pi.GetValue(parms, null);
                            sortDics.Add(fieldName, fieldValue);
                        }
                    }
                }
            }
            else
            {
                //连接串请求格式
                retModel.device_id = requestForm["device_id"] ?? string.Empty;
                retModel.sign = requestForm["sign"] ?? string.Empty;
                if (retModel.OperationType == 1)
                {
                    retModel.task_id = requestForm["task_id"] ?? string.Empty;
                    retModel.print_result = requestForm["print_result"] != null ? requestForm["print_result"].ToInt() : 0;

                }
                if (retModel.OperationType == 2)
                {
                    retModel.status = requestForm["status"] != null ? requestForm["status"].ToInt() : 0;
                }
            }

            //1.参数校验
            if (string.IsNullOrWhiteSpace(retModel.device_id))
            {
                checkResult = RespMsg(ErrorCodeType.ParamError, "设备Id不可为空");
            }
            if (retModel.OperationType == 1 && string.IsNullOrWhiteSpace(retModel.task_id))
            {
                checkResult = RespMsg(ErrorCodeType.ParamError, "任务Id不可为空");
            }
            if (retModel.OperationType == 1 && (retModel.print_result > TaskStatus.PrinterFault.GetHashCode() || retModel.print_result < TaskStatus.PrintSuccess.GetHashCode()))
            {
                checkResult = RespMsg(ErrorCodeType.ParamError, "打印结果不正确");
            }
            if (retModel.OperationType == 2 && (retModel.status > DeviceStatus.Fault.GetHashCode() || retModel.status < DeviceStatus.Enable.GetHashCode()))
            {
                checkResult = RespMsg(ErrorCodeType.ParamError, "打印机状态不正确");
            }
            //if (string.IsNullOrWhiteSpace(retModel.sign))
            //{
            //    checkResult = RespMsg(ErrorCodeType.ParamError, "签名参数不可为空");
            //}
            //2.安全校验
            if (!string.IsNullOrWhiteSpace(retModel.sign))
            {
                var sysSign = MD5Util.GetParmsSign(sortDics, ConfigUtil.TaskSecrcetKey, keyName);
                if (retModel.sign == null || !retModel.sign.Equals(sysSign, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    checkResult = RespMsg(ErrorCodeType.Invalid, "安全校验未通过");
                }
            }
            return checkResult;
        }
    }
}
