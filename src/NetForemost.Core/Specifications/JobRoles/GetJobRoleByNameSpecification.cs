using Ardalis.Specification;
using NetForemost.Core.Entities.JobRoles;

namespace NetForemost.Core.Specifications.JobRoles
{
    public class GetJobroleByNameSpecification : Specification<JobRole>
    {
        /// <summary>
        /// Obtains the job role by the job role name and the companyId.
        /// </summary>
        /// <param name="jobRoleName">The job role name.</param>
        public GetJobroleByNameSpecification(string jobRoleName, int? companyId)
        {
            Query.Where(jobRole => jobRole.CompanyId == companyId || jobRole.IsDefault == true)
                .Where(jobRole => jobRole.Name.ToUpper() == jobRoleName.ToUpper());
        }
    }
}
