using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Projects;

public class ProjectCompanyUserInvitation : BaseEntity
{
    public int ProjectId { get; set; }
    public int CompanyUserInvitationId { get; set; }
    public virtual Project Project { get; set; }
    public virtual CompanyUserInvitation CompanyUserInvitation { get; set; }
}
