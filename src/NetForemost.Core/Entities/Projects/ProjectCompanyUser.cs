using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Projects;

public class ProjectCompanyUser : BaseEntity
{
    public int ProjectId { get; set; }
    public int CompanyUserId { get; set; }
    public int JobRoleId { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual Project Project { get; set; }
    public virtual CompanyUser CompanyUser { get; set; }
    public virtual JobRole JobRole { get; set; }
}