using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Data.Repository;
using Yme.Mcp.Entity.OrderManage;

namespace Yme.Mcp.Service.OrderManage
{
    public class OrderDetailService : RepositoryFactory<OrderDetailEntity>, IOrderDetailService
    {
        public OrderDetailEntity GetEntity(string entityId)
        {
            return this.BaseRepository().FindEntity(entityId);
        }
    }
}
