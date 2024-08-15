using System.ComponentModel.DataAnnotations;

namespace NetForemost.Report.API.Requests.Timer;
public class GetMonitoringsByBlockRequest
{
    [Required]
    public int DailyTimeBlockId { get; set; }
}
