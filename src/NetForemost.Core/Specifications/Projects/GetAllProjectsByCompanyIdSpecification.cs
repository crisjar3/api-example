using Ardalis.Specification;
using NetForemost.Core.Entities.Projects;

namespace NetForemost.Core.Specifications.Projects;

public class GetProjectsByCompanyIdSpecification : Specification<Project>
{
    public GetProjectsByCompanyIdSpecification(int companyId)
    {
        Query.Include(project => project.ProjectCompanyUsers.Where(projectCompanyUser => projectCompanyUser.IsActive))
                .ThenInclude(projectCompanyUser => projectCompanyUser.CompanyUser)
                .ThenInclude(companyUser => companyUser.User);

        Query.Where(project => project.CompanyId == companyId);
    }
}
