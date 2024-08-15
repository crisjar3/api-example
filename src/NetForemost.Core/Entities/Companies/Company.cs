using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Industries;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Companies;

public class Company : BaseEntity
{
    public string Name { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? ZipCode { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool State { get; set; }
    public int? EmployeesNumber { get; set; }
    public string? Description { get; set; }
    public string? Website { get; set; }
    public string? CompanyImageUrl { get; set; }

    public int CityId { get; set; }
    public int TimeZoneId { get; set; }
    public int? IndustryId { get; set; }

    public virtual City City { get; set; }
    public virtual TimeZones.TimeZone TimeZone { get; set; }
    public virtual Industry? Industry { get; set; }

    public virtual ICollection<CompanyUser> CompanyUsers { get; set; }
    public virtual ICollection<CompanySettings> CompanySettings { get; set; }
}