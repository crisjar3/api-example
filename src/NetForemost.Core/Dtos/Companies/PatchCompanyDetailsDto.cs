namespace NetForemost.Core.Dtos.Companies;

public class PatchCompanyDetailsDto
{
    public int CompanyId { get; set; }
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
}