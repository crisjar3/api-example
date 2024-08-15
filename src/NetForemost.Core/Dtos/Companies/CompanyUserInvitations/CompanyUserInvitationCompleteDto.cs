using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.JobRoles;
using NetForemost.Core.Dtos.Projects;

namespace NetForemost.Core.Dtos.Companies.CompanyUserInvitations;

public class CompanyUserInvitationCompleteDto
{
    public int Id { get; set; }
    public Guid InvitationToken { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string EmailInvited { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsValid { get; set; }
    public RoleDto Role { get; set; }
    public JobRoleDto JobRole { get; set; }
    public IEnumerable<ProjectSimpleDto> ProjectToColaborate { get; set; }
}
