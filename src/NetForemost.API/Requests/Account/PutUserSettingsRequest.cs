using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account
{
    public class PutUserSettingsRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool BlurScreenshots { get; set; }

        [Required]
        public bool CanEditTime { get; set; }

        [Required]
        public bool DeleteScreencasts { get; set; }

        [Required]
        public bool ShowInReports { get; set; }

        [Required]
        public TimeSpan ScreencastsFrecuency { get; set; }

        [Required]
        public TimeSpan TimeOutAfter { get; set; }

        [Range(0, 100)]
        public int TimeZoneId { get; set; }

        [Required]
        public int LanguageId { get; set; }
    }
}
