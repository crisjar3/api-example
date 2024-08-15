using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.CompanyUsers;
public class GetCompanyUserByDateSpecification : Specification<CompanyUser>
{
    /// <summary>
    /// Obtains all company users created in a date range and that are active.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    public GetCompanyUserByDateSpecification(DateTime startDate, DateTime endDate)
    {
        Query.Where(companyUser => companyUser.IsActive);

        Query.Where(companyUser => companyUser.CreatedAt <= startDate && companyUser.CreatedAt >= endDate);
    }
}
