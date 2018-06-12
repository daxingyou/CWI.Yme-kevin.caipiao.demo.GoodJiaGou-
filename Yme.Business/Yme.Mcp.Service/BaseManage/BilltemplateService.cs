using Yme.Data.Repository;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.IService.BaseManage;
using System.Linq;
using Yme.Util.Extension;
using System.Collections.Generic;
using Yme.Mcp.Model.Enums;


namespace Yme.Mcp.Service.BaseManage
{
    /// <summary>
    /// 小票模版服务
    /// </summary>
    public class BilltemplateService : RepositoryFactory<BilltemplateEntity>, IBilltemplateService
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public BilltemplateEntity GetEntity(long keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        public List<BilltemplateEntity> GetList()
        {
            var expression = Extensions.True<BilltemplateEntity>();
            expression = expression.And(t => t.EnabledFlag == (int)EnabledFlagType.Valid);
            expression = expression.And(t => t.DeleteFlag == (int)DeleteFlagType.Valid);
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <returns></returns>
        public List<BilltemplateEntity> GetList(BusinessType businessType)
        {
            var expression = Extensions.True<BilltemplateEntity>();
            expression = expression.And(t => t.BusinessType == (int)businessType);
            expression = expression.And(t => t.EnabledFlag == (int)EnabledFlagType.Valid);
            expression = expression.And(t => t.DeleteFlag == (int)DeleteFlagType.Valid);
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int InsertEntity(BilltemplateEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int UpdateEntity(BilltemplateEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public int Remove(long keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }
    }
}