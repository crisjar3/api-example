using System.ComponentModel.DataAnnotations;

namespace NetForemost.Report.API.Requests.Timer;

public class GetBlockTimeByDailyEntryRequest
{
    [Required]
    public double TimeZone { get; set; }
    [Required]
    public DateTime DateDay { get; set; }
    [Required]
    public int CompanyUser { get; set; }
}