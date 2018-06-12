using Yme.Mcp.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.BaseManage
{
    public class ShopPrinterMap : EntityTypeConfiguration<ShopPrinterEntity>
    {
        public ShopPrinterMap()
        {
            this.ToTable("bll_shop_printer");

            this.HasKey(t => t.PrinterCode); //主键
        }
    }
}
