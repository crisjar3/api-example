using NetForemost.Core.Dtos.Cities;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Dtos.JobRoles;
using NetForemost.Core.Dtos.Seniorities;
using NetForemost.Core.Dtos.TimeZone;

namespace NetForemost.Core.Dtos.Account;

public class UserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime Registered { get; set; }
    public string? UserImageUrl { get; set; }

    public List<CompanyRoleDto> Companies { get; set; }
    public CityDto? City { get; set; }
    public SeniorityDto? Seniority { get; set; }
    public JobRoleDto? JobRole { get; set; }
    public TimeZoneDto? TimeZone { get; set; }

    public List<UserSkillDto> UserSkills { get; set; }
    public List<UserLanguageDto> UserLanguages { get; set; }
}