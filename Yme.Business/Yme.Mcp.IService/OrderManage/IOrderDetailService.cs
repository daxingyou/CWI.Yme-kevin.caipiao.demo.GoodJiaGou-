using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yme.Mcp.Entity.OrderManage;

namespace Yme.Mcp.Service.OrderManage
{
    public interface IOrderDetailService
    {
        OrderDetailEntity GetEntity(string entityId);
    }
}
