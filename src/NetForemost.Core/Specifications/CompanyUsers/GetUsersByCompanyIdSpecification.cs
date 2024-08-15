using Ardalis.Specification;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.CompanyUsers;

public class GetUsersByCompanyIdSpecification : Specification<CompanyUser, UsersByCompanyDto>
{
    public GetUsersByCompanyIdSpecification(int companyId)
    {
        Query.Include(companyUser => companyUser.User);
        Query.Include(companyUser => companyUser.Company);

        Query.Where(companyUser => companyUser.CompanyId == companyId);

        Query.Select(companyUser => new UsersByCompanyDto()
        {
            CompanyUserId = companyUser.Id,
            FullName = companyUser.User.FirstName + " " + companyUser.User.LastName,
            CompanyId = companyUser.CompanyId
        });
    }
}
