using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects;

public class GetProjectsByCompanyIdRequest
{
    [Required]
    public int CompanyId { get; set; }
}
