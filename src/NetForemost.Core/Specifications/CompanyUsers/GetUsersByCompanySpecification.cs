using Ardalis.Specification;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Dtos.TimeZone;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.CompanyUsers;

public class GetUsersByCompanySpecification : Specification<CompanyUser, UserSettingCompanyUserDto>
{
    public GetUsersByCompanySpecification(
        int companyId,
        int[] timeZones,
        int[] companyUserIds,
        bool isArchived,
        DateTime? from,
        DateTime? to,
        int pageNumber,
        int perPage,
        bool paginate)
    {
        //Include the necessaries entities
        Query.Include(companyUser => companyUser.JobRole);
        Query.Include(companyUser => companyUser.Role);
        Query.Include(companyUser => companyUser.TimeZone);

        //Perform the search
        Query.Where(userCompany => userCompany.CompanyId == companyId);
        Query.Where(userCompany => userCompany.IsArchived == isArchived);

        //Filters searches
        if (timeZones.Any())
        {
            Query.Where(companyUser => timeZones.Contains((int)companyUser.TimeZoneId));
        }

        if (companyUserIds.Any())
        {
            Query.Where(companyUser => companyUserIds.Contains(companyUser.Id));
        }
        //Deleted because not needed at this time 
        //if (to is not null && from is not null && from <= to)
        //{
        //    Query.Where(companyUser => companyUser.CreatedAt.Date >= from.Value.Date && companyUser.CreatedAt.Date <= to.Value.Date);
        //}

        if (paginate)
        {
            perPage = pageNumber == 0 ? 0 : perPage;
            Query.Skip((pageNumber - 1) * perPage).Take(perPage);
        }

        //Order By Name Result
        Query.OrderBy(userCompany => userCompany.UserName);

        Query.Select(companyUser => new UserSettingCompanyUserDto
        {
            Id = companyUser.Id,
            UserImageUrl = companyUser.UserImageUrl,
            UserName = companyUser.UserName,
            TimeZone = new TimeZoneDto()
            {
                Id = companyUser.TimeZone.Id,
                Offset = companyUser.TimeZone.Offset,
                Text = companyUser.TimeZone.Text

            },
            Role = new Dtos.Account.RoleDto()
            {
                Id = companyUser.RoleId,
                Name = companyUser.Role.Name
            },
            JobRole = new Dtos.JobRoles.JobRoleDto()
            {
                Id = companyUser.JobRole.Id,
                Name = companyUser.JobRole.Name
            },
            CreatedAt = companyUser.CreatedAt,
            IsArchived = isArchived
        });
    }
}