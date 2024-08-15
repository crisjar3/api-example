using NetForemost.Core.Enumerations.Companies;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies;

public class PostCompanySettingsRequest
{
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public DayOfWeek FirstDayOfWeek { get; set; }

    [Required]
    public TrackingTypeEnum TrackingType { get; set; }

    [Required]
    public BlurScreenshotsEnum BlurScreenshots { get; set; }

    [Required]
    public DontTimeOutOnCallsEnum DontTimeOutOnCalls { get; set; }

    [Required]
    public bool AutoStartTracking { get; set; }

    [Required]
    public bool UserProjectsTasks { get; set; }

    [Required]
    public bool UsePlayroll { get; set; }

    [Required]
    public bool UseWorkSchedules { get; set; }

    [Required]
    public bool AllowManagersCreateProjects { get; set; }

    [Required]
    public bool AllowManagersInviteNewUsers { get; set; }

    [Required]
    public bool AllowManagersSetUpWorkSchedules { get; set; }
}
