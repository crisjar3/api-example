namespace NetForemost.Core.Dtos.Timer;
public class DailyMonitoringBlockDto
{
    public int Id { get; set; }
    public double KeystrokesMin { get; set; }
    public double MouseMovementsMin { get; set; }
    public string UrlImage { get; set; }
}
