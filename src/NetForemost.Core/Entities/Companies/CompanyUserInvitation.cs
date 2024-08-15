using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Roles;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Companies;

public class CompanyUserInvitation : BaseEntity
{
    public Guid InvitationToken { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string EmailInvited { get; set; }
    public bool IsAccepted { get; set; } = false;
    public bool IsValid { get; set; } = true;
    public int CompanyId { get; set; }
    public string RoleId { get; set; }
    public int JobRoleId { get; set; }
    public virtual Role Role { get; set; }
    public virtual JobRole JobRole { get; set; }
    public virtual Company Company { get; set; }
    public virtual ICollection<ProjectCompanyUserInvitation> ProjectCompanyUserInvitations { get; set; }
}
