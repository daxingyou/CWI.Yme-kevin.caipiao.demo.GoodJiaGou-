using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Cache;
using Yme.Cache.Factory;
using Yme.Mcp.Model.Enums;
using Yme.Mcp.IService.SystemManage;
using Yme.Mcp.Service.SystemManage;
using Yme.Util;

namespace Yme.Mcp.BLL.SystemManage
{
    public class SystemBLL
    {
        #region 私有变量

        private static ICache cache = CacheFactory.Cache();
        private ISystemService service = new SystemService();

        #endregion

        #region 公有方法

        /// <summary>
        /// 获取DB数据时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetDbTime()
        {
            return service.GetDbTime();
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            var val = cache.GetCache<string>(key);
            return val ?? string.Empty;
        }
        
        /// <summary>
        /// 获取统计区间日期
        /// </summary>
        /// <param name="sType"></param>
        /// <returns></returns>
        public Tuple<DateTime, DateTime> GetDateZone(int sType)
        {
            var eDate = TimeUtil.Now.Date.AddDays(-1);
            var bDate = TimeUtil.Now.Date.AddDays(-30);
            switch (sType)
            {
                case (int)StatisticsType.OneWeek:
                    {
                        bDate = TimeUtil.Now.Date.AddDays(-7);
                        break;
                    }
                case (int)StatisticsType.HalfMonth:
                    {
                        bDate = TimeUtil.Now.Date.AddDays(-15);
                        break;
                    }
                case (int)StatisticsType.ThreeMonths:
                    {
                        bDate = TimeUtil.Now.Date.AddDays(-90);
                        break;
                    }
                case (int)StatisticsType.OneMonth:
                default:
                    {
                        bDate = TimeUtil.Now.Date.AddDays(-30);
                        break;
                    }
            }

            return Tuple.Create(bDate, eDate);
        }

        #endregion
    }
}
