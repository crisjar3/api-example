using NetForemost.SharedKernel.Helpers;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.Reports.API.Requests.ProjectAndGoal;

public class GetProjectAndGoalsSummaryBarRequest : DateRangeHelper
{
    [Required]
    public int CompanyId { get; set; }
    public int[] CompanyUserIds { get; set; } = Array.Empty<int>();
    public int[] ProjectIds { get; set; } = Array.Empty<int>();
    [Required]
    public double TimeZone { get; set; }
}
