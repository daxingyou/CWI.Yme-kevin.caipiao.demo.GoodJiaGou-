
using Yme.Mcp.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.SystemManage
{
    public class AreaMap : EntityTypeConfiguration<AreaEntity>
    {
        public AreaMap()
        {
            this.ToTable("sys_area");

            this.HasKey(t => t.AreaCode); //主键
        }
    }
}
