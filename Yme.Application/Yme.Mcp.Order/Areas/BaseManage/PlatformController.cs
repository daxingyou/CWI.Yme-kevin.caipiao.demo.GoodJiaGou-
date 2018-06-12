using System.Web.Http;

using Yme.Util;
using Yme.Util.Log;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.BLL.BaseManage;
using Yme.Mcp.Order.Controllers;
using Yme.Mcp.Order.Handel;
using Yme.Mcp.Model.Definitions;
using System.Text;
using Yme.Mcp.Model;
using System.Web;

namespace Yme.Mcp.Order.Controllers.Api
{
    /// <summary>
    /// 平台控制器
    /// </summary>
    public class PlatformController : ApiBaseController
    {
        /// <summary>
        /// 获取平台列表
        /// </summary>
        /// <returns></returns>
        [NonAuthorize]
        [HttpGet]
        public object GetList()
        {
            LogUtil.Info("执行获取平台列表请求...");

            var result = SingleInstance<PlatformBLL>.Instance.GetPlatforms(BusinessType.Waimai);
            return OK(result);
        }

        /// <summary>
        /// 获取门店平台列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetShopPlatformList()
        {
            LogUtil.Info("执行获取门店平台列表请求...");
            var result = SingleInstance<PlatformBLL>.Instance.GetShopPlatforms(base.CurrId, BusinessType.Waimai);
            return OK(result);
        }

        /// <summary>
        /// 获取门店平台详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetShopPlatformDetail(long authId)
        {
            LogUtil.Info("执行获取门店平台详情请求...");
            var result = SingleInstance<PlatformBLL>.Instance.GetShopPlatformDetail(authId);
            return OK(result);
        }

        /// <summary>
        /// 解除门店平台授权
        /// </summary>
        /// <param name="authId">授权记录Id</param>
        /// <returns></returns>
        [HttpGet]
        public object UnMapShopAuth(long authId)
        {
            LogUtil.Info("执行解除门店平台授权请求...");
            var isOK = SingleInstance<PlatformBLL>.Instance.UnMapShopAuth(base.CurrId, authId);
            if (isOK)
            {
                return OK();
            }
            else
            {
                return Error("取消授权失败！");
            }
        }

        /// <summary>
        /// 提交门店平台授权申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public object MapShopAuth([FromBody]MapPlatformViewModel model)
        {
            LogUtil.Info("执行提交授权申请请求...");
            var isOK = SingleInstance<PlatformBLL>.Instance.MapBaiduShopAuth(base.CurrId, model.PlatformShopId);
            if (isOK)
            {
                return OK(string.Format(BaiduwmConsts.SHOP_AUTH_URL, ConfigUtil.BaiduAuthSourceKey));
            }
            else
            {
                return Error("提交授权申请失败！");
            }
        }
    }
}