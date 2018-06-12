using Yme.Mcp.Entity.DemoManage;
using System.Data.Entity.ModelConfiguration;

namespace Yme.Mcp.Mapping.DemoManage
{
    public class TaskMap : EntityTypeConfiguration<TaskEntity>
    {
        public TaskMap()
        {
            this.ToTable("task_info");

            this.HasKey(t => t.TaskId); //主键
        }
    }
}
