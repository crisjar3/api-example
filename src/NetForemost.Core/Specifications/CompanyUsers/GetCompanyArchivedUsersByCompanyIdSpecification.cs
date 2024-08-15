using Ardalis.Specification;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.Companies
{
    public class GetCompanyArchivedUsersByCompanyIdSpecification : Specification<CompanyUser, GetCompanyArchivedUsersDto>
    {
        public GetCompanyArchivedUsersByCompanyIdSpecification(int companyId, int pageNumber, int perPage, bool paginate)
        {
            Query.Where(cu => cu.CompanyId == companyId && cu.IsArchived && !cu.isDeleted);

            if (paginate)
            {
                Query.Skip((pageNumber - 1) * perPage).Take(perPage);
            }

            Query.Select(companyUser => new GetCompanyArchivedUsersDto()
            {
                CompanyUserId = companyUser.Id,
                UserName = companyUser.UserName
            });
        }
    }
}
