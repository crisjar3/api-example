using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects;
public class PutProjectCompanyUserStatusRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int CompanyUserId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int ProjectId { get; set; }

}
