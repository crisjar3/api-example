namespace NetForemost.Core.Dtos.Timer;

public class GetMonitoringByDailyTimeBlockDto
{
    public int KeystrokesMin { get; set; }
    public int MouseMovementsMin { get; set; }
    public string UrlImage { get; set; }
    public DateTime CreatedAt { get; set; }
}
