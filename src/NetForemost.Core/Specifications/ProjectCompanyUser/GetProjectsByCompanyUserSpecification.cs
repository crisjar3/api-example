using Ardalis.Specification;
using NetForemost.Core.Dtos.Projects;

namespace NetForemost.Core.Specifications.ProjectCompanyUser;

public class GetProjectsByCompanyUserSpecification : Specification<Entities.Projects.ProjectCompanyUser>
{
    public GetProjectsByCompanyUserSpecification(int projectId)
    {
        Query.Include(projectCompanyUser => projectCompanyUser.CompanyUser);
        Query.Where(projectCompanyUser => projectCompanyUser.ProjectId == projectId);
    }
}