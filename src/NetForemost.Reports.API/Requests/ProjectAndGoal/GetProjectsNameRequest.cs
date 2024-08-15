using NetForemost.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.Reports.API.Requests.ProjectAndGoal;

public class GetProjectsNameRequest : PaginationRequest
{
    [Required]
    public int CompanyId { get; set; }
}
