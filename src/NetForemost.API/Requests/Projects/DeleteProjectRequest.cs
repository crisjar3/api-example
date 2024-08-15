using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects
{
    public class DeleteProjectRequest
    {
        [Required]
        public int ProjectId { get; set; }
    }
}
