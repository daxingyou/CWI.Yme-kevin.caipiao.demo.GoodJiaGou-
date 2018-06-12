using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Data;
using Yme.Data.Repository;
using Yme.Mcp.IService.SystemManage;
using Yme.Util.Extension;

namespace Yme.Mcp.Service.SystemManage
{
    public class SystemService : RepositoryFactory, ISystemService
    {
        /// <summary>
        /// 获取DB数据时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetDbTime()
        {
            var sql = "SELECT NOW()";
            return Extensions.ToDate(this.BaseRepository().FindObject(sql));
        }
    }
}
