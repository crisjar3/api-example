using Ardalis.Specification;

namespace NetForemost.Core.Specifications.CompanyUsers
{
    public class GetCompanyUserByCompanyUserIdSpecification : Specification<NetForemost.Core.Entities.Companies.CompanyUser>
    {
        public GetCompanyUserByCompanyUserIdSpecification(int companyUserId)
        {
            Query.Include(projectCompanyUser => projectCompanyUser.ProjectCompanyUsers)
                .ThenInclude(project => project.Project);
            Query.Include(projectCompanyUser => projectCompanyUser.Role);
            Query.Include(projectCompanyUser => projectCompanyUser.JobRole);
            Query.Include(projectCompanyUser => projectCompanyUser.TimeZone);
            Query.Where(projectCompanyUser => projectCompanyUser.Id == companyUserId);

            Query.Take(1);
        }
    }
}
