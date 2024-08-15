using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies;

public class PostTeamMemberRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    [Default(1)]
    public int CompanyId { set; get; }
    [Required]
    public string UserId { set; get; }
    [Required]
    public string UserName { set; get; }
    [Required]
    public int? TimeZoneId { get; set; }
    [Required]
    public int? JobRoleId { get; set; }
    [Required]
    public string RoleId { get; set; }
}