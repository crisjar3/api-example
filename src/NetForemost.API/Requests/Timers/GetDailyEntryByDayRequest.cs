using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Timers;

public class GetDailyEntryByDayRequest
{
    [Required]
    public DateTime Date { get; set; }
}