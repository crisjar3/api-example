using Ardalis.Specification;
using NetForemost.Core.Entities.JobRoles;

namespace NetForemost.Core.Specifications.JobRoles;
public class GetJobRoleByCompanyIdSpecification : Specification<JobRole>
{
    public GetJobRoleByCompanyIdSpecification(int id, int companyId)
    {
        Query.Where(jobrole => (jobrole.Id == id && jobrole.CompanyId == companyId) || (jobrole.Id == id && jobrole.IsDefault == true));
    }
}

