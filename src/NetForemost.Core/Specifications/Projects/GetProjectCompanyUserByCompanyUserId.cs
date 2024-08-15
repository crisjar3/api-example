using Ardalis.Specification;
using NetForemost.Core.Entities.Projects;

namespace NetForemost.Core.Specifications.Projects
{
    public class GetProjectCompanyUserByCompanyUserId : Specification<NetForemost.Core.Entities.Projects.ProjectCompanyUser>
    {
        public GetProjectCompanyUserByCompanyUserId(int companyUserId, int[]? projectCompanyUserList)
        {
            Query.Where(projectCompanyUser => projectCompanyUser.CompanyUserId == companyUserId 
                && (projectCompanyUserList == null || projectCompanyUserList.Contains(projectCompanyUser.ProjectId))
            );
            Query.Include(projectCompanyUser => projectCompanyUser.Project);
        }
    }
}