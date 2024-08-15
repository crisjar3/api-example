using NetForemost.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.TaskTypes;

public class GetTaskTypesRecentlyRequest : PaginationRequest
{
    [Required]
    public int CompanyUserId { get; set; }
    [Required]
    public int ProjectId { get; set; }
    public string Search { get; set; }
}
