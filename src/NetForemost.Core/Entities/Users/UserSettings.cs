using NetForemost.Core.Entities.Languages;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Users;

public class UserSettings : BaseEntity
{
    public bool BlurScreenshots { get; set; }
    public bool CanEditTime { get; set; }
    public bool DeleteScreencasts { get; set; }
    public bool ShowInReports { get; set; }
    public TimeSpan ScreencastsFrecuency { get; set; }
    public TimeSpan TimeOutAfter { get; set; }
    public string UserId { get; set; }
    public int LanguageId { get; set; }
    public virtual User User { get; set; }
    public virtual Language Language { get; set; }

    public UserSettings()
    {
        LanguageId = 1;
        BlurScreenshots = false;
        CanEditTime = false;
        DeleteScreencasts = false;
        ShowInReports = false;
        ScreencastsFrecuency = new TimeSpan(0, 3, 0);
        TimeOutAfter = new TimeSpan(0, 5, 0);
    }

    public void NewSettings(string userId)
    {

        UserId = userId;
        CreatedAt = DateTime.Now;
        CreatedBy = userId;
    }
}