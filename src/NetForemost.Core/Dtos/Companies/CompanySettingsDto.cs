using NetForemost.Core.Enumerations.Companies;

namespace NetForemost.Core.Dtos.Companies;

public class CompanySettingsDto
{
    public DayOfWeek FirstDayOfWeek { get; set; }
    public TrackingTypeEnum TrackingType { get; set; }
    public BlurScreenshotsEnum BlurScreenshots { get; set; }
    public DontTimeOutOnCallsEnum DontTimeOutOnCalls { get; set; }
    public bool AutoStartTracking { get; set; }
    public bool UserProjectsTasks { get; set; }
    public bool UsePlayroll { get; set; }
    public bool UseWorkSchedules { get; set; }
    public bool AllowManagersCreateProjects { get; set; }
    public bool AllowManagersInviteNewUsers { get; set; }
    public bool AllowManagersSetUpWorkSchedules { get; set; }
    public int CompanyId { get; set; }
}
