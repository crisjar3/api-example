using Ardalis.Specification;
using NetForemost.Core.Entities.Benefits;

namespace NetForemost.Core.Specifications.Benefits;

public class GetBenefitsByCompanyIdSpecification : Specification<Benefit>
{
    public GetBenefitsByCompanyIdSpecification(int companyId)
    {
        Query.Where(benefit => benefit.IsDefault || benefit.CompanyId == companyId);
    }
}