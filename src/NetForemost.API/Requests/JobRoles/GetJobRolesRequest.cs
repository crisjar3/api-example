using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.JobRoles
{
    public class GetJobRolesRequest
    {
        [Required]
        public int CompanyId { get; set; }
    }
}
