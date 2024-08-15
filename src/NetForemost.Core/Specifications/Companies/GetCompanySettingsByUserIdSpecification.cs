using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Specifications.Companies
{
    public class GetCompanySettingsByUserIdSpecification : Specification<CompanySettings>, ISingleResultSpecification
    {
        /// <summary>
        ///     Obtains the configuration of a company by means of the user's Id.
        /// </summary>
        /// <param name="userId">The userId.</param>
        public GetCompanySettingsByUserIdSpecification(string userId)
        {
            Query.Include(companySettings => companySettings.Company).ThenInclude(company => company.CompanyUsers).ThenInclude(companyUser => companyUser.Role);

            Query.Where(companySettings => companySettings.Company.CompanyUsers.Any(comapnyUser => comapnyUser.UserId == userId && comapnyUser.Role.Name == NameStrings.RoleName_Owner));
        }
    }
}