using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects;
public class PostProjectCompanyUserListByCompanyUsersIdsRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProjectId { get; set; }
    [Required]
    public int[] CompanyUserIdList { get; set; }
}