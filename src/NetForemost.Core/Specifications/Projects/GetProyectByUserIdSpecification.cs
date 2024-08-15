using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.Projects;

public class GetProyectByUserId : Specification<CompanyUser>
{
    public GetProyectByUserId(string userId, int projectId)
    {
        Query.Where(companyUser => companyUser.UserId.Equals(userId) && companyUser.ProjectCompanyUsers.Any(project => project.ProjectId.Equals(projectId)));
    }
}
