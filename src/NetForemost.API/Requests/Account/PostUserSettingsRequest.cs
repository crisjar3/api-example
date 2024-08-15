using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account;

public class PostUserSettingsRequest
{
    public bool BlurScreenshots { get; set; } = true;

    public bool CanEditTime { get; set; } = false;

    public bool DeleteScreencasts { get; set; } = false;

    public bool ShowInReports { get; set; } = true;

    [Required]
    public TimeSpan ScreencastsFrecuency { get; set; }

    [Required]
    public TimeSpan TimeOutAfter { get; set; }

    [Range(0, 100)]
    public int TimeZoneId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int LanguageId { get; set; }
}