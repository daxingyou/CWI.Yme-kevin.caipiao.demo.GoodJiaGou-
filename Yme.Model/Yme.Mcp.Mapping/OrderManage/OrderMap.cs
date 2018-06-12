using Yme.Mcp.Entity.OrderManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.OrderManage
{
    public class OrderMap : EntityTypeConfiguration<OrderEntity>
    {
        public OrderMap()
        {
            this.ToTable("bll_order");

            this.HasKey(t => t.MorderId); //主键
        }
    }
}
