using Yme.Mcp.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.BaseManage
{
    public class MerchantMap : EntityTypeConfiguration<MerchantEntity>
    {
        public MerchantMap()
        {
            this.ToTable("bll_merchant");

            this.HasKey(t => t.MerchantId); //主键
        }
    }
}
