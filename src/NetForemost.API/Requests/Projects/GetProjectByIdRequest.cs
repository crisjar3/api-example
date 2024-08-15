using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects;
public class GetProjectByIdRequest
{
    [Required, Range(1, int.MaxValue)]
    public int ProjectId { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int CompanyId { get; set; }
}

