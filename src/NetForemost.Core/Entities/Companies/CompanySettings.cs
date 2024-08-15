using NetForemost.Core.Enumerations.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Companies;

public class CompanySettings : BaseEntity
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
    public TooManyHoursPerDayEnum? TooManyHoursPerDay { get; set; }
    public LateHoursInUsersTimeZoneAfterEnum? LateHoursInUsersTimeZoneAfter { get; set; }
    public WorkOutsideShiftPerDayLongerThanEnum? WorkOutsideShiftPerDayLongerThan { get; set; }

    public Company Company { get; set; }
}