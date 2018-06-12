
using Yme.Mcp.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.SystemManage
{
    public class ParameterMap : EntityTypeConfiguration<ParameterEntity>
    {
        public ParameterMap()
        {
            this.ToTable("sys_parameter");

            this.HasKey(t => t.ParamCode); //主键
        }
    }
}
