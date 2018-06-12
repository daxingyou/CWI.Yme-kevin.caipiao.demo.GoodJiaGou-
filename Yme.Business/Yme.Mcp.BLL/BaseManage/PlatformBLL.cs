using System.Linq;
using System.Collections.Generic;

using Yme.Util;
using Yme.Mcp.Service.Common;
using Yme.Mcp.Model.Waimai.Meituan;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Service.BaseManage;
using Yme.Util.Extension;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.Model.QueryModels;
using Yme.Util.Log;
using Yme.Mcp.Model.Definitions;
using System.Text;
using System;
using Evt.Framework.Common;
using Yme.Data.Repository;
using Yme.Util.Exceptions;
using Yme.Cache.Factory;
using Yme.Mcp.Model.ViewModels.Waimai.Eleme;
using Yme.Cache;
using Yme.Mcp.Model.ViewModels.Waimai.Baidu;

namespace Yme.Mcp.BLL.BaseManage
{
    /// <summary>
    /// 平台业务
    /// </summary>
    public class PlatformBLL
    {
        #region 私有变量

        private static ICache cache = CacheFactory.Cache();
        private IPlatformService pServ = new PlatformService();
        private IShopPlatformService spServ = new ShopPlatformService();
        private MeituanService meiServ = SingleInstance<MeituanService>.Instance;
        private ElemeService eleServ = SingleInstance<ElemeService>.Instance;
        private BaiduwmService baiServ = SingleInstance<BaiduwmService>.Instance;

        #endregion

