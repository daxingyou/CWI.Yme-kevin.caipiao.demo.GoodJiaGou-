using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Data.Repository;
using Yme.Mcp.Entity.OrderManage;

namespace Yme.Mcp.Service.OrderManage
{
    public class OrderInvoiceService : RepositoryFactory<OrderInvoiceEntity>, IOrderInvoiceService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public OrderInvoiceEntity GetEntity(string entityId)
        {
            return this.BaseRepository().FindEntity(entityId);
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int InsertEntity(OrderInvoiceEntity entity)
        {
            return this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int UpdateEntity(OrderInvoiceEntity entity)
        {
            return this.BaseRepository().Update(entity);
        }
    }
}
