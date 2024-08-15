using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.Companies;

public class GetCompanyUsersByUserIdSpecification : Specification<CompanyUser>
{
    public GetCompanyUsersByUserIdSpecification(int companyId)
    {
        Query.Include(companyUser => companyUser.User);
        Query.Where(companyUser => companyUser.CompanyId == companyId);
    }
}
