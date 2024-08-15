using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Tasks;
public class GetTaskByIdRequest
{
    [Required]
    [Range(0, int.MaxValue)]
    public int Id { get; set; }
}
