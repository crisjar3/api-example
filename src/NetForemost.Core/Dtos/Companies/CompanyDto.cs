using NetForemost.Core.Dtos.Industries;
using NetForemost.Core.Dtos.TimeZone;

namespace NetForemost.Core.Dtos.Companies;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? ZipCode { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CompanyImageUrl { get; set; }
    public TimeZoneDto TimeZone { get; set; }
    public int EmployeesNumber { get; set; }
    public IndustryDto Industry { get; set; }
}