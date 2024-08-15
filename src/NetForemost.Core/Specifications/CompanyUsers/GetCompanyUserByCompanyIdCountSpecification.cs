using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.CompanyUsers;

public class GetCompanyUserByCompanyIdCountSpecification : Specification<CompanyUser>
{
    public GetCompanyUserByCompanyIdCountSpecification(int companyId)
    {
        Query.Where(companyUser => companyUser.CompanyId == companyId);
    }
}
