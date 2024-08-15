using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Tasks
{
    public class PostTaskTypeRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int CompanyId { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int ProjectId { get; set; }
    }
}
