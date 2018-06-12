﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yme.Mcp.Demo.Handel;
using Yme.Util;
using Yme.Util.Log;
using Yme.Mcp.Entity.DemoManage;
using Yme.Mcp.BLL.DemoManage;
using System.Text;

namespace Yme.Mcp.Demo.Controllers.Api
{
    /// <summary>
    /// Demo控制器
    /// </summary>
    public class DemoController : ApiBaseController
    {
        /// <summary>
        /// 买彩票
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        public object DoBuylottery()
        {
            var actionDesc = "买彩票";
            var requestForm = base.GetRequestParams(true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count > 0)
            {
                var data = requestForm["data"] ?? string.Empty;
                var decevIdList = ConfigUtil.LotDeviceIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                var deviceDics = new Dictionary<string,int>();
                foreach(var decevId in decevIdList)
                {
                    deviceDics.Add(decevId,1);
                }

                var task = new TaskEntity();
                task.TaskData = data;

                try
                {
                    SingleInstance<TaskBLL>.Instance.InsertTask(task, deviceDics);
                    return OK();
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("保存失败,参考信息：{0}", ex.Message));
                    return Failed("保存失败");
                }
            }
            else
            {
                return Failed( "缺少参数");
            }
        }

        /// <summary>
        /// 买彩票
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        public object DoBuylotterynew()
        {
            //从配置文件取值
            var url = ConfigUtil.GetValue("url");
            var app_id = ConfigUtil.GetValue("app_id");
            var access_token = ConfigUtil.GetValue("access_token");
            var merchant_code = ConfigUtil.GetValue("merchant_code");
            var template_id = ConfigUtil.GetValue("template_id");
            var copies = ConfigUtil.GetValue("copies");
            //var bill_no = ConfigUtil.GetValue("bill_no");
            var bill_no = "demo_"+DateTime.Now.ToString("yyyyMMddHHmmss");
            var bill_type = ConfigUtil.GetValue("bill_type");

            var actionDesc = "买彩票";
            var requestForm = base.GetRequestParams(true, string.Format("执行{0}", actionDesc));
            var data = requestForm["data"] ?? string.Empty;
            var dd = JsonUtil.ToJObject(data);
            string printer_codes = dd["printer_codes"].ToString();
            var bill_content = dd["params"].ToString();


            var parms = string.Format("app_id={0}&access_token={1}&merchant_code={2}&printer_codes={3}&copies={4}&bill_no={5}&bill_type={6}&template_id={7}&bill_content={8}",
                app_id.Trim(), access_token.Trim(), merchant_code.Trim(), printer_codes.Trim(),
                copies.Trim(), bill_no.Trim(), bill_type.Trim(),
                template_id.Trim(), HttpUtility.UrlEncode(bill_content.Trim(), Encoding.UTF8));
            LogUtil.Info(string.Format("准备提交请求：url:{0},参数：{1}",url,parms));
            var result = HttpRequestUtil.HttpPost(url, parms);
            var resultojb = JsonUtil.ToJObject(result);
            var status = resultojb["status"].ToString();
            var info = resultojb["data"].ToString();
            if (status=="1")
            {
                return OK();

            }
            else
            {
                return Failed(info);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpPost]
        public object DoLogin()
        {
            var actionDesc = "买彩票";
            var requestForm = base.GetRequestParams(true, string.Format("执行{0}", actionDesc));
            if (requestForm.Keys.Count > 0)
            {
                var loginCmd = requestForm["cmd"] ?? string.Empty;
                if(loginCmd == ConfigUtil.LoginCmd)
                {
                    return OK();
                }
                else
                {
                    return Failed("身份校验未通过！");
                }
            }
            else
            {
                return Failed("缺少参数");
            }
        }
    }
}
