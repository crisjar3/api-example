using Ardalis.Specification;
using NetForemost.Core.Entities.Policies;

namespace NetForemost.Core.Specifications.Policies;
public class GetPolicyByCompanyIdSpecification : Specification<Policy>
{
    public GetPolicyByCompanyIdSpecification(int companyId)
    {
        Query.Where(policy => policy.CompanyId == companyId || policy.CompanyId == null);
    }
}

