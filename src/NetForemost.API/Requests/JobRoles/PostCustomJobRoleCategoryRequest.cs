using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.JobRoles
{
    public class PostCustomJobRoleCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        [Required]
        public int CompanyId { get; set; }
    }
}
