using NetForemost.SharedKernel.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies;

public class GetTeamMemberRequest : DateRangeWithPaginationHelper
{
    [Required]
    public int CompanyId { get; set; }
    public int[]? TimeZonesIds { get; set; } = Array.Empty<int>();
    public int[]? CompanyUserIds { get; set; } = Array.Empty<int>();
    public bool isArchived { get; set; }
    public new DateTime From { get; set; }
    public new DateTime To { get; set; }

}