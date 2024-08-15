using Ardalis.Specification;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Specifications.Users;

public class IncludeUserEntitiesSpecification : Specification<User>
{
    public IncludeUserEntitiesSpecification()
    {
        Query.Include(user => user.TimeZone)
           .Include(user => user.Seniority)
           .Include(user => user.JobRole)
           .Include(user => user.City)
           .ThenInclude(city => city.Country);

        Query.Include(user => user.CompanyUsers)
            .ThenInclude(companyUser => companyUser.Role);

        Query.Include(user => user.CompanyUsers)
            .ThenInclude(companyUser => companyUser.ProjectCompanyUsers)
            .ThenInclude(projectCompanyUsers => projectCompanyUsers.Project);

        Query.Include(user => user.CompanyUsers)
            .ThenInclude(companyUser => companyUser.ProjectCompanyUsers)
            .ThenInclude(projectCompanyUsers => projectCompanyUsers.JobRole);

        Query.Include(user => user.CompanyUsers)
            .ThenInclude(companyUser => companyUser.Company)
            .ThenInclude(company => company.TimeZone);

        Query.Include(user => user.UserSkills)
            .ThenInclude(userSkill => userSkill.Skill);

        Query.Include(user => user.UserLanguages)
            .ThenInclude(userLanguages => userLanguages.Language);

        Query.Include(user => user.UserLanguages)
            .ThenInclude(userLanguages => userLanguages.LanguageLevel);
    }
}