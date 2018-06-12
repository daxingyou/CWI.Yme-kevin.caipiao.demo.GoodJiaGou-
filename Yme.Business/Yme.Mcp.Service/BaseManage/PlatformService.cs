using System.Collections.Generic;
using Yme.Data.Repository;
using Yme.Mcp.Entity.BaseManage;
using Yme.Mcp.IService.BaseManage;
using Yme.Mcp.Model.Enums;
using System.Linq;
using Yme.Util.Extension;

namespace Yme.Mcp.Service.BaseManage
{
    /// <summary>
    /// 平台服务
    /// </summary>
    public class PlatformService : RepositoryFactory<PlatformEntity>, IPlatformService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public PlatformEntity GetEntity(long keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <returns></returns>
        public List<PlatformEntity> GetList()
        {
            var expression = Extensions.True<PlatformEntity>();
            expression = expression.And(t => t.EnabledFlag == (int)EnabledFlagType.Valid);
            expression = expression.And(t => t.DeleteFlag == (int)DeleteFlagType.Valid);
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// 获取指定业务类型实体列表
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public List<PlatformEntity> GetList(BusinessType businessType)
        {
            var expression = Extensions.True<PlatformEntity>();
            expression = expression.And(t => t.EnabledFlag == (int)EnabledFlagType.Valid);
            expression = expression.And(t => t.DeleteFlag == (int)DeleteFlagType.Valid);
            expression = expression.And(t => t.BusinessType == (int)businessType);
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int InsertEntity(PlatformEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int UpdateEntity(PlatformEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int Remove(long keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }
    }
}
