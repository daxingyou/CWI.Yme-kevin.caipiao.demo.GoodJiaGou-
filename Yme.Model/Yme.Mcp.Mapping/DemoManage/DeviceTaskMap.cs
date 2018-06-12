using Yme.Mcp.Entity.DemoManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.DemoManage
{
    public class DeviceTaskMap : EntityTypeConfiguration<DeviceTaskEntity>
    {
        public DeviceTaskMap()
        {
            this.ToTable("device_task");

            this.HasKey(t => t.DeviceTaskId); //主键
        }
    }
}
