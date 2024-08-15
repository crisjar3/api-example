using Ardalis.Specification;

namespace NetForemost.Core.Specifications.Projects
{
    public class GetProjectCompanyUserByProjectId : Specification<Entities.Projects.ProjectCompanyUser>
    {
        public GetProjectCompanyUserByProjectId(int projectId, int[]? companyUsersIdsList)
        {
            Query.Where(projectCompanyUser => projectCompanyUser.ProjectId == projectId 
                && (companyUsersIdsList == null || companyUsersIdsList.Contains(projectCompanyUser.CompanyUserId))
            );
        }
    }
}