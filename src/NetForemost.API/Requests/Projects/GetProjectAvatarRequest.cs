using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects
{
    public class GetProjectAvatarRequest
    {
        [Required]
        public int ProjectId { get; set; }
    }
}