        #region 公有方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InserEntity(ShopPlatformEntity entity)
        {
            return spServ.InsertEntity(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEntity(ShopPlatformEntity entity)
        {
            return spServ.UpdateEntity(entity);
        }

        /// <summary>
        /// 修改门店平台接单配置
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="status">启用状态</param>
        public int UpadatePlatfromIsAutoCofrim(long shopId, int platform, int status)
        {
            var dbStatus = -1;
            try
            {
                var shopPlatform = spServ.GetEntity(shopId, platform, BusinessType.Waimai.GetHashCode(), AuthBussinessType.Waimai);
                if (shopPlatform != null)
                {
                    var cnt = 0;
                    if (shopPlatform.IsAutoConfirm != status)
                    {
                        shopPlatform.IsAutoConfirm = status;
                        cnt = spServ.UpdateEntity(shopPlatform);
                    }
                    dbStatus = cnt > 0 ? shopPlatform.IsAutoConfirm : -1;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("修改门店平台接单配置失败,参考信息:{0},请求参数:{1}-{2}-{3}", ex.Message, shopId, platform, status));
                throw new BusinessException(ex.Message);
            }
            return dbStatus;
        }

        /// <summary>
        /// 获取平台列表
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public object GetPlatforms(BusinessType businessType)
        {
            var list = pServ.GetList(businessType);
            var ps = (from p in list
                      orderby p.PlatformId ascending
                      select new
                      {
                          PlatformId = p.PlatformId,
                          PlatformName = p.PlatformName
                      }).ToList();

            var platformDic = new Dictionary<string, object>();
            platformDic.Add("Platforms", ps);
            return platformDic;
        }

        /// <summary>
        /// 店铺授权平台
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="businessType">业务类型</param>
        /// <returns></returns>
        public object GetShopPlatforms(long shopId, BusinessType businessType)
        {
            var list = spServ.FindShopPlatformList(shopId, businessType);
            var spList = (from sl in list
                          select new
                          {
                              PlatformId = sl.PlatformId,
                              PlatformName = sl.PlatformName,
                              AuthId = sl.AuthId,
                              AuthStasus = sl.AuthStasus,
                              LinkUrl = sl.AuthStasus <= 0 ? GetShopPlatformMapUrl(shopId, sl.PlatformId, sl.IsKaMerchant, sl.KaMerchantAuthUrl) : string.Empty
                          }).ToList();

            var platformDic = new Dictionary<string, object>();
            platformDic.Add("Platforms", spList);
            return platformDic;
        }

        /// <summary>
        /// 获取店铺已授权平台配置
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <param name="businessType">业务类型</param>
        /// <returns></returns>
        public object GetShopPlatformConfigs(long shopId, BusinessType businessType)
        {
            var list = spServ.FindShopAuthPlatformList(shopId, businessType);
            var spList = (from sl in list
                          select new
                          {
                              PlatformId = sl.PlatformId,
                              PlatformName = sl.PlatformName,
                              Status = sl.IsAutoConfirm,
                          }).ToList();

            var platformDic = new Dictionary<string, object>();
            platformDic.Add("Configs", spList);
            return platformDic;
        }

        /// <summary>
        /// 获取门店平台详细
        /// </summary>
        /// <param name="authId"></param>
        /// <returns></returns>
        public object GetShopPlatformDetail(long authId)
        {
            var entity = spServ.GetEntity(authId);
            var result = new
            {
                AuthId = entity != null ? entity.AuthId : 0,
                PlatformId = entity != null ? entity.PlatformId : 0,
                PlatformName = entity != null ? entity.PlatformName : string.Empty,
                ShopName = entity != null ? entity.PlatformShopName : string.Empty,
                ShopmanName = entity != null ? entity.ShopmanName : string.Empty,
                Mobile = entity != null ? entity.Mobile.HideMoblie() : string.Empty,
                CityArea = entity != null ? string.Format("{0}{1}", entity.CityArea, string.IsNullOrWhiteSpace(entity.BusinessDistrict) ? string.Empty : string.Format("({0})", entity.BusinessDistrict)) : string.Empty,
                ShopAddress = entity != null ? entity.ShopAddress : string.Empty,
                BusinessScope = entity != null ? entity.BusinessScope : string.Empty,
                CancelUrl = GetShopPlatformUnMapUrl(entity.ShopId, authId),
            };
            return result;
        }

        /// <summary>
        /// 平台列表
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public List<PlatformEntity> GetPlatformList(BusinessType businessType)
        {
            return pServ.GetList(businessType);
        }

        /// <summary>
        /// 门店已授权平台列表
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public List<ShopPlatformEntity> GetShopAuthPlatformList(long shopId, BusinessType businessType, int platformId = 0)
        {
            return spServ.FindShopAuthPlatformList(shopId, businessType, platformId);
        }
        
        /// <summary>
        /// 门店平台授权列表
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public List<ShopPlatformQueryModel> GetShopPlatformList(long shopId, BusinessType businessType)
        {
            return spServ.FindShopPlatformList(shopId, businessType);
        }

        /// <summary>
        /// 查询门店平台信息
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public List<ShopPlatformEntity> FindShopPlatformList(BusinessType businessType)
        {
            return spServ.FindShopPlatformList(businessType);
        }

        /// <summary>
        /// 获取门店平台授权信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="platformId"></param>
        /// <param name="businessType"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        public ShopPlatformEntity GetShopPlatformInfo(long shopId, int platformId, int businessType, AuthBussinessType authType)
        {
            return spServ.GetEntity(shopId, platformId, businessType, authType);
        }

        /// <summary>
        /// 获取门店平台授权信息
        /// </summary>
        /// <param name="platformId">平台Id</param>
        /// <param name="pShopId">平台门店Id</param>
        /// <returns></returns>
        public ShopPlatformEntity GetShopPlatformInfo(int platformId, string pShopId)
        {
            return spServ.GetEntity(platformId, pShopId);
        }

        /// <summary>
        /// 获取门店百度平台授权信息
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public ShopPlatformEntity GetShopBaiduPlatformInfo(string sourceId)
        {
            return spServ.GetEntityBySourceId(sourceId);
        }

        /// <summary>
        /// 获取门店平台详细
        /// </summary>
        /// <param name="authId"></param>
        /// <returns></returns>
        public ShopPlatformEntity GetShopPlatformInfo(long authId)
        {
            return spServ.GetEntity(authId);
        }

        /// <summary>
        /// 获取门店授权平台Url
        /// </summary>
        /// <returns></returns>
        public string GetShopPlatformMapUrl(long shopId, int platformId, int isKaMerchant, string kaMerchantAuthUrl)
        {
            var requestApi = "#";
            switch (platformId)
            {
                case (int)PlatformType.Meituan:
                    {
                        requestApi = string.Format(MeituanConsts.SHOP_PLATFORM_MAP_API,
                         ConfigUtil.MeiDeveloperId,
                         AuthBussinessType.Waimai.GetHashCode(),
                         shopId,
                         ConfigUtil.MeiSignKey,
                         WebUtil.UrlDecode(ConfigUtil.MeiMapCallBack, Encoding.UTF8)
                         );
                        break;
                    }
                case (int)PlatformType.Eleme:
                    {
                        requestApi = string.Format(ElemeConsts.SHOP_AUTH_API,
                         ConfigUtil.EleAppKey,
                         WebUtil.UrlDecode(ConfigUtil.EleAuthCallBack, Encoding.UTF8),
                         DESEncryptUtil.Encrypt(shopId.ToString())
                         );
                        break;
                    }
                case (int)PlatformType.Baidu:
                    {
                        if (isKaMerchant == 1 && !string.IsNullOrWhiteSpace(kaMerchantAuthUrl))
                        {
                            requestApi = kaMerchantAuthUrl;
                        }
                        else
                        {
                            requestApi = string.Format(BaiduwmConsts.SHOP_AUTH_URL + "&otherShopId={1}",
                                ConfigUtil.BaiduAuthSourceKey,
                                shopId.ToString());
                        }
                        break;
                    }
                default:
                    break;
            }
            return requestApi;
        }

        /// <summary>
        /// 获取门店平台解绑Url
        /// </summary>
        /// <returns></returns>
        public string GetShopPlatformUnMapUrl(long shopId, long authId)
        {
            var requestApi = "#";
            var shopPlatform = GetShopPlatformInfo(authId);
            if (shopPlatform != null)
            {
                if (shopPlatform.ShopId != shopId)
                {
                    throw new BusinessException("对不起，您无权执行此操作！");
                }

                LogUtil.Info(string.Format("门店Id：{0},平台Id：{1}准备解绑...", shopPlatform.ShopId, shopPlatform.PlatformId));
                switch (shopPlatform.PlatformId)
                {
                    case (int)PlatformType.Meituan:
                        {
                            requestApi = string.Format(MeituanConsts.SHOP_PLATFORM_UNMAP_API,
                                ConfigUtil.MeiSignKey,
                                AuthBussinessType.Waimai.GetHashCode(),
                                shopPlatform.AuthToken,
                                WebUtil.UrlDecode(ConfigUtil.MeiUnMapCallBack, Encoding.UTF8)
                                );
                            break;
                        }
                    case (int)PlatformType.Eleme:
                        {
                            //删除授权记录
                            requestApi = string.Format("/platform/UnMapShopAuth?authId={0}",authId );
                            break;
                        }
                    case (int)PlatformType.Baidu:
                        {
                            requestApi = BaiduwmConsts.SHOP_UNAUTH_URL;
                            break;
                        }
                    default:
                        break;
                }
            }
            return requestApi;
        }

        /// <summary>
        /// 获取授权平台门店信息
        /// </summary>
        /// <returns></returns>
        public dynamic GetPlatformShopModel(int platformId, string appAuthToken, string ePoiIds, string pShopId = "", string source = "", string secret = "")
        {
            object shopModel = null;
            switch (platformId)
            {
                case (int)PlatformType.Meituan:
                    {
                        //美团外卖
                        shopModel = meiServ.GetMeituanShop(appAuthToken, ePoiIds);
                        break;
                    }
                case (int)PlatformType.Eleme:
                    {
                        //饿了么
                        var eShopId = pShopId.ToInt();
                        shopModel = eleServ.GetShopInfo(eShopId, appAuthToken);
                        break;
                    }
                case (int)PlatformType.Baidu:
                    {
                        //百度外卖
                        shopModel = GetBaiShopInfo(pShopId, source, secret);
                        break;
                    }
                default:
                    break;
            }
            return shopModel;
        }

        /// <summary>
        /// 同步门店平台数据
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="appAuthToken"></param>
        /// <param name="expiresIn"></param>
        /// <param name="refreshToken"></param>
        /// <param name="paltformType"></param>
        /// <param name="authType"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public int SyncShopPlatformData(string shopId, string appAuthToken, long expiresIn, string refreshToken, PlatformType paltformType, AuthBussinessType authType, BusinessType businessType, string pShopId = "", string source = "", string secret = "",string remark = "")
        {
            //同步平台数据
            LogUtil.Info(string.Format("开始同步门店授权【{0}】平台数据", paltformType.GetRemark()));
            var cnt = 0;
            var platformId = paltformType.GetHashCode();
            var shop = GetPlatformShopModel(platformId, appAuthToken, shopId, pShopId, source, secret);
            var db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                var dbNow = TimeUtil.Now;
                var authPlatform = GetShopPlatformInfo(shopId.ToInt(), platformId, businessType.GetHashCode(), authType);
                if (authPlatform != null)
                {
                    authPlatform.AuthToken = appAuthToken;
                    authPlatform.ExpiresIn = expiresIn;
                    authPlatform.IsAutoConfirm = 1;

                    if (paltformType == PlatformType.Meituan)
                    {
                        //美团外卖
                        authPlatform.PlatformShopName = shop != null ? shop.name : string.Empty;
                        authPlatform.Mobile = shop != null ? shop.phone : string.Empty;
                        authPlatform.ShopAddress = shop != null ? shop.address : string.Empty;
                        authPlatform.BusinessScope = shop != null ? shop.tagName : string.Empty;
                        authPlatform.Latitude = shop != null ? shop.latitude : string.Empty;
                        authPlatform.Longitude = shop != null ? shop.longitude : string.Empty;
                        authPlatform.PictureUrl = shop != null ? shop.pictureUrl : string.Empty;
                        authPlatform.OpenTime = shop != null ? shop.openTime : string.Empty;
                        authPlatform.ShippingFee = shop != null ? shop.shippingFee : 0.00f;
                        authPlatform.PreBook = shop != null ? shop.preBook : 0;
                        authPlatform.TimeSelect = shop != null ? shop.timeSelect : 0;
                    }
                    else if (paltformType == PlatformType.Eleme)
                    {
                        //饿了么
                        if (shop != null && shop.error != null)
                        {
                            LogUtil.Error(shop.error.ToString());
                        }
                        var eShop = Yme.Util.JsonUtil.ToObject<Dictionary<string, object>>((shop != null && shop.result != null) ? shop.result.ToString() : string.Empty);
                        authPlatform.PlatformShopName = (eShop != null && eShop["name"] != null) ? eShop["name"].ToString() : string.Empty;
                        authPlatform.PlatformShopId = (eShop != null && eShop["id"] != null) ? eShop["id"].ToString() : string.Empty;
                        authPlatform.Mobile = (eShop != null && eShop["mobile"] != null) ? eShop["mobile"].ToString() : string.Empty;
                        authPlatform.ShopAddress = (eShop != null && eShop["addressText"] != null) ? eShop["addressText"].ToString() : string.Empty;
                        authPlatform.Latitude = (eShop != null && eShop["latitude"] != null) ? eShop["latitude"].ToString() : string.Empty;
                        authPlatform.Longitude = (eShop != null && eShop["longitude"] != null) ? eShop["longitude"].ToString() : string.Empty;
                        authPlatform.PictureUrl = (eShop != null && eShop["imageUrl"] != null) ? eShop["imageUrl"].ToString() : string.Empty;
                        authPlatform.OpenTime = (eShop != null && eShop["servingTime"] != null) ? eShop["servingTime"].ToString() : string.Empty;
                        authPlatform.RefreshToken = refreshToken;
                        authPlatform.RefreshExpireTime = TimeUtil.Now.AddDays(ConfigUtil.EleReTokenExpiresDays);
                    }
                    else if (paltformType == PlatformType.Baidu)
                    {
                        //百度外卖
                        if (shop != null && shop.error != null)
                        {
                            LogUtil.Error(shop.error.ToString());
                        }
                        var category = string.Empty;
                        var bShop = Yme.Util.JsonUtil.ToObject<Dictionary<string, object>>((shop != null && shop.data != null) ? shop.data.ToString() : string.Empty);
                        if (bShop != null)
                        {
                            if (!string.IsNullOrWhiteSpace(bShop["category1"]))
                            {
                                category = bShop["category1"].ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(bShop["category2"]))
                            {
                                category += string.Format("-{0}", bShop["category2"].ToString());
                            }
                            if (!string.IsNullOrWhiteSpace(bShop["category3"]))
                            {
                                category += string.Format("-{0}", bShop["category3"].ToString());
                            }
                        }
                        authPlatform.PlatformShopName = (bShop != null && bShop["name"] != null) ? bShop["name"].ToString() : string.Empty;
                        authPlatform.PlatformShopId = (bShop != null && bShop["baidu_shop_id"] != null) ? bShop["baidu_shop_id"].ToString() : string.Empty;
                        authPlatform.Mobile = (bShop != null && bShop["service_phone"] != null) ? bShop["service_phone"].ToString() : string.Empty;
                        authPlatform.ShopAddress = (bShop != null && bShop["address"] != null) ? bShop["address"].ToString() : string.Empty;
                        authPlatform.BusinessScope = category;
                        authPlatform.Latitude = (bShop != null && bShop["latitude"] != null) ? bShop["latitude"].ToString() : string.Empty;
                        authPlatform.Longitude = (bShop != null && bShop["longitude"] != null) ? bShop["longitude"].ToString() : string.Empty;
                        authPlatform.PictureUrl = (bShop != null && bShop["shop_logo"] != null) ? bShop["shop_logo"].ToString() : string.Empty;
                        authPlatform.OpenTime = (bShop != null && bShop["business_time"] != null) ? bShop["business_time"].ToString() : string.Empty;
                        authPlatform.RefreshToken = refreshToken;
                        authPlatform.Description = remark;
                    }

                    authPlatform.ModifyUserId = paltformType.GetRemark();
                    authPlatform.ModifyDate = dbNow;
                    db.Update(authPlatform);
                }
                else
                {
                    authPlatform = new ShopPlatformEntity();
                    authPlatform.ShopId = shopId.ToInt();
                    authPlatform.PlatformId = platformId;
                    authPlatform.PlatformName = paltformType.GetRemark();
                    authPlatform.BusinessType = businessType.GetHashCode();
                    authPlatform.AuthBussiness = authType.GetHashCode();
                    authPlatform.AuthToken = appAuthToken;
                    authPlatform.ExpiresIn = expiresIn;
                    authPlatform.IsAutoConfirm = 1;

                    if (paltformType == PlatformType.Meituan)
                    {
                        //美团外卖
                        authPlatform.PlatformShopName = shop != null ? shop.name : string.Empty;
                        authPlatform.Mobile = shop != null ? shop.phone : string.Empty;
                        authPlatform.ShopAddress = shop != null ? shop.address : string.Empty;
                        authPlatform.BusinessScope = shop != null ? shop.tagName : string.Empty;
                        authPlatform.Latitude = shop != null ? shop.latitude : string.Empty;
                        authPlatform.Longitude = shop != null ? shop.longitude : string.Empty;
                        authPlatform.PictureUrl = shop != null ? shop.pictureUrl : string.Empty;
                        authPlatform.OpenTime = shop != null ? shop.openTime : string.Empty;
                        authPlatform.ShippingFee = shop != null ? shop.shippingFee : 0.00f;
                        authPlatform.PreBook = shop != null ? shop.preBook : 0;
                        authPlatform.TimeSelect = shop != null ? shop.timeSelect : 0;
                    }
                    else if (paltformType == PlatformType.Eleme)
                    {
                        //饿了么
                        if (shop != null && shop.error != null)
                        {
                            LogUtil.Error(shop.error.ToString());
                        }
                        var eShop = Yme.Util.JsonUtil.ToObject<Dictionary<string, object>>((shop != null && shop.result != null) ? shop.result.ToString() : string.Empty);
                        authPlatform.PlatformShopName = (eShop != null && eShop["name"] != null) ? eShop["name"].ToString() : string.Empty;
                        authPlatform.PlatformShopId = (eShop != null && eShop["id"] != null) ? eShop["id"].ToString() : appAuthToken;
                        authPlatform.Mobile = (eShop != null && eShop["mobile"] != null) ? eShop["mobile"].ToString() : string.Empty;
                        authPlatform.ShopAddress = (eShop != null && eShop["addressText"] != null) ? eShop["addressText"].ToString() : string.Empty;
                        authPlatform.Latitude = (eShop != null && eShop["latitude"] != null) ? eShop["latitude"].ToString() : string.Empty;
                        authPlatform.Longitude = (eShop != null && eShop["longitude"] != null) ? eShop["longitude"].ToString() : string.Empty;
                        authPlatform.PictureUrl = (eShop != null && eShop["imageUrl"] != null) ? eShop["imageUrl"].ToString() : string.Empty;
                        authPlatform.OpenTime = (eShop != null && eShop["servingTime"] != null) ? eShop["servingTime"].ToString() : string.Empty;
                        authPlatform.RefreshToken = refreshToken;
                        authPlatform.RefreshExpireTime = TimeUtil.Now.AddDays(ConfigUtil.EleReTokenExpiresDays);
                        authPlatform.ModifyDate = dbNow;
                    }
                    else if (paltformType == PlatformType.Baidu)
                    {
                        //百度外卖
                        if (shop != null && shop.errno != 0)
                        {
                            LogUtil.Error(shop.error.ToString());
                        }
                        var category = string.Empty;
                        var bShop = Yme.Util.JsonUtil.ToObject<Dictionary<string, object>>((shop != null && shop.data != null) ? shop.data.ToString() : string.Empty);
                        if (bShop != null)
                        {
                            if (!string.IsNullOrWhiteSpace(bShop["category1"]))
                            {
                                category = bShop["category1"].ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(bShop["category2"]))
                            {
                                category += string.Format("-{0}", bShop["category2"].ToString());
                            }
                            if (!string.IsNullOrWhiteSpace(bShop["category3"]))
                            {
                                category += string.Format("-{0}", bShop["category3"].ToString());
                            }
                        }
                        authPlatform.PlatformShopName = (bShop != null && bShop["name"] != null) ? bShop["name"].ToString() : string.Empty;
                        authPlatform.PlatformShopId = (bShop != null && bShop["baidu_shop_id"] != null) ? bShop["baidu_shop_id"].ToString() : string.Empty;
                        authPlatform.Mobile = (bShop != null && bShop["service_phone"] != null) ? bShop["service_phone"].ToString() : string.Empty;
                        authPlatform.ShopAddress = (bShop != null && bShop["address"] != null) ? bShop["address"].ToString() : string.Empty;
                        authPlatform.BusinessScope = category;
                        authPlatform.Latitude = (bShop != null && bShop["latitude"] != null) ? bShop["latitude"].ToString() : string.Empty;
                        authPlatform.Longitude = (bShop != null && bShop["longitude"] != null) ? bShop["longitude"].ToString() : string.Empty;
                        authPlatform.PictureUrl = (bShop != null && bShop["shop_logo"] != null) ? bShop["shop_logo"].ToString() : string.Empty;
                        authPlatform.OpenTime = (bShop != null && bShop["business_time"] != null) ? bShop["business_time"].ToString() : string.Empty;
                        authPlatform.RefreshToken = refreshToken;
                        authPlatform.Description = remark;
                    }

                    authPlatform.EnabledFlag = EnabledFlagType.Valid.GetHashCode();
                    authPlatform.DeleteFlag = DeleteFlagType.Valid.GetHashCode();
                    authPlatform.CreateUserId = paltformType.GetRemark();
                    authPlatform.CreateDate = dbNow;
                    db.Insert(authPlatform);
                }

                //同步门店数据
                var platformList = GetShopPlatformList(shopId.ToInt(), businessType);
                if (platformList != null && platformList.Count > 0)
                {
                    var isFirstAuth = platformList.Count(d => d.AuthStasus == 1) == 0;
                    if (isFirstAuth)
                    {
                        var shopEntity = SingleInstance<ShopBLL>.Instance.GetShopById(shopId.ToInt());
                        if (shopEntity != null)
                        {
                            shopEntity.ShopName = authPlatform.PlatformShopName;
                            shopEntity.ShopAddress = authPlatform.ShopAddress;
                            shopEntity.BusinessScope = authPlatform.BusinessScope;
                            shopEntity.ModifyUserId = paltformType.GetRemark();
                            shopEntity.ModifyDate = dbNow;
                            db.Update(shopEntity);
                        }
                    }
                }

                db.Commit();
                cnt = 1;
            }
            catch (Exception ex)
            {
                db.Rollback();
                var msg = string.Format("同步门店平台数据失败，参考消息:{0}", ex.Message);
                LogUtil.Error(msg);
                throw new BusinessException(msg);
            }
            return cnt;
        }

        /// <summary>
        /// 解除门店平台映射
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="authId"></param>
        /// <returns></returns>
        public bool UnMapShopAuth(long shopId, long authId)
        {
            try
            {
                var shopPlatform = GetShopPlatformInfo(authId);
                if (shopPlatform != null)
                {
                    if (shopPlatform.ShopId != shopId)
                    {
                        throw new BusinessException("对不起，您无权执行此操作！");
                    }
                }
                return spServ.Delete(authId) > 0;
            }
            catch (Exception ex)
            {
                var msg = string.Format("解除门店平台数据失败，参考消息:{0}", ex.Message);
                LogUtil.Error(msg);
                throw new BusinessException(msg);
            }
        }

        /// <summary>
        /// 门店百度平台映射申请
        /// </summary>
        /// <param name="shopId">微云打门店ID</param>
        /// <param name="pShopId">百度平台门店ID</param>
        /// <returns></returns>
        public bool MapBaiduShopAuth(long shopId, string pShopId)
        {
            try
            {
                var platformId = PlatformType.Baidu.GetHashCode();
                var businessType = BusinessType.Waimai;
                var pList = FindShopPlatformList(businessType);
                var isExists = pList.Exists(t => t.PlatformId == platformId && t.PlatformShopId == pShopId && t.ShopId != shopId);
                if (!isExists)
                {
                    var dbNow = TimeUtil.Now;
                    var platformName = PlatformType.Baidu.GetRemark();
                    var authType = AuthBussinessType.Waimai;
                    var authPlatform = GetShopPlatformInfo(shopId, platformId, businessType.GetHashCode(), authType);
                    if (authPlatform != null)
                    {
                        //更新映射
                        authPlatform.PlatformShopId = pShopId;
                        authPlatform.PlatformId = platformId;
                        authPlatform.PlatformName = platformName;
                        authPlatform.BusinessType = businessType.GetHashCode();
                        authPlatform.AuthBussiness = authType.GetHashCode();
                        authPlatform.ModifyUserId = shopId.ToString();
                        authPlatform.ModifyDate = dbNow;
                        return spServ.UpdateEntity(authPlatform) > 0;
                    }
                    else
                    {
                        //新增映射
                        authPlatform = new ShopPlatformEntity();
                        authPlatform.ShopId = shopId;
                        authPlatform.PlatformShopId = pShopId;
                        authPlatform.PlatformId = platformId;
                        authPlatform.PlatformName = platformName;
                        authPlatform.BusinessType = businessType.GetHashCode();
                        authPlatform.AuthBussiness = authType.GetHashCode();
                        authPlatform.AuthToken = string.Empty;
                        authPlatform.ExpiresIn = 0;
                        authPlatform.PlatformShopName = string.Empty;
                        authPlatform.Mobile = string.Empty;
                        authPlatform.EnabledFlag = EnabledFlagType.Valid.GetHashCode();
                        authPlatform.DeleteFlag = DeleteFlagType.Valid.GetHashCode();
                        authPlatform.CreateUserId = shopId.ToString();
                        authPlatform.CreateDate = dbNow;
                        return spServ.InsertEntity(authPlatform) > 0;
                    }
                }
                else
                {
                    var msg = string.Format("百度门店ID：【{0}】已授权,请核对！", pShopId);
                    LogUtil.Error(msg);
                    throw new BusinessException(msg);
                }
            }
            catch (Exception ex)
            {
                var msg = string.Format("授权申请失败,{0}", ex.Message);
                LogUtil.Error(msg);
                throw new BusinessException(msg);
            }
        }

        /// <summary>
        /// 解除门店平台映射
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="pType"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        public int SyncUnMapShopPlatformData(long shopId, PlatformType pType, AuthBussinessType authType)
        {
            LogUtil.Info(string.Format("开始同步解除门店授权平台数据", string.Empty));
            return spServ.Delete(shopId, pType.GetHashCode(), authType.GetHashCode());
        }

        /// <summary>
        ///  校验门店是否关联第三方平台
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public bool CheckShopIsAuhtPlatform(long shopId)
        {
            return spServ.CheckShopIsAuhtPlatform(shopId);
        }

        #region 饿了么

        /// <summary>
        /// 通过授权码获取访问令牌【饿了么】
        /// </summary>
        /// <param name="code">授权码</param>
        /// <param name="eShopIds">饿了么授权门店ID列表</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public ElemeRetTokenModel GetEleAccessTokenByCode(string code, out List<string> eShopIds, out string errMsg)
        {
            LogUtil.Info(string.Format("通过授权码Code：{0},获取AccessToken", code));

            errMsg = string.Empty;
            eShopIds = new List<string>();
            var token = eleServ.GetAccessTokenByCode(code);
            var accessToken = token != null ? token.access_token.ToString() : string.Empty;
            var expiresIn =  token != null ? Extensions.ToInt(token.expires_in, 0) :0;
            eShopIds = GetEleMerchatAccountInfo(accessToken, out errMsg);
            return token;
        }

        /// <summary>
        /// 获取饿了么门店令牌
        /// </summary>
        /// <param name="shopId">门店Id</param>
        /// <returns></returns>
        public string GetEleShopAccessToken(long shopId)
        {
            var shopAuth = GetShopPlatformInfo(shopId, PlatformType.Eleme.GetHashCode(), BusinessType.Waimai.GetHashCode(), AuthBussinessType.Waimai);
            return GetEleShopAccessToken(shopAuth);
        }

        /// <summary>
        /// 获取饿了么门店令牌
        /// </summary>
        /// <param name="shopAuth">门店饿了么平台授权信息</param>
        /// <returns></returns>
        public string GetEleShopAccessToken(ShopPlatformEntity shopAuth)
        {
            var token = string.Empty;
            if (shopAuth != null && !string.IsNullOrWhiteSpace(shopAuth.AuthToken))
            {
                token = shopAuth.AuthToken;
                LogUtil.Info(string.Format("token有效期：{0},当前时间：{1}", shopAuth.ModifyDate.Value.AddSeconds(shopAuth.ExpiresIn), TimeUtil.Now));
                if (shopAuth.ModifyDate.Value.AddSeconds(shopAuth.ExpiresIn) < TimeUtil.Now)
                {
                    var rToken = eleServ.GetAccessTokenByRefreshToken(shopAuth.RefreshToken);
                    token = rToken != null ? rToken.access_token : string.Empty;
                    var refreshToken = rToken != null ? rToken.refresh_token : string.Empty;
                    var platformName = PlatformType.Eleme.GetRemark();
                    LogUtil.Info(string.Format("{0}门店：{1}, accessToken：{2}已过期,通过refreshToken获取新accessToken：{3},RefreshToken：{4}", platformName, shopAuth.ShopId, shopAuth.AuthToken, token, refreshToken));

                    //更新token到DB
                    shopAuth.AuthToken = token;
                    shopAuth.ExpiresIn = rToken.expires_in;
                    shopAuth.RefreshToken = refreshToken;
                    shopAuth.RefreshExpireTime = TimeUtil.Now.AddDays(ConfigUtil.EleReTokenExpiresDays);
                    shopAuth.ModifyDate = TimeUtil.Now;
                    SingleInstance<PlatformBLL>.Instance.UpdateEntity(shopAuth);
                }
            }
            return token;
        }

        /// <summary>
        /// 获取饿了么商户账号信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<string> GetEleMerchatAccountInfo(string token, out string errMsg)
        {
            errMsg = string.Empty;
            var eShopIds = new List<string>();
            if (!string.IsNullOrWhiteSpace(token))
            {
                var merchant = GetEleMerchatAccountInfo(token);
                if (merchant != null)
                {
                    if (merchant.error != null)
                    {
                        var error = Yme.Util.JsonUtil.ToObject<ElemeApiRetErrorModel>(merchant.error.ToString());
                        errMsg = error != null ? error.message.ToString() : string.Empty;
                        LogUtil.Error(string.Format("授权时获取商户账号信息失败,返回错误信息：{0}", errMsg));
                    }
                    else
                    {
                        if (merchant.result != null)
                        {
                            var ms = Yme.Util.JsonUtil.ToObject<Dictionary<string, object>>(merchant.result.ToString());
                            var slist = ms != null ? ms["authorizedShops"].ToString() : null;
                            if (!string.IsNullOrWhiteSpace(slist))
                            {
                                var shops = Yme.Util.JsonUtil.ToObject<List<Dictionary<string, object>>>(slist);
                                foreach (var shop in shops)
                                {
                                    eShopIds.Add(shops[0]["id"].ToString());
                                }
                            }
                            else
                            {
                                errMsg = "授权商户门店列表为空";
                                LogUtil.Error(errMsg);
                            }
                        }
                        else
                        {
                            errMsg = "授权商户信息为空";
                            LogUtil.Error(errMsg);
                        }
                    }
                }
                else
                {
                    errMsg = "授权时获取商户账号信息失败";
                    LogUtil.Error(errMsg);
                }
            }
            else
            {
                errMsg = "授权时获取访问令牌失败";
                LogUtil.Error(errMsg);
            }
            return eShopIds;
        }

        /// <summary>
        /// 获取商户账号信息【饿了么】
        /// </summary>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel GetEleMerchatAccountInfo(string token)
        {
            return eleServ.GetMerchatAccountInfo(token);
        }

        /// <summary>
        /// 获取门店信息【饿了么】
        /// </summary>
        /// <param name="eShopId">饿了么门店Id</param>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel GetEleShopInfo(long eShopId, string token)
        {
            return eleServ.GetShopInfo(eShopId, token); 
        }

        /// <summary>
        /// 获取订单信息【饿了么】
        /// </summary>
        /// <param name="eOrderId">饿了么订单Id</param>
        /// <param name="token">访问令牌</param>
        /// <returns>请求结果</returns>
        public ElemeApiRetModel GetEleOrderInfo(string eOrderId, string token)
        {
            return eleServ.GetOrderInfo(eOrderId, token); 
        }

        /// <summary>
        /// 定时更新饿了么访问令牌
        /// </summary>
        public void UpdateElemeToken()
        {
             //待更新平台列表
            var platforms = spServ.FindTokenWaitUpdateList();

            //更新后的平台令牌列表
            var ptokens = GetUpdatePlatformAccessTokens(platforms);

            //更新平台令牌列表
            spServ.UpdateEntities(ptokens);
        }

        /// <summary>
        /// 处理平台待更新平台令牌列表
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public List<ShopPlatformEntity> GetUpdatePlatformAccessTokens(List<ShopPlatformEntity> platforms)
        {
            var platfromTokens = new List<ShopPlatformEntity>();
            if (platforms == null && platforms.Count <= 0)
            {
                return platfromTokens;
            }

            //查询更新后令牌列表
            var cnt = 0;
            foreach (var p in platforms)
            {
                var token = eleServ.GetAccessTokenByRefreshToken(p.RefreshToken);
                if (token != null)
                {
                    p.AuthToken = token.access_token ?? string.Empty;
                    p.ExpiresIn = token.expires_in;
                    p.RefreshToken = token.refresh_token ?? string.Empty;
                    p.RefreshExpireTime = TimeUtil.Now.AddDays(ConfigUtil.EleReTokenExpiresDays);
                    p.ModifyDate = TimeUtil.Now;
                    platfromTokens.Add(p);

                    //更新到数据库中
                    UpdateEntity(p);
                    cnt++;

                    LogUtil.Info(string.Format("第{0}个饿了么门店Id：{1},AccessToken:{2},RefreshToken:{3},定时任务更新DB,token有效期:{4},refreshToken有效期:{5}", cnt, p.PlatformShopId, p.AuthToken, p.RefreshToken, p.ModifyDate.Value.AddSeconds(p.ExpiresIn), p.RefreshExpireTime));
                }
                else
                {
                    LogUtil.Warn(string.Format("第{0}饿了么门店Id：{1}获取新令牌失败", cnt, p.PlatformShopId));
                }
            }
            return platfromTokens;
        }

        #endregion

        #region 百度外卖

        /// <summary>
        /// 获取门店信息【百度外卖】
        /// </summary>
        /// <param name="bShopId">百度外卖门店Id</param>
        /// <param name="source">百度外卖门店帐号</param>
        /// <param name="encrypt">加密方式</param>
        /// <returns>请求结果</returns>
        public BaiduApiRetModel GetBaiShopInfo(string bShopId, string source = "", string secret = "", string encrypt = "")
        {
            if(string.IsNullOrWhiteSpace(source))
            {
                source = ConfigUtil.BaiduSourceId;
            }
            if (string.IsNullOrWhiteSpace(secret))
            {
                var spInfo = GetShopPlatformInfo(PlatformType.Baidu.GetHashCode(), source);
                secret = spInfo != null ? spInfo.AuthToken : string.Empty;
                if (string.IsNullOrWhiteSpace(secret))
                {
                    source = ConfigUtil.BaiduSourceSecret;
                }
            }
            return baiServ.GetShopInfo(bShopId, source, secret, encrypt);
        }

        /// <summary>
        /// 获取订单信息【百度外卖】
        /// </summary>
        /// <param name="bOrderId">百度外卖订单Id</param>
        /// <param name="source">百度外卖门店帐号</param>
        /// <param name="encrypt">加密方式</param>
        /// <returns>请求结果</returns>
        public BaiduApiRetModel GetBaiOrderInfo(string bOrderId, string source = "", string secret = "", string encrypt = "")
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                source = ConfigUtil.BaiduSourceId;
            }
            if (string.IsNullOrWhiteSpace(secret))
            {
                var spInfo = GetShopPlatformInfo(PlatformType.Baidu.GetHashCode(), source);
                secret = spInfo != null ? spInfo.AuthToken : string.Empty;
                if (string.IsNullOrWhiteSpace(secret))
                {
                    source = ConfigUtil.BaiduSourceSecret;
                }
            }
            return baiServ.GetOrderInfo(bOrderId, source, secret, encrypt);
        }

        /// <summary>
        /// API接口返回【百度外卖】
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="body"></param>
        /// <param name="error"></param>
        /// <param name="source"></param>
        /// <param name="secret"></param>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public object GetBaiApiRetResponse(string cmd, Dictionary<string, object> body, string error = "success", string source = "", string secret = "", string encrypt = "")
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                source = ConfigUtil.BaiduSourceId;
            }
            if (string.IsNullOrWhiteSpace(secret))
            {
                secret = ConfigUtil.BaiduSourceSecret;
            }
            return baiServ.RetApiResponse(cmd, body, error, source, secret, encrypt);
        }

        #endregion

        #endregion

        #region 平台心跳业务

        /// <summary>
        /// 发送平台普通心跳
        /// </summary>
        public void SendCommonHeartbeat()
        {
            //发送美团平台心跳
            meiServ.SendCommonHeartbeat();
        }

        /// <summary>
        /// 发送平台补充数据
        /// </summary>
        public void SendUploadData()
        {
            //发送美团平台补充数据
            meiServ.SendUploadData();
        }

        #endregion
    }
}
