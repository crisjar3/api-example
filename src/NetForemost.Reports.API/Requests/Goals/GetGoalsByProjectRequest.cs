using NetForemost.SharedKernel.Helpers;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.Reports.API.Requests;

public class GetGoalsByProjectRequest : DateRangeWithPaginationHelper
{
    [Required]
    public int ProjectId { get; set; }
    public int[] CompaniesUsers { get; set; } = Array.Empty<int>();
    [Required]
    public double TimeZone { get; set; }
}