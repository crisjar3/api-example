using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.CompanyUsers
{
    public class GetCompanyUsersByCompanyIdSpecification : Specification<CompanyUser>
    {
        public GetCompanyUsersByCompanyIdSpecification(int companyId, int[]? companyUsersIdsList)
        {
            Query.Where(companyUser => companyUser.CompanyId == companyId 
                && (companyUsersIdsList == null || companyUsersIdsList.Contains(companyUser.Id))
            );
        }
    }
}