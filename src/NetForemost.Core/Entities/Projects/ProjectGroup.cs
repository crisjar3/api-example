using NetForemost.Core.Entities.Groups;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Projects;

public class ProjectGroup : BaseEntity
{
    public int ProjectId { get; set; }
    public int GroupId { get; set; }

    public virtual Project Project { get; set; }
    public virtual Group Group { get; set; }
}