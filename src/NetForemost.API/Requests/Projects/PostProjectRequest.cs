using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects;
public class PostProjectRequest
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(1, int.MaxValue)]
    [Required]
    public int CompanyId { get; set; }
    public string? ProjectImageUrl { get; set; }
}
