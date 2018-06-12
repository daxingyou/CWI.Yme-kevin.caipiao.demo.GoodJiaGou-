
using Yme.Mcp.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.SystemManage
{
    public class LogMap : EntityTypeConfiguration<LogEntity>
    {
        public LogMap()
        {
            this.ToTable("sys_log");

            this.HasKey(t => t.LogId); //主键
        }
    }
}
