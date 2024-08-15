using Ardalis.Specification;

namespace NetForemost.Core.Specifications.ProjectCompanyUser;

public class GetProjectCompanyUserSpecification : Specification<NetForemost.Core.Entities.Projects.ProjectCompanyUser>
{
    public GetProjectCompanyUserSpecification(int companyUserId, int projectId)
    {
        Query.Where(projectCompanyUser => projectCompanyUser.CompanyUserId == companyUserId && projectCompanyUser.ProjectId == projectId);

        Query.Take(1);
    }
}
