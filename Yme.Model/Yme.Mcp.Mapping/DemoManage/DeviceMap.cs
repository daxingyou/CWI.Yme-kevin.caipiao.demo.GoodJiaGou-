using Yme.Mcp.Entity.DemoManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.DemoManage
{
    public class DeviceMap : EntityTypeConfiguration<DeviceEntity>
    {
        public DeviceMap()
        {
            this.ToTable("device_info");

            this.HasKey(t => t.DeviceId); //主键
        }
    }
}
