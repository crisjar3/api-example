using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.CompanyUsers;

public class GetCompanyUserByCompanyIdSpecification : Specification<CompanyUser>
{
    public GetCompanyUserByCompanyIdSpecification(int companyId, int pageNumber, int perPage, bool paginate)
    {
        //Perform the search
        Query.Where(userCompany => userCompany.CompanyId == companyId && !userCompany.IsArchived && !userCompany.isDeleted);

        if (paginate)
        {
            perPage = pageNumber == 0 ? 0 : perPage;
            Query.Skip((pageNumber - 1) * perPage).Take(perPage);
        }

        //Order By Name Result
        Query.OrderBy(userCompany => userCompany.UserName);
    }
}
