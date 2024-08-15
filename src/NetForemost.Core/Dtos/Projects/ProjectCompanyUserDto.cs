using NetForemost.Core.Dtos.JobRoles;

namespace NetForemost.Core.Dtos.Projects;

public class ProjectCompanyUserDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public ProjectDto Project { get; set; }
    public JobRoleDto JobRole { get; set; }
}
