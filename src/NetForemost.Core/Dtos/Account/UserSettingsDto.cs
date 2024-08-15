namespace NetForemost.Core.Dtos.Account;

public class UserSettingsDto
{
    public int Id { get; set; }
    public bool BlurScreenshots { get; set; }
    public bool CanEditTime { get; set; }
    public bool DeleteScreencasts { get; set; }
    public bool ShowInReports { get; set; }
    public TimeSpan ScreencastsFrecuency { get; set; }
    public TimeSpan TimeOutAfter { get; set; }
    public int TimeZoneId { get; set; }
    public string UserId { get; set; }
    public int LanguageId { get; set; }
}