using Yme.Mcp.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.BaseManage
{
    public class ShopMap : EntityTypeConfiguration<ShopEntity>
    {
        public ShopMap()
        {
            this.ToTable("bll_shop");

            this.HasKey(t => t.ShopId); //主键
        }
    }
}
