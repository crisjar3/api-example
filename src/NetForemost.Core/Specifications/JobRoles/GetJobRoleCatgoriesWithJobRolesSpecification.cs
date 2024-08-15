using Ardalis.Specification;
using NetForemost.Core.Entities.JobRoles;

namespace NetForemost.Core.Specifications.JobRoles;

public class GetJobRoleCatgoriesWithJobRolesSpecification : Specification<JobRoleCategory>
{
    public GetJobRoleCatgoriesWithJobRolesSpecification(int companyId)
    {
        Query.Include(jobRoleCategory => jobRoleCategory.JobRoleCategorySkills)
            .ThenInclude(jobRoleCategoryKill => jobRoleCategoryKill.Skill);

        Query.Include(workRoleCategory => workRoleCategory.JobRoles.Where(jobRole => jobRole.CompanyId == companyId || jobRole.IsDefault == true)).
            Where(jobRoleCategory => (jobRoleCategory.IsDefault == true || jobRoleCategory.CompanyId == companyId));
    }
}