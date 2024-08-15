using Ardalis.Specification;

namespace NetForemost.Core.Specifications.Users;
public class GetUserSpecification : IncludeUserEntitiesSpecification
{
    /// <summary>
    /// Specification to get user by many fields
    /// </summary>
    /// <param name="skillsId">a list of skill</param>
    /// <param name="jobRolesId">a list of job roles</param>
    /// <param name="senioritiesId">a list of seniorities</param>
    /// <param name="Email">a email</param>
    /// <param name="isActive">the user is active</param>
    /// <param name="startRegistrationDate">the initial registration date</param>
    /// <param name="endRegistrationDate">the end registration date</param>
    /// <param name="name">the name of the user</param>
    public GetUserSpecification(
        int[] skillsId,
        int[] jobRolesId,
        int[] senioritiesId,
        int[] countries,
        int[] cities,
        string Email,
        bool? isActive,
        DateTime? startRegistrationDate,
        DateTime? endRegistrationDate,
        string name,
        Dictionary<int, int> languages,
        int pageNumber,
        int perPage,
        bool paginate
        )
    {
        if (skillsId is not null)
        {
            Query.Where(user => user.UserSkills.Any(userSkill => skillsId.Any(skill => skill.Equals(userSkill.SkillId))));
        }

        if (jobRolesId is not null)
        {
            Query.Where(user => jobRolesId.Any(jobRole => jobRole.Equals(user.JobRoleId)));
        }

        if (senioritiesId is not null)
        {
            Query.Where(user => senioritiesId.Any(seniority => seniority.Equals((int)user.SeniorityId)));
        }

        if (cities is not null)
        {
            Query.Where(user => cities.Any(city => city.Equals((int)user.CityId)));
        }

        if (countries is not null && cities is null)
        {
            Query.Where(user => countries.Any(country => country.Equals((int)user.City.CountryId)));
        }

        if (!string.IsNullOrEmpty(Email))
        {
            Query.Search(user => user.Email, Email);
        }

        if (isActive is not null)
        {
            Query.Where(user => user.IsActive == isActive);
        }

        if (!string.IsNullOrEmpty(name))
        {
            Query.Search(user => user.FirstName + " " + user.LastName, "%" + name + "%");
        }

        if (startRegistrationDate is not null && endRegistrationDate is not null)
        {
            Query.Where(user => user.Registered.Date >= startRegistrationDate.Value.Date && user.Registered.Date <= endRegistrationDate.Value.Date);
        }

        if (languages.Count > 0)
        {
            foreach (var language in languages)
            {
                Query.Where(user => user.UserLanguages.Any(userLanguage => userLanguage.LanguageId == language.Key && userLanguage.LanguageLevelId == language.Value));
            }
        }

        if (paginate)
        {
            perPage = pageNumber == 0 ? 0 : perPage;
            Query.Skip((pageNumber - 1) * perPage).Take(perPage);
        }
    }
}
