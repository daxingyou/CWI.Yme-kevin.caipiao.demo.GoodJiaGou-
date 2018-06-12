
using Yme.Mcp.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.SystemManage
{
    public class BlackListMap : EntityTypeConfiguration<BlackListEntity>
    {
        public BlackListMap()
        {
            this.ToTable("sys_blacklist");

            this.HasKey(t => t.BlackId); //主键
        }
    }
}
