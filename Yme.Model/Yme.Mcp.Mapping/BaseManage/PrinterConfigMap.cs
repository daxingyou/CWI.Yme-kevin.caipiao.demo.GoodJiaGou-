using Yme.Mcp.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.BaseManage
{
    public class PrinterConfigMap : EntityTypeConfiguration<PrinterConfigEntity>
    {
        public PrinterConfigMap()
        {
            this.ToTable("bll_printer_config");

            this.HasKey(t => t.PrinterConfigId); //主键
        }
    }
}
