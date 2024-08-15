using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account
{
    public class GetUsersByRoleRequest
    {
        [Required]
        public string RoleName { get; set; }
    }
}