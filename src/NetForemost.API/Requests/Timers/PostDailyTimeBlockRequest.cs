using NetForemost.API.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Timers
{
    public class PostDailyTimeBlockRequest
    {
        [Range(1, int.MaxValue)]
        [Required]
        public int TaskId { get; set; }

        [Required]
        [DateRangeValidation]
        public DateTime TimeStart { get; set; }

        [Required]
        public DateTime TimeEnd { get; set; }
    }

}
