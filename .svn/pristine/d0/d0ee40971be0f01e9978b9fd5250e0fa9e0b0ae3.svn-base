using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Data.Repository;
using Yme.Mcp.Entity.SystemManage;
using Yme.Mcp.IService.SystemManage;
using Yme.Util.WebControl;
using Yme.Util.Extension;
using Yme.Util.Attributes;
using Yme.Util;
using Yme.Code;

namespace Yme.Mcp.Service.SystemManage
{
    public class LoginLogService : RepositoryFactory<LoginLogEntity>, ILoginLogService
    {
        #region 获取数据
        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<LoginLogEntity> GetPageList(Pagination pagination, string queryJson)
        {
            //var expression = Extensions.True<LoginLogEntity>();
            //var queryParam = queryJson.ToJObject();
            
            return this.BaseRepository().FindList( pagination);
        }
        /// <summary>
        /// 日志实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LoginLogEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 清空日志
        /// </summary>
        /// <param name="categoryId">日志分类Id</param>
        /// <param name="keepTime">保留时间段内</param>
        public void RemoveLog(int categoryId, string keepTime)
        {
            DateTime operateTime = DateTime.Now;
            if (keepTime == "7")//保留近一周
            {
                operateTime = DateTime.Now.AddDays(-7);
            }
            else if (keepTime == "1")//保留近一个月
            {
                operateTime = DateTime.Now.AddMonths(-1);
            }
            else if (keepTime == "3")//保留近三个月
            {
                operateTime = DateTime.Now.AddMonths(-3);
            }
            var expression = Extensions.True<LogEntity>();
            expression = expression.And(t => t.OperateTime <= operateTime);
            //expression = expression.And(t => t.CategoryId == categoryId);
            this.BaseRepository().Delete(expression);
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logEntity">对象</param>
        public void WriteLog(LoginLogEntity entity)
        {
            entity.LogId = Guid.NewGuid().ToString();
            entity.HostName = NetUtil.GetHostName();
            entity.HostIP = NetUtil.Ip;
            entity.LoginCity = NetUtil.GetLocation();
            entity.OperateDate = DateTime.Now;
            //entity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
            entity.OperateType = "1";
            entity.OperateUserId = OperatorProvider.Provider.Current().UserId;
            entity.OperateUserName = OperatorProvider.Provider.Current().UserName;
            this.BaseRepository().Insert(entity);
        }
        #endregion
    }
}
