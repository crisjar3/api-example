using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies;

public class PutCompanyRequest
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? ZipCode { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [DefaultValue(0)]
    [Range(1, int.MaxValue)]
    [Required]
    public int CityId { get; set; }

    [DefaultValue(0)]
    [Range(1, int.MaxValue)]
    [Required]
    public int TimeZoneId { get; set; }

    [DefaultValue(0)]
    [Range(1, int.MaxValue)]
    public int? EmployeesNumber { get; set; }

    public string? Description { get; set; }

    public string? Website { get; set; }

    [DefaultValue(0)]
    [Range(1, int.MaxValue)]
    [Required]
    public int IndustryId { get; set; }
}