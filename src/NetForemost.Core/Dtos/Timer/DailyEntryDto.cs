namespace NetForemost.Core.Dtos.Timer;
public class DailyEntryDto
{
    public DateTime DateStart { get; set; }
    public string OwnerId { get; set; }
    public double TotalTrackingTime { get; set; }
    public double KeystrokesMin { get; set; }
    public double MouseMovementsMin { get; set; }
}