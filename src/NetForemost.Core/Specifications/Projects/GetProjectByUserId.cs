using Ardalis.Specification;

namespace NetForemost.Core.Specifications.Projects;
public class GetProjectByUserId : Specification<NetForemost.Core.Entities.Projects.ProjectCompanyUser>
{
    public GetProjectByUserId(string userId, int projectId, int companyId)
    {
        Query.Where(projectCompanyUser => projectCompanyUser.CompanyUser.UserId == userId && projectCompanyUser.CompanyUser.CompanyId == companyId && projectCompanyUser.ProjectId == projectId);
    }
}