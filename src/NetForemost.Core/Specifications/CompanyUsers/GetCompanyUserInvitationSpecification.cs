using Ardalis.Specification;
using NetForemost.Core.Dtos.Companies.CompanyUserInvitations;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.CompanyUsers;

public class GetCompanyUserInvitationSpecification : Specification<CompanyUserInvitation, CompanyUserInvitationCompleteDto>
{
    public GetCompanyUserInvitationSpecification(int companyId)
    {
        Query.Where(cui => cui.CompanyId == companyId);
        Query.Include(cui => cui.Role);
        Query.Include(cui => cui.JobRole);
        Query.Include(cui => cui.ProjectCompanyUserInvitations)
                .ThenInclude(pcui => pcui.Project);

        Query.OrderBy(cui => cui.CreatedAt);

        Query.Select(cui => new CompanyUserInvitationCompleteDto()
        {
            Id = cui.Id,
            InvitationToken = cui.InvitationToken,
            ExpirationDate = cui.ExpirationDate,
            EmailInvited = cui.EmailInvited,
            IsAccepted = cui.IsAccepted,
            IsValid = cui.IsValid,
            Role = new Dtos.Account.RoleDto()
            {
                Id = cui.Role.Id,
                Name = cui.Role.Name
            },
            JobRole = new Dtos.JobRoles.JobRoleDto()
            {
                Id = cui.JobRole.Id,
                Name = cui.JobRole.Name,
                Description = cui.JobRole.Description
            },
            ProjectToColaborate = cui.ProjectCompanyUserInvitations.Select(p => new Dtos.Projects.ProjectSimpleDto()
            {
                Id = p.Project.Id,
                Name = p.Project.Name
            })
        });
    }
}