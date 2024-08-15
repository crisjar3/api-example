using Ardalis.Specification;
using NetForemost.Core.Entities.JobRoles;

namespace NetForemost.Core.Specifications.JobRoles
{
    public class GetJobRoleCategoriesByNameSpecification : Specification<JobRoleCategory>
    {
        public GetJobRoleCategoriesByNameSpecification(string jobRoleCategoryName, int? companyId)
        {
            Query.Where(jobRoleCategory => jobRoleCategory.CompanyId == companyId || jobRoleCategory.IsDefault == true)
                .Where(jobRoleCategory => jobRoleCategory.Name.ToUpper() == jobRoleCategoryName.ToUpper());
        }
    }
}
