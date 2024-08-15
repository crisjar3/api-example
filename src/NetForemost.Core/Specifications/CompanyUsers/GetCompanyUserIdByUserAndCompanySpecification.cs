using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.CompanyUsers;

public class GetCompanyUserIdByUserAndCompanySpecification : Specification<CompanyUser>
{
    public GetCompanyUserIdByUserAndCompanySpecification(string userId, int companyId)
    {
        Query.Where(companyUser => companyUser.UserId == userId && companyUser.CompanyId == companyId);

        Query.Take(1);
    }
}
