using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects;
public class PutProjectRequest
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(1, int.MaxValue)]
    [Required]
    public int CompanyId { get; set; }
    public string? ProjectImageUrl { get; set; }

    // ==== REMOVED PROPERTIES FOR ISSUE: https://netforemost.atlassian.net/browse/NFCA-339
    // [Required]
    // public DateTime StartedDate { get; set; }
    // [Required]
    // public DateTime EndEstimatedDate { get; set; }
    // [Required, Range(1.0, float.MaxValue)]
    // public float Budget { get; set; }
    // [Required, MinLength(1)]
    // public string[] TechStack { get; set; }
    // [Range(1, int.MaxValue)]
    // [Required]
    // public int EstimatedSize { get; set; }
}
