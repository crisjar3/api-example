using Ardalis.Specification;

namespace NetForemost.Core.Specifications.Users;

public class GetUserIdByProjectCompanyUserSpecification : IncludeUserEntitiesSpecification
{
    public GetUserIdByProjectCompanyUserSpecification(int projectCompanyUserId)
    {
        Query.Include(user => user.CompanyUsers).ThenInclude(companyUser => companyUser.ProjectCompanyUsers);

        Query.Where(user => user.CompanyUsers
        .Any(companyUser => companyUser.ProjectCompanyUsers
        .Any(projectCompanyUser => projectCompanyUser.Id == projectCompanyUserId)));
    }
}
