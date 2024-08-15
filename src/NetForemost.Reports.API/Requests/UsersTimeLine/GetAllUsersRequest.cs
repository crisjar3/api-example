using NetForemost.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.Reports.API.Requests.UsersTimeLine;

public class GetAllUsersRequest : PaginationRequest
{
    [Required]
    public int CompanyId { get; set; }
}
