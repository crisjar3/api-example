using Ardalis.Specification;

namespace NetForemost.Core.Specifications.Projects;
public class GetProjectCompanyUserByCompanyUserIdSpecification : Specification<NetForemost.Core.Entities.Projects.ProjectCompanyUser>
{
    public GetProjectCompanyUserByCompanyUserIdSpecification(int projectId, int companyuserId)
    {
        Query.Include(companyUser => companyUser.Project);
        Query.Where(projectCompanyUser => projectCompanyUser.ProjectId == projectId && projectCompanyUser.CompanyUserId == companyuserId);
    }
}

