using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.CompanyUsers;

public class CheckUserIsInCompanySpecification : Specification<CompanyUser>
{
    public CheckUserIsInCompanySpecification(int companyId, string userId)
    {
        Query.Where(companyUser => companyUser.CompanyId == companyId && companyUser.UserId == userId && companyUser.IsActive && !companyUser.isDeleted);
    }
}