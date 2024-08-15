using Ardalis.Specification;

namespace NetForemost.Core.Specifications.ProjectCompanyUser
{
    public class CheckUserExistInProjectSpecification : Specification<NetForemost.Core.Entities.Projects.ProjectCompanyUser>
    {
        public CheckUserExistInProjectSpecification(string userId, int projectId)
        {

            Query.Include(pcu => pcu.CompanyUser).Where(pcu => pcu.CompanyUser.UserId == userId && pcu.ProjectId == projectId);
        }
    }
}
