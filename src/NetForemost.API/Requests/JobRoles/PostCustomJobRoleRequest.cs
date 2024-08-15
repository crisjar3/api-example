using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.JobRoles
{
    public class PostCustomJobRoleRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int JobRoleCategoryId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int CompanyId { get; set; }
    }
}
