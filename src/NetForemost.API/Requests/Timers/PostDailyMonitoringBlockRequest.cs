using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Timers
{
    public class PostDailyMonitoringBlockRequest
    {
        [Range(1, int.MaxValue)]
        public int DailyTimeBlockId { get; set; }
        [Required]
        public string UrlImage { get; set; }
        public double KeystrokesMin { get; set; }
        public double MouseMovementsMin { get; set; }
    }
}
