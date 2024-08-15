using Microsoft.AspNetCore.Identity;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Seniorities;

using Task = NetForemost.Core.Entities.Tasks.Task;

namespace NetForemost.Core.Entities.Users;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; }
    public DateTime Registered { get; set; }
    public int? CityId { get; set; }
    public int? SeniorityId { get; set; }
    public int? JobRoleId { get; set; }
    public int? TimeZoneId { get; set; }
    public string? UserImageUrl { get; set; }
    public virtual City? City { get; set; }
    public virtual Seniority? Seniority { get; set; }
    public virtual JobRole? JobRole { get; set; }
    public virtual TimeZones.TimeZone? TimeZone { get; set; }
    public virtual UserSettings UserSettings { get; set; }
    public virtual ICollection<CompanyUser> CompanyUsers { get; set; }
    public virtual ICollection<UserSkill> UserSkills { get; set; }
    public virtual ICollection<UserLanguage> UserLanguages { get; set; }
}