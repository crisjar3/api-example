using NetForemost.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects;

public class GetUsersByCompanyRequest : PaginationRequest
{
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public int ProjectId { get; set; }
}
