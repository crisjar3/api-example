using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Specifications.Companies;

public class GetCompanyByUserIdSpecification : Specification<Company>, ISingleResultSpecification
{
    /// <summary>
    ///     Obtains the company by means of the user's Id.
    /// </summary>
    /// <param name="userId">The userId.</param>
    public GetCompanyByUserIdSpecification(string userId)
    {
        Query.Include(company => company.City)
            .Include(company => company.TimeZone)
            .Include(company => company.Industry)
            .Include(company => company.CompanyUsers).ThenInclude(companyUser => companyUser.Role);

        Query.Where(company => company.CompanyUsers.Any(comapnyUser => comapnyUser.UserId == userId && comapnyUser.Role.Name == NameStrings.RoleName_Owner));
    }
}