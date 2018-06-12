using Yme.Mcp.Entity.OrderManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.OrderManage
{
    public class OrderDetailMap : EntityTypeConfiguration<OrderDetailEntity>
    {
        public OrderDetailMap()
        {
            this.ToTable("bll_order_detail");

            this.HasKey(t => t.MorderId); //主键
        }
    }
}
